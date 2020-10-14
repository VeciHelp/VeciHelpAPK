using Refit;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Principal : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        Usuario user = new Usuario();

        

        public Principal(Usuario usr)
        {
            InitializeComponent();
            ButtonAdministrador.IsEnabled = false;
            ButtonAdministrador.IsVisible = false;
            user = usr;

            var rolename = Preferences.Get("Ses_rolename",null);
            if (rolename=="Administrador")
            {
                ButtonAdministrador.IsEnabled = true;
                ButtonAdministrador.IsVisible = true;
            }
        }


        private async void ButtonSOSRobo_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SOSRobo());
        }

        private async void ButtonAyuda_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ayuda());
        }

        private async void ButtonSospecha_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Sospecha());
        }

        private async void ButtonAdministrador_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdministradorView());
        }

        private async void ButtonAlertas_Clicked(object sender, EventArgs e)
        {
                    await Navigation.PushAsync(new AlertasActivas());
        }

        private async void ButtonmisDatos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Crear_Usuario(user));
        }
    }
}