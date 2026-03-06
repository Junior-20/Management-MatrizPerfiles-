"use strict";

// Banco Popular palette colors for JS charts or interactions
const colors = {
    primary: '#003087',
    secondary: '#0057A8',
    accent: '#0078D4',
    lightBg: '#F4F7FB',
    textDark: '#1A1A2E'
};

$(document).ready(function () {
    // Global toastr configuration
    if (typeof toastr !== 'undefined') {
        toastr.options = {
            "closeButton": true,
            "progressBar": true,
            "positionClass": "toast-top-right",
            "timeOut": "3000"
        };
    }

    // Global Ajax Spinner configuration
    $(document).ajaxStart(function () {
        $('#ajax-spinner').show();
    }).ajaxStop(function () {
        $('#ajax-spinner').hide();
    });
});

// Function to confirm deletions with SweetAlert2
function confirmDelete(url, id, dataTableId) {
    Swal.fire({
        title: '¿Está seguro de eliminar este registro?',
        text: "¡Esta acción no se puede revertir!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: colors.primary,
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "DELETE",
                url: url + "/" + id,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        if (dataTableId) {
                            $('#' + dataTableId).DataTable().ajax.reload();
                        } else {
                            location.reload();
                        }
                    } else {
                        toastr.error(data.message);
                    }
                },
                error: function () {
                    toastr.error("Error al procesar la solicitud.");
                }
            });
        }
    })
}
