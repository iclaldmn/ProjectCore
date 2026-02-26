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
                    s.KategoriDegerleri.Where(x => !x.Silindi)));
    }
}