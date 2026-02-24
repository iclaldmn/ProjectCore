using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class CreateKategoriCommand : IRequest<Result<long>>
{
    public string Adi { get; set; }
}