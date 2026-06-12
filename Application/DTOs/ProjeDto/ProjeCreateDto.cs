using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.ProjeDto
{
    public class ProjeCreateDto
    {
        public string Adi { get; set; }
        public string? Aciklama { get; set; }

        public decimal Bedeli { get; set; }
        public decimal IlaveSozlesmeBedeli { get; set; }

        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }

        public List<ProjeIlceDagilimiCreateDto> IlceDagilimlari { get; set; }
        public List<ProjeKategoriDegerDto> KategoriDegerleri { get; set; } = new();

        public List<ProjeFaaliyetAlaniDto> FaaliyetAlanlari { get; set; } = [];
    }


}
