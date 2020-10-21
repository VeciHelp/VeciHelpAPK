using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodigoValidacion : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public CodigoValidacion()
        {
            InitializeComponent();
        }

        private async void ButtonValidar_Clicked(object sender, EventArgs e)
        {
            if (correo.Text != null && correo.Text != "" && codigoVerificacion.Text != null && codigoVerificacion.Text != "")
            {
                Usuario usr = new Usuario(correo.Text, codigoVerificacion.Text);


                var endPoint = RestService.For<IUsuario>(BaseAddress);

               // var jsonas = JsonConvert.SerializeObject(usr);

                var response = await endPoint.ValidarCodigo(usr);

                response = response.Replace("\"", "");

                if (response.Equals("Codigo Validado Correctamente"))
                {
                    await DisplayAlert("Exito", response, "Ok");

                    await Navigation.PushAsync(new Crear_Usuario(usr,1));
                }
                else
                    await DisplayAlert("Error", response, "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Ingrese datos validos", "Ok");
            }

        }
    }
}