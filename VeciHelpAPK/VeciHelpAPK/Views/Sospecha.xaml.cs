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

        string foto;
        public string direccionBase = "http://201.238.247.59/vecihelp/api/v1/";

        public Sospecha()
        {
            InitializeComponent();
        }

        private async void ButtonEnviar_Clicked(object sender, EventArgs e)
        {
            var idUsuario = int.Parse(Preferences.Get("Ses_id_Usuario", null));

            if (FotoSospecha.Source==null)
            {

                var action = await DisplayAlert("Confirmacion?", "Esta seguro que quiere enviar la alerta sin foto", "Yes", "No");

                if (action)
                {
                    var respuesta = await Alerta.EnviarAlerta(idUsuario, "sospecha", textoSospecha.Text,null);

                    await DisplayAlert(" ", respuesta, "Ok");
                }
                
            }
            else
            {
                var respuesta = await Alerta.EnviarAlerta(idUsuario, "sospecha", textoSospecha.Text,foto);

                await DisplayAlert(" ", respuesta, "Ok");
            }

            

        }

        private async void ButtonFotoSospecha_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("No Camera", ":( No camera available.", "OK");
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

            await DisplayAlert("File Location", file.Path, "OK");

            FotoSospecha.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //asigno la foto recien tomada a la alerta
            foto= ConvertToBase64(file.GetStream());
        }

        public string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }
    }
}