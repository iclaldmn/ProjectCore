using Application.Common;
using AutoMapper;
using Domain.Entities.Ortak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.KategoriDegerDto;

public class KategoriListDto : IMapFrom<Kategori>
{
    public long Id { get; set; }
    public string Adi { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Kategori, KategoriListDto>();
    }
}