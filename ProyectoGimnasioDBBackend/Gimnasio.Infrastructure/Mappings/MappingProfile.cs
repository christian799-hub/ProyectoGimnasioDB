using AutoMapper;
using Gimnasio.Core.DTOs;
using Gimnasio.Core.Entities;

namespace Gimnasio.Core.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            
            CreateMap<Asistencium, AsistenciaDto>();
            CreateMap<AsistenciaDto, Asistencium>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Horario, opt => opt.Ignore());

            CreateMap<Usuario, UsuarioDto>();
            CreateMap<UsuarioDto, Usuario>()
                .ForMember(dest => dest.Asistencia, opt => opt.Ignore())
                .ForMember(dest => dest.UsuarioMembresia, opt => opt.Ignore());

            CreateMap<Horario, HorarioDto>();
            CreateMap<HorarioDto, Horario>()
                .ForMember(dest => dest.Clase, opt => opt.Ignore())
                .ForMember(dest => dest.Asistencia, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive == 1));

            CreateMap<Clase, ClaseDto>();
            CreateMap<ClaseDto, Clase>()
                .ForMember(dest => dest.Horarios, opt => opt.Ignore())
                .ForMember(dest => dest.Instructor, opt => opt.Ignore());

            CreateMap<UsuarioMembresia, UsuarioMembresiaDto>();
            CreateMap<UsuarioMembresiaDto, UsuarioMembresia>()
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Membresia, opt => opt.Ignore());

            CreateMap<Membresia, MembresiaDto>();
            CreateMap<MembresiaDto, Membresia>()
                .ForMember(dest => dest.UsuarioMembresia, opt => opt.Ignore());
        }
    }
}