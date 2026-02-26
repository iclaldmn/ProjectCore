using Application.DTOs.KategorilerDegerlerDto;
using Application.DTOs.ProjeDto;
using Application.Helpers;
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

public class GetProjeKategorileriQueryHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<GetProjeKategorileriQuery, Result<List<KategoriListDto>>>
{
    public async Task<Result<List<KategoriListDto>>> Handle(
        GetProjeKategorileriQuery request,
        CancellationToken cancellationToken)
    {
        var data = await uow.Repository<Kategori>()
            .Query()
            .Where(x => !x.Silindi && x.Aktif && x.ProjedeGoster)
            .ProjectTo<KategoriListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return Result<List<KategoriListDto>>.Ok(data);
    }
}