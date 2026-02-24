using Application.DTOs.KategoriDegerDto;
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

namespace Application.Queries.GetKategoriDegerList;

public class GetKategorilerQueryHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<GetKategorilerQuery, List<KategoriListDto>>
{
    public async Task<List<KategoriListDto>> Handle(
        GetKategorilerQuery request,
        CancellationToken cancellationToken)
    {
        return await uow.Repository<Kategori>()
            .Query()
            .Where(x => !x.Silindi)
            .ProjectTo<KategoriListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}