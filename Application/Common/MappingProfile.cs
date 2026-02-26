using Application.Commands;
using Application.DTOs.ProjeDto;
using AutoMapper;
using Domain.Entities.ProjeModul;
using System.Reflection;

namespace Application.Common;

using AutoMapper;
using System.Reflection;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // 🔹 DTO assembly scan
        ApplyMappingsFromAssembly(typeof(ProjeListDto).Assembly);

        // 🔹 CREATE -> ENTITY
        CreateMap<CreateProjeCommand, Proje>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.OlusturanKullanici, opt => opt.Ignore())
            .ForMember(dest => dest.OlusturmaZamani, opt => opt.Ignore())
            .ForMember(dest => dest.GuncelleyenKullanici, opt => opt.Ignore())
            .ForMember(dest => dest.GuncellemeZamani, opt => opt.Ignore())
            .ForMember(dest => dest.Silindi, opt => opt.Ignore())
            .ForMember(dest => dest.ToplamBedel, opt => opt.Ignore())
            .ForMember(dest => dest.IlceDagilimlari, opt => opt.Ignore())
            .ForMember(dest => dest.KategoriDegerleri, opt => opt.Ignore());

        // 🔹 UPDATE -> ENTITY
        CreateMap<UpdateProjeCommand, Proje>()
            .ForMember(dest => dest.OlusturanKullanici, opt => opt.Ignore())
            .ForMember(dest => dest.OlusturmaZamani, opt => opt.Ignore())
            .ForMember(dest => dest.GuncelleyenKullanici, opt => opt.Ignore())
            .ForMember(dest => dest.GuncellemeZamani, opt => opt.Ignore())
            .ForMember(dest => dest.Silindi, opt => opt.Ignore())
            .ForMember(dest => dest.ToplamBedel, opt => opt.Ignore())
            .ForMember(dest => dest.IlceDagilimlari, opt => opt.Ignore())
            .ForMember(dest => dest.KategoriDegerleri, opt => opt.Ignore());

        // 🔹 İlçe mapping
        CreateMap<UpdateProjeIlceDagilimiCommand, ProjeIlceDagilimi>();
    }

    private void ApplyMappingsFromAssembly(Assembly assembly)
    {
        var types = assembly.GetExportedTypes()
            .Where(t => t.IsClass && !t.IsAbstract && !t.ContainsGenericParameters)
            .Where(t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                (i.GetGenericTypeDefinition() == typeof(IMapFrom<>) ||
                 i.GetGenericTypeDefinition() == typeof(IMapTo<>))))
            .ToList();

        foreach (var type in types)
        {
            try
            {
                var instance = Activator.CreateInstance(type);
                var method = type.GetMethod("Mapping");

                if (method != null)
                {
                    method.Invoke(instance, new object[] { this });
                }
                else
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
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Mapping yüklenemedi: {type.Name} - {ex.Message}");
            }
        }
    }
}