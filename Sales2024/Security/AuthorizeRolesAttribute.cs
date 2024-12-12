using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Security
{
    public class AuthorizeRolesAttribute : Attribute
    {
        public string[] Roles { get; }

        public AuthorizeRolesAttribute(params string[] roles)
        {
            Roles = roles;
        }
    }
}
