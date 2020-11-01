using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPrincipal : MasterDetailPage
    {
        public Usuario usr = new Usuario();

        public MenuPrincipal(Usuario user)
        {
            InitializeComponent();
            usr = user;
            MyMenu();
        }

       
        public void MyMenu()
        {
            
            Detail = new NavigationPage(new Principal(usr));
            List<Menu> menu = new List<Menu>
            {

                new Menu{ MenuTitle="INICIO",  MenuDetail="VeciHelp",icon="escudo.png"},
                new Menu{ MenuTitle="ACTUALIZAR",  MenuDetail="Mis Datos",icon="userVerde.png"},
                new Menu{ MenuTitle="CAMBIAR",  MenuDetail="Contraseña",icon="key.png"},
                new Menu{ MenuTitle="SALIR",  MenuDetail="Cerrar Sesión",icon="logout.png"},
            };
            ListMenu.ItemsSource = menu;
        }
        private async void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu;
            //if (menu != null)
            //{
            //    IsPresented = false;
            //    Detail = new NavigationPage(menu.Page);
            //}
            if(menu.MenuTitle == "INICIO")
            {
                IsPresented = false;
               // Detail = new NavigationPage();
                await Detail.Navigation.PushAsync(new Principal(usr));
            }
            else if (menu.MenuTitle == "ACTUALIZAR")
            {
                IsPresented = false;

                await Detail.Navigation.PushAsync(new Crear_Usuario(usr.id_Usuario));
            }
           else if (menu.MenuTitle == "CAMBIAR")
            {
                IsPresented = false;
                await Detail.Navigation.PushAsync(new ActualizarClave(usr));
            }
            else if (menu.MenuTitle == "SALIR")
            {
                var action = await DisplayAlert("Atención", "Quiere cerrar Sesion?", "Si", "No");
                if (action)
                {
                    IsPresented = false;
                    Preferences.Remove("AutoLogin");

                    //Asigno el Login como MainPage
                    Application.Current.MainPage = new LoginView();

                    await Navigation.PopModalAsync();

                    //redirecciona a la pagina principal
                    await Navigation.PushModalAsync(new Principal(usr));
                }
              
                
            }

        }
        public class Menu
        {
            public string MenuTitle
            {
                get;
                set;
            }
            public string MenuDetail
            {
                get;
                set;
            }

            public ImageSource icon
            {
                get;
                set;
            }

        }

       
    }
}