using AutoMapper;
using TaskApp.Entidades;
using TaskApp.Models.DTO;

namespace TaskApp.Servicios.AutoMapperProfiles
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Tarea, TareaDTO>()
            .ForMember(dest => dest.PasosCompletados, opt => opt.MapFrom(src => src.Pasos.Count(x => x.Completado)))
            .ForMember(dest => dest.PasosTotal, opt => opt.MapFrom(src => src.Pasos.Count));
        }
    }
}
