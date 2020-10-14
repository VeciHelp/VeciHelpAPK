using System;
using Newtonsoft.Json;

namespace VeciHelpAPK.Models
{
    public class Login
    {
        [JsonProperty("correo")]
        public string Correo { get; set; }

        [JsonProperty("clave")]
        public string Clave { get; set; }

        [JsonProperty("tokenFireBase")]
        public string TokenFireBase { get; set; }
    }
}
