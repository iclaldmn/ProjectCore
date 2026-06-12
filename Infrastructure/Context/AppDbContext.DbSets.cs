using Domain.Entities.FileMinio;
using Domain.Entities.Kullanici;
using Domain.Entities.Log;
using Domain.Entities.Ortak;
using Domain.Entities.ProjeModul;
using Domain.Entities.FileMinio;
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
    //public DbSet<IdentityUserRole<long>> UserRoles { get; set; }
    public DbSet<AppUserRole> UserRoles { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<FileEntity> Files => Set<FileEntity>();
    public DbSet<FileReference> FileReferences => Set<FileReference>();
    public DbSet<ProjeIlceDagilimi> ProjeIlceDagilimlari { get; set; }
    public DbSet<ProjeFaaliyetAlani> ProjeFaaliyetAlanlari { get; set; }
    public DbSet<DaireBaskanligi> DaireBaskanliklari { get; set; }
    public DbSet<ProjePaydasBirim> ProjePaydasBirimleri { get; set; }
}