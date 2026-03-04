using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class CreateRoleCommand : IRequest<Result<long>>
{
    public string Name { get; set; } = null!;
}