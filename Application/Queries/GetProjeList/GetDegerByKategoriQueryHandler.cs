using Application.DTOs.ProjeDto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Ortak;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetProjeList;

public class GetDegerByKategoriQueryHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<GetDegerByKategoriQuery, List<LookupDto>>
{
    public async Task<List<LookupDto>> Handle(
        GetDegerByKategoriQuery request,
        CancellationToken cancellationToken)
    {
        return await uow
            .Repository<Deger>()
            .Query()
            .Where(x => x.KategoriId == request.KategoriId && !x.Silindi)
            .ProjectTo<LookupDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
