using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class UpdateDegerCommand : IRequest<Result<long>>
{
    public long Id { get; set; }
    public string Adi { get; set; }
    public string Kodu { get; set; }
    public int SiraNo { get; set; }
}