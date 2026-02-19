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

        // 🔥 İlçe dağılımlarının toplamını hesapla
        entity.ToplamBedel = entity.IlceDagilimlari?
            .Sum(x => x.IlceyeOdenenBedeli) ?? 0;

        await uow.Repository<Proje>().AddAsync(entity);
        await uow.SaveAsync(cancellationToken);

        return entity.Id;
    }
}

