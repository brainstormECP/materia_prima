// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

var buscarum = function (id) {
    $('#umresult').load("/productos/um/" + id);
};
$('#agregar').on("click", function () {
    $('.modal').modal();
});
var id = $('#producto').val()
buscarum(id);
$('#producto').on("change", function () {
    var id = $(this).val();
    buscarum(id);
});
var onSuccess = function (context) {
    $('.modal').modal('hide');
    $('#nuevo_detalle_form').trigger('reset');
    toastr.options = {
        "positionClass": "toast-bottom-right",
        "closeButton": true,
        "showDuration": 600,
        "hideDuration": 300,
    };

    window.setTimeout(function () { toastr.success("Producto agregado satisfactoriamente"); }, 300);
};
var onFailed = function (context) {
    var errors = context.responseJSON
    var s = ""
    $.each(errors, function (index, value) {
        s += "<li>" + value + "</li>";
    });
    $('#error_msg').show(250);
    $('.errors').html($.parseHTML(s));
    toastr.options = {
        "positionClass": "toast-bottom-right",
        "closeButton": true,
        "showDuration": 600,
        "hideDuration": 300,
    };

    window.setTimeout(function () { toastr.error("No se pudo  agregar el producto"); }, 300);
};

var DeleteSuccess = function (context) {
    $('.modal').modal('hide');
    $('#nuevo_detalle_form').trigger('reset');
    toastr.options = {
        "positionClass": "toast-bottom-right",
        "closeButton": true,
        "showDuration": 600,
        "hideDuration": 300,
    };

    window.setTimeout(function () { toastr.success("Producto eliminado satisfactoriamente"); }, 300);
};
var DeleteFail = function (context) {
    $('.modal').modal('hide');
    $('#nuevo_detalle_form').trigger('reset');
    toastr.options = {
        "positionClass": "toast-bottom-right",
        "closeButton": true,
        "showDuration": 600,
        "hideDuration": 300,
    };

    window.setTimeout(function () { toastr.error("No se pudo eliminar este producto"); }, 300);
};


$('.message .close')
    .on('click', function () {
        $('#error_msg').hide("350");
    });
