
function AgregarCarrito(idP, idE, cant) {
    var parametros = {
        "idP": idP, "idE": idE, "cant": cant
    };
    alert("idP" + idP + "idE" + idE + "cant" + cant);

    $.ajax(
        {
            data: parametros,
            url: 'AgregarCarrito',
            type: 'post',
            beforeSend: function () {
                $("#mensaje").html("Procesando, \n\ espere por favor...");
            },
            success: function (response) {
                alert("Se agregó al carrito ");
            }
        }
    );
}

function aumentar(c, p, t, ca, idP, idE, asc) { // se crean la funcion y se agrega al evento onclick en en la etiqueta button con id aumentar
    cantStock = 10;
    alert('Pro' + idP + 'idE' + idE);
    inicio = document.getElementById(c).value;
    var cantidad = document.getElementById(c).value = ++inicio; //se obtiene el valor del input, y se incrementa en 1 el valor que tenga.

    if (cantidad <= cantStock) {
        document.getElementById(ca).value = cantidad;
        var precio = document.getElementById(p).value;
        var total = cantidad * precio;
        document.getElementById(t).value = total;
        
        var subtotalTemp = document.getElementById("subtotal").value;

        var subtotal = parseFloat(subtotalTemp) + parseFloat(precio);
        document.getElementById("subtotal").value = subtotal;

        var parametros = {
            "idP": idP, "idE": idE, "asc": asc
        };

        $.ajax(
            {
                data: parametros,
                url: 'AumentaDisminuyeCarrito',
                type: 'post',
                beforeSend: function () {
                    $("#mensaje").html("Procesando, \n\ espere por favor...");
                },
                success: function (response) {
                    alert("Se agregó al carrito ");
                }
            }
        );

    } else if (cantidad > cantStock) {
        alert("La cantidad deseada no está disponible");
        document.getElementById(ca).value = document.getElementById(ca).value;
    }
  
}


function disminuir(c, p, t, ca, idP, idE, asc) { // se crean la funcion y se agrega al evento onclick en en la etiqueta button con id disminuir
    inicio = document.getElementById(c).value;
    if (inicio > 1) {

        var cantidad = document.getElementById(c).value = --inicio; //se obtiene el valor del input, y se incrementa en 1 el valor que tenga.
        document.getElementById(ca).value = cantidad;
        var precio = document.getElementById(p).value;
        var total = cantidad * precio;
        document.getElementById(t).value = total;
        var subtotalTemp = document.getElementById("subtotal").value;
        var subtotal = parseFloat(subtotalTemp) - parseFloat(precio);
        document.getElementById("subtotal").value = subtotal;

        var parametros = {
            "idP": idP, "idE": idE, "asc": asc
        };

        $.ajax(
            {
                data: parametros,
                url: 'AumentaDisminuyeCarrito',
                type: 'post',
                beforeSend: function () {
                    $("#mensaje").html("Procesando, \n\ espere por favor...");
                },
                success: function (response) {
                    alert("Se agregó al carrito ");
                }
            }
        );

    }
}

function EliminaCarrito(idP, idE, idF, idT) {
    var parametros = {
        "idP": idP, "idE": idE
    };

   
    $.ajax(
        {
            data: parametros,
            url: 'EliminaCarrito',
            type: 'post',
            success: function (response) {
                alert("Artículo eliminado ");
                $("#" + idF).remove();

                var subtotalTemp = document.getElementById("subtotal").value;
                var subtotal = parseFloat(subtotalTemp) - parseFloat(idT);
                document.getElementById("subtotal").value = subtotal;

            }
        }
    );
}
/*---------------------------------------------- */
