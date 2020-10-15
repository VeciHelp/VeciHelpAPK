using Nancy.Json;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
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
        public int participantes { get; set; }
        public string foto { get; set; }



        public static string SendNotification(string[] tokenList, string titulo, string mensaje)
        {
            string webAddr = "https://fcm.googleapis.com/fcm/send";
            string serverKey = "AAAAJsP8Zq8:APA91bG4UU1OvFpwQRq3Xl_uw4JC7MMYHcGm8d2mVwkF45Km0L0ztw3Gt1hjbInUweqWd9NqdV8OQmlOxa440aw4snOcUsDq0Ty8eDQg5KSe-IzI1GbLMPDDBlXIo1jTIwG-smyl_eTd";

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
            httpWebRequest.Method = "POST";

            var payload = new
            {
                registration_ids = tokenList,
                priority = "high",
                content_available = true,
                notification = new
                {
                    body = mensaje,
                    title = titulo
                },
            };
            var serializer = new JavaScriptSerializer();
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = serializer.Serialize(payload);
                streamWriter.Write(json);
                streamWriter.Flush();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            if (httpResponse.StatusCode == HttpStatusCode.OK)
            {
                return "Alerta enviada Correctamente";
            }
            else
                return "Error al enviar alerta";

            //aqui se revisa el resultado de la peticion 
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    result = streamReader.ReadToEnd();
            //}
        }

        public static async Task<string> EnviarAlerta(int idVecino,string tipoAlerta,string datosAlerta)
        {
            string mensaje=string.Empty;
            string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
            RequestAlerta alerta = new RequestAlerta();

            var token = Preferences.Get("Ses_token", null);

            alerta.idUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            alerta.idVecino = idVecino;

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            if (tipoAlerta == "robo")
            {
                var response = await endPoint.AlertaRobo(alerta); 

                mensaje =  await validaRespuesta(response,datosAlerta,"Alerta de Robo");

                return mensaje;

            }
            else if (tipoAlerta == "ayuda")
            {
                var response = await endPoint.AlertaAyuda(alerta);

                 mensaje = await validaRespuesta(response, datosAlerta,"Ayuda!!");

                return mensaje;
            }
            else if (tipoAlerta == "sospecha")
            {
                //var response = await endPoint.(alerta);
            }

            return mensaje;
        }

        public static async Task<string> validaRespuesta(HttpResponseMessage response,string datosAlerta,string tipoAlerta)
        {
            string mensaje=string.Empty;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                var theArray = JsonConvert.DeserializeObject<string[]>(jsonString);

                mensaje= SendNotification(theArray, tipoAlerta, datosAlerta);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                mensaje = "La alerta ya ha sido reportada anteriormente";
            }

            return mensaje;
        }
    }

}
