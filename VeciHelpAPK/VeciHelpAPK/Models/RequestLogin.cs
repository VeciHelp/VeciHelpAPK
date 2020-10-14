using System;

namespace VeciHelpAPK.Models
{
    public class RequestLogin
    {
        public int existe { get; set; }
        public int id_Usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string roleName { get; set; }
        public string token { get; set; }
        public string mensaje { get; set; }
    }
}
