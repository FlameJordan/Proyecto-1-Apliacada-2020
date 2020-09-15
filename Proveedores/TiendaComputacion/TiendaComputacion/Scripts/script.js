function obtenerKey() {
    $.ajax(
        {
            url: "/Home/obtenerKey",
            type: "POST",
            contentType: false,
            beforeSend: function () {
                $("#mensaje").html("Procesando, \n\ espere por favor...");
            },
            success: function (data) {
                $("#keyrecibida").html(data);
                $("#mensaje").html(" ");
            }
        }
    );


}