using Nancy.Json;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Security;
using Xamarin.Essentials;

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
        public string nroEmergencia { get; set; }
        public int participantes { get; set; }
        public string foto { get; set; }
        public string opcionBoton { get; set; }
        public int idVecino { get; set; }
        public string antecedentesSalud { get; set; }


        public static async Task<string> EnviarAlerta(int idVecino,string tipoAlerta,string datosAlerta,byte[] foto)
        {
            string mensaje=string.Empty;
            string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
            RequestAlerta alerta = new RequestAlerta();

            var token = Preferences.Get("Ses_token", null);

            alerta.idUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            alerta.idVecino = idVecino;
            //utilizo la variable texto que es varchar max , para enviar la foto de la sospecha
            alerta.texto = foto;
            alerta.coordenadas = datosAlerta;

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            try
            {
                if (tipoAlerta == "robo")
                {
                    var response = await endPoint.AlertaRobo(alerta);

                    mensaje = await validaRespuesta(response);

                    return mensaje;

                }
                else if (tipoAlerta == "ayuda")
                {
                    var response = await endPoint.AlertaAyuda(alerta);

                    mensaje = await validaRespuesta(response);

                    return mensaje;
                }
                else if (tipoAlerta == "sospecha")
                {
                    var jsonobjt = JsonConvert.SerializeObject(alerta);
                    var response =  await endPoint.Sospecha(alerta);

                    mensaje = await validaRespuesta(response);

                    return mensaje;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }
            

            return mensaje;
        }

        public static async Task<string> validaRespuesta(HttpResponseMessage response)
        {
            string mensaje=string.Empty;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                mensaje = JsonConvert.DeserializeObject<string>(jsonString);

            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                mensaje = "La alerta ya ha sido reportada anteriormente";
            }

            return mensaje;
        }

    }

}
