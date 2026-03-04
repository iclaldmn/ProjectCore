using Application.Helpers;
using MediatR;

namespace Application.Commands;

public class LoginCommand : IRequest<Result<LoginResult>>
{
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}