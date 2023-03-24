using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace brendacarpinteria
{
    public class Dcorreosoporte : Clsrecuperacion
    {
        public Dcorreosoporte()
        {
            remitentecorreo = "josedavidmendezrivera@gmail.com";
            pass = "opwchgsnuxlqqkbw";
            host = "smtp.gmail.com";
            port = 587;
            ssl = true; 
            initializestmpcliente();

        }
    }
}
