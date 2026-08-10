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
    public decimal NakdiGerceklesmeTutari { get; set; }
    public decimal FizikiGerceklesmeOrani { get; set; }
    public List<ProjeIlceDagilimiDto> IlceDagilimlari { get; set; }
    public List<ProjeKategoriDegerDto> KategoriDegerleri { get; set; } = new();
    public List<ProjeFaaliyetAlaniDto> FaaliyetAlanlari { get; set; } = [];
    public List<long> PaydasDaireBaskanligiIds { get; set; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Proje, ProjeUpdateDto>()
       .ForMember(
           dest => dest.IlceDagilimlari,
           opt => opt.MapFrom(src =>
               src.IlceDagilimlari
                   .Where(x => !x.Silindi)
           )
       )
       .ForMember(
           dest => dest.FaaliyetAlanlari,
           opt => opt.MapFrom(src =>
               src.IlceDagilimlari
                   .Where(x => !x.Silindi)
                   .SelectMany(x => x.FaaliyetAlanlari)
           )
       )
       .ForMember(
           dest => dest.PaydasDaireBaskanligiIds,
           opt => opt.MapFrom(src =>
               src.PaydasBirimler
                   .Where(x => !x.Silindi)
                   .Select(x => x.DaireBaskanligiId)
           )
       );
    }
}
