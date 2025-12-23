using Application.Common;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;

public class UpdateProjeCommand : IRequest<long>, IMapTo<Proje>
{
    public long Id { get; set; } // Güncellenecek Proje Id
    public string Adi { get; set; }
    public string? Aciklama { get; set; }
    public decimal Bedeli { get; set; }
    public decimal IlaveSozlesmeBedeli { get; set; }
    public long IhaleTuruId { get; set; }
    public long HedefKitleId { get; set; }
    public long ProjeTipiId { get; set; }
    public long ProjeDurumuId { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }

    public List<UpdateProjeIlceDagilimiCommand>? IlceDagilimlari { get; set; }
}