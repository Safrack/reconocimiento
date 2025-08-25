$(document).ready(function () {
    // Filtro por texto
    $("#buscarUsuario").on("keyup", function () {
        const value = $(this).val().toLowerCase();
        $("#tablaUsuarios tbody tr").filter(function () {
            $(this).toggle($(this).text().toLowerCase().indexOf(value) > -1);
        });
    });

    // Filtro por rol
    $("#filtroRol").on("change", function () {
        const rol = $(this).val().toLowerCase();
        $("#tablaUsuarios tbody tr").filter(function () {
            const rolCelda = $(this).find(".td-rol").text().toLowerCase();
            $(this).toggle(rol === "" || rolCelda === rol);
        });
    });

    // Abrir modal editar
    $(document).on("click", ".btn-editar", function () {
        const userId = $(this).data("id");
        $.get(`/Usuarios/GetUser/${userId}`, function (user) {
            $("#userId").val(user.id);
            $("#username").val(user.username);
            $("#rol").val(user.rol);
            $("#modalEditar").modal("show");
        });
    });

    // Limpiar formulario
    $("#limpiar").click(function () {
        $("#username").val("");
        $("#rol").val("administrador");
    });

    // Guardar cambios
    $("#formEditarUsuario").submit(function (e) {
        e.preventDefault();

        const data = {
            Id: $("#userId").val(),
            Username: $("#username").val(),
            Rol: $("#rol").val()
        };

        $.ajax({
            type: "POST",
            url: "/Usuarios/UpdateUser",
            data: data,
            success: function () {
                const id = data.Id;
                $(`#fila-${id} .td-username`).text(data.Username);
                $(`#fila-${id} .td-rol`).text(data.Rol);
                $("#modalEditar").modal("hide");
                mostrarToast("✅ Usuario actualizado correctamente.", "success");
            },
            error: function () {
                Swal.fire("Error", "Ocurrió un error al actualizar el usuario.", "error");
            }
        });
    });

    // Cambiar estado con Swal y actualizar botón dinámicamente
    $(document).on('click', '.btn-toggle-estado', function (e) {
        e.preventDefault();
        const btn = $(this);
        const id = btn.data('id');
        const estadoActual = btn.data('estado') === true || btn.data('estado') === "True";
        const nombre = btn.data('nombre');
        const nuevoEstado = !estadoActual;

        Swal.fire({
            title: '¿Estás seguro?',
            text: `¿Deseas ${nuevoEstado ? "habilitar" : "inhabilitar"} al usuario "${nombre}"?`,
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: `Sí, ${nuevoEstado ? "habilitar" : "inhabilitar"}`,
            cancelButtonText: 'Cancelar',
            customClass: {
                confirmButton: nuevoEstado ? 'swal2-confirm swal2-success' : 'swal2-confirm swal2-danger',
                cancelButton: 'swal2-cancel'
            },
            buttonsStyling: false
        }).then((result) => {
            if (result.isConfirmed) {
                $.post(`/Usuarios/ToggleEstado/${id}`, function () {
                    const mensaje = nuevoEstado
                        ? `✔ Usuario ${nombre} habilitado correctamente.`
                        : `⛔ Usuario ${nombre} inhabilitado correctamente.`;

                    mostrarToast(mensaje, nuevoEstado ? "success" : "danger");
                    // Espera 2 segundos para mostrar el toast antes de recargar
                    setTimeout(() => {
                        location.reload();
                    }, 1000);
                }).fail(function () {
                    mostrarToast("❌ Error al cambiar el estado.", "danger");
                });
            }
        });
    });
});

// Toast personalizado
function mostrarToast(mensaje, tipo = "success") {
    const toast = document.createElement("div");
    toast.className = "toast-message " + (tipo === "danger" ? "toast-danger" : "");
    toast.textContent = mensaje;
    document.getElementById("toast-container").appendChild(toast);
    setTimeout(() => toast.remove(), 4000);
}
