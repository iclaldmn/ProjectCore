using Domain.Common;
using Domain.Entities.ProjeModul;

namespace Domain.Entities.Ortak
{
    public class Ilce : BaseEntity
    {
        public string Adi { get; set; }
        public List<ProjeIlceDagilimi> IlceDagilimlari { get; set; }
    }

}
