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
        var query = uow
            .Repository<Proje>()
            .Query()
            .Where(p => !p.Silindi);

        if (request.IlceId.HasValue)
        {
            query = query.Where(p =>
                p.IlceDagilimlari.Any(d =>
                    !d.Silindi &&
                    d.IlceId == request.IlceId.Value
                )
            );
        }

        return await query
            .ProjectTo<ProjeListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}