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

    public long ProjeDurumuId { get; set; }
    public long ProjeTipiId { get; set; }
    public long IhaleTuruId { get; set; }
    public long HedefKitleId { get; set; }
    public decimal ToplamBedel { get; set; }

    public List<ProjeIlceDagilimiDto> IlceDagilimlari { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Proje, ProjeUpdateDto>().ForMember(
        dest => dest.IlceDagilimlari,
        opt => opt.MapFrom(src =>
            src.IlceDagilimlari
               .Where(x => x.Silindi == false)
        )
    );
    }
}
