
async function manejarErrorApi(respuesta) {
    let mensajeError = "";

    if (respuesta.status === 400) {
        mensajeError = await respuesta.text();
    }
    else if (respuesta.status === 404) {
        mensajeError = recursoNoEncontrado;
    }
    else if (respuesta.status === 500) {
        mensajeError = errorInesperado;
    }

    mostrarMensajeError(mensajeError);

}


function mostrarMensajeError(mensaje) {

    swal.fire({
        icon: 'error',
        title: 'Error!!',
        text: mensaje
    });
}


function confirmarAccion({ callBackAceptar, callbackCancelar, titulo }) {
    Swal.fire({
        title: titulo || '¿Realmente deseas hacer esto?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sí',
        focusConfirm: true
    }).then((resultado) => {
        if (resultado.isConfirmed) {
            callBackAceptar();
        } else if (callbackCancelar) {
            // El usuario ha presionado el botón de cancelar
            callbackCancelar();
        }
    })
}


function descargarArchivo(url, nombre) {
    var link = document.createElement('a');
    link.download = nombre;
    link.target = "_blank";
    link.href = url;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}