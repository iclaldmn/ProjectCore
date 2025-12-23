using Application.Commands;
using AutoMapper;
using Domain.Entities.ProjeModul;
using System.Reflection;

namespace Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        CreateMap<CreateProjeCommand, Proje>()
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.OlusturanKullanici, opt => opt.Ignore())
           .ForMember(dest => dest.OlusturmaZamani, opt => opt.Ignore())
           .ForMember(dest => dest.GuncelleyenKullanici, opt => opt.Ignore())
           .ForMember(dest => dest.GuncellemeZamani, opt => opt.Ignore())
           .ForMember(dest => dest.Silindi, opt => opt.Ignore())
           .ForMember(dest => dest.IhaleTuru, opt => opt.Ignore())
           .ForMember(dest => dest.HedefKitle, opt => opt.Ignore())
           .ForMember(dest => dest.ProjeTipi, opt => opt.Ignore())
           .ForMember(dest => dest.ProjeDurumu, opt => opt.Ignore());

        CreateMap<UpdateProjeCommand, Proje>()
            .ForMember(dest => dest.OlusturanKullanici, opt => opt.Ignore())
            .ForMember(dest => dest.OlusturmaZamani, opt => opt.Ignore())
            .ForMember(dest => dest.GuncelleyenKullanici, opt => opt.Ignore())
            .ForMember(dest => dest.GuncellemeZamani, opt => opt.Ignore())
            .ForMember(dest => dest.Silindi, opt => opt.Ignore())
            .ForMember(dest => dest.IhaleTuru, opt => opt.Ignore())
            .ForMember(dest => dest.HedefKitle, opt => opt.Ignore())
            .ForMember(dest => dest.ProjeTipi, opt => opt.Ignore())
            .ForMember(dest => dest.ProjeDurumu, opt => opt.Ignore())
            .ForMember(dest => dest.IlceDagilimlari, opt => opt.Ignore());

        CreateMap<UpdateProjeIlceDagilimiCommand, ProjeIlceDagilimi>();
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.IsClass
                        && !t.IsAbstract
                        && !t.ContainsGenericParameters)
            .ToList();

        foreach (var type in types)
        {
            var mapFrom = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>))
                .Select(i => i.GetGenericArguments().First());

            foreach (var source in mapFrom)
            {
                CreateMap(source, type);
            }

            var mapTo = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapTo<>))
                .Select(i => i.GetGenericArguments().First());

            foreach (var dest in mapTo)
            {
                CreateMap(type, dest);
            }
        }
    }
}
