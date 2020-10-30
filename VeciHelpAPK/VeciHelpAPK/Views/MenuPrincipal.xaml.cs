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

                new Menu{ Page= new Principal(usr),MenuTitle="INICIO",  MenuDetail="Mi perfil",icon="escudo.png"},
                new Menu{ Page= new Crear_Usuario(usr.id_Usuario),MenuTitle="ACTUALIZAR",  MenuDetail="Mis Datos",icon="message.png"},
                new Menu{ Page= new ActualizarClave(usr),MenuTitle="CAMBIAR",  MenuDetail="Contraseña",icon="contacts.png"},
                //new Menu{ Page= new Principal(usr),MenuTitle="CERRAR SESIÓN",  MenuDetail="Configuración",icon="settings.png"}
            };
            ListMenu.ItemsSource = menu;
        }
        private void ListMenu_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menu = e.SelectedItem as Menu;
            if (menu != null)
            {
                IsPresented = false;
                Detail = new NavigationPage(menu.Page);
            }
            else if(menu.MenuTitle == "INICIO")
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

            public Page Page
            {
                get;
                set;
            }
        }
    }
}