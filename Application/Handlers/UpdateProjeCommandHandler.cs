using Application.Commands;
using Application.Validators;
using AutoMapper;
using Domain.Entities.ProjeModul;
using MediatR;
using Repository.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace Application.Handlers;

public class UpdateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<UpdateProjeCommand, long>
{
    public async Task<long> Handle(
        UpdateProjeCommand request,
        CancellationToken cancellationToken)
    {
        // 🎯 1) Entity + lookup navigation'ları yükle
        var entity = await uow.Repository<Proje>()
            .GetWithIncludeAsync(
                x => x.Id == request.Id,
                cancellationToken,
                x => x.ProjeTipi,
                x => x.ProjeDurumu,
                x => x.IhaleTuru,
                x => x.HedefKitle
            );

        // ❌ Validation burada YOK
        // UpdateProjeCommandValidator + Pipeline bunu zaten yapıyor

        // 🎯 2) Scalar + FK alanları map et
        mapper.Map(request, entity);

        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        // 🎯 3) EF'ye entity güncellendiğini bildir
        uow.Repository<Proje>().Update(entity);

        // ℹ️ Navigation’lar lookup olduğu için
        // sadece Id (FK) üzerinden güncellenir

        // 🎯 4) Save
        await uow.SaveAsync(cancellationToken);

        return entity.Id;
    }
}



