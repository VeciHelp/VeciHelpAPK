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
    public partial class RoboVecino : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public RoboVecino()
        {
            InitializeComponent();
            cargaUsuarios();
        }

        private async void cargaUsuarios()
        {
            List<Usuario> usrLst = new List<Usuario>();

            var token = Preferences.Get("Ses_token", null);
            var idUsuario = Preferences.Get("Ses_id_Usuario", null);
            var nombre = Preferences.Get("Ses_nombre", null);

            var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            var response = await endPoint.GetListaVecinos(int.Parse(idUsuario.ToString()));

            usrLst = response;

            foreach (var item in usrLst)
            {
                Button btnCliente = new Button();
                btnCliente.Text = item.direccion + " " + item.nombre + " " + item.apellido;
                btnCliente.ClassId = item.id_Usuario.ToString();
                btnCliente.Clicked += BtnCliente_Click;
                btnCliente.BackgroundColor = Color.FromHex("#3b83bd");
                btnCliente.TextColor = Color.White;

                sl.Children.Add(btnCliente);
            }

            if (usrLst.Count == 0)
            {
                await DisplayAlert("Alerta", "El Usuario no posee vecinos enrolados", "Ok");
                await Navigation.PushAsync(new SOSRobo());
            }
        }


        private async void BtnCliente_Click(object sender, EventArgs args)
        {
            RequestAlerta alerta = new RequestAlerta();


            var button = (Button)sender;
            var IdVecino =button.ClassId;
            var idUsuario = Preferences.Get("Ses_id_Usuario", null);

            alerta.idUsuario = int.Parse(idUsuario);
            alerta.idVecino = int.Parse(IdVecino);


            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });


            var response = await endPoint.AlertaRobo(alerta);


            await DisplayAlert("Alerta", response.ToString(), "Ok");
        }
    }
}