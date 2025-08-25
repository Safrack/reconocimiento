let video = null;
let canvas = null;
let context = null;
let stream = null;

$(document).ready(function () {
    $('#btnAgregarVisitante').click(function () {
        $('#formVisitante')[0].reset();
        $('#previewImg').hide();
        $('#modalVisitante').modal('show');
        iniciarCamara();
    });

    $('.btn-cancelar-visitante').click(function () {
        detenerCamara();
        $('#modalVisitante').modal('hide');
    });

    $('#btnCapturar').click(function () {
        context.drawImage(video, 0, 0, canvas.width, canvas.height);
        const dataUrl = canvas.toDataURL('image/png');
        $('#imagenBase64').val(dataUrl);
        $('#previewImg').attr('src', dataUrl).show();
    });

    $('#formVisitante').submit(function (e) {
        e.preventDefault();
        
        if (!$(this).valid()) {
        return; 
        }

        const formData = $(this).serialize();
        console.log("Datos enviados:", formData);

        $.ajax({
            url: '/Visitantes/Create',
            type: 'POST',
            data: formData,
            success: function () {
                alert('Visitante registrado exitosamente');
                detenerCamara();
                $('#modalVisitante').modal('hide');
                location.reload();
            },
            error: function (xhr) {
                console.error("Respuesta completa del error:", xhr);

                let response = xhr.responseJSON;
                let mensaje = "Error al registrar:";

                if (response?.errors) {
                    for (const campo in response.errors) {
                        mensaje += `\n- ${campo}: ${response.errors[campo].join(', ')}`;
                    }
                } else if (response?.error) {
                    mensaje += `\n- ${response.error}`;
                    if (response.inner) {
                        mensaje += `\n- Detalle: ${response.inner}`;
                    }
                } else {
                    mensaje += `\n- Estado HTTP: ${xhr.status} ${xhr.statusText}`;
                }

                alert(mensaje);
            }
        });
    });

    video = document.getElementById('videoCamara');
    canvas = document.getElementById('canvasFoto');
    context = canvas.getContext('2d');

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
            alert('No se pudo acceder a la cámara');
        });
}

function detenerCamara() {
    if (stream) {
        stream.getTracks().forEach(track => track.stop());
    }
}