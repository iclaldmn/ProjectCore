using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class AssignPermissionCommand : IRequest<Result<string>>
{
    public string RoleName { get; set; } = null!;
    public List<string> Permissions { get; set; } = new();
}