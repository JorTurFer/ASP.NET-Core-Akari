//Lanzo la llamada para obtener los roles
function GetRoles(url) {
    $.ajax({
        url: url,
        data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        type: "post",
        success: function (data) {
            $("#content").html(data);
            $("#btnUsers").removeClass("active");
            $("#btnRoles").addClass("active");
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Guarda un nuevo rol
function saveNewRole(url, permission) {
    if (!$("#roleName").val()) {
        alert("Introduce un nombre para el rol");
        return;
    }
    $.ajax({
        url: url,
        data: {
            roleName: $("#roleName").val(),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        },
        type: "post",
        success: function (data) {
            $("#listaRolesUpd").append(new Option($("#roleName").val(), data, false, true));
            $("#listaRolesDel").append(new Option($("#roleName").val(), data, false, true));
            getPermission(permission);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Elimina un rol
function removeRole(url, permission) {
    if (!$("#listaRolesDel").val()) {
        alert("Para poder borrar un rol primero tienes que seleccionarlo");
        return;
    }
    $.ajax({
        url: url,
        data: {
            roleId: $("#listaRolesDel").val(),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        },
        type: "post",
        success: function (data) {
            $(".desplegables option[value=" + $("#listaRolesDel").val() + "]").remove();
            getPermission(permission);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Muestra los permission
function getPermission(url) {
    if (!$("#listaRolesUpd").val()) {
        $("#permission").html("");
        return;
    }
    $.ajax({
        url: url,
        data: {
            roleId: $("#listaRolesUpd").val(),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        },
        type: "post",
        success: function (data) {
            $("#permission").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Actualiza los cailms del rol
function updatePermission(url, id) {
    $.ajax({
        url: url,
        data: {
            roleId: $("#roleId").val(),
            permissionId: id,
            set: $("#" + id).is(':checked'),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        },
        type: "post",
        //success: function (data) {

        //},
        error: function () {
            $("#" + id).checked = !$("#" + id).is(':checked');
            alert("Oops, hemos tenido un problema...");
        }
    });
}