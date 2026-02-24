using Application.Common;
using Application.Helpers;
using Domain.Entities.Ortak;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class CreateDegerCommand : IRequest<Result<long>>, IMapTo<Deger>
{
    public string Adi { get; set; }
    public string Kodu { get; set; }
    public int SiraNo { get; set; }
    public long KategoriId { get; set; }
}
