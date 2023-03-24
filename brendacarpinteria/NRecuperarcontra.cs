using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brendacarpinteria
{
    public class NRecuperarcontra
    {
        public string recoverpass(string user)
        {
            return new Clsrecuperacion().recoverpassword(user);
        }
    }
}
