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
    public partial class ListadoComunidad : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        public List<Usuario> usrLst;
        public int idUsr;
        public int idAdmin;


        public ListadoComunidad()
        {
            InitializeComponent();
            this.Title = "COMUNIDAD";
                ;

        }

        public ListadoComunidad(List<Usuario> lst,int idUsuario, int idAdministrador)
        {
            this.usrLst = lst;
            this.idUsr = idUsuario;
            this.idAdmin = idAdministrador;
           
            InitializeComponent();
            cargarVecinos();
        }

        private void cargarVecinos()
        {

            foreach (var item in usrLst)
            {
                btnCliente.Text = item.direccion + "\n" + item.nombre + " " + item.apellido;
                btnCliente.ClassId = item.id_Usuario.ToString();
                btnCliente.Clicked += BtnCliente_Click;
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

                var action = await DisplayAlert("Confirmacion?", "Esta seguro que quiere asociar este vecino", "Yes", "No");

                if (action)
                {
                    var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });
                    var response = await endPoint.AsociarVecinos(asociacion);

                    await DisplayAlert("Mensaje", response, "Ok");
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                throw;
            }
           
        }


    }
}