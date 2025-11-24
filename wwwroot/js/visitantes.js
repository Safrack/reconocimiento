let video = null;
let canvas = null;
let context = null;
let stream = null;

$(document).ready(function () {
    // Abrir modal para agregar visitante
    $('#btnAgregarVisitante').click(function () {
        $('#formVisitante')[0].reset();
        $('#previewImg').hide();
        $('#modalVisitante').modal('show');
        iniciarCamara();
    });

    // Cancelar modal y detener cámara
    $('.btn-cancelar-visitante').click(function () {
        detenerCamara();
        $('#modalVisitante').modal('hide');
    });

    // Capturar foto de la cámara
    $('#btnCapturar').click(function () {
        context.drawImage(video, 0, 0, canvas.width, canvas.height);
        const dataUrl = canvas.toDataURL('image/png');
        $('#imagenBase64').val(dataUrl);
        $('#previewImg').attr('src', dataUrl).show();
    });

    // Enviar formulario
    $('#formVisitante').submit(function (e) {
        e.preventDefault();

        if (!$(this).valid()) return;

        const formData = $(this).serialize();
        console.log("Datos enviados:", formData);

        $.ajax({
            url: '/Visitantes/Create',
            type: 'POST',
            data: formData,
            success: function () {
                Swal.fire({
                    icon: 'success',
                    title: 'Éxito',
                    text: 'Visitante registrado exitosamente',
                    confirmButtonText: 'OK'
                }).then(() => {
                    detenerCamara();
                    $('#modalVisitante').modal('hide');
                    location.reload();
                });
            },
            error: function (xhr) {
                console.error("Respuesta completa del error:", xhr);

                let response = xhr.responseJSON;
                let mensaje = "";

                if (response?.errors) {
                    for (const campo in response.errors) {
                        mensaje += `<p><strong>${campo}:</strong> ${response.errors[campo].join(', ')}</p>`;
                    }
                } else if (response?.error) {
                    mensaje += `<p>${response.error}</p>`;
                    if (response.inner) {
                        mensaje += `<p>Detalle: ${response.inner}</p>`;
                    }
                } else {
                    mensaje += `<p>Estado HTTP: ${xhr.status} ${xhr.statusText}</p>`;
                }

                Swal.fire({
                    icon: 'error',
                    title: 'Error al registrar',
                    html: mensaje
                });
            }
        });
    });

    // Inicializar cámara
    video = document.getElementById('videoCamara');
    canvas = document.getElementById('canvasFoto');
    context = canvas.getContext('2d');

    // Inicializar Select2
    $('#selectRecluso').select2({
        dropdownParent: $('#modalVisitante')
    });
});

function iniciarCamara() {
    navigator.mediaDevices.getUserMedia({ video: true })
        .then(function (s) {
            stream = s;
            video.srcObject = stream;
            video.play();
        })
        .catch(function (err) {
            console.error('Error al acceder a la cámara: ', err);
            Swal.fire({
                icon: 'error',
                title: 'Error',
                text: 'No se pudo acceder a la cámara'
            });
        });
}

function detenerCamara() {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
    }
}
