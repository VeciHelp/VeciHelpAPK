using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using VeciHelpAPK.Security;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SOSRobo : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public string direccionBaseFirebase = "https://fcm.googleapis.com/";
        public string tokenServerFireBase = "AAAAJsP8Zq8:APA91bG4UU1OvFpwQRq3Xl_uw4JC7MMYHcGm8d2mVwkF45Km0L0ztw3Gt1hjbInUweqWd9NqdV8OQmlOxa440aw4snOcUsDq0Ty8eDQg5KSe-IzI1GbLMPDDBlXIo1jTIwG-smyl_eTd";

        public SOSRobo()
        {
            InitializeComponent();
        }

        

        private async void ButtonPropia_Clicked(object sender, EventArgs e)
        {
            RequestAlerta alerta = new RequestAlerta();
            List<string> TokenFireBaseLst;

            var idUsuario = Preferences.Get("Ses_id_Usuario", null);

            alerta.idUsuario = int.Parse(idUsuario);
            alerta.idVecino = int.Parse(idUsuario);

            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });


            var response = await endPoint.AlertaRobo(alerta);


            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();

                TokenFireBaseLst = JsonConvert.DeserializeObject<List<string>>(jsonString);


                string[] theArray = new string[TokenFireBaseLst.Count()];


                for (int i = 0; i < TokenFireBaseLst.Count; i++)
                {
                    theArray[i] = TokenFireBaseLst[i].ToString();
                }

                EnviarNotificaciones(theArray,"Robo","dirijase a su aplicacion VeciHelp en la apestaña alertas para mas información");

            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                await DisplayAlert("Error", "La alerta ya ha sido creada anteriormente", "Ok");
            }
        }

        private async void EnviarNotificaciones(string[] tokenList,string titulo,string mensaje)
        {
            Rootobject fireBaserequest = new Rootobject();
            Notification notifi = new Notification();
            Data data = new Data();

            notifi.body = mensaje;
            notifi.title = "Alerta de "+titulo;

            data.body = "curpo del mensaje";
            data.title = "titulo";
            data.key_1 = "key 1";
            data.key_2 = "key 2";

            fireBaserequest.registration_ids = tokenList;
            fireBaserequest.collapse_key = "type_a";
            fireBaserequest.notification = notifi;
            fireBaserequest.data = data;


            var endPoint = RestService.For<INotificaciones>(new HttpClient(new AuthenticatedHttpClientHandler(tokenServerFireBase)) { BaseAddress = new Uri(direccionBaseFirebase) });

            var JsonObject = JsonConvert.SerializeObject(fireBaserequest);

            var response = await endPoint.Push(JsonObject);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonobj = response.Content.ReadAsStringAsync();

                await DisplayAlert("Mensaje", jsonobj.ToString(), "Ok");
                
            }
        }


        private async void ButtonVecino_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RoboVecino());
        }
    }
}

