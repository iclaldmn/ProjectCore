using Application.Commands;
using Domain.Entities.Kullanici;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Handlers;

public class LoginCommandHandler(
    UserManager<AppUser> userManager,
    IConfiguration configuration)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 🔍 Kullanıcıyı bul
        var user = await userManager.FindByNameAsync(request.UserName);

        if (user == null)
            throw new FluentValidation.ValidationException(
                "Kullanıcı adı veya şifre hatalı");

        // 🔐 Şifre kontrolü
        var passwordValid = await userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            throw new FluentValidation.ValidationException("Kullanıcı adı veya şifre hatalı");

        // 🔑 Claims
        var claims = new List<Claim>
        {
            // ✅ ASP.NET Core uyumlu
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),

            // 🔁 Mevcut yapı bozulmasın diye KALIYOR
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 👤 ROLLERİ EKLE (EN KRİTİK KISIM)
        var roles = await userManager.GetRolesAsync(user);

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        // 🔐 JWT Key
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expires = DateTime.UtcNow.AddMinutes(30);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return new LoginResult
        {
            Success = true,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expires
        };
    }
}