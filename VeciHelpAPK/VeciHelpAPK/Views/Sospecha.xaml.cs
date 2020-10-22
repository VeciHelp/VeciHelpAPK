using Newtonsoft.Json;
using Plugin.Media;
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
    public partial class Sospecha : ContentPage
    {

        byte[] foto;
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";

        public Sospecha()
        {
            InitializeComponent();
        }

        private async void ButtonEnviar_Clicked(object sender, EventArgs e)
        {
            try
            {
                var idUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));

                if (FotoSospecha.Source == null)
                {

                    var action = await DisplayAlert("Atención", "Desea enviar la alerta sin foto", "Si", "No");

                    if (action)
                    {
                        var respuesta = await Alerta.EnviarAlerta(idUsuario, "sospecha", textoSospecha.Text, null);

                        await DisplayAlert("Atención", respuesta, "Aceptar");
                    }

                }
                else
                {
                    //var jsonfoto = JsonConvert.SerializeObject(foto);
                    var respuesta = await Alerta.EnviarAlerta(idUsuario, "sospecha", textoSospecha.Text, foto);

                    

                    await DisplayAlert("Atención", respuesta, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
                throw;
            }          

        }

        private async void ButtonFotoSospecha_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Atención", "La cámara no se encuentra disponible", "Aceptar");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Sample",
                Name = "test.jpg",
                SaveToAlbum = true,
                CompressionQuality = 75,
                CustomPhotoSize = 50,
                PhotoSize = Plugin.Media.Abstractions.PhotoSize.MaxWidthHeight,
                MaxWidthHeight = 2000,
                DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Front
            });

            if (file == null)
                return;

            //await DisplayAlert("File Location", file.Path, "OK");

            FotoSospecha.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //asigno la foto recien tomada a la alerta
            //foto= ConvertToBase64(file.GetStream());
            foto = ConvertToBase64(file.GetStream());
        }

        public byte[] ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return bytes;
            //return Convert.ToBase64String(bytes);
        }
    }
}