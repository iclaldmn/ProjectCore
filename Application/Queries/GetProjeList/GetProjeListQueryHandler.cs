using Application.DTOs.ProjeDto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.ProjeModul;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetProjeList;

public class GetProjeListQueryHandler(
IUnitOfWork uow,
IMapper mapper
) : IRequestHandler<GetProjeListQuery, List<ProjeListDto>>
{
    public async Task<List<ProjeListDto>> Handle(
    GetProjeListQuery request,
    CancellationToken cancellationToken)
    {
        return await uow
            .Repository<Proje>()
            .Query().Where(p=>!request.IlceId.HasValue || p.IlceDagilimlari.Any(x=>x.IlceId==request.IlceId.Value))
            .Include(p => p.IlceDagilimlari)
            .ThenInclude(d => d.Ilce)
            .ProjectTo<ProjeListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

}