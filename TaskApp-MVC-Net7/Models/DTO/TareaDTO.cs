namespace TaskApp.Models.DTO
{
    public class TareaDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } 
        public string Descripcion { get; set; } 
        public int PasosCompletados { get; set; }   
        public int PasosTotal { get; set; }   
    }
}
