using Application.DTOs.ProjeDto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.GetProjeList;

public class GetProjeByIdQuery : IRequest<ProjeUpdateDto>
{
    public long Id { get; set; }
}
