﻿//Obtiene el grid de referencias
function getReferenciasGrid(url, search, sort, ascending, page, pageSize) {
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
            $("#referencias").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}