using Application.Common;
using Application.Helpers;
using Domain.Entities.ProjeModul;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class CreateProjeFaaliyetAlaniCommand : IRequest<Result<long>>, IMapTo<ProjeFaaliyetAlani>
{
    public long IlceDagilimiId { get; set; }
    public short Yil { get; set; }
    public byte Ay { get; set; }
    public long KategoriDegerId { get; set; }
    public decimal FaaliyetMiktari { get; set; }
}