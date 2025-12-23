namespace Domain.Common;
    public class HistoryEntity : BaseEntity
    {
        public DateTime OlusturmaZamani { get; set; } = DateTime.UtcNow;
        public DateTime? GuncellemeZamani { get; set; }
        public long OlusturanKullanici { get; set; }
        public long? GuncelleyenKullanici { get; set; }
    }

