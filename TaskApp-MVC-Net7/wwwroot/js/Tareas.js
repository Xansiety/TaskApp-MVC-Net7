

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


    tareas.forEach(tarea => {
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


$(function () {
    $("#reordenable").sortable({
        axis: 'y',
        stop: async function () {
            await actualizarOrdenTareas();
        }
    })
})