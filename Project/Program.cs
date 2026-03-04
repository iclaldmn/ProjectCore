using Application;
using Application.Common;
using Domain.Entities.Kullanici;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Contracts;
using Repository.Interfaces;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Host.UseDefaultServiceProvider(options =>
//{
//    options.ValidateScopes = true;
//    options.ValidateOnBuild = true;
//});

#region SERVICES

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact", policy =>
    {
        policy.WithOrigins("http://localhost:5175")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddApplication();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()   // 🔥 BU YOKTU
.AddDefaultTokenProviders();                // 🔥 BU DA YOKTU

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});

builder.Services.AddAuthorization(options =>
{
    // 🔥 Tüm permission’lar için otomatik policy
    foreach (var permission in Permissions.GetAll())
    {
        options.AddPolicy(permission,
            policy => policy.RequireClaim("permission", permission));
    }
});

//var testPermissions = Permissions.GetAll();
//Console.WriteLine("PERMISSION COUNT: " + testPermissions.Count);

//foreach (var p in testPermissions)
//{
//    Console.WriteLine(p);
//}

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

#endregion

var app = builder.Build();

#region MIDDLEWARE

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReact");
app.UseCors("AllowReact");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion


await SeedUsersAndRolesAsync(app);

app.Run();

#region SEED

static async Task SeedUsersAndRolesAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

    string[] roles = { "Admin", "User" };

    foreach (var roleName in roles)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole<long>(roleName));
        }
    }

    // 🔥 ADMIN ROLE → TÜM PERMISSION'LAR
    var adminRole = await roleManager.FindByNameAsync("Admin");

    if (adminRole != null)
    {
        var existingClaims = await roleManager.GetClaimsAsync(adminRole);
        var allPermissions = Permissions.GetAll();

        foreach (var permission in allPermissions)
        {
            if (!existingClaims.Any(c =>
                c.Type == "permission" &&
                c.Value == permission))
            {
                await roleManager.AddClaimAsync(
                    adminRole,
                    new Claim("permission", permission));
            }
        }
    }

    // 🔹 USER ROLE → SADECE VIEW
    var userRole = await roleManager.FindByNameAsync("User");

    if (userRole != null)
    {
        var existingClaims = await roleManager.GetClaimsAsync(userRole);

        var userPermissions = new[]
        {
            Permissions.Proje.View
        };

        foreach (var permission in userPermissions)
        {
            if (!existingClaims.Any(c =>
                c.Type == "permission" &&
                c.Value == permission))
            {
                await roleManager.AddClaimAsync(
                    userRole,
                    new Claim("permission", permission));
            }
        }
    }

    // 🔹 ADMIN USER
    var adminEmail = "admin@test.com";
    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        admin = new AppUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true,
            IsActive = true
        };

        await userManager.CreateAsync(admin, "Muaz_123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    // 🔹 NORMAL USER
    var userEmail = "user1@test.com";
    var user = await userManager.FindByEmailAsync(userEmail);

    if (user == null)
    {
        user = new AppUser
        {
            UserName = userEmail,
            Email = userEmail,
            EmailConfirmed = true,
            IsActive = true
        };

        await userManager.CreateAsync(user, "User123");
        await userManager.AddToRoleAsync(user, "User");
    }
}

#endregion