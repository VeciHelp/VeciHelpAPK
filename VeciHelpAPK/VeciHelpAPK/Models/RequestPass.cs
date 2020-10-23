using System;
using System.Collections.Generic;
using System.Text;

namespace VeciHelpAPK.Models
{
    public class RequestPass
    {
        public int id_usuario { get; set; }
        public string claveAntigua { get; set; }
        public string claveNueva { get; set; }

    }
}
