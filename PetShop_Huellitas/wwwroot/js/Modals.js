document.addEventListener('DOMContentLoaded', function () {
    const deleteButtons = document.querySelectorAll('.eliminar');

    deleteButtons.forEach(button => {
        button.addEventListener('click', function () {
            const url = button.getAttribute('data-url');  // Obtener la URL del atributo data-url

            Swal.fire({
                title: '¿Estás seguro?',
                text: 'No podrás revertir esta acción.',
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Sí, eliminar'
            }).then(result => {
                if (result.isConfirmed) {
                    // Realizar la solicitud AJAX
                    fetch(url, {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'                        }
                    })
                        .then(response => response.json())
                        .then(data => {
                            if (data.success) {
                                Swal.fire('Eliminado', data.message, 'success')
                                    .then(() => {
                                        location.reload(); // Recargar la página para actualizar la tabla
                                    });
                            } else {
                                Swal.fire('Error', data.message, 'error');
                            }
                        })
                        .catch(() => {
                            Swal.fire('Error', 'No se pudo completar la operación.', 'error');
                        });
                }
            });
        });
    });
});
