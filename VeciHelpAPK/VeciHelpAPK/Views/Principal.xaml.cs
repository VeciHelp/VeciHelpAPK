using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Principal : ContentPage
    {
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";
        Usuario user = new Usuario();

        

        public Principal(Usuario usr)
        {
            InitializeComponent();
            var usrTemp = JsonConvert.SerializeObject(usr);

            Preferences.Set("UsuarioMasterPage", usrTemp);
            NavigationPage.SetHasBackButton(this, false);


            ButtonAdministrador.IsEnabled = false;
            ButtonAdministrador.IsVisible = false;
            lblAdministrador.IsVisible = false;
            lblAdministrador.IsEnabled = false;
            user = usr;

            if (user.rolename=="Administrador")
            {
                ButtonAdministrador.IsEnabled = true;
                ButtonAdministrador.IsVisible = true;
                lblAdministrador.IsEnabled = true;
                lblAdministrador.IsVisible = true;
            }

            ToolbarItem item = new ToolbarItem();
            item.Text = "Mis Datos";
            item.Order = ToolbarItemOrder.Secondary;
            item.Clicked += ButtonActualizarDatos_Clicked;
        }


        private async void ButtonSOSRobo_Clicked(object sender, EventArgs e)
        {
            //var pag =  new LoginView() ;
            //await PopToPage(pag);
            
            await Navigation.PushAsync(new SOSRobo());
        }

        private async void ButtonAyuda_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Ayuda());
        }

        private async void ButtonSospecha_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Sospecha());
        }

        private async void ButtonAdministrador_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdministradorView());
        }

        private async void ButtonAlertas_Clicked(object sender, EventArgs e)
        {
                    await Navigation.PushAsync(new AlertasActivas());
        }

        private async void ButtonActualizarDatos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Crear_Usuario(user.id_Usuario));
        }

        private async void ButtonActualizarClave_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActualizarClave(user));
        }





        protected async Task PopToPage(Page destination)
        {
            List<Page> toRemove = new List<Page>();

            if (destination == null) return;

            //First, we get the navigation stack as a list
            var pages = Navigation.NavigationStack.ToList();

            //Then we invert it because it's from first to last and we need in the inverse order
            pages.Reverse();

            //Then we discard the current page
            pages.RemoveAt(0);

            foreach (var page in pages)
            {
                if (page == destination) break; //We found it.

                toRemove.Add(page);
            }

            foreach (var rvPage in toRemove)
            {
                Navigation.RemovePage(rvPage);
            }

            await Navigation.PopAsync();
        }

        private void toolBarCerrarSesion_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("AutoLogin");

            Navigation.PushModalAsync(new NavigationPage(new LoginView()));
        }

        private async void toolBarMisDatos_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new Crear_Usuario(user.id_Usuario)));
        }

        private async void toolBarPrincipal_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new Principal(user)));

        }

        private async void toolBarClave_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ActualizarClave(user));

        }

        
    }

}