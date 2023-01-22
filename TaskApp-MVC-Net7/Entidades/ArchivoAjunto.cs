using Microsoft.EntityFrameworkCore;

namespace TaskApp_MVC_Net7.Entidades
{
    public class ArchivoAjunto
    {
        public Guid Id { get; set; }
        public int TareaId { get; set; }
        public Tarea Tarea { get; set; }

        [Unicode] // no acepta string con caracteres especiales
        public string Url { get; set; }
        public string Titulo { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCreacion { get; set; } 
    }
}
