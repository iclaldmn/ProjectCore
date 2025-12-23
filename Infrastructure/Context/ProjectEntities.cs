using Domain.Entities.ProjeModul;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public partial class AppDbContext
{
    public DbSet<Proje> Projeler => Set<Proje>();
}



