using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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
        }

        private async void ButtonAcudir_Clicked(object sender, EventArgs e)
        {
            var IdUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });
            RequestAlerta aler = new RequestAlerta();

            aler.idUsuario = IdUsuario;
            aler.idAlerta = alerta.idAlerta;

            DisplayAlert("Atencion", "Su participacion se representa con un ticket en el listado de alertas", "Aceptar");
            
            
            if(alerta.opcionBoton == "Finalizar")
            {
                var response = await endPoint.FinalizarAlerta(aler);
                await DisplayAlert("Atención", response, "Aceptar");
                await Navigation.PopAsync();
                GlobalClass.varGlobal = false;
            }
            else if(alerta.opcionBoton == "Acudir")
            {
                var response = await endPoint.AcudirAlerta(aler);
                GlobalClass.varGlobal = true;

                //await DisplayAlert("Atención", response, "Aceptar");
            }

            ActualizarAlerta(alerta.idAlerta);
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
            if (alerta.TipoAlerta=="Sospecha")
            {
                LblDetalle.IsVisible = true;
                LblDetalle.Text = alerta.coordenadaSospecha;
            }
            LblNombre.Text = alerta.nombreAyuda + " " + alerta.apellidoAyuda + "\n\n" + alerta.direccion;
            
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
            else if (LblTipoAlerta.Text == "SOSPECHA")
            {
                LblTipoAlerta.TextColor = Color.FromHex("#2FBB62");
            }

            LblHoraAlerta.Text = "Generada a las "+ alerta.horaAlerta.ToString("HH:mm");
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
                ButtonAcudir.BackgroundColor = Color.FromHex("#d92027");
                ButtonAcudir.Text = "Finalizar alerta";
            }

        }
    }

}