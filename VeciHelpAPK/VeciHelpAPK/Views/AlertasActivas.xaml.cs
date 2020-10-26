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
                Image caraFeliz = new Image();
                caraFeliz.Source = "happy.png";
                LabelAlertasActivas.Text = "Que bien, no hay alertas activas";
                LabelAlertasActivas.FontAttributes = FontAttributes.Bold;
                LabelAlertasActivas.TextColor = Color.White;
                LabelAlertasActivas.HorizontalTextAlignment = TextAlignment.Center;
                
            }

            foreach (var item in response)
            {
                Button btnAlertas = new Button();
               
                    if (item.TipoAlerta == "SOS")
                    {
                        btnAlertas.BackgroundColor = Color.FromHex("#d92027");
                        btnAlertas.TextColor = Color.White;
                }
                    else if (item.TipoAlerta == "Emergencia")
                    {
                        btnAlertas.BackgroundColor = Color.FromHex("#ffcd3c");
                        btnAlertas.TextColor = Color.FromHex("#242525");
                }
                    else if (item.TipoAlerta == "Sospecha")
                    {
                        btnAlertas.BackgroundColor = Color.FromHex("#2FBB62");
                        btnAlertas.TextColor = Color.White;

                }
                
                btnAlertas.Text = item.TipoAlerta.ToUpper() + " " + item.horaAlerta.ToString("HH:mm") + "\n " + item.direccion + "\n " + item.nombreAyuda + " " + 
                    item.apellidoAyuda;
                    btnAlertas.ClassId = item.idAlerta.ToString();
                    btnAlertas.CornerRadius = 25;
                    btnAlertas.Clicked += btnAlertas_Click;
                    btnAlertas.CommandParameter = item;
                    //btnAlertas.TextColor = Color.White;
                    btnAlertas.FontSize = 17;

                    StackAlertas.Children.Add(btnAlertas);
            }
        }

        private async void btnAlertas_Click(object sender, EventArgs args)
        {
            var button = (Button)sender;

            var alert = (Alerta)button.CommandParameter;

            await Navigation.PushAsync(new NotificacionView(alert.idAlerta));
            
        }
    }
}