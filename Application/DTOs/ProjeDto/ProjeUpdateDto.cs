using Application.Common;
using AutoMapper;
using Domain.Entities.ProjeModul;

namespace Application.DTOs.ProjeDto;

public class ProjeUpdateDto : IMapFrom<Proje>
{
    public long Id { get; set; }
    public string Adi { get; set; }
    public string Aciklama { get; set; }
    public decimal Bedeli { get; set; }
    public decimal IlaveSozlesmeBedeli { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public decimal ToplamBedel { get; set; }
    public List<ProjeIlceDagilimiDto> IlceDagilimlari { get; set; }
    public List<ProjeKategoriDegerDto> KategoriDegerleri { get; set; } = new();
    public List<ProjeFaaliyetAlaniDto> FaaliyetAlanlari { get; set; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Proje, ProjeUpdateDto>()
            .ForMember(
                dest => dest.IlceDagilimlari,
                opt => opt.MapFrom(src =>
                    src.IlceDagilimlari
                       .Where(x => x.Silindi == false)
                )
            )
            .ForMember(
                dest => dest.FaaliyetAlanlari,
                opt => opt.MapFrom(src =>
                    src.IlceDagilimlari
                       .Where(x => !x.Silindi)
                       .SelectMany(x => x.FaaliyetAlanlari)
                )
            );
    }
}
