using Domain.Entities.Kullanici;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class AppDbContext
{
    public DbSet<Proje> Projeler => Set<Proje>();
    public DbSet<Kategori> Kategoriler => Set<Kategori>();
    public DbSet<Deger> Degerler => Set<Deger>();

    public DbSet<AppUser> Kullanicilar => Set<AppUser>();
    public DbSet<AppRole> Roller => Set<AppRole>();
    public DbSet<IdentityUserRole<long>> UserRoles { get; set; }
}