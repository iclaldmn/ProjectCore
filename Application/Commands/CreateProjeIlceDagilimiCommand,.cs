using Application.Common;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;
    public class CreateProjeIlceDagilimiCommand : IRequest<long>, IMapTo<ProjeIlceDagilimi>
    {
        public decimal IlceyeOdenenBedeli { get; set; }
        public long IlceId { get; set; }
        public long ProjeId { get; set; }
    }

