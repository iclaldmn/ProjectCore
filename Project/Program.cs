using Application;
using Domain.Entities.Kullanici;
using Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Contracts;
using Repository.Interfaces;
using System.Text;
using MediatR;


var builder = WebApplication.CreateBuilder(args);

// =======================
// 🔹 CONTROLLERS
// =======================
builder.Services.AddControllers();

// =======================
// 🔹 CORS (REACT İÇİN ŞART)
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5174") // VITE
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// =======================
// 🔹 APPLICATION
// =======================
builder.Services.AddApplication();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationServices();

// =======================
// 🔹 EF CORE
// =======================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =======================
// 🔹 IDENTITY
// =======================
builder.Services.AddIdentity<AppUser, IdentityRole<long>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// =======================
// 🔹 AUTH (JWT)
// =======================
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
        )
    };
});


builder.Services.AddAuthorization();

// =======================
// 🔹 UOW + REPO
// =======================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));



// =======================
// 🔹 SWAGGER + JWT
// =======================
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


var app = builder.Build();

// =======================
// 🔹 MIDDLEWARE
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 🔥 CORS MUTLAKA Authentication'tan ÖNCE
app.UseCors("AllowReact");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ✅ SEED
await SeedAdminUserAsync(app);

app.Run();


// =======================
// 🔹 SEED METHOD
// =======================

static async Task SeedAdminUserAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();

    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<long>>>();

    string[] roles = { "Admin", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<long>(role));
        }
    }

    // 🔹 ADMIN
    var admin = await userManager.FindByNameAsync("admin");
    if (admin == null)
    {
        admin = new AppUser
        {
            UserName = "admin",
            Email = "admin@test.com",
            EmailConfirmed = true
        };

        await userManager.CreateAsync(admin, "Muaz_123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    // 🔹 NORMAL USER
    var user = await userManager.FindByNameAsync("user1");
    if (user == null)
    {
        user = new AppUser
        {
            UserName = "user1",
            Email = "user1@test.com",
            EmailConfirmed = true
        };

        await userManager.CreateAsync(user, "User123");
        await userManager.AddToRoleAsync(user, "User");
    }
}



