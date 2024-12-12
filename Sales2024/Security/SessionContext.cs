using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


namespace Security
{
    public static class SessionContext
    {
        private static readonly AsyncLocal<string> _username = new AsyncLocal<string>();
        private static readonly AsyncLocal<string> _rol = new AsyncLocal<string>();

        public static string Username
        {
            get => _username.Value;
            set => _username.Value = value;
        }

        public static string Rol
        {
            get => _rol.Value;
            set => _rol.Value = value;
        }
    }
}
