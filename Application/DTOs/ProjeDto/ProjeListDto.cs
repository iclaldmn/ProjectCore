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
public class ProjeListDto: IMapFrom<Proje>
{
    public long Id { get; set; }
    public string Adi { get; set; }
    public string Aciklama { get; set; }
    public decimal ToplamBedel { get; set; }

    public long ProjeDurumuId { get; set; }
    public string ProjeDurumuAdi { get; set; }

    public long ProjeTipiId { get; set; }
    public string ProjeTipiAdi { get; set; }

    public long IhaleTuruId { get; set; }
    public string IhaleTuruAdi { get; set; }

    public long HedefKitleId { get; set; }
    public string HedefKitleAdi { get; set; }

    public List<ProjeIlceDagilimiDto> IlceDagilimlari { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Proje, ProjeListDto>()
            .ForMember(d => d.ProjeDurumuAdi,
                opt => opt.MapFrom(s => s.ProjeDurumu.Adi))
            .ForMember(d => d.ProjeTipiAdi,
                opt => opt.MapFrom(s => s.ProjeTipi.Adi))
            .ForMember(d => d.IhaleTuruAdi,
                opt => opt.MapFrom(s => s.IhaleTuru.Adi))
            .ForMember(d => d.HedefKitleAdi,
                opt => opt.MapFrom(s => s.HedefKitle.Adi));
    }
}