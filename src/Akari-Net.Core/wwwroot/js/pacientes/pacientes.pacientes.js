//Obtiene el grid de pacientes
function getPacientesGrid(url, search, sort, ascending, page, pageSize) {
    $.ajax({
        url: url,
        data: {
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
            Text: search,
            Sort: sort,
            Ascending: ascending,
            Page: page,
            PageSize: pageSize
        },
        type: "post",
        success: function (data) {
            $("#pacientes").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

function toggleAntecedentes() { 
    if ($("#toggleButton").val() === "Mostrar Antecedentes") {
        $(".sensible").show();
        $("#toggleButton").val("Ocultar Antecedentes");
    }
    else {
        $(".sensible").hide();
        $("#toggleButton").val("Mostrar Antecedentes");
    }
}

function openDetailsForm(url,id) {
    $.ajax({
        url: url,
        data: {
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
            id: id            
        },
        type: "post",
        success: function (data) {
            $("#modal-detalles").html(data);
            //alert("hola");
            //$("#hdEventID").val(selectedEvent.eventID);
            //$("#txtPaciente").val(selectedEvent.title);
            //$("#txtStart").val(selectedEvent.start.format("DD/MM/YYYY HH:mm"));
            //$("#chkIsFullDay").prop("checked", selectedEvent.allDay || false);
            //$("#chkIsFullDay").change();
            //$("#txtEnd").val(selectedEvent.end !== null ? selectedEvent.end.format("DD/MM/YYYY HH:mm") : "");
            //$("#txtDescription").val(selectedEvent.description);
            //$("#ddThemeColor").val(selectedEvent.color);
            $("#details-paciente").modal();
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}