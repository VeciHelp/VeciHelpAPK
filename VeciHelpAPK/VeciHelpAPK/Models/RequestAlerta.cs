using System;
using System.Collections.Generic;
using System.Text;

namespace VeciHelpAPK.Models
{
    public class RequestAlerta
    {
        public int idUsuario { get; set; }
        public int idVecino { get; set; }
        public int idAlerta { get; set; }
        public string coordenadas { get; set; }
        public byte[] texto { get; set; }
    }
}
