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
        public Ayuda()
        {
            InitializeComponent();
        }

        private async void ButtonPropia_Clicked(object sender, EventArgs e)
        {
            string datosAlerta;
            var nombre = Preferences.Get("Ses_nombre", null);
            var apellido = Preferences.Get("Ses_apellido", null);
            var direccion = Preferences.Get("Ses_direccion", null);

            datosAlerta = nombre + " " + apellido + " Direccion:" + direccion;


            var idVeci = int.Parse(Preferences.Get("Ses_id_Usuario", null));

            var respuesta = await Alerta.EnviarAlerta(idVeci, "ayuda", datosAlerta,null);

            await DisplayAlert("Atención", respuesta, "Aceptar");
            await Navigation.PopAsync();
        }

        private async  void ButtonVecino_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AyudaVecino());
        }

        
    }
}