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
    public partial class Ayuda : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public Ayuda()
        {
            InitializeComponent();
        }

        private async void ButtonPropia_Clicked(object sender, EventArgs e)
        {
            RequestAlerta alerta = new RequestAlerta();

            var idUsuario = Preferences.Get("Ses_id_Usuario", null);

            alerta.idUsuario = int.Parse(idUsuario);
            alerta.idVecino = int.Parse(idUsuario);


            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });


            var response = await endPoint.AlertaAyuda(alerta);


            await DisplayAlert("Alerta", response.ToString(), "Ok");
        }

        private async  void ButtonVecino_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AyudaVecino());
        }
    }
}