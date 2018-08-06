function openAddEditForm() {
    if (selectedEvent !== null) {
        $("#hdEventID").val(selectedEvent.eventID);
        $("#txtSubject").val(selectedEvent.title);
        $("#txtStart").val(selectedEvent.start.format("DD/MM/YYYY HH:mm"));
        $("#chkIsFullDay").prop("checked", selectedEvent.allDay || false);
        $("#chkIsFullDay").change();
        $("#txtEnd").val(selectedEvent.end !== null ? selectedEvent.end.format("DD/MM/YYYY HH:mm") : "");
        $("#txtDescription").val(selectedEvent.description);
        $("#ddThemeColor").val(selectedEvent.color);
    }
    $("#myModal").modal("hide");
    $("#myModalSave").modal();
}

function saveEvent(getUrl, saveUrl, data) {
    $.ajax({
        type: "POST",
        url: saveUrl,
        data: data,
        success: function (data) {
            if (data) {
                //Refresh the calender
                fetchEventAndRenderCalendar(getUrl, saveUrl);
                $("#myModalSave").modal("hide");
            }
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

function generateHandlers(getUrl, saveUrl, delUrl) {
    $("#btnEdit").click(function () {
        //Open modal dialog for edit event
        openAddEditForm();
    });
    $("#btnDelete").click(function () {
        if (selectedEvent !== null && confirm("¿Estas seguro?")) {
            $.ajax({
                type: "POST",
                url: delUrl,
                data: { "eventID": selectedEvent.eventID },
                success: function (data) {
                    if (data) {
                        //Refresh the calender
                        fetchEventAndRenderCalendar(getUrl, saveUrl);
                        $("#myModal").modal("hide");
                    }
                },
                error: function () {
                    alert("Oops, hemos tenido un problema...");
                }
            });
        }
    });
    $("#dtp1,#dtp2").datetimepicker({
        format: "DD/MM/YYYY HH:mm"
    });
    $("#chkIsFullDay").change(function () {
        if ($(this).is(":checked")) {
            $("#divEndDate").hide();
        }
        else {
            $("#divEndDate").show();
        }
    });
    $("#btnSave").click(function () {
        //Validation/
        if ($("#txtSubject").val().trim() === "") {
            alert("Cabecera necesaria");
            return;
        }
        if ($("#txtStart").val().trim() === "") {
            alert("Inicio necesario");
            return;
        }
        if ($("#chkIsFullDay").is(":checked") === false && $("#txtEnd").val().trim() === "") {
            alert("Fin necesario");
            return;
        }
        else {
            var startDate = moment($("#txtStart").val(), "DD/MM/YYYY HH:mm A").toDate();
            var endDate = moment($("#txtEnd").val(), "DD/MM/YYYY HH:mm A").toDate();
            if (startDate > endDate) {
                alert("Fin invalido");
                return;
            }
        }

        var data = {
            EventID: $("#hdEventID").val(),
            Subject: $("#txtSubject").val().trim(),
            Start: $("#txtStart").val().trim(),
            End: $("#chkIsFullDay").is(":checked") ? null : $("#txtEnd").val().trim(),
            Description: $("#txtDescription").val(),
            ThemeColor: $("#ddThemeColor").val(),
            IsFullDay: $("#chkIsFullDay").is(":checked")
        };
        saveEvent(getUrl, saveUrl, data);
        // call function for submit data to the server 
    });
}

function generateCalendar(getUrl, saveUrl, events) {
    $("#calendar").fullCalendar("destroy");
    $("#calendar").fullCalendar({
        use24hours: true,
        locale: "es",
        contentHeight: 600,
        defaultDate: new Date(),
        themeSystem: "bootstrap3",
        timeFormat: "HH:mm",
        scrollTime: "08:00:00",
        weekNumbers: true,
        weekNumberCalculation: "ISO",
        defaultView: "agendaWeek",
        header: {
            left: "prev,next today",
            center: "title",
            right: "month,agendaWeek,agendaDay"
        },
        eventLimit: true,
        eventColor: "#378006",
        events: events,
        eventClick: function (calEvent, jsEvent, view) {
            selectedEvent = calEvent;
            $("#myModal #eventTitle").text(calEvent.title);
            var $description = $("<div/>");
            $description.append($("<p/>").html("<b>Empieza:</b>" + calEvent.start.format("DD-MMM-YYYY HH:mm")));
            if (calEvent.end !== null) {
                $description.append($("<p/>").html("<b>Termina:</b>" + calEvent.end.format("DD-MMM-YYYY HH:mm")));
            }
            if (calEvent.description !== null) {
                $description.append($("<p/>").html("<b>Descripcion:</b>" + calEvent.description));
            }
            $("#myModal #pDetails").empty().html($description);

            $("#myModal").modal();
        },
        selectable: true,
        select: function (start, end) {
            selectedEvent = {
                eventID: 0,
                title: "",
                description: "",
                start: start,
                end: end,
                allDay: false,
                color: ""
            };
            openAddEditForm();
            $("#calendar").fullCalendar("unselect");
        },
        editable: true,
        eventDrop: function (event) {
            var data = {
                EventID: event.eventID,
                Subject: event.title,
                Start: event.start.format("DD/MM/YYYY HH:mm"),
                End: event.end !== null ? event.end.format("DD/MM/YYYY HH:mm") : null,
                Description: event.description,
                ThemeColor: event.color,
                IsFullDay: event.allDay
            };
            saveEvent(getUrl, saveUrl, data);
        }
    });
}

function fetchEventAndRenderCalendar(getUrl, saveUrl) {
    var date = new Date().toISOString();
    var type = "week";
    var calendar = $("#calendar").html;
    if($.trim($("#calendar").html()) !== "") {
        date = $("#calendar").fullCalendar("getDate").toISOString();
        type = $("#calendar").fullCalendar("getView").name;
    }
    $.ajax({
        type: "GET",
        url: getUrl,
        data: {
            Date: date,
            Type: type
        },
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
            generateCalendar(getUrl, saveUrl, events);
        },
        error: function (error) {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

function startCalendar(getUrl, saveUrl, delUrl) {
    fetchEventAndRenderCalendar(getUrl, saveUrl);
    generateHandlers(getUrl, saveUrl, delUrl);
}


