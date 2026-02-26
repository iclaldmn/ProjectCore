using Domain.Common;
using Domain.Entities.ProjeModul;

namespace Domain.Entities.Ortak
{
    public class Deger : BaseEntity
    {
        public string Adi { get; set; }
        public long KategoriId { get; set; }
        public string Kodu { get; set; }
        public int SiraNo { get; set; }
        public Kategori Kategori { get; set; }

    }

}
