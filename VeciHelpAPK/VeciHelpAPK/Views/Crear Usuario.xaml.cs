using Java.Util.Regex;
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
using System.Text.RegularExpressions;
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

        public  Crear_Usuario()
        {
            InitializeComponent();
        }

        public Crear_Usuario(int idUser)
        {
            InitializeComponent();
            RecargarDatosUsuario(idUser);
            
        }

        //constructor para entrar por la vista de validacion de codigo
        public Crear_Usuario(Usuario user, int validacion)
        {
            this.usr = user;
            InitializeComponent();
            mostrarcampos();
            CargarUsuarioValidado();

        }


        //trae al usuario de la bd con los datos actualizados
        private async void RecargarDatosUsuario(int idUsuario)
        {
            var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });
            var result = await endPoint.GetUserId(idUsuario);
            CargarUsuario(result);
        }





        private  async void ButtonCrear_Clicked(object sender, EventArgs e)
        {
            if (ButtonCrear.Text == "Actualizar")
            {
                asignarDatos();

                var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });
                
                var jsonstring = JsonConvert.SerializeObject(usr);

                var request = await endPoint.ActualizarPerfil(usr);

                

                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();

                    await DisplayAlert("Atención", jsonString, "Aceptar");
                    //actualizo los datos actuales con los de la bd
                    RecargarDatosUsuario(usr.id_Usuario);
                }
            }
            /*
            else
            {
                if (ValidaterUT())
                {

                    if (validateDV())
                    {

                        if (ValidaterCel())
                        {
                            if (camposOK())
                            {
                                //esta opcion se utiliza para que el usuario cree sus datos previo a la validacion del codigo enviado al correo
                                asignarDatos();
                                var endPoint = RestService.For<IUsuario>(BaseAddress);

                                var request = await endPoint.RegistrarUsuario(usr);

                                if (request.StatusCode == HttpStatusCode.OK)
                                {
                                    var jsonString = await request.Content.ReadAsStringAsync();

                                    await DisplayAlert("Atención", jsonString, "Aceptar");

                                    //retrocedo a la ventana anterior que seria el login
                                    await Navigation.PopAsync();
                                }
                            }
                            else
                            {
                                await this.DisplayAlert("Advertencia", "Todos los campos deben ser completados", "ACEPTAR");

                            }

                        }
                        else
                        {
                            await this.DisplayAlert("Advertencia", "El telefono debe contener 9 digitos", "ACEPTAR");

                        }
                    }
                    else
                    {
                        await this.DisplayAlert("Advertencia", "El DV debe contener 1 digito", "ACEPTAR");

                    }




                }
                else
                {
                    await this.DisplayAlert("Advertencia", "El rut debe contener 7 8 digitos", "ACEPTAR");

                }
                

            }
            */
        }


        private void DPFechaNacimiento_DateSelected(object sender, DateChangedEventArgs e)
        {
            usr.fechaNacimiento = DPFechaNacimiento.Date;
        }


        public class NumericValidationBehavior : Behavior<Entry>
        {

            protected override void OnAttachedTo(Entry entry)
            {
                entry.TextChanged += OnEntryTextChanged;
                base.OnAttachedTo(entry);
            }

            protected override void OnDetachingFrom(Entry entry)
            {
                entry.TextChanged -= OnEntryTextChanged;
                base.OnDetachingFrom(entry);
            }

            private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
            {

                if (!string.IsNullOrWhiteSpace(args.NewTextValue))
                {
                    bool isValid = args.NewTextValue.ToCharArray().All(x => char.IsDigit(x)); //Make sure all characters are numbers

                    ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
                }
            }


        }
        
        public  bool camposOK()
        {
            if(nombre.Text.Length>=3 && apellido.Text.Length>=3 && direccion.Text.Length>=3 && clave.Text.Length >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /*
        public bool ValidaterUT()
        {
            if (rut.Text.ToCharArray().All(Char.IsDigit) && (rut.Text.Length == 8 || rut.Text.Length == 7))
            {
                return true;
            }
            return false;
            
            return true;

        }

        public bool ValidaterCel()
        {
            if (celular.Text.ToCharArray().All(Char.IsDigit) || rut.Text.Length == 9)
            {
                return true;
            }
            return false;
        }
        public bool validateDV()
        {
            if (int.Parse(codigoVerificacion.Text)>=0 && int.Parse(codigoVerificacion.Text)<=9 || codigoVerificacion.Text  == "K" ) 
            {
                return true;
            }
            return false;

        }
            */




        public async void asignarDatos()
        {

                usr.nombre = nombre.Text;
                usr.apellido = apellido.Text;
                usr.correo = correo.Text;
                usr.rut = rut.Text;
                usr.antecedentesSalud = AntecedentesSalud.Text;
                usr.celular = int.Parse(celular.Text);
                usr.direccion = direccion.Text;
                usr.clave = clave.Text;
                usr.codigoVerificacion = codigoVerificacion.Text;
            
        }

        //aqui se abre la camara para tomar una foto
        private async void ButtonAgregarFoto_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Atención", "No hay cámara disponible", "Aceptar");
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

            ImagenPerfil.Source = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });

            //asigno la foto recien tomada al usuario que se esta llenando
            usr.Foto=ConvertToBase64(file.GetStream());
            cambioFoto = 1;
        }


        //aqui se busca una foto en la galeria
        private async void ButtonCargarFoto_Clicked(object sender, EventArgs e)
        {
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await DisplayAlert("Atención", "No tiene los permisos correspondientes", "Aceptar");
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




        //metodo que llena los campos del usuario con la info de la bd
        private void CargarUsuario(Usuario user)
        {
            usr = user;
            if (user.Foto!="vacio")
            {
                //cargo la foto de la base de datos
                ImagenPerfil.Source = Xamarin.Forms.ImageSource.FromStream(
                    () => new MemoryStream(Convert.FromBase64String(user.Foto)));
            }

            nombre.Text = user.nombre;
            nombre.IsReadOnly = true;
            if (nombre.IsReadOnly)
            {
                nombre.Opacity = 0.7;
            }
            apellido.Text= user.apellido;
            apellido.IsReadOnly = true;
            if (apellido.IsReadOnly)
            {
                apellido.Opacity = 0.7;
            }
            correo.Text= user.correo;
            correo.IsReadOnly = true;
            if (correo.IsReadOnly)
            {
                correo.Opacity = 0.7;
            }
            rut.Text= user.rut;
            rut.IsReadOnly = true;
            if (rut.IsReadOnly)
            {
                rut.Opacity = 0.7;
            }
            digito.Text= user.digito.ToString();
            digito.IsReadOnly = true;
            if (digito.IsReadOnly)
            {
                digito.Opacity = 0.7;
            }
            AntecedentesSalud.Text= user.antecedentesSalud;
            celular.Text= user.celular.ToString();
            direccion.Text= user.direccion;
            direccion.IsReadOnly = true;
            if (direccion.IsReadOnly)
            {
                direccion.Opacity = 0.7;
            }
            clave.IsVisible = false;
            codigoVerificacion.IsVisible = false;
            ButtonCrear.Text = "Actualizar";
            ButtonCrear.BackgroundColor = Color.FromHex("#ffcd3c");
            DPFechaNacimiento.Date = user.fechaNacimiento;
        }


        //completa algunos datos del usuario previo a ser validado su codigo de verificacion
        private void CargarUsuarioValidado()
        {
            correo.IsReadOnly = true;
            correo.Text = usr.correo;

            codigoVerificacion.IsReadOnly = true;
            codigoVerificacion.Text = usr.codigoVerificacion;
        }


        //resetea todos los campos del formulario
        private void mostrarcampos()
        {
            correo.IsVisible = true;
            correo.IsReadOnly = false;
            nombre.IsVisible = true;
            nombre.IsReadOnly = false;
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


        //encripta la contraseña a sha256
        public string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }


        //boton que actualiza los cambios realizados en la foto de perfil
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

                    await DisplayAlert("Atención", jsonString, "Aceptar");

                    RecargarDatosUsuario(usr.id_Usuario);
                }
            }
            else
            {
                await DisplayAlert("Atención", "Cargue una fotografia nueva para actualizar", "Aceptar");
            }
        }
    }
}