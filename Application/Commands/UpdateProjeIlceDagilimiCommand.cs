using Application.Common;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;

public class UpdateProjeIlceDagilimiCommand : IRequest<long>, IMapTo<ProjeIlceDagilimi>
{
    public long? Id { get; set; } 

    public decimal IlceyeOdenenBedeli { get; set; }
    public long IlceId { get; set; }

    public long ProjeId { get; set; }
}
