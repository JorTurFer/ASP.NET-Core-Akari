//Obtiene el grid de usuarios
function GetUsersGrid(url, search, sort, ascending, page, pageSize, ) {
    $.ajax({
        url: url,
        data: {
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