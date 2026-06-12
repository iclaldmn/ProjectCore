using Domain.Common;
using Domain.Entities.ProjeModul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Ortak;

public class ProjeKategoriDeger : BaseEntity
{
    public long ProjeId { get; set; }
    public long KategoriId { get; set; }
    public long DegerId { get; set; }
    public Proje Proje { get; set; }
    public Kategori Kategori { get; set; }
    public List<ProjeFaaliyetAlani> ProjeFaaliyetAlanlari { get; set; } = new();
    public Deger Deger { get; set; }

}