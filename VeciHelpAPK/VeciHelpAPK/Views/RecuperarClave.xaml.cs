using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RecuperarClave : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public RecuperarClave()
        {
            InitializeComponent();
        }

        private async void ButtonRecuperar_Clicked(object sender, EventArgs e)
        {
            if (txtCorreo.Text!=null && txtCorreo.Text!=string.Empty)
            {
                var endPoint = RestService.For<IUsuario>(BaseAddress);

                var response = await endPoint.RecuperarClave(txtCorreo.Text);

                if (response == "Exito")
                {
                    await DisplayAlert("Atención", response, "Aceptar");
                    await Navigation.PopAsync();
                }
                else
                    await DisplayAlert("Atención", response, "Aceptar");
            }
            else
                await DisplayAlert("Atención", "Contraseñas no coinciden", "Aceptar");
        }
    }
}