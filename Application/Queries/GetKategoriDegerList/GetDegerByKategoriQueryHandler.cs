using Application.DTOs.KategoriDegerDto;
using Application.DTOs.ProjeDto;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities.Ortak;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;

namespace Application.Queries.GetKategoriDegerList;

public class GetDegerByKategoriQueryHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<GetDegerByKategoriQuery, List<DegerListDto>>
{
    public async Task<List<DegerListDto>> Handle(
        GetDegerByKategoriQuery request,
        CancellationToken cancellationToken)
    {
        return await uow.Repository<Deger>()
            .Query()
            .Where(x => !x.Silindi && x.KategoriId == request.KategoriId)
            .OrderBy(x => x.SiraNo)
            .ProjectTo<DegerListDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}