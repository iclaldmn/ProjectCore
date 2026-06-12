using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Kullanici;

public class DaireBaskanligi : BaseEntity
{
    public string Adi { get; set; }

    public ICollection<AppUser> Kullanicilar { get; set; } = new List<AppUser>();
}
