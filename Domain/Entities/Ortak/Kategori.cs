using Domain.Common;

namespace Domain.Entities.Ortak
{
    public class Kategori : BaseEntity
    {
        public string Adi { get; set; }
        public List<Deger> Degerler { get; set; }
    }
}
