//Obtiene el grid de referencias
function getFacturasGrid(url, search, sort, ascending, page, pageSize,year) {
    $.ajax({
        url: url,
        data: {
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
            Text: search,
            Sort: sort,
            Ascending: ascending,
            Page: page,
            PageSize: pageSize,
            Year: year
        },
        type: "post",
        success: function (data) {
            $("#facturas").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}