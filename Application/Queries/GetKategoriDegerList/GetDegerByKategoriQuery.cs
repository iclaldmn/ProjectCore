using Application.DTOs.KategoriDegerDto;
using Application.DTOs.ProjeDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetKategoriDegerList;
public record GetDegerByKategoriQuery(long KategoriId)
    : IRequest<List<DegerListDto>>;

