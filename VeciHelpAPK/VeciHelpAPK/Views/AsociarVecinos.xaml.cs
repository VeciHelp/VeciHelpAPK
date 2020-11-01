using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public partial class AsociarVecinos : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";

        public Usuario usr;
        public List<Usuario> usrLst;
        public string nombreUsuario;

        public AsociarVecinos(Usuario usrIn,List<Usuario> lst,int tipo)
        {
            this.usrLst = lst;
            this.usr = usrIn;


            InitializeComponent();

            if (tipo ==1)
            {            
                cargarVecinosLabel();
            }
            else
            {
                cargarVecinosButton();
            }
            
            
        }


        private void cargarVecinosLabel()
        {
            Nombre.Text = usr.nombre+" "+usr.apellido;
            

                foreach (var item in usrLst)
                {
                Button btnVecinos = new Button();
                btnVecinos.Text = item.direccion + "\n" + item.nombre + " " + item.apellido;
                btnVecinos.TextColor = Color.FromHex("#1d83d4");
                btnVecinos.BackgroundColor = Color.White;
                btnVecinos.CornerRadius = 20;
                btnVecinos.FontSize = 17;
                sl.Children.Add(btnVecinos);
            }
        }

        private void cargarVecinosButton()
        {
            ButtonAsociarVecino.IsVisible = false;
            Nombre.Text = usr.nombre + " " + usr.apellido;

            if (usrLst.Count > 0)
            {
                foreach (var item in usrLst)
                {
                    Button ButtonDesasociarVecino = new Button();
                    ButtonDesasociarVecino.Text = item.direccion + "\n" + item.nombre + " " + item.apellido;
                    ButtonDesasociarVecino.ClassId = item.id_Usuario.ToString();
                    ButtonDesasociarVecino.TextColor = Color.FromHex("#1d83d4");
                    ButtonDesasociarVecino.BackgroundColor = Color.White; ButtonDesasociarVecino.CornerRadius = 25;
                    ButtonDesasociarVecino.FontSize = 17;
                    ButtonDesasociarVecino.Clicked += ButtonDesasociarVecino_Clicked;
                    sl.Children.Add(ButtonDesasociarVecino);
                }
            }
        }


        private async void ButtonDesasociarVecino_Clicked(object sender, EventArgs e)
        {
            Nombre.Text = usr.nombre + " " + usr.apellido;
            try
            {
                var button = (Button)sender;

                RequestAsoc asociacion = new RequestAsoc();

                //id del usuario al cual le asignaremos un vecino
                asociacion.idUsuario = usr.id_Usuario;
                //id del vecino de la comunidad seleccionado
                asociacion.idVecino = int.Parse(button.ClassId);
                //id del administrador que realizará la asociacion
                asociacion.idAdmin = int.Parse(Preferences.Get("Ses_id_Usuario", null));
                //token del administrador
                var token = Preferences.Get("Ses_token", null);


                var action = await DisplayAlert("Atención", "Desea desasociar este vecino ?", "Aceptar", "Cancelar");

                if (action)
                {
                        var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                        var response = await endPoint.EliminarAsociacion(asociacion);
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

            //boton asociar vecino de la pantalla asociar Vecinos
            private async void ButtonAsociarVecino_Clicked(object sender, EventArgs e)
            {
                    List<Usuario> usrlista = new List<Usuario>();

                try
                {
                    var token = Preferences.Get("Ses_token", null);

                    var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                    //Guardo la respuesta del endPoint que es el litado de usuarios con los cuales puedo asociar al id de usuario enviado
                    var response = await endPoint.GetUsuarios(usr.id_Usuario);


                //pregunto la respuesta del endpoint para ver si encontro vecinos con quien asociar
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                     usrlista = JsonConvert.DeserializeObject<List<Usuario>>(jsonString);

                    //rescato el id del administrador desde la variable guardada en el login
                    var idAdministrador = int.Parse(Preferences.Get("Ses_id_Usuario", null));
                     //usrLst = response;
                     await Navigation.PushAsync(new ListadoComunidad(usrlista, usr.id_Usuario,usr.nombre+" "+usr.apellido , idAdministrador));

                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    await DisplayAlert("Atención", "No hay usuarios disponibles para asociar", "Aceptar");
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