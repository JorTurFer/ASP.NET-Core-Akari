var hasSavePermission = false;
var getUrl = "";
var saveUrl = "";
var delUrl = "";
var patUrl = "";
//SignalR methods
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/CalendarioHub")
    .withHubProtocol(new signalR.protocols.msgpack.MessagePackHubProtocol())
    .build();

connection.on("updateCalendar", function () {
    fetchEventAndRenderCalendar();
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});


function openAddEditForm() {
    if (selectedEvent !== null) {
        $("#hdEventID").val(selectedEvent.eventID);
        $("#txtPaciente").val(selectedEvent.title);
        $("#txtStart").val(selectedEvent.start.format("DD/MM/YYYY HH:mm"));
        $("#chkIsFullDay").prop("checked", selectedEvent.allDay || false);
        $("#chkIsFullDay").change();
        $("#txtEnd").val(selectedEvent.end !== null ? selectedEvent.end.format("DD/MM/YYYY HH:mm") : "");
        $("#txtDescription").val(selectedEvent.description);
        $("#ddThemeColor").val(selectedEvent.color);
    }
    $("#details-event-modal").modal("hide");
    $("#save-event-modal").modal();
}

function saveEvent(data) {
    if (hasSavePermission === "True") {
        $.ajax({
            type: "POST",
            url: saveUrl,
            data: data,
            success: function (data) {
                if (data) {
                    //Refresh the calender
                    fetchEventAndRenderCalendar();
                    $("#save-event-modal").modal("hide");
                }
            },
            error: function () {
                alert("Oops, hemos tenido un problema...");
            }
        });
    }
    else {
        fetchEventAndRenderCalendar();
        $("#save-event-modal").modal("hide");
    }
}

function generateHandlers() {
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
                        fetchEventAndRenderCalendar();
                        $("#details-event-modal").modal("hide");
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
        if ($("#txtPaciente").val().trim() === "") {
            alert("Paciente necesario");
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
        if ($('#ddThemeColor :selected').text() === "") {
            alert("Seleccione tipo de cita");
            return;
        }
        var data = {
            EventID: $("#hdEventID").val(),
            Subject: $("#txtPaciente").val().trim(),
            Start: $("#txtStart").val().trim(),
            End: $("#chkIsFullDay").is(":checked") ? null : $("#txtEnd").val().trim(),
            Description: $("#txtDescription").val(),
            ThemeColor: $("#ddThemeColor").val(),
            IsFullDay: $("#chkIsFullDay").is(":checked")
        };
        saveEvent(data);
        // call function for submit data to the server 
    });
    $("#txtPaciente").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: patUrl,
                data: {
                    Nombre: request.term,
                },
                success: function (data) {
                    response($.map(data, function (item) {
                        return { label: item.nombre };
                    }));
                },
                error: function (error) {
                    alert("Oops, hemos tenido un problema...");
                }
            });
        }
    });
}

function generateCalendar(events) {
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
            $("#details-event-modal #eventTitle").text(calEvent.title);
            var $description = $("<div/>");
            $description.append($("<p/>").html("<b>Empieza:</b>" + calEvent.start.format("DD-MMM-YYYY HH:mm")));
            if (calEvent.end !== null) {
                $description.append($("<p/>").html("<b>Termina:</b>" + calEvent.end.format("DD-MMM-YYYY HH:mm")));
            }
            if (calEvent.description !== null) {
                $description.append($("<p/>").html("<b>Descripcion:</b>" + calEvent.description));
            }
            $("#details-event-modal #pDetails").empty().html($description);

            $("#details-event-modal").modal();
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
            var viewName = $('#calendar').fullCalendar('getView').name;
            if (viewName === "month")
                $('#calendar').fullCalendar('changeView', 'agendaDay', start);
            else
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
            saveEvent(data);
        }
    });
}

function fetchEventAndRenderCalendar() {
    var date = new Date().toISOString();
    var type = "week";
    var calendar = $("#calendar").html;
    if ($.trim($("#calendar").html()) !== "") {
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
            generateCalendar(events);
        },
        error: function (error) {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

function startCalendar(get, save, del, pat, canSave) {
    hasSavePermission = canSave;
    getUrl = get;
    saveUrl = save;
    delUrl = del;
    patUrl = pat;
    fetchEventAndRenderCalendar();
    generateHandlers();
}