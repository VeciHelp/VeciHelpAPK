using System;
using System.Collections.Generic;
using System.Text;

namespace VeciHelpAPK.Models
{
    public class Organizacion
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string nroEmergencia { get; set; }
        public int cantMinAyuda { get; set; }
        public string comuna { get; set; }
        public string ciudad { get; set; }
        public string provincia { get; set; }
        public string region { get; set; }
        public string pais { get; set; }
    }
}
