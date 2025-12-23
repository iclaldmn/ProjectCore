using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Kullanici;

public class AppUser : IdentityUser<long>
{
    // Şimdilik ekstra alan yok
}
