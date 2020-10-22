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
    public partial class EnrolarUsuario : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public EnrolarUsuario()
        {
            InitializeComponent();
        }

        private async void ButtonEnrolar_Clicked(object sender, EventArgs e)
        {
            if (correo.Text!=null)
            {
                Administrador adm = new Administrador();
                adm.Correo = correo.Text;
                adm.IdUsuarioCreador = int.Parse(Preferences.Get("idUsuario", null));
                var token = Preferences.Get("token", null);

                var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                var response = await endPoint.EnrolarUsuario(adm);

                await DisplayAlert("Atención", response.ToString(), "Aceptar");

                correo.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Atención", "Favor ingrese un correo valido", "Aceptar");
            }
           
        }
    }
}