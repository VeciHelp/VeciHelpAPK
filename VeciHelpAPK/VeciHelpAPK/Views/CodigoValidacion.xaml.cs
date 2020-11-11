using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CodigoValidacion : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public CodigoValidacion()
        {
            InitializeComponent();
        }

        private async void ButtonValidar_Clicked(object sender, EventArgs e)
        {
            if (correo.Text != null && correo.Text != "" && codigoVerificacion.Text != null && codigoVerificacion.Text != "")
            {
                Usuario usr = new Usuario(correo.Text, codigoVerificacion.Text);


                var endPoint = RestService.For<IUsuario>(BaseAddress);

                var request = await endPoint.ValidarCodigo(usr);

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ValidacionCodigoResponse>(jsonString);
                    var mensaje = obj.resp.Replace("\"", "");

                    if (obj.tipo!=0)
                    {
                        await DisplayAlert("Atención", mensaje, "Aceptar");

                        await Navigation.PushModalAsync(new Crear_Usuario(usr, 1, obj.tipo));
                    }
                    else
                    {
                        await DisplayAlert("Atención", mensaje, "Aceptar");
                        await Navigation.PopModalAsync();
                    }
                    

                }
                else
                {
                    await DisplayAlert("Atención", "Error de conexion", "Aceptar");
                    await Navigation.PopModalAsync();
                }
                    
            }
            else
            {
                await DisplayAlert("Atención", "Ingrese datos validos", "Aceptar");
            }

        }
    }
}