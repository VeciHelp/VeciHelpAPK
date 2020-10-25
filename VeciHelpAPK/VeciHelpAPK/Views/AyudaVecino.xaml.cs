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
    public partial class AyudaVecino : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public AyudaVecino()
        {
            InitializeComponent();
            cargaUsuarios();
        }

        private async void cargaUsuarios()
        {
            List<Usuario> usrLst = new List<Usuario>();

            var token = Preferences.Get("Ses_token", null);
            var idUsuario = Preferences.Get("Ses_id_Usuario", null);
            var nombre = Preferences.Get("Ses_nombre", null);

            var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

            var response = await endPoint.GetListaVecinos(int.Parse(idUsuario.ToString()));

            usrLst = response;

            foreach (var item in usrLst)
            {
                Button btnCliente = new Button();
                btnCliente.Text = item.direccion+ "\n " + item.nombre+" "+item.apellido;
                btnCliente.CommandParameter = item;
                btnCliente.Clicked += BtnCliente_Click;
                btnCliente.BackgroundColor = Color.FromHex("#3b83bd");
                btnCliente.TextColor = Color.White;
                btnCliente.FontAttributes = FontAttributes.Bold;
                btnCliente.CornerRadius = 25;
                sl.Children.Add(btnCliente);
            }

            if (usrLst.Count==0)
            {
                await DisplayAlert("Atención", "Usuario no posee vecinos enrolados", "Aceptar");
                await Navigation.PushAsync(new Ayuda());
            }

        }

        private async void BtnCliente_Click(object sender, EventArgs args)
        {
            var button = (Button)sender;
            var vecino = (Usuario)button.CommandParameter;

            string datosAlerta;
            var nombre = vecino.nombre;
            var apellido = vecino.apellido;
            var direccion = vecino.direccion;
            datosAlerta = nombre + " " + apellido + " Direccion:" + direccion;


            var respuesta = await Alerta.EnviarAlerta(vecino.id_Usuario, "ayuda", datosAlerta,null);

            await DisplayAlert("Atención", respuesta, "Aceptar");
        }
    }
}