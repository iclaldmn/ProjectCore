namespace Application.Handlers;

using Application.Commands;
using Application.Helpers;
using Domain.Entities.Kullanici;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class LoginCommandHandler(
    UserManager<AppUser> userManager,
    RoleManager<AppRole> roleManager,
    IConfiguration configuration)
    : IRequestHandler<LoginCommand, Result<LoginResult>>
{
    public async Task<Result<LoginResult>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        // 🔍 Kullanıcı kontrolü
        var user = await userManager.FindByNameAsync(request.UserName);

        if (user == null)
            return Result<LoginResult>.Fail("Kullanıcı adı veya şifre hatalı.");

        if (!user.IsActive)
            return Result<LoginResult>.Fail("Kullanıcı pasif.");

        var passwordValid =
            await userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            return Result<LoginResult>.Fail("Kullanıcı adı veya şifre hatalı.");

        // 🔐 Temel Claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName!),

            // JWT standard claimleri
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

        };
        claims.Add(
            new Claim(
                "DaireBaskanligiId",
                user.DaireBaskanligiId.ToString()
            )
);

        // 🔥 Roller + Permission Claimleri
        var roles = await userManager.GetRolesAsync(user);

        var permissionSet = new HashSet<string>(); // duplicate önler

        foreach (var roleName in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
                continue;

            var roleClaims = await roleManager.GetClaimsAsync(role);

            foreach (var rc in roleClaims
                         .Where(c => c.Type == "permission"))
            {
                if (permissionSet.Add(rc.Value))
                {
                    claims.Add(new Claim("permission", rc.Value));
                }
            }
        }

        // 🔑 JWT oluşturma
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var expires = DateTime.UtcNow.AddMinutes(30);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: expires,
            signingCredentials: creds
        );

        return Result<LoginResult>.Ok(
            new LoginResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires
            },
            "Giriş başarılı."
        );
    }
}