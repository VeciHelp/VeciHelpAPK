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

                new Menu{ MenuTitle="INICIO",  MenuDetail="Mi perfil",icon="escudo.png"},
                new Menu{ MenuTitle="ACTUALIZAR",  MenuDetail="Mis Datos",icon="userVerde.png"},
                new Menu{ MenuTitle="CAMBIAR",  MenuDetail="Contraseña",icon="key.png"},
                new Menu{ MenuTitle="SALIR",  MenuDetail="Cerrar Sesión",icon="logout.png"},
            };
            ListMenu.ItemsSource = menu;
        }
        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
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
                Detail = new NavigationPage(new Principal(usr));
            }
            else if (menu.MenuTitle == "ACTUALIZAR")
            {
                IsPresented = false;
                
                Detail = new NavigationPage(new Crear_Usuario(usr.id_Usuario));
            }
           else if (menu.MenuTitle == "CAMBIAR")
            {
                IsPresented = false;
                Detail = new NavigationPage(new ActualizarClave(usr));
            }
            else if (menu.MenuTitle == "CERRAR SESIÓN")
            {
                IsPresented = false;
                Preferences.Remove("AutoLogin");
                Navigation.PopModalAsync();
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