using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class AlertasActivas : ContentPage
    {
        List<Alerta> AlertaLst = new List<Alerta>();
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";

        public AlertasActivas()
        {
            InitializeComponent();
            CargarAlertas();
        }

        private async void CargarAlertas()
        {
            var token = Preferences.Get("Ses_token", null);
            var idUsuario =int.Parse(Preferences.Get("Ses_id_Usuario", null));

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            var response = await endPoint.AlertasActivas(idUsuario);

            if (response == null)
            {
                LabelAlertasActivas.Text = "Que bien, no hay alertas activas";
                LabelAlertasActivas.FontAttributes = FontAttributes.Bold;
                LabelAlertasActivas.TextColor = Color.White;
            }

            foreach (var item in response)
            {
                if(item == null)
                {
                    LabelAlertasActivas.Text = "Que bien, no hay alertas activas";
                    LabelAlertasActivas.FontAttributes = FontAttributes.Bold;
                    LabelAlertasActivas.TextColor = Color.White;
                }

                Button alertBtn = new Button();

                    if (item.TipoAlerta == "SOS")
                    {
                    alertBtn.BackgroundColor = Color.FromHex("#d92027");
                    }
                    else if (item.TipoAlerta == "Emergencia")
                    {
                    alertBtn.BackgroundColor = Color.FromHex("#ffcd3c");
                    }
                    else if (item.TipoAlerta == "Sospecha")
                    {
                    alertBtn.BackgroundColor = Color.FromHex("#2FBB62");
                    }

                    

                alertBtn.Text = item.direccion + "\n " + item.nombreAyuda + " " + item.apellidoAyuda + " \n " + item.horaAlerta.ToString("HH:mm");
                alertBtn.Clicked += btnAlertas_Click;
                alertBtn.CommandParameter = item;
                alertBtn.TextColor = Color.White;
                StackAlertas.Children.Add(alertBtn);
                alertBtn.FontAttributes =FontAttributes.Bold;
                alertBtn.CornerRadius = 10;


            }
        }

        private async void btnAlertas_Click(object sender, EventArgs args)
        {
            var button = (Button)sender;

            var alert = (Alerta)button.CommandParameter;

            await Navigation.PushAsync(new NotificacionView(alert));
        }
    }
}