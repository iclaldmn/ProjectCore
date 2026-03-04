using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class AssignRoleCommand : IRequest<Result<bool>>
{
    public long UserId { get; set; }
    public List<string> Roles { get; set; } = new();
}

