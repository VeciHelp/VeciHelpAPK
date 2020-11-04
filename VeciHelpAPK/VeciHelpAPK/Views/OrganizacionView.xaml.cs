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
    public partial class OrganizacionView : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public string token = Preferences.Get("Ses_token", null);
        public int idUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));
        public Organizacion organ;

        public OrganizacionView()
        {
            InitializeComponent();
            DescargarDatosOrganizacion();
        }

        public async void DescargarDatosOrganizacion()
        {
            var endPoint = RestService.For<IOrganizacion>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });

            var request = await endPoint.GetOrganizacion(idUsuario);

            if (request.StatusCode == HttpStatusCode.OK)
            {
                Organizacion org = new Organizacion();

                var jsonString = await request.Content.ReadAsStringAsync();

                org = JsonConvert.DeserializeObject<Organizacion>(jsonString);
                organ = org;
                llenarCampos();
            }
            else
            {
                await DisplayAlert("Atención", "Error al cargar datos", "Aceptar");
            }
        }

        public void llenarCampos()
        {
            nombre.Text = organ.nombre;
            nroEmergencia.Text = organ.nroEmergencia;
            cantMinima.Text = organ.cantMinAyuda.ToString();
            comuna.Text = organ.comuna;
            ciudad.Text = organ.ciudad;
            provincia.Text = organ.provincia;
            region.Text = organ.region;
            pais.Text = organ.pais;
        }

        private async void ButtonActualizar_Clicked(object sender, EventArgs e)
        {
            AsignarDatos();

            var endPoint = RestService.For<IOrganizacion>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });

            var jsonstring = JsonConvert.SerializeObject(organ);

            var request = await endPoint.ActualizarOrganizacion(organ);

            if (request!=null)
            {
                await DisplayAlert("Atención", request, "Aceptar");
            }
            else
            {
                await DisplayAlert("Atención", "Error al actualizar datos", "Aceptar");
            }
        }

        public void AsignarDatos()
        {
            organ.idUsuario = idUsuario;
            organ.nombre = nombre.Text;
            organ.nroEmergencia = nroEmergencia.Text;
            organ.cantMinAyuda = int.Parse(cantMinima.Text);
        }
    }
}