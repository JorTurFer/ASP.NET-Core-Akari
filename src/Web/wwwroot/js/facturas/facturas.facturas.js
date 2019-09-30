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

function calculateTotal() {
    var sum = 0;
    var irpf = parseFloat($("#Factura_IRPF").val())/100;
    var descuento = parseFloat($("#Factura_Descuento").val())/100;
    $("#tableLineas tr").not(':first').not(':last').each(function () {
        sum += getnum($(this).find("td:eq(4)").text());
        function getnum(t) {
            if (isNumeric(t)) {
                return parseInt(t, 10);
            }
            return 0;
            function isNumeric(n) {
                return !isNaN(parseFloat(n)) && isFinite(n);
            }
        }
    });
    var conDescuento = sum * (1 - descuento);
    var final = conDescuento * (1 + irpf);
    var finalRedondeado = Number((final).toFixed(2));
    $("#totalFactura").text(finalRedondeado);
}


function registerHandlers(patUrl, refUrl,saveUrl,redirectUrl) {
    $("body").on("click", "#btnSave", function () {
        //Loop through the Table rows and build a JSON array.
        var lineas = new Array();
        $("#tableLineas TBODY TR").each(function () {
            var row = $(this);
            var linea = {};
            linea.idReferencia = parseInt(row.find("TD").eq(0).html());
            linea.Cantidad = parseInt(row.find("TD").eq(3).html());
            linea.IdLinea = -1;
            linea.IdFactura = -1;

            lineas.push(linea);
        });

        var factura = {}; 
        factura.Codigo = $("#Factura_Codigo").val();
        factura.Fecha = $("#Factura_Fecha").val();
        factura.IRPF = $("#Factura_IRPF").val();
        factura.Descuento = $("#Factura_Descuento").val();

        //Send the JSON array to Controller using AJAX.
        $.ajax({
            type: "POST",
            url: saveUrl,
            data: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                paciente: $("#Paciente").val(),
                lineas: lineas,
                factura: factura
            },
            success: function (r) {
                window.location.href = redirectUrl;
            },
            failure: function (response) {
                alert(response);
            }
        });
    });

    $("body").on("click", "#btnAdd", function () {
        //Reference the Name and Country TextBoxes.
        var txtReferencia = $("#txtReferencia");
        var txtConcepto = $("#txtConcepto");
        var txtCantidad = $("#txtCantidad");
        var txtPrecio = $("#txtPrecio");
        var txtTotal = $("#txtTotal");
        var idReferencia = $("#idReferencia");
        if (txtTotal.text() === "") {
            alert("Selecciona una referencia");
            return;
        }

        //Get the reference of the Table's TBODY element.
        var tBody = $("#tableLineas > TBODY")[0];

        //Add Row.
        var row = tBody.insertRow(0);

        //Add idRefrencia
        var cell = $(row.insertCell(-1));
        cell.html(idReferencia.text());
        cell.css("display", "none");
        //Add referencia
        cell = $(row.insertCell(-1));
        cell.html(txtReferencia.val());
        //Add concepto
        cell = $(row.insertCell(-1));
        cell.html(txtConcepto.text());
        //Add cantidad.
        cell = $(row.insertCell(-1));
        cell.html(txtCantidad.val());
        //Add precio
        cell = $(row.insertCell(-1));
        cell.html(txtPrecio.text());
        //Add total
        cell = $(row.insertCell(-1));
        cell.html(txtTotal.text());
        //Add total
        cell = $(row.insertCell(-1));
        cell.html('<input type="button" class="btnDel btn btn-danger" value="Borrar" />');

        //Clear the TextBoxes.
        idReferencia.text("");
        txtReferencia.val("");
        txtConcepto.text("");
        txtCantidad.val("");
        txtPrecio.text("");
        txtTotal.text("");
        calculateTotal();
    });

    $('body').on('click', 'input.btnDel', function () {
        $(this).parents('tr').remove();
        calculateTotal();
    });

    $('body').on('keyup', 'input#txtCantidad', function () {
        var cantidad = parseFloat($("#txtCantidad").val());
        var precio = parseFloat($("#txtPrecio").text());
        if (Number.isFinite(cantidad) && Number.isFinite(precio)) {
            $("#txtTotal").text(cantidad * precio);
        } else {
            $("#txtTotal").text(0);
        }
    });


    $('body').on('keyup', 'input#Factura_IRPF', function () {
        calculateTotal();
    });

    $('body').on('keyup', 'input#Factura_Descuento', function () {
        calculateTotal();
    });

    $("#Paciente").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: patUrl,
                data: {
                    Nombre: request.term,
                },
                success: function (data) {
                    response($.map(data,
                        function (item) {
                            return { label: item.nombre };
                        }));
                },
                error: function (error) {
                    alert("Oops, hemos tenido un problema...");
                }
            });
        }
    });

    $("#txtReferencia").autocomplete({
        source: function (request, response) {
            $.ajax({
                type: "POST",
                url: refUrl,
                data: {
                    Nombre: request.term,
                },
                success: function (data) {
                    if (data.length === 1) {
                        var cantidad = $("#txtCantidad").val();
                        if (cantidad === "") {
                            cantidad = 1;
                            $("#txtCantidad").val(cantidad);
                        }
                        $("#txtConcepto").text(data[0].concepto);
                        $("#txtPrecio").text(data[0].precio);
                        $("#txtTotal").text(data[0].precio * cantidad);
                        $("#idReferencia").text(data[0].idReferencia);
                    } else {
                        response($.map(data,
                            function (item) {
                                return { label: item.identificador };
                            }));
                    }
                    
                },
                error: function (error) {
                    alert("Oops, hemos tenido un problema...");
                }
            });
        },
        select: function (e, ui) {

            $.ajax({
                type: "POST",
                url: refUrl,
                data: {
                    Nombre: ui.item.value,
                },
                success: function (data) {
                    var cantidad = $("#txtCantidad").val();
                    if (cantidad === "") {
                        cantidad = 1;
                        $("#txtCantidad").val(cantidad);
                    }
                    $("#txtConcepto").text(data[0].concepto);
                    $("#txtPrecio").text(data[0].precio);
                    $("#txtTotal").text(data[0].precio * cantidad);
                    $("#idReferencia").text(data[0].idReferencia);
                    
                },
                error: function (error) {
                    alert("Oops, hemos tenido un problema...");
                }
            });
        },
    });
}