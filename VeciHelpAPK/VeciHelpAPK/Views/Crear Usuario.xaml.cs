using Newtonsoft.Json;
using Plugin.Media;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Interface;
using VeciHelpAPK.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace VeciHelpAPK.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Crear_Usuario : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        Usuario usr = new Usuario();
        public Crear_Usuario()
        {
            InitializeComponent();
        }

        public Crear_Usuario(Usuario user)
        {
            this.usr = user;
            InitializeComponent();
            CargarUsuario();
            
        }

        private  async void ButtonCrear_Clicked(object sender, EventArgs e)
        {
            asignarDatos();
            var endPoint = RestService.For<IUsuario>(BaseAddress);

            var usuariostring = JsonConvert.SerializeObject(usr);
            Console.WriteLine("");

            var request = await endPoint.RegistrarUsuario(usr);

            await DisplayAlert("Mensaje", request.ToString(), "Ok");
        }

        private void DPFechaNacimiento_DateSelected(object sender, DateChangedEventArgs e)
        {
            usr.fechaNacimiento = DPFechaNacimiento.Date;
        }

        public void asignarDatos()
        {
            usr.nombre = nombre.Text;
            usr.apellido = apellido.Text;
            usr.correo = correo.Text;
            usr.rut = rut.Text;
            usr.digito = char.Parse(digito.Text);
            usr.antecedentesSalud = AntecedentesSalud.Text;
            usr.celular = int.Parse(celular.Text);
            usr.direccion = direccion.Text;
            usr.clave = clave.Text;
            usr.codigoVerificacion = codigoVerificacion.Text;
            
        }

        private async void ButtonAgregarFoto_Clicked(object sender, EventArgs e)
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

            ImagenPerfil.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //asigno la foto recien tomada al usuario que se esta llenando
            usr.Foto=ConvertToBase64(file.GetStream());
        }

        private async void ButtonCargarFoto_Clicked(object sender, EventArgs e)
        {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return;
                }
                var file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                });


                if (file == null)
                    return;

                ImagenPerfil.Source = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    file.Dispose();
                    return stream;
                });

            ConvertToBase64(file.GetStream());


        }

        private void CargarUsuario()
        {
            //cargo la foto de la base de datos
            ImagenPerfil.Source = Xamarin.Forms.ImageSource.FromStream(
                () => new MemoryStream(Convert.FromBase64String(usr.Foto)));

            nombre.Text = usr.nombre;
            nombre.IsReadOnly = true;
            apellido.Text=usr.apellido;
            apellido.IsReadOnly = true;
            correo.Text=usr.correo;
            correo.IsReadOnly = true;
            rut.Text=usr.rut;
            rut.IsReadOnly = true;
            digito.Text=usr.digito.ToString();
            digito.IsReadOnly = true;
            AntecedentesSalud.Text=usr.antecedentesSalud;
            celular.Text=usr.celular.ToString();
            direccion.Text=usr.direccion;
            direccion.IsReadOnly = true;
            clave.IsVisible = false;
            codigoVerificacion.IsVisible = false;

        
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