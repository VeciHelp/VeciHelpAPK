using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using VeciHelpAPK.Security;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace VeciHelpAPK.Views
{
    public partial class NotificacionView : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        Alerta alerta = new Alerta();

        public NotificacionView(int idAlerta)
        {
            InitializeComponent();
            LblDetalle.IsVisible = false;
            ActualizarAlerta(idAlerta);
           // RefreshCommand = new Command(async () => await LoadPublications());
        }


        private async void ButtonAcudir_Clicked(object sender, EventArgs e)
        {
            var IdUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });
            RequestAlerta aler = new RequestAlerta();

            aler.idUsuario = IdUsuario;
            aler.idAlerta = alerta.idAlerta;

           
            if(alerta.opcionBoton == "Finalizar")
            {
                var response = await endPoint.FinalizarAlerta(aler);
                var jsonstring = JsonConvert.SerializeObject(aler);
                response = response.Replace("\"", "");
                await DisplayAlert("MUCHAS GRACIAS !!!", "Agradecemos su participación en la comunidad", "Aceptar");



                //elimino 2 ventanas

                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                await Navigation.PopAsync();

                GlobalClass.varGlobal = false;
            }
            else if(alerta.opcionBoton == "Acudir")
            {
                
                var response = await endPoint.AcudirAlerta(aler);
                GlobalClass.varGlobal = true;
                response = response.Replace("\"", "");
                if (response.Equals("No puede acudir a mas de una alerta"))
                {
                    await DisplayAlert("Atención", response, "Aceptar");
                }
                else
                {
                    await DisplayAlert("Atención", "Su participación se representa con un ticket en el listado de alertas", "Aceptar");

                }

                //elimino 2 ventanas

                Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
                await Navigation.PopAsync();
            }

        }
        private async void ActualizarAlerta(int idAlerta)
        {
            var IdUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            var response = await endPoint.AlertaById(idAlerta, IdUsuario);

            alerta = response;

            LlenarCamposDeAlerta();
        }

        private void LlenarCamposDeAlerta()
        {
          
            LblNombre.Text = alerta.nombreAyuda + " " + alerta.apellidoAyuda + "\n\n" + alerta.direccion + "\n\n" + alerta.antecedentesSalud;
            //LabelAntecedentesSalud.Text = alerta.antecedentesSalud;

            //LblDireccion.Text = alerta.direccion;
            LblTipoAlerta.Text =   alerta.TipoAlerta.ToUpper();
            


            if (LblTipoAlerta.Text == "SOS")
            {
                LblTipoAlerta.TextColor = Color.FromHex("#d92027");
            }
            else if (LblTipoAlerta.Text == "EMERGENCIA")
            {
                LblTipoAlerta.TextColor = Color.FromHex("#ffcd3c");
            }

            

            LblHoraAlerta.Text = "Generada a las "+ alerta.horaAlerta.ToString("HH:mm") + " Hrs.";
            LblContadorPersonas.Text = " Esperando ayuda " + alerta.participantes.ToString();

            FotoPerfil.Source = Xamarin.Forms.ImageSource.FromStream(
               () => new MemoryStream(Convert.FromBase64String(alerta.foto)));



            //cambio el boton dependiendo de lo que le corresponda
            if (alerta.opcionBoton == "Ocultar")
            {
                ButtonAcudir.IsVisible = false;
                ButtonAcudir.IsEnabled = false;
            }
            else if (alerta.opcionBoton == "Finalizar")
            {
                ButtonAcudir.BackgroundColor = Color.FromHex("#1d83d4");
                ButtonAcudir.Text = "Ayuda Realizada";
            }

        }

        private void BtnBomberos_Clicked(object sender, EventArgs e)
        {
            PhoneDialer.Open("132");
        }

        private void BtnCarabineros_Clicked(object sender, EventArgs e)
        {
            PhoneDialer.Open(alerta.nroEmergencia);
        }

        private void ButtonSync_Clicked(object sender, EventArgs e)
        {
            ActualizarAlerta(alerta.idAlerta);
        }

    }

}