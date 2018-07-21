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
function saveNewRole(url, claims) {
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
            getClaims(claims);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Elimina un rol
function removeRole(url, claims) {
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
            getClaims(claims);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Muestra los claims
function getClaims(url) {
    if (!$("#listaRolesUpd").val()) {
        $("#claims").html("");
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
            $("#claims").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Actualiza los cailms del rol
function updateClaims(url, id) {
    $.ajax({
        url: url,
        data: {
            roleId: $("#roleId").val(),
            policyId: id,
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