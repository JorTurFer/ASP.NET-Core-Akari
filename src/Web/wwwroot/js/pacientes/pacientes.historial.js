//Obtiene el grid de pacientes
function getHistorialGrid(url,id, search, sort, ascending, page, pageSize) {
    $.ajax({
        url: url,
        data: {
            Id: id,
            Text: search,
            Sort: sort,
            Ascending: ascending,
            Page: page,
            PageSize: pageSize
        },
        type: "post",
        success: function (data) {
            $("#historial").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

function loadRegistry(url,id) {
    $.ajax({
        type: "GET",
        url: url,
        data: {id},
        success: function (data) {
            $('#input-update').val(data);
            $('#registry-id').val(id);
            $('#update-registry').modal();
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

function saveNewRegistry(url, idPaciente, registry) {
    $.ajax({
        type: "POST",
        url: url,
        data:
        {
            idPaciente: idPaciente,
            registry: registry.val()
        },
        success: function () {
            $("#create-registry").modal("hide");
            loadGrid();
            registry.val("");
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
};

function updateRegistry(url, id, registry) {
    $.ajax({
        type: "POST",
        url: url,
        data: {id,registry},
        success: function () {
            $("#update-registry").modal("hide");
            loadGrid();            
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
};