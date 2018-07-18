function openInNewTab(url) {
    var win = window.open(url, '_blank');
    win.focus();
}

$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

