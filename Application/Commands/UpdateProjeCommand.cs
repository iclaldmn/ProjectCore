using Application.Common;
using Application.DTOs.ProjeDto;
using Application.Helpers;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;

public class UpdateProjeCommand : IRequest<Result<long>>, IMapTo<Proje>
{
    public long Id { get; set; } // Güncellenecek Proje Id
    public string Adi { get; set; }
    public string? Aciklama { get; set; }
    public decimal Bedeli { get; set; }
    public decimal IlaveSozlesmeBedeli { get; set; }
    public DateTime BaslangicTarihi { get; set; }
    public DateTime BitisTarihi { get; set; }

    public List<UpdateProjeIlceDagilimiCommand>? IlceDagilimlari { get; set; }
    public List<ProjeKategoriDegerCommand>? KategoriDegerleri { get; set; }
    public List<ProjeFaaliyetAlaniDto>? FaaliyetAlanlari { get; set; }
}