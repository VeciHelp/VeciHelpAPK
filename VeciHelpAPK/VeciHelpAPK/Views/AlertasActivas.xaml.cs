﻿using Refit;
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
    public partial class AlertasActivas : ContentPage
    {
        List<Alerta> AlertaLst = new List<Alerta>();
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";

        public AlertasActivas()
        {
            InitializeComponent();
            CargarAlertas();
        }

        private async void CargarAlertas()
        {
            var token = Preferences.Get("Ses_token", null);
            var idUsuario =int.Parse(Preferences.Get("Ses_id_Usuario", null));

            var endPoint = RestService.For<IAlertas>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            var response = await endPoint.AlertasActivas(idUsuario);

            if (response == null)
            {
                LabelAlertasActivas.Text = "Que bien, no hay alertas activas";
                LabelAlertasActivas.FontAttributes = FontAttributes.Bold;
                LabelAlertasActivas.TextColor = Color.White;
            }

            foreach (var item in response)
            {
                Button btnAlertas = new Button();
                if(item == null)
                {
                    LabelAlertasActivas.Text = "Que bien, no hay alertas activas";
                    LabelAlertasActivas.FontAttributes = FontAttributes.Bold;
                    LabelAlertasActivas.TextColor = Color.White;
                }
               
                  

                    if (item.TipoAlerta == "SOS")
                    {
                        btnAlertas.BackgroundColor = Color.FromHex("#d92027");
                    }
                    else if (item.TipoAlerta == "Emergencia")
                    {
                        btnAlertas.BackgroundColor = Color.FromHex("#ffcd3c");
                    }
                    else if (item.TipoAlerta == "Sospecha")
                    {
                        btnAlertas.BackgroundColor = Color.FromHex("#2FBB62");
                 
                    }

                    btnAlertas.Text = item.direccion + "\n " + item.nombreAyuda + " " + item.apellidoAyuda + " \n " + item.horaAlerta.ToString("HH:mm");
                    btnAlertas.ClassId = item.idAlerta.ToString();
                    btnAlertas.CornerRadius = 25;
                    btnAlertas.Clicked += btnAlertas_Click;
                    btnAlertas.FontAttributes = FontAttributes.Bold;
                    btnAlertas.CommandParameter = item;
                    btnAlertas.TextColor = Color.White;

                    StackAlertas.Children.Add(btnAlertas);
                

                
            }
        }

        private async void btnAlertas_Click(object sender, EventArgs args)
        {
            var button = (Button)sender;

            var alert = (Alerta)button.CommandParameter;

            await Navigation.PushAsync(new NotificacionView(alert));
            
        }
    }
}