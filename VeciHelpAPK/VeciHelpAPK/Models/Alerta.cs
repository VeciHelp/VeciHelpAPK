using System;
using System.Collections.Generic;
using System.Text;

namespace VeciHelpAPK.Models
{
   public class Alerta
    {
        public int idAlerta { get; set; }
        public DateTime fechaAlerta { get; set; }
        public DateTime horaAlerta { get; set; }
        public string TipoAlerta { get; set; }
        public string nombreGenerador { get; set; }
        public string apellidoGenerador { get; set; }
        public string nombreAyuda { get; set; }
        public string apellidoAyuda { get; set; }
        public string coordenadaSospecha { get; set; }
        public string textoSospecha { get; set; }
        public string direccion { get; set; }
        public string organizacion { get; set; }
        public int participantes { get; set; }
        public string foto { get; set; }
    }
}
