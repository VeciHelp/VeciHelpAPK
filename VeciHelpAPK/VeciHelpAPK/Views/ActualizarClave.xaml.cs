using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Security.Cryptography;
using System.Net.Http;
using VeciHelpAPK.Security;
using System.Net;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActualizarClave : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public Usuario user;
        public ActualizarClave(Usuario usr)
        {
            InitializeComponent();
            this.user = usr;
        }

        private async void ButtonValidar_Clicked(object sender, EventArgs e)
        {
            RequestPass pass = new RequestPass();

            if (txtNewPass.Text == txtNewPass2.Text)
            {
                pass.id_usuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
                pass.claveAntigua = Encriptar(txtOldPass.Text);

                //encripto la clave antes de enviarla
                pass.claveNueva = Encriptar(txtNewPass.Text);

                var token = Preferences.Get("Ses_token", null);

                var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });

                var response = await endPoint.UpdatePass(pass);

                
                if (response.StatusCode==HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    await DisplayAlert("Exito", jsonString, "ok");
                    //await Navigation.PushAsync(new Principal(user));
                    await Navigation.PopAsync();
                }
                else
                    await DisplayAlert("Atención", "Hubo un problema", "Aceptar");
            }
            else
                await DisplayAlert("Atención", "Contraseñas no coinciden", "Aceptar");
        }

        private string Encriptar(string clave)
        {
                using (var sha256 = new SHA256Managed())
                {
                    return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(clave))).Replace("-", "");
                }
        }
    }
}