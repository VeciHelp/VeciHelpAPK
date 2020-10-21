using Newtonsoft.Json;
using Plugin.Media;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class Crear_Usuario : ContentPage
    {
        public string BaseAddress = "http://201.238.247.59/vecihelp/api/v1/";
        public string token= Preferences.Get("Ses_token", null);
        Usuario usr = new Usuario();
        int cambioFoto=0;

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

        //constructor para entrar por la vista de validacion de codigo
        public Crear_Usuario(Usuario user, int validacion)
        {
            this.usr = user;
            InitializeComponent();
            mostrarcampos();
            CargarUsuarioValidado();

        }

        private  async void ButtonCrear_Clicked(object sender, EventArgs e)
        {
            if (ButtonCrear.Text == "Actualizar")
            {
                asignarDatos();

                var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });

                var request = await endPoint.ActualizarPerfil(usr);

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();

                    await DisplayAlert("Exito", jsonString, "OK");

                    await Navigation.PushAsync(new Principal(usr));
                }
            }
            else
            {
                asignarDatos();
                var endPoint = RestService.For<IUsuario>(BaseAddress);

                var request = await endPoint.RegistrarUsuario(usr);

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();

                    await DisplayAlert("Exito", jsonString, "OK");

                    await Navigation.PushAsync(new LoginView());
                }
            }
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
            cambioFoto = 1;
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

            usr.Foto=ConvertToBase64(file.GetStream());
            cambioFoto = 1;
        }

        private void CargarUsuario()
        {
            if (usr.Foto!="vacio")
            {
                //cargo la foto de la base de datos
                ImagenPerfil.Source = Xamarin.Forms.ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(usr.Foto)));
            }

            nombre.Text = usr.nombre;
            nombre.IsReadOnly = true;
            if (nombre.IsReadOnly)
            {
                nombre.Opacity = 0.7;
            }
            apellido.Text=usr.apellido;
            apellido.IsReadOnly = true;
            if (apellido.IsReadOnly)
            {
                apellido.Opacity = 0.7;
            }
            correo.Text=usr.correo;
            correo.IsReadOnly = true;
            if (correo.IsReadOnly)
            {
                correo.Opacity = 0.7;
            }
            rut.Text=usr.rut;
            rut.IsReadOnly = true;
            if (rut.IsReadOnly)
            {
                rut.Opacity = 0.7;
            }
            digito.Text=usr.digito.ToString();
            digito.IsReadOnly = true;
            if (digito.IsReadOnly)
            {
                digito.Opacity = 0.7;
            }
            AntecedentesSalud.Text=usr.antecedentesSalud;
            celular.Text=usr.celular.ToString();
            direccion.Text=usr.direccion;
            direccion.IsReadOnly = true;
            if (direccion.IsReadOnly)
            {
                direccion.Opacity = 0.7;
            }
            clave.IsVisible = false;
            codigoVerificacion.IsVisible = false;
            ButtonCrear.Text = "Actualizar";
            ButtonCrear.BackgroundColor = Color.FromHex("#ffcd3c");
            DPFechaNacimiento.Date = usr.fechaNacimiento;
            

        }

        private void CargarUsuarioValidado()
        {
            correo.IsReadOnly = true;
            correo.Text = usr.correo;

            codigoVerificacion.IsReadOnly = true;
            codigoVerificacion.Text = usr.codigoVerificacion;
        }

        private void mostrarcampos()
        {
            correo.IsVisible = true;
            correo.IsReadOnly = true;
            nombre.IsVisible = true;
            nombre.IsReadOnly = true;
            apellido.IsVisible = true;
            apellido.IsReadOnly = false;
            rut.IsVisible = true;
            rut.IsReadOnly = false;
            digito.IsVisible = true;
            digito.IsReadOnly = false;
            AntecedentesSalud.IsVisible = true;
            AntecedentesSalud.IsReadOnly = false;
            DPFechaNacimiento.IsVisible = true;
            celular.IsVisible = true;
            celular.IsReadOnly = false;
            direccion.IsVisible = true;
            direccion.IsReadOnly = false;
            codigoVerificacion.IsVisible = true;
            codigoVerificacion.IsReadOnly = false;
        }


        public string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }

        private async void ButtonSubirCambios_Clicked(object sender, EventArgs e)
        {
            RequestFotoUpd foto = new RequestFotoUpd();

            foto.id_Usuario = usr.id_Usuario;
            foto.Foto = usr.Foto;

            if (cambioFoto==1)
            {
                var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });

                var request = await endPoint.UpdatePhoto(foto);

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();

                    await DisplayAlert("Exito", jsonString, "OK");

                    await Navigation.PushAsync(new LoginView());
                }
            }
            else
            {
                await DisplayAlert("Mensaje", "Favor Cargue una foto nueva para actualizar", "OK");
            }
        }
    }
}