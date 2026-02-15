using Application.Common;
using AutoMapper;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProjeDto;

public class ProjeIlceDagilimiDto : IMapFrom<ProjeIlceDagilimi>
{
    public decimal IlceyeOdenenBedeli { get; set; }
    public long IlceId { get; set; }
    //public string Ilce { get; set; }
    public long ProjeId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProjeIlceDagilimi, ProjeIlceDagilimiDto>();
    }

    //public void Mapping(Profile profile)
    //{
    //    profile.CreateMap<ProjeIlceDagilimi, ProjeIlceDagilimiDto>()
    //        .ForMember(
    //            dest => dest.Ilce,
    //            opt => opt.MapFrom(src => src.Ilce.Adi)
    //        );
    //}
}