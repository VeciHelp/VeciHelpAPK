using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginView : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public Login log;

        public LoginView()
        {
            InitializeComponent();
        }

        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            if (correo.Text!=null && correo.Text != "" && clave.Text!=null &&  clave.Text != "")
            {
                Usuario usr = new Usuario();
                log = new Models.Login();
                log.Correo = correo.Text;
                log.Clave = Encriptar(clave.Text);
                log.TokenFireBase = Preferences.Get("TokenFirebase", null);

                string mensaje = "Usuario o Contraseña inválida";
                          

                var endPoint = RestService.For<ILogin>(BaseAddress);

                var response= await endPoint.PostLogin(log);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    //obtengo el contenido del HttpResponseMessage como string
                    var jsonString = await response.Content.ReadAsStringAsync();

                    //convierto el contenido de json al objeto usuario
                    usr = JsonConvert.DeserializeObject<Usuario>(jsonString);

                    //guardo los datos del objeto usuario en variables locales de la aplicacion
                    GuardarDatosSesion(usr);
                    //redirecciona a la pagina principal
                    await Navigation.PushAsync(new Principal(usr));
                }
                else if(response.StatusCode == HttpStatusCode.NotFound)
                {
                    await DisplayAlert("Atención", mensaje, "Aceptar");
                }
            }
            else
            {
                await DisplayAlert("Atención", "Ingrese datos validos", "Aceptar");
            }
        }

        private void GuardarDatosSesion(Usuario usr)
        {
            Preferences.Set("Ses_token", usr.token);
            Preferences.Set("Ses_id_Usuario", usr.id_Usuario.ToString());
            Preferences.Set("Ses_correo", usr.correo);
            Preferences.Set("Ses_nombre", usr.nombre);
            Preferences.Set("Ses_apellido", usr.apellido);
            Preferences.Set("Ses_rut", usr.rut);
            Preferences.Set("Ses_digito", usr.digito);
            Preferences.Set("Ses_Foto", usr.Foto);
            Preferences.Set("Ses_antecedentesSalud", usr.antecedentesSalud);
            Preferences.Set("Ses_fechaNacimiento", usr.fechaNacimiento.ToString());
            Preferences.Set("Ses_celular", usr.celular);
            Preferences.Set("Ses_direccion", usr.direccion);
            Preferences.Set("Ses_numeroEmergencia", usr.numeroEmergencia);
            Preferences.Set("Ses_rolename", usr.rolename);
        }

        private async void ButtonRegistrate_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CodigoValidacion());
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