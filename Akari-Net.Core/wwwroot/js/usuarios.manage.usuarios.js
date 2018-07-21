//Lanzo la llamada para obtener usuarios
function GetUsers(url) {
    $.ajax({
        url: url,
        data: { __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val() },
        type: "post",
        success: function (data) {
            $("#content").html(data);
            $("#btnUsers").addClass("active");
            $("#btnRoles").removeClass("active");
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

//Obtiene el grid de usuarios
function getUsersGrid(url, search, sort, ascending, page, pageSize) {
    $("#usuarios").hide("fast");
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
            $("#usuarios").html(data);
            $("#usuarios").show("fast");
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

//Actualiza el rol del usuario
function updateUserRole(url, userId,roleName) {
    $.ajax({
        url: url,
        data: {
            id: userId,
            roleName: roleName,
            set: $("#" + roleName).is(':checked'),
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
        },
        type: "post",
        //success: function (data) {

        //},
        error: function () {
            $("#" + roleName).checked = !$("#" + roleName).is(':checked');
            alert("Oops, hemos tenido un problema...");
        }
    });
}
//Obtiene los roles del usuario
function getUserRoles(url, search, sort, ascending, page, pageSize,userId) {
    $.ajax({
        url: url,
        data: {
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
            Text: search,
            Sort: sort,
            Ascending: ascending,
            Page: page,
            PageSize: pageSize,
            id : userId
        },
        type: "post",
        success: function (data) {
            $("#usuarios").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}