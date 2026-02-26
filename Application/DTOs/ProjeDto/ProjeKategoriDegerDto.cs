using Application.Common;
using AutoMapper;
using Domain.Entities.Ortak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProjeDto;

public class ProjeKategoriDegerDto : IMapFrom<ProjeKategoriDeger>
{
    public long KategoriId { get; set; }
    public string KategoriAdi { get; set; }

    public long DegerId { get; set; }
    public string DegerAdi { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProjeKategoriDeger, ProjeKategoriDegerDto>()
            .ForMember(d => d.KategoriAdi,
                opt => opt.MapFrom(s => s.Kategori.Adi))
            .ForMember(d => d.DegerAdi,
                opt => opt.MapFrom(s => s.Deger.Adi));
    }
}
