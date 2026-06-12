using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands;

public class CreateProjeFaaliyetAlaniItemCommand
{
    public long IlceId { get; set; }

    public short Yil { get; set; }

    public byte Ay { get; set; }

    public long KategoriDegerId { get; set; }

    public decimal FaaliyetMiktari { get; set; }
}
