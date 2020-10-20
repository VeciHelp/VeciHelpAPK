using System;

namespace VeciHelpAPK.Models
{
    public class Usuario
    {
        public int id_Usuario { get; set; }
        public string organizacion { get; set; }
        public string correo { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string rut { get; set; }
        public char digito { get; set; }
        public string Foto { get; set; }
        public string antecedentesSalud { get; set; }
        public DateTime fechaNacimiento { get; set; }
        public int celular { get; set; }
        public string direccion { get; set; }
        public string clave { get; set; }
        public string codigoVerificacion { get; set; }
        public int id_TipoUsuario { get; set; }
        public DateTime fechaRegistro { get; set; }
        public string nombreCreador { get; set; }
        public string numeroEmergencia { get; set; }
        public string token { get; set; }
        public string mensaje { get; set; }
        public int existe { get; set; }
        public string rolename { get; set; }

        public Usuario(string correo , string codigo)
        {
            this.correo = correo;
            this.codigoVerificacion = codigo;
            this.Foto = "vacio";
        }

        public Usuario()
        { 

        }


    }
}
