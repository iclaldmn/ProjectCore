using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class UpdateKategoriCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Adi { get; set; }
    public bool Aktif { get; set; }
    public bool ProjedeGoster { get; set; }
    public bool ProjedeZorunlu { get; set; }
}