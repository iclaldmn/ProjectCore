using Domain.Common;
using Domain.Entities.Ortak;

namespace Domain.Entities.ProjeModul
{
    public class Proje : HistoryEntity
    {
        public string Adi { get; set; }
        public string Aciklama { get; set; }
        public decimal Bedeli { get; set; }
        public decimal IlaveSozlesmeBedeli { get; set; }
        public long IhaleTuruId { get; set; }
        public Deger IhaleTuru { get; set; }
        public long HedefKitleId { get; set; }
        public Deger HedefKitle { get; set; }
        public long ProjeTipiId { get; set; }
        public Deger ProjeTipi { get; set; }
        public long ProjeDurumuId { get; set; }
        public Deger ProjeDurumu { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public decimal ToplamBedel { get; set; }
        public string CreatedByUserId { get; set; }
        public List<ProjeIlceDagilimi> IlceDagilimlari { get; set; }

    }

}

