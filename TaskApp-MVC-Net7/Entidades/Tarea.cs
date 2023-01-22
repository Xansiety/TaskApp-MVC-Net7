using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TaskApp_MVC_Net7.Entidades
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }
        public int Orden { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioCreacionId { get; set; }
        
        // Propiedades de navegación (relación 1 a muchos)
        public IdentityUser UsuarioCreacion { get; set; }
        public List<Paso> Pasos { get; set; }
        public List<ArchivoAjunto> ArchivoAjuntos { get; set; } 
    }
}
