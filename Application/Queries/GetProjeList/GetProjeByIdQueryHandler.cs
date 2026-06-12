using Application.DTOs.ProjeDto;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetProjeList;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.ProjeModul;
using Repository.Interfaces;

public class GetProjeByIdQueryHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<GetProjeByIdQuery, ProjeUpdateDto>
{
    public async Task<ProjeUpdateDto> Handle(
        GetProjeByIdQuery request,
        CancellationToken cancellationToken)
    {
        var proje = await uow
            .Repository<Proje>()
            .Query()
            .Where(p => p.Id == request.Id)
            .Include(p => p.IlceDagilimlari)
            .ThenInclude(x => x.FaaliyetAlanlari)
            .ProjectTo<ProjeUpdateDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        return proje;
    }
}