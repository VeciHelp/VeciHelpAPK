using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
    public partial class ListadoComunidad : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public List<Usuario> usrLst;
        public int idUsr;
        public int idAdmin;
        public string nombreUsuario;

        public ListadoComunidad()
        {
            InitializeComponent();
            
        }

        public ListadoComunidad(List<Usuario> lst,int idUsuario,string nombreUsuario, int idAdministrador)
        {
            this.usrLst = lst;
            this.idUsr = idUsuario;
            this.idAdmin = idAdministrador;
            this.nombreUsuario = nombreUsuario;
           
            InitializeComponent();
            Nombre.Text = nombreUsuario;
            cargarVecinos();
            
        }

        private void cargarVecinos()
        {
            
            foreach (var item in usrLst)
            {
                Button btnCliente = new Button();
                btnCliente.Text = item.direccion + "\n" + item.nombre + " " + item.apellido ;
                btnCliente.ClassId = item.id_Usuario.ToString();
                btnCliente.Clicked += BtnCliente_Click;
                btnCliente.FontSize = 17;
                btnCliente.BackgroundColor = Color.White;
                btnCliente.TextColor = Color.FromHex("#1d83d4");
                btnCliente.CornerRadius = 20;
               
                sl.Children.Add(btnCliente);
            }
        }

        private async void BtnCliente_Click(object sender, EventArgs args)
        {
            try
            {
                var button = (Button)sender;

                RequestAsoc asociacion = new RequestAsoc();

                //id del usuario a asignar los vecinos
                asociacion.idUsuario = idUsr;
                //id del vecino seleccionado
                asociacion.idVecino = int.Parse(button.ClassId);
                asociacion.idAdmin = idAdmin;

                
                //token del administrador
                var token = Preferences.Get("Ses_token", null);

                var action = await DisplayAlert("Atención", "Desea asociar este vecino ?", "Aceptar", "Cancelar");

                if (action)
                {
                    var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });
                    var response = await endPoint.AsociarVecinos(asociacion);
                    response = response.Replace("\"", "");
                    await DisplayAlert("Atención", response, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
           
        }

        private void toolBarPrincipal_Clicked(object sender, EventArgs e)
        {

        }

        private void toolBarMisDatos_Clicked(object sender, EventArgs e)
        {

        }

        private void toolBarClave_Clicked(object sender, EventArgs e)
        {

        }

        private void toolBarCerrarSesion_Clicked(object sender, EventArgs e)
        {

        }
    }
}