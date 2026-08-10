using Application.Common;
using Application.Helpers;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;

public class CreateProjeCommand : IRequest<Result<long>>, IMapTo<Proje>
{
    public string Adi { get; set; }
    public string? Aciklama { get; set; }
    public decimal Bedeli { get; set; }
    public decimal IlaveSozlesmeBedeli { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }
    public decimal NakdiGerceklesmeTutari { get; set; }
    public decimal FizikiGerceklesmeOrani { get; set; }
    public List<CreateProjeIlceDagilimiCommand>? IlceDagilimlari { get; set; } = [];
    public List<ProjeKategoriDegerCommand> KategoriDegerleri { get; set; } = [];
    public List<CreateProjeFaaliyetAlaniItemCommand> FaaliyetAlanlari { get; set; } = [];
    public List<long> PaydasDaireBaskanligiIds { get; set; } = [];


}

