using Application.Commands;
using Application.Helpers;
using AutoMapper;
using Domain.Entities.ProjeModul;
using MediatR;
using Repository.Interfaces;

namespace Application.Handlers;

public class CreateProjeCommandHandler(
    IUnitOfWork uow,
    IMapper mapper
) : IRequestHandler<CreateProjeCommand, Result<long>>
{
    public async Task<Result<long>> Handle(
        CreateProjeCommand request,
        CancellationToken cancellationToken)
    {
        if (request.IlceDagilimlari == null || !request.IlceDagilimlari.Any())
            return Result<long>.Fail("En az bir ilçe eklenmelidir");

        var duplicateCheck = request.IlceDagilimlari
            .GroupBy(x => x.IlceId)
            .Any(g => g.Count() > 1);

        if (duplicateCheck)
            return Result<long>.Fail("Aynı ilçe birden fazla eklenemez");

        var entity = mapper.Map<Proje>(request);

        // 🔥 Toplam hesap (Bedeli + İlave)
        entity.ToplamBedel = entity.Bedeli + entity.IlaveSozlesmeBedeli;

        var dagilimToplam = entity.IlceDagilimlari
            .Where(x => !x.Silindi)
            .Sum(x => x.IlceyeOdenenBedeli);

        if (dagilimToplam != entity.ToplamBedel)
            return Result<long>.Fail(
                "İlçe dağılım toplamı proje toplamına eşit olmalıdır");

        await uow.Repository<Proje>().AddAsync(entity);
        await uow.SaveAsync(cancellationToken);

        return Result<long>.Ok(entity.Id, "Proje başarıyla oluşturuldu");
    }
}

