﻿using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using VeciHelpAPK.Security;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotificacionSospechaView : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        Alerta alerta = new Alerta();
        public NotificacionSospechaView(Alerta alert)
        {
            this.alerta = alert;
            InitializeComponent();
            ActualizarAlerta();
        }



        private async void ButtonAcudir_Clicked(object sender, EventArgs e)
        {
            var IdUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            var token = Preferences.Get("Ses_token", null);

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });



            RequestAlerta aler = new RequestAlerta();
            aler.idUsuario = IdUsuario;
            aler.idAlerta = alerta.idAlerta;

            var response = await endPoint.AcudirAlerta(aler);

            await DisplayAlert("Exito", response, "Ok");

            ActualizarAlerta();
        }
        private async void ActualizarAlerta()
        {
            var IdUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
            var token = Preferences.Get("Ses_token", null);
            var idAlerta = alerta.idAlerta;

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            var response = await endPoint.AlertaById(idAlerta, IdUsuario);

            alerta = response;

            LlenarCamposDeAlerta();


        }

        private void LlenarCamposDeAlerta()
        {
            LblNombre.Text = alerta.nombreAyuda + " " + alerta.apellidoAyuda;
            LblDireccion.Text = alerta.direccion;
            LblTipoAlerta.Text = alerta.TipoAlerta;


            if (LblTipoAlerta.Text == "SOS")
            {
                LblTipoAlerta.TextColor = Color.FromHex("#d92027");
            }
            else if (LblTipoAlerta.Text == "Emergencia")
            {
                LblTipoAlerta.TextColor = Color.FromHex("#ffcd3c");
            }
            else if (LblTipoAlerta.Text == "Sospecha")
            {
                LblTipoAlerta.TextColor = Color.FromHex("#2FBB62");
            }

            LblHoraAlerta.Text = alerta.horaAlerta.ToString("HH:mm");
            LblContadorPersonas.Text = alerta.participantes.ToString();

            FotoPerfil.Source = Xamarin.Forms.ImageSource.FromStream(
               () => new MemoryStream(Convert.FromBase64String(alerta.foto)));

        }
    }
}