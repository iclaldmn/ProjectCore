using Application.Commands;
using Application.Common;
using AutoMapper;
using Domain.Entities.ProjeModul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProjeDto;

public class ProjeFaaliyetAlaniDto : IMapFrom<ProjeFaaliyetAlani>
{
    public long IlceId { get; set; }

    public short Yil { get; set; }

    public byte Ay { get; set; }

    public long KategoriDegerId { get; set; }

    public decimal FaaliyetMiktari { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ProjeFaaliyetAlani, ProjeFaaliyetAlaniDto>()
            .ForMember(
                d => d.IlceId,
                o => o.MapFrom(s => s.IlceDagilimi.IlceId)
            );
    }

}
