
using Application.Helpers;
using MediatR;

namespace Application.Commands;

public class DeleteRoleCommand : IRequest<Result<bool>>
{
    public long Id { get; set; }
}