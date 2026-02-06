using Application.Commands;
using Application.DTOs.ProjeDto;
using AutoMapper;
using Domain.Entities.ProjeModul;
using System.Reflection;

namespace Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Console.WriteLine("🔥 MappingProfile CONSTRUCTOR ÇALIŞTI");
        // 🔥 DTO'ların olduğu assembly'yi NET veriyoruz
        ApplyMappingsFromAssembly(typeof(ProjeListDto).Assembly);

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
                        && !t.ContainsGenericParameters
                        && t.GetInterfaces().Any(i =>
                            i.IsGenericType &&
                            (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ||
                             i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
            .ToList();

        foreach (var type in types)
        {
            //Console.WriteLine($"🟢 Mapping bulundu: {type.FullName}");
            var instance = Activator.CreateInstance(type);

            var method = type.GetMethod("Mapping");

            if (method != null)
            {
                // ✅ DTO kendi mapping'ini tanımlamış
                //Console.WriteLine($"✅ Mapping() çağrılıyor: {type.Name}");
                method.Invoke(instance, new object[] { this });
            }
            else
            {
                // fallback: default mapping
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
}
