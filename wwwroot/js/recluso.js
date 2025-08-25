$(document).ready(function () {

    // Mostrar modal para agregar
    $('#btnAgregar').click(function () {
        limpiarFormulario();
        $('#modalReclusoLabel').text("Agregar Recluso");
        $('#modalRecluso').modal('show');
    });

    // Editar
    $(document).on('click', '.btn-editar', function () {
        const id = $(this).data('id');
        $.get(`/Reclusos/GetRecluso/${id}`, function (data) {
            $('#idRecluso').val(data.idRecluso);
            $('#nombreCompleto').val(data.nombreCompleto);
            $('#ci').val(data.ci);
            $('#edad').val(data.edad);
            $('#delito').val(data.delito);
            $('#fechaIngreso').val(data.fechaIngreso);
            $('#pabellonId').val(data.idPabellon);
            $('#modalReclusoLabel').text("Editar Recluso");
            $('#modalRecluso').modal('show');
        });
    });

    // Guardar (Create o Update)
    $('#formRecluso').submit(function (e) {
        e.preventDefault();
        const id = $('#idRecluso').val();
        const form = document.getElementById("formRecluso");
        const formData = new FormData(form);
        const url = id ? '/Reclusos/Update' : '/Reclusos/Create';

        // DEBUG: Mostrar lo que se envía al servidor
        console.log("Datos enviados:");
        for (const [key, value] of formData.entries()) {
            console.log(`${key}: ${value}`);
        }

        if (!id) {
            formData.delete("IdRecluso");
        }

        $.ajax({
            url: url,
            type: 'POST',
            data: formData,
            contentType: false,
            processData: false,
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Guardado exitoso',
                    showConfirmButton: false,
                    timer: 1500
                }).then(() => location.reload());
            },
            error: function (xhr) {
                console.error("Respuesta del servidor:", xhr.responseText);
                if (xhr.responseJSON?.errores) {
                    Swal.fire("Errores", xhr.responseJSON.errores.join(", "), "error");
                } else {
                    Swal.fire("Error", "No se pudo guardar", "error");
                }
            }
        });
    });

    // Botón limpiar formulario
    $('#limpiar').click(function () {
        limpiarFormulario();
    });

    // Botón cancelar
    $('.btn-cancelar').click(function () {
        $('#modalRecluso').modal('hide');
    });

    // Función para limpiar el formulario
    function limpiarFormulario() {
        $('#formRecluso')[0].reset();
        $('#idRecluso').val('');
        $('#pabellonId').val('');
    }

});

// Filtro en tiempo real por nombre, CI, pabellón y fechas
$('#buscarRecluso, #filtroPabellon, #filtroDesde, #filtroHasta').on('input change', function () {
    filtrarReclusos();
});

function filtrarReclusos() {
    const texto = $('#buscarRecluso').val().toLowerCase();
    const pabellonFiltro = $('#filtroPabellon').val();
    const desde = $('#filtroDesde').val();
    const hasta = $('#filtroHasta').val();

    $('#tbodyReclusos tr').each(function () {
        const nombre = $(this).find('.nombre').text().toLowerCase();
        const ci = $(this).find('.ci').text().toLowerCase();
        const pabellon = $(this).find('.pabellon').text();
        const fecha = $(this).find('.fechaIngreso').text();

        const coincideTexto = nombre.includes(texto) || ci.includes(texto);
        const coincidePabellon = !pabellonFiltro || pabellon === pabellonFiltro;
        const coincideFecha = (!desde || fecha >= desde) && (!hasta || fecha <= hasta);

        if (coincideTexto && coincidePabellon && coincideFecha) {
            $(this).show();
        } else {
            $(this).hide();
        }
    });
}
