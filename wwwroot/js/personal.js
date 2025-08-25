$(document).ready(function () {
    let idSeleccionado = 0;

    // Abrir modal agregar personal
    $('#btnAgregar').click(function () {
        $('#formPersonal')[0].reset();
        $('#idPersonal').val(0);
        $('#modalPersonal').modal('show');
    });

    // Cancelar modal personal
    $('.btn-cancelar-personal').click(function () {
        $('#modalPersonal').modal('hide');
    });

    // Limpiar formulario
    $('#limpiar').click(function () {
        $('#formPersonal')[0].reset();
    });

    // Cargar datos para editar personal
    $('.btn-editar').click(function () {
        const fila = $(this).closest('tr');
        $('#idPersonal').val(fila.data('id'));
        $('#nombre').val(fila.find('.nombre').text().split(' ')[0]);
        $('#apellidos').val(fila.find('.nombre').text().split(' ').slice(1).join(' '));
        $('#ci').val(fila.find('.ci').text());
        $('#cargo').val(fila.find('.cargo').text());
        $('#sexo').val(fila.find('.sexo').text());
        $('#fechaNacimiento').val(fila.data('fecha'));
        $('#modalPersonal').modal('show');
    });

    // Guardar datos personal
    $('#formPersonal').submit(function (e) {
        e.preventDefault();
        const datos = $(this).serialize();
        const id = $('#idPersonal').val();
        const url = id && id !== "0" ? '/Personal/Update' : '/Personal/Create';

        $.post(url, datos, function () {
            $('#modalPersonal').modal('hide');
            location.reload();
        }).fail(function (xhr) {
            alert('Error al guardar: ' + xhr.responseText);
        });
    });

    // Abrir modal usuario (crear)
    $('.btn-usuario').click(function () {
        const idPersonal = $(this).closest('tr').data('id');
        $('#idUsuario').val(0);
        $('#idPersonalUsuario').val(idPersonal);
        $('#formUsuario')[0].reset();
        $('#password').attr('required', true);
        $('#modalUsuario').modal('show');
    });

    // Abrir modal usuario (modificar)
    $('.btn-modificar-usuario').click(function () {
        const idPersonal = $(this).data('id');
        $('#idPersonalUsuario').val(idPersonal);
        $.get(`/Usuarios/GetPorPersonal/${idPersonal}`, function (data) {
            $('#idUsuario').val(data.id);
            $('#username').val(data.username);
            $('#password').val('');
            $('#password').removeAttr('required');
            $('select[name="Rol"]').val(data.rol);
            $('select[name="Estado"]').val(data.estado.toString());
            $('#modalUsuario').modal('show');
        }).fail(function () {
            alert("No se pudo cargar el usuario.");
        });
    });

    // Cancelar modal usuario
    $('.btn-cancelar-usuario').click(function () {
        $('#modalUsuario').modal('hide');
    });

    // Mostrar/ocultar contraseña
    $('#togglePassword').click(function () {
        const passInput = $('#password');
        const tipo = passInput.attr('type') === 'password' ? 'text' : 'password';
        passInput.attr('type', tipo);
        $(this).find('i').toggleClass('fa-eye fa-eye-slash');
    });

    // Guardar usuario (crear o editar)
    $('#formUsuario').submit(function (e) {
        e.preventDefault();
        const data = $(this).serialize();
        const idUsuario = $('#idUsuario').val();
        const url = (idUsuario && idUsuario !== "0") ? '/Usuarios/UpdateUser' : '/Usuarios/Guardar';

        $.post(url, data, function () {
            $('#modalUsuario').modal('hide');
            alert('Usuario guardado correctamente');
            location.reload();
        }).fail(function (xhr) {
            alert('Error al guardar usuario: ' + xhr.responseText);
        });
    });
});
