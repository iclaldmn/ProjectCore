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
    public string ProjeAdi { get; set; }
    public decimal ToplamBedel { get; set; }

    public void Mapping(Profile profile)
    {
        Console.WriteLine("🔥 ProjeListDto.Mapping ÇALIŞTI");
        profile.CreateMap<Proje, ProjeListDto>();
    }
}