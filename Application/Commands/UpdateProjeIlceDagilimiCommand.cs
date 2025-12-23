using Application.Common;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;

public class UpdateProjeIlceDagilimiCommand : IRequest<long>, IMapTo<ProjeIlceDagilimi>
{
    public long Id { get; set; } // Güncellenecek dağılım Id

    public decimal IlceyeOdenenBedeli { get; set; }
    public long IlceId { get; set; }

    public long ProjeId { get; set; }
}
