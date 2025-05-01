using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Configuration
{
    public class AuthSettings
    {
        public int Expires { get; set; }
        public string SecretKey { get; set; }
    }
}
