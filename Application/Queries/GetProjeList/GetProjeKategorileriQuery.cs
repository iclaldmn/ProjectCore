using Application.DTOs.KategorilerDegerlerDto;
using Application.DTOs.ProjeDto;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetProjeList;

public record GetProjeKategorileriQuery()
    : IRequest<Result<List<KategoriListDto>>>;