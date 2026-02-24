using Domain.Common;

namespace Domain.Entities.Ortak
{
    public class Kategori : BaseEntity
    {
        public string Adi { get; set; }
        public bool ProjedeGoster { get; set; }
        public bool ProjedeZorunlu { get; set; }
        public bool Aktif { get; set; }
        public List<Deger> Degerler { get; set; }
    }
}
