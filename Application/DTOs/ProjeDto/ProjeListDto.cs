using Application.Common;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common;
using AutoMapper;
using Domain.Entities.ProjeModul;

namespace Application.DTOs.ProjeDto;
public class ProjeListDto : IMapFrom<Proje>
{
    public long Id { get; set; }
    public string Adi { get; set; }
    public string? Aciklama { get; set; }
    public decimal ToplamBedel { get; set; }

    public decimal TamamlanmaYuzdesi { get; set; }

    public List<ProjeIlceDagilimiDto> IlceDagilimlari { get; set; }

    // 🔥 Dinamik kategori değerleri
    public List<ProjeKategoriDegerDto> KategoriDegerleri { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Proje, ProjeListDto>()
            .ForMember(d => d.IlceDagilimlari,
                opt => opt.MapFrom(s =>
                    s.IlceDagilimlari.Where(x => !x.Silindi)))
            .ForMember(d => d.KategoriDegerleri,
                opt => opt.MapFrom(s =>
                    s.KategoriDegerleri.Where(x => !x.Silindi)))
            .ForMember(
                dest => dest.TamamlanmaYuzdesi,
                opt => opt.MapFrom(src =>
                    src.BitisTarihi <= src.BaslangicTarihi
                        ? 100
                        : DateTime.Today <= src.BaslangicTarihi
                            ? 0
                            : DateTime.Today >= src.BitisTarihi
                                ? 100
                                : Math.Round(
                                    (
                                        (decimal)(DateTime.Today - src.BaslangicTarihi).TotalDays
                                        /
                                        (decimal)(src.BitisTarihi - src.BaslangicTarihi).TotalDays
                                    ) * 100,
                                    2)
                )
            );
    }
}