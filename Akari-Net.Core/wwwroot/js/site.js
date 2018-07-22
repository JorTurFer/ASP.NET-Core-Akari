//Abre una nueva ventana
function openInNewTab(url) {
    var win = window.open(url, '_blank');
    win.focus();
}
//Muestra los tooltip
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});
//Llamada AJAX para enviar correo de verificacion desde el profile
function sendVerificationMail(url) {
    $.ajax({
        url: url,
        data: {
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
            Email: $("#Email").val(),
        },
        type: "post",
        success: function (data) {
            $("body").html(data);
        },
        error: function () {
            alert("Oops, hemos tenido un problema...");
        }
    });
}

