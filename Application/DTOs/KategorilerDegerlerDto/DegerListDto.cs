using Application.Common;
using AutoMapper;
using Domain.Entities.Ortak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.KategorilerDegerlerDto;

public class DegerListDto : IMapFrom<Deger>
{
    public long Id { get; set; }
    public string Adi { get; set; }
    public string Kodu { get; set; }
    public int SiraNo { get; set; }
    public long KategoriId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Deger, DegerListDto>();
    }
}