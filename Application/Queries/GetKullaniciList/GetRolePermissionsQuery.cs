using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetKullaniciList;

public class GetRolePermissionsQuery : IRequest<Result<List<string>>>
{
    public string RoleName { get; set; } = null!;
}