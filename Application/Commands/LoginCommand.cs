using Application.Common;
using Domain.Entities.ProjeModul;
using MediatR;

namespace Application.Commands;

public class LoginCommand : IRequest<LoginResult>
{
    public string UserName { get; set; }
    public string Password { get; set; }
}