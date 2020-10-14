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
    public partial class AdministradorView : CarouselPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";

        public AdministradorView()
        {
            InitializeComponent();
        }


        private async void ButtonEnrolar_Clicked(object sender, EventArgs e)
        {
            if (EnrolarCorreo.Text != null)
            {
                Administrador adm = new Administrador();
                adm.Correo = EnrolarCorreo.Text;
                adm.IdUsuarioCreador = int.Parse(Preferences.Get("Ses_id_Usuario", null));
                var token = Preferences.Get("Ses_token", null);

                var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                var response = await endPoint.EnrolarUsuario(adm);

                await DisplayAlert("Mensaje", response.ToString(), "Ok");

                EnrolarCorreo.Text = string.Empty;
            }
            else
            {
                await DisplayAlert("Error", "Favor ingrese un correo valido", "Ok");
            }
        }

        private async void ButtonEliminar_Clicked(object sender, EventArgs e)
        {
            Usuario usr = new Usuario();

            try
            {
                if (ValidaCorreo(EliminarCorreo.Text))
                {
                    var token = Preferences.Get("Ses_token", null);

                    var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                    var response = await endPoint.GetUsuarioByCorreo(EliminarCorreo.Text);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();

                        usr = JsonConvert.DeserializeObject<Usuario>(jsonString);

                        var action = await DisplayAlert("Confirmacion?", "Esta seguro que quiere borrar al usuario: " + usr.nombre + " " + usr.apellido, "Yes", "No");

                        if (action)
                        {
                            var response2 = await endPoint.EliminarUsuario(usr.id_Usuario);
                            await DisplayAlert("Mensaje", "Usuario eliminado correctamente", "Ok");
                            EliminarCorreo.Text = string.Empty;
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Error", "El correo Ingresado no pertenece a la comunidad", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Error", "Favor ingrese un correo valido", "Ok");
                }
            }
            catch (ApiException ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        public bool ValidaCorreo(string correo)
        {
            if (correo != null && correo.Trim() != string.Empty)
            {
                return true;
            }
            else
                return false;
        }


        private async void ButtonAsociar_Clicked(object sender, EventArgs e)
        {
            List<Usuario> usrLst = new List<Usuario>();

            Usuario usr = new Usuario();

            try
            {
                if (ValidaCorreo(CorreoAsociar.Text))
                {
                    var token = Preferences.Get("Ses_token", null);

                    var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                    var response = await endPoint.GetUsuarioByCorreo(CorreoAsociar.Text);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();


                        //guardo el usuario que se busco con el correo
                        usr = JsonConvert.DeserializeObject<Usuario>(jsonString);

                        //listo los vecinos asociados al usuario que buscamos por el correo
                        var response2 = await endPoint.GetListaVecinos(usr.id_Usuario);

                        if (response2.Count == 0)
                        {
                            await DisplayAlert("Alerta", "El Usuario no posee vecinos enrolados", "Ok");
                            await Navigation.PushAsync(new AsociarVecinos(usr, response2, 1));
                        }
                        else
                        {
                            await Navigation.PushAsync(new AsociarVecinos(usr, response2, 1));
                        }

                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Error", "El correo Ingresado no pertenece a la comunidad", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Error", "Favor ingrese un correo valido", "Ok");
                }
            }

            catch (ApiException ex)
            {
                ex.Message.ToString();
                throw;
            }
        }

        private async void ButtonDesasociar_Clicked(object sender, EventArgs e)
        {
            List<Usuario> usrLst = new List<Usuario>();

            Usuario usr = new Usuario();

            try
            {
                if (ValidaCorreo(CorreoDesasociar.Text))
                {
                    var token = Preferences.Get("Ses_token", null);

                    var endPoint = RestService.For<IAdministrador>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(direccionBase) });

                    var response = await endPoint.GetUsuarioByCorreo(CorreoDesasociar.Text);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonString = await response.Content.ReadAsStringAsync();


                        //guardo el usuario que se busco con el correo
                        usr = JsonConvert.DeserializeObject<Usuario>(jsonString);

                        //listo los vecinos asociados al usuario que buscamos por el correo
                        var response2 = await endPoint.GetListaVecinos(usr.id_Usuario);

                        if (response2.Count == 0)
                        {
                            await DisplayAlert("Alerta", "El Usuario no posee vecinos enrolados", "Ok");
                        }
                        else
                        {
                            await Navigation.PushAsync(new AsociarVecinos(usr, response2, 2));
                        }

                    }
                    else if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        await DisplayAlert("Error", "El correo Ingresado no pertenece a la comunidad", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Error", "Favor ingrese un correo valido", "Ok");
                }
            }

            catch (ApiException ex)
            {
                ex.Message.ToString();
                throw;
            }
        }
    }
    
}