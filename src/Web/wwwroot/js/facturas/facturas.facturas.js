//Obtiene el grid de referencias
function getFacturasGrid(url, search, sort, ascending, page, pageSize) {
    var year = $("#listayears").val();
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

function getFacturacion(url, year) {
    $.ajax({
        url: url,
        data: {
            __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
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
    var descuento = parseFloat($("#Factura_Descuento").val())/100;
    $("#tableLineas tr").not(':first').not(':last').each(function () {
        sum += getnum($(this).find("td:eq(5)").text());
        function getnum(t) {
            if (isNumeric(t)) {
                return parseFloat(t, 10);
            }
            return 0;
        }
    });
    var final = sum * (1 - descuento);
    var finalRedondeado = Number((final).toFixed(2));
    $("#totalFactura").text(finalRedondeado);
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}


function registerHandlers(patUrl, refUrl,saveUrl,redirectUrl,today) {
    $("body").on("click", "#btnSave", function () {
        var idFactura = $("#idFactura").text();
        //Loop through the Table rows and build a JSON array.
        var lineas = new Array();
        $("#tableLineas TBODY TR").each(function () {
            var row = $(this);
            var linea = {};
            linea.Concepto = row.find("TD").eq(2).html();
            linea.Cantidad = parseInt(row.find("TD").eq(3).html());
            linea.Precio = row.find("TD").eq(4).html().replace(",",".");
            linea.IdLine = parseInt(row.find("TD").eq(0).html());
            linea.IdFactura = idFactura;

            lineas.push(linea);
        });

        var paciente = $("#Paciente").val();
        if (paciente === "") {
            alert("Debes introducir un paciente");
            return;
        }

        var irpf = parseFloat($("#Factura_IRPF").val());
        if (!isNumeric(irpf)) {
            alert("Debes introducir un valor válido en el 'IRPF'");
            return;
        }
        var descuento = parseFloat($("#Factura_Descuento").val());
        if (!isNumeric(descuento)) {
            alert("Debes introducir un valor válido en el 'Descuento'");
            return;
        }

        if (lineas.length === 0) {
            alert("Debes introducir al menos un registro");
            return;
        }

        var factura = {}; 
        factura.IdFactura = idFactura;
        factura.Codigo = $("#codigoFactura").text();

        factura.Fecha = $("#dtp1").data("DateTimePicker").date().toISOString();
        factura.IRPF = $("#Factura_IRPF").val();
        factura.Descuento = $("#Factura_Descuento").val();

        //Send the JSON array to Controller using AJAX.
        $.ajax({
            type: "POST",
            url: saveUrl,
            data: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                paciente: paciente,
                lineas: lineas,
                factura: factura
            },
            success: function () {
                window.location.href = redirectUrl;
            },
            failure: function () {
                alert("Oops, hemos tenido un problema...");
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

        var total = parseFloat(txtTotal.text());
        if (!isNumeric(total) || total === 0.0) {
            alert("La linea que desea introducir no es correcta");
            return;
        }
        var concepto = txtConcepto.val();
        if (concepto === "") {
            alert("Debe introducir un concepto");
            return;
        }

        //Get the reference of the Table's TBODY element.
        var tBody = $("#tableLineas > TBODY")[0];

        //Add Row.
        var row = tBody.insertRow(0);

        //Add idLinea
        cell = $(row.insertCell(-1));
        cell.css("display","none");

        //Add referencia
        cell = $(row.insertCell(-1));
        cell.html(txtReferencia.val());
        //Add concepto
        cell = $(row.insertCell(-1));
        cell.html(txtConcepto.val());
        //Add cantidad.
        cell = $(row.insertCell(-1));
        cell.html(txtCantidad.val());
        //Add precio
        cell = $(row.insertCell(-1));
        cell.html(txtPrecio.val());
        //Add total
        cell = $(row.insertCell(-1));
        cell.html(txtTotal.text());
        //Add total
        cell = $(row.insertCell(-1));
        cell.html('<input type="button" class="btnDel btn btn-danger" value="Borrar" />');

        //Clear the TextBoxes.
        txtReferencia.val("");
        txtConcepto.val("");
        txtCantidad.val("");
        txtPrecio.val("");
        txtTotal.text("");
        calculateTotal();
    });

    $('body').on('click', 'input.btnDel', function () {
        $(this).parents('tr').remove();
        calculateTotal();
    });

    function calculateLineTotal() {
        if ($("#txtCantidad").val() === "") {
            return;
        }

        if ($("#txtPrecio").val() === "") {
            return;
        }

        var precio = parseFloat($("#txtPrecio").val());
        var cantidad = parseInt($("#txtCantidad").val());

        if (Number.isFinite(cantidad) && Number.isFinite(precio)) {
            $("#txtTotal").text(Number((cantidad * precio).toFixed(2)));
        } else {
            $("#txtTotal").text(0);
        }
    }

    $('body').on('keyup', 'input#txtCantidad', function () {
        if ($("#txtCantidad").val() === "") {
            return;
        }
        var cantidad = parseInt($("#txtCantidad").val());
        if (cantidad != $("#txtCantidad").val()) {
            alert("Debe introducir un valor válido");
            return;
        }
        
        calculateLineTotal();
    });

    $('body').on('keyup', 'input#txtPrecio', function () {
        if ($("#txtPrecio").val() === "") {
            return;
        }

        var precio = parseFloat($("#txtPrecio").val());
        if (precio != $("#txtPrecio").val()) {
            alert("Debe introducir un valor válido");
            return;
        }

        calculateLineTotal();
    });


    $('body').on('keyup', 'input#Factura_IRPF', function () {
        calculateTotal();
    });

    $('body').on('keyup', 'input#Factura_Descuento', function () {
        calculateTotal();
    });

    $("#dtp1").datetimepicker({
        format: "DD/MM/YYYY"
    });

    $("#dtp1").data("DateTimePicker").date(today);

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
                        $("#txtConcepto").val(data[0].concepto);
                        $("#txtPrecio").val(data[0].precio);
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
                    $("#txtConcepto").val(data[0].concepto);
                    $("#txtPrecio").val(data[0].precio);
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