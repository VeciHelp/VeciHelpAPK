using System;
using System.Collections.Generic;
using System.Text;

namespace VeciHelpAPK.Models
{
    public class Administrador
    {
        public int IdUser { get; set; }
        public string Correo { get; set; }
        public string CodigoVerificacion { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Rut { get; set; }
        public char Digito { get; set; }
        public string AntecedentesSalud { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Celular { get; set; }
        public string Direccion { get; set; }
        public string Clave { get; set; }
        public int IdUsuarioCreador { get; set; }
    }
}
