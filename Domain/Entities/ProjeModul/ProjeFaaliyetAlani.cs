using Domain.Common;
using Domain.Entities.Ortak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProjeModul;

public class ProjeFaaliyetAlani : BaseEntity
{

    public short Yil { get; set; }
    public byte Ay { get; set; }
    public decimal FaaliyetMiktari { get; set; }
    public long KategoriDegerId { get; set; }
    public ProjeKategoriDeger KategoriDeger { get; set; } = null!;
    public long IlceDagilimiId { get; set; }
    public ProjeIlceDagilimi IlceDagilimi { get; set; } = null!;

}