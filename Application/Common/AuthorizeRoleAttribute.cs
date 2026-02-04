using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class AuthorizeRoleAttribute : Attribute
{
    public string[] Roles { get; }

    public AuthorizeRoleAttribute(params string[] roles)
    {
        Roles = roles;
    }
}
