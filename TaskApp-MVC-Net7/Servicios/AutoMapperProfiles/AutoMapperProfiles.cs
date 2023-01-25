using AutoMapper;
using TaskApp.Entidades;
using TaskApp.Models.DTO;

namespace TaskApp.Servicios.AutoMapperProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tarea, TareaDTO>();
        }
    }
}
