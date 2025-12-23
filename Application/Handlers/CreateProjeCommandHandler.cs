using Application.Commands;
using AutoMapper;
using Domain.Entities.ProjeModul;
using MediatR;
using Repository.Interfaces;

namespace Application.Handlers;

public class CreateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<CreateProjeCommand, long>
{
    public async Task<long> Handle(
        CreateProjeCommand request,
        CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Proje>(request);

        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        // 🎯 EF'ye new entity bildir
        uow.Repository<Proje>().AddAsync(entity);

        // 🎯 Save (audit otomatik)
        await uow.SaveAsync(cancellationToken);

        return entity.Id;
    }
}
