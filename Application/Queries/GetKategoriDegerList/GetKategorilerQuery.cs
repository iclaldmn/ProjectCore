using Application.DTOs.KategorilerDegerlerDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetKategoriDegerList;

public class GetKategorilerQuery : IRequest<List<KategoriListDto>>
{
}
