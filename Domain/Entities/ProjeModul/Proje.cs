using Domain.Common;
using Domain.Entities.FileMinio;
using Domain.Entities.Ortak;

namespace Domain.Entities.ProjeModul
{
    public class Proje : HistoryEntity
    {
        public string Adi { get; set; }
        public string? Aciklama { get; set; }
        public decimal Bedeli { get; set; }
        public decimal IlaveSozlesmeBedeli { get; set; }
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public decimal ToplamBedel { get; set; }
        public List<ProjeIlceDagilimi> IlceDagilimlari { get; set; } = new();
        public List<ProjeKategoriDeger> KategoriDegerleri { get; set; } = new();

        public ICollection<FileReference> FileReferences { get; set; }

    }

}

