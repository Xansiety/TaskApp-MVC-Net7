

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
        // MOSTRAR ERROR
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
        tareasListadoViewModel.cargando(false)
        return
    }

    const tareas = await response.json();
    
    console.log({ tareas });

    tareasListadoViewModel.tareas([]);

    
    tareas.forEach(tarea => {
        tareasListadoViewModel.tareas.push(new tareaElementoListadoViewModel(tarea));
    })

    tareasListadoViewModel.cargando(false);

};