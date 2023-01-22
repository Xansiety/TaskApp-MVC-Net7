namespace TaskApp.Entidades
{
    public class Paso
    {
        public Guid Id { get; set; } // Id con un formato: 00000000-0000-0000-0000-000000000000
        public int TareaId { get; set; }
        // Propiedad de navegación (relación)
        public Tarea Tarea { get; set; }
        public string Descripcion { get; set; }
        public bool Completado { get; set; }
        public int Orden { get; set; }



    }
}
