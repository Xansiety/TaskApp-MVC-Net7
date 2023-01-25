
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