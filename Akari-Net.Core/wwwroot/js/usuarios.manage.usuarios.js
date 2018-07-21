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
function GetUsersGrid(url, search, sort, ascending, page, pageSize) {
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
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}