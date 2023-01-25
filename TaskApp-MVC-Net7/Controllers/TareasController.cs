﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskApp.Entidades;
using TaskApp.Models.DTO;
using TaskApp.Servicios;

namespace TaskApp.Controllers
{
    [Route("api/tareas")]
    [ApiController]
    public class TareasController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IServicioUsuarios servicioUsuarios;

        public TareasController(ApplicationDbContext context, IServicioUsuarios servicioUsuarios)
        {
            this.context = context;
            this.servicioUsuarios = servicioUsuarios;
        }


        [HttpPost]
        public async Task<ActionResult<Tarea>> Post([FromBody] string titulo)
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var existenTareas = await context.Tareas.AnyAsync(x => x.UsuarioCreacionId == usuarioId);

            var ordenMayor = 0;
            if (existenTareas)
            {
                ordenMayor = await context.Tareas.Where(x => x.UsuarioCreacionId == usuarioId)
                    .Select(x => x.Orden).MaxAsync();
            }

            var nuevaTarea = new Tarea
            {
                Titulo = titulo,
                Orden = ordenMayor + 1,
                FechaCreacion = DateTime.UtcNow,
                UsuarioCreacionId = usuarioId
            };

            context.Add(nuevaTarea);
            await context.SaveChangesAsync();

            return nuevaTarea;
        }

        // Obtener todas las tareas del usuario
        [HttpGet]
        public async Task<ActionResult<List<TareaDTO>>> Get()
        {
            var usuarioId = servicioUsuarios.ObtenerUsuarioId();

            var tareas = await context.Tareas.Where(x => x.UsuarioCreacionId == usuarioId)
                .OrderBy(x => x.Orden)
                .Select(x => new TareaDTO
                {
                    Id = x.Id,
                    Titulo = x.Titulo
                })
                .ToListAsync();

            return tareas;
        }

    }
}
