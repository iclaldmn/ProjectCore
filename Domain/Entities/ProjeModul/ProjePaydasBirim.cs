using Domain.Common;
using Domain.Entities.Kullanici;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProjeModul;

public class ProjePaydasBirim : BaseEntity
{
    public long ProjeId { get; set; }
    public Proje Proje { get; set; }

    public long DaireBaskanligiId { get; set; }
    public DaireBaskanligi DaireBaskanligi { get; set; }
}