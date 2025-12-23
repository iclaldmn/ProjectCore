using Domain.Common;
using Domain.Entities.Ortak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProjeModul
{
    public class ProjeIlceDagilimi:BaseEntity
    {
        public decimal IlceyeOdenenBedeli { get; set; }
        public long IlceId { get; set; }
        public Ilce Ilce { get; set; }
        public long ProjeId { get; set; }
        public Proje Proje { get; set; }

    }
}
