using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Kullanici;

public class AppUser : IdentityUser<long>
{
    public bool IsActive { get; set; } = true;

    //public ICollection<IdentityUserRole<long>> UserRoles { get; set; }
    public long DaireBaskanligiId { get; set; }
    public DaireBaskanligi DaireBaskanligi { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; }
}
