using Application.DTOs.KullaniciDto;
using Application.Helpers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetKullaniciList;

public class GetRolesQuery : IRequest<Result<List<RoleDto>>>
{
}