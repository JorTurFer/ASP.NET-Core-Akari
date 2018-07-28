function startCalendar(getUrl,saveUrl,delUrl) {
	var events = [];
	var selectedEvent = null;
    FetchEventAndRenderCalendar(getUrl, saveUrl);
	generateHandlers(getUrl,saveUrl, delUrl);
}


function generateHandlers(getUrl,saveUrl, delUrl) {
	$('#btnEdit').click(function () {
		//Open modal dialog for edit event
		openAddEditForm();
	});
	$('#btnDelete').click(function () {
		if (selectedEvent !== null && confirm('¿Estas seguro?')) {
			$.ajax({
				type: "POST",
				url: delUrl,
				data: { 'eventID': selectedEvent.eventID },
				success: function (data) {
					if (data) {
						//Refresh the calender
                        FetchEventAndRenderCalendar(getUrl, saveUrl);
						$('#myModal').modal('hide');
					}
				},
				error: function () {
					alert("Oops, hemos tenido un problema...");
				}
			});
		}
	});
	$('#dtp1,#dtp2').datetimepicker({
		format: 'DD/MM/YYYY HH:mm A'
	});
	$('#chkIsFullDay').change(function () {
		if ($(this).is(':checked')) {
			$('#divEndDate').hide();
		}
		else {
			$('#divEndDate').show();
		}
	});
	$('#btnSave').click(function () {
		//Validation/
		if ($('#txtSubject').val().trim() === "") {
			alert('Cabecera necesaria');
			return;
		}
		if ($('#txtStart').val().trim() === "") {
			alert('Inicio necesario');
			return;
		}
		if ($('#chkIsFullDay').is(':checked') === false && $('#txtEnd').val().trim() === "") {
			alert('Fin necesario');
			return;
		}
		else {
			var startDate = moment($('#txtStart').val(), "DD/MM/YYYY HH:mm A").toDate();
			var endDate = moment($('#txtEnd').val(), "DD/MM/YYYY HH:mm A").toDate();
			if (startDate > endDate) {
				alert('Fin invalido');
				return;
			}
		}

		var data = {
			EventID: $('#hdEventID').val(),
			Subject: $('#txtSubject').val().trim(),
			Start: $('#txtStart').val().trim(),
			End: $('#chkIsFullDay').is(':checked') ? null : $('#txtEnd').val().trim(),
			Description: $('#txtDescription').val(),
			ThemeColor: $('#ddThemeColor').val(),
			IsFullDay: $('#chkIsFullDay').is(':checked')
		};
		SaveEvent(getUrl, saveUrl, data);
		// call function for submit data to the server 
	});
}

function FetchEventAndRenderCalendar(getUrl, saveUrl) {
	$.ajax({
		type: "GET",
		url: getUrl,
		success: function (data) {
			events = [];
			$.each(data, function (i, v) {
				events.push({
					eventID: v.eventID,
					title: v.subject,
					description: v.description,
					start: moment(v.start),
					end: v.end !== null ? moment(v.end) : null,
					color: v.themeColor,
					allDay: v.isFullDay,
					idPaciente: v.idPaciente
				});
			});
            GenerateCalendar(getUrl, saveUrl, events);
		},
		error: function (error) {
			alert("Oops, hemos tenido un problema...");
		}
	});
}

function GenerateCalendar(getUrl, saveUrl, events) {
	$('#calendar').fullCalendar('destroy');
    $('#calendar').fullCalendar({
        use24hours: true,
		locale: 'es',
		contentHeight: 400,
		defaultDate: new Date(),
		timeFormat: 'h(:mm)a',
		header: {
			left: 'prev,next today',
			center: 'title',
			right: 'month,basicWeek,basicDay,agenda'
		},
		eventLimit: true,
		eventColor: '#378006',
		events: events,
		eventClick: function (calEvent, jsEvent, view) {
			selectedEvent = calEvent;
			$('#myModal #eventTitle').text(calEvent.title);
			var $description = $('<div/>');
			$description.append($('<p/>').html('<b>Empieza:</b>' + calEvent.start.format("DD-MMM-YYYY HH:mm a")));
			if (calEvent.end !== null) {
				$description.append($('<p/>').html('<b>Termina:</b>' + calEvent.end.format("DD-MMM-YYYY HH:mm a")));
			}
			if (calEvent.description !== null) {
				$description.append($('<p/>').html('<b>Descripcion:</b>' + calEvent.description));
			}
			$('#myModal #pDetails').empty().html($description);

			$('#myModal').modal();
		},
		selectable: true,
		select: function (start, end) {
			selectedEvent = {
				eventID: 0,
				title: '',
				description: '',
				start: start,
				end: end,
				allDay: false,
				color: ''
			};
			openAddEditForm();
			$('#calendar').fullCalendar('unselect');
		},
		editable: true,
		eventDrop: function (event) {
			var data = {
				EventID: event.eventID,
				Subject: event.title,
				Start: event.start.format('DD/MM/YYYY HH:mm A'),
				End: event.end !== null ? event.end.format('DD/MM/YYYY HH:mm A') : null,
				Description: event.description,
				ThemeColor: event.color,
				IsFullDay: event.allDay
			};
            SaveEvent(getUrl, saveUrl, data);
        }
	});
}

function openAddEditForm() {
	if (selectedEvent !== null) {
		$('#hdEventID').val(selectedEvent.eventID);
		$('#txtSubject').val(selectedEvent.title);
		$('#txtStart').val(selectedEvent.start.format('DD/MM/YYYY HH:mm A'));
		$('#chkIsFullDay').prop("checked", selectedEvent.allDay || false);
		$('#chkIsFullDay').change();
		$('#txtEnd').val(selectedEvent.end !== null ? selectedEvent.end.format('DD/MM/YYYY HH:mm A') : '');
		$('#txtDescription').val(selectedEvent.description);
		$('#ddThemeColor').val(selectedEvent.color);
	}
	$('#myModal').modal('hide');
	$('#myModalSave').modal();
}

function SaveEvent(getUrl,saveUrl,data) {
	$.ajax({
		type: "POST",
		url: saveUrl,
		data: data,
		success: function (data) {
			if (data) {
				//Refresh the calender
                FetchEventAndRenderCalendar(getUrl, saveUrl);
				$('#myModalSave').modal('hide');
			}
		},
		error: function () {
			alert("Oops, hemos tenido un problema...");
		}
	});
}
