

function agregarNuevaTareaAlListado() {
    tareasListadoViewModel.tareas.push(new tareaElementoListadoViewModel({ id: 0, titulo: '' }));
    $("[name=titulo-tarea]").last().focus();
}

async function manejarFocusOutTituloTarea(tarea) {

    const titulo = tarea.titulo();
    if (!titulo) {
        tareasListadoViewModel.tareas.pop();
        return
    }

    const data = JSON.stringify(titulo);
    const response = await fetch(UrlTareas, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: data
    });

    if (response.ok) {
        const tareaCreada = await response.json();
        tarea.id(tareaCreada.id);
    }
    else {
        manejarErrorApi(response);
    }
}


async function ObtenerTareas() {
    tareasListadoViewModel.cargando(true);

    const response = await fetch(UrlTareas, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!response.ok) {
        tareasListadoViewModel.cargando(false);
        manejarErrorApi(response);
        return
    }

    const tareas = await response.json(); 

    tareasListadoViewModel.tareas([]);

    console.log(tareas)

    tareas.forEach(tarea => {
        console.log(tarea)
        tareasListadoViewModel.tareas.push(new tareaElementoListadoViewModel(tarea));
    })

    tareasListadoViewModel.cargando(false);

};

async function actualizarOrdenTareas() {
    const ids = ObtenerIdsTareas(); 
    await enviarIdsTareasAlBackend(ids);

    const arregloOrdenado = tareasListadoViewModel.tareas.sorted(function (a, b) {
        return ids.indexOf(a.id().toString()) - ids.indexOf(b.id().toString());
    });

    tareasListadoViewModel.tareas([]);
    tareasListadoViewModel.tareas(arregloOrdenado);
     
}


function ObtenerIdsTareas() {
    const ids = $("[name=titulo-tarea]").map(function () {
        return $(this).attr("data-id");
    }).get(); 
    return ids;
}


async function enviarIdsTareasAlBackend(ids) {
    var data = JSON.stringify(ids);
    await fetch(`${UrlTareas}/ordenar`, {
        method: 'POST',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });
}


async function manejarClickTarea(tarea) {
    if (tarea.esNuevo()) {
        return;
    }

    const respuesta = await fetch(`${UrlTareas}/${tarea.id()}`, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
        return;
    }

    const { id, titulo, descripcion, pasos, archivoAjuntos } = await respuesta.json();

    //console.log(reposne);

    tareaEditarVM.id = id;
    tareaEditarVM.titulo(titulo);
    tareaEditarVM.descripcion(descripcion);


    tareaEditarVM.pasos([]);

    pasos.forEach(item => {
        tareaEditarVM.pasos.push(new pasoViewModel({...item, modoEdicion: false}))
    })
     
    prepararArchivoAjuntos(archivoAjuntos);

    modalEditarTareaBootstrap.show();

}


async function manejarCambioEditarTarea() {
    const obj = {
        id: tareaEditarVM.id,
        titulo: tareaEditarVM.titulo(),
        descripcion: tareaEditarVM.descripcion()
    };

    if (!obj.titulo) {
        return;
    }

    await editarTareaCompleta(obj);

    const indice = tareasListadoViewModel.tareas().findIndex(t => t.id() === obj.id);
    const tarea = tareasListadoViewModel.tareas()[indice];
    tarea.titulo(obj.titulo);
}


async function editarTareaCompleta(tarea) {
    const data = JSON.stringify(tarea);

    const respuesta = await fetch(`${UrlTareas}/${tarea.id}`, {
        method: 'PUT',
        body: data,
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (!respuesta.ok) {
        manejarErrorApi(respuesta);
        throw "error";
    }
}


function intentarBorrarTarea(tarea) {
    modalEditarTareaBootstrap.hide();

    confirmarAccion({
        callBackAceptar: () => {
            borrarTarea(tarea);
        },
        callbackCancelar: () => {
            modalEditarTareaBootstrap.show();
        },
        titulo: `¿Desea borrar la tarea ${tarea.titulo()}?`
    })

}

async function borrarTarea(tarea) {
    const idTarea = tarea.id;

    const respuesta = await fetch(`${UrlTareas}/${idTarea}`, {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    if (respuesta.ok) {
        const indice = obtenerIndiceTareaEnEdicion();
        tareasListadoViewModel.tareas.splice(indice, 1);
    }
}

function obtenerIndiceTareaEnEdicion() {
    return tareasListadoViewModel.tareas().findIndex(t => t.id() == tareaEditarVM.id);
}

function obtenerTareaEnEdicion() {
    const indice = obtenerIndiceTareaEnEdicion();
    return tareasListadoViewModel.tareas()[indice];
}


$(function () {
    $("#reordenable").sortable({
        axis: 'y',
        stop: async function () {
            await actualizarOrdenTareas();
        }
    })
})