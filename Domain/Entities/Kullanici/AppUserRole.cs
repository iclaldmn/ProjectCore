using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Kullanici;

public class AppUserRole : IdentityUserRole<long>
{
    public AppUser User { get; set; }
    public AppRole Role { get; set; }
}