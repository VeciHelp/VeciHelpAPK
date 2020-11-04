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

        public string mensajeValidaciones=string.Empty;
        public bool estadoValidacion=false;

        Usuario usr = new Usuario();

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
            LayoutFoto.IsVisible = false;
            //mostrarcampos();
            CargarUsuarioValidado();

        }


        //trae al usuario de la bd con los datos actualizados
        private async void RecargarDatosUsuario(int idUsuario)
        {
            var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });
            var result = await endPoint.GetUserId(idUsuario);
            CargarUsuario(result);
        }





        //boton aceptar o actualizar
        private  async void ButtonCrear_Clicked(object sender, EventArgs e)
        {
            if (ButtonCrear.Text == "ACTUALIZAR")
            {
                //le envio un 2 para indicarle que necesito que realice las validaciones pero solo para el actualizar datos
                validacionesCampos(2);

                if (estadoValidacion == true)
                {

                    asignarDatosActualizar();

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
                else
                {
                   await DisplayAlert("Atención", mensajeValidaciones, "Aceptar");
                }

            }
            else
            {
                validacionesCampos(1);

                if (estadoValidacion == true)
                {
                    asignarDatos();
                    var endPoint = RestService.For<IUsuario>(BaseAddress);

                    var request = await endPoint.RegistrarUsuario(usr);

                    if (request.StatusCode == HttpStatusCode.OK)
                    {
                        var jsonString = await request.Content.ReadAsStringAsync();

                        await DisplayAlert("Atención", jsonString, "Aceptar");

                        //retrocedo a la ventana anterior
                        await Navigation.PopModalAsync();
                    }
                }
                else
                {
                    await DisplayAlert("Atención", mensajeValidaciones, "Aceptar");
                }

            }
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

       
        public  void asignarDatos()
        {
                usr.nombre = nombre.Text;
                usr.apellido = apellido.Text;
                usr.correo = correo.Text;
                usr.rut = rut.Text;
                usr.digito = char.Parse(digito.Text);
                usr.antecedentesSalud = AntecedentesSalud.Text;
                usr.celular = int.Parse(celular.Text);
                usr.direccion = direccion.Text;
                usr.clave = ActualizarClave.Encriptar(clave.Text);
                usr.codigoVerificacion = codigoVerificacion.Text;
        }

        public void asignarDatosActualizar()
        {

            usr.nombre = nombre.Text;
            usr.apellido = apellido.Text;
            usr.correo = correo.Text;
            usr.rut = rut.Text;
            usr.digito = char.Parse(digito.Text);
            usr.antecedentesSalud = AntecedentesSalud.Text;
            usr.celular = int.Parse(celular.Text);
            usr.direccion = direccion.Text;
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

             SubirFotoNueva();

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
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,

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

            SubirFotoNueva();
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
            //nombre.IsReadOnly = true;
            if (nombre.IsReadOnly)
            {
                nombre.Opacity = 0.7;
            }
            apellido.Text= user.apellido;
            //apellido.IsReadOnly = true;
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
            //rut.IsReadOnly = true;
            if (rut.IsReadOnly)
            {
                rut.Opacity = 0.7;
            }
            digito.Text= user.digito.ToString();
           // digito.IsReadOnly = true;
            if (digito.IsReadOnly)
            {
                digito.Opacity = 0.7;
            }
            AntecedentesSalud.Text= user.antecedentesSalud;
            celular.Text= user.celular.ToString();
            direccion.Text= user.direccion;
           // direccion.IsReadOnly = true;
            if (direccion.IsReadOnly)
            {
                direccion.Opacity = 0.7;
            }
            clave.IsVisible = false;
            codigoVerificacion.IsVisible = false;
            ButtonCrear.Text = "ACTUALIZAR";
            ButtonCrear.TextColor = Color.Black;
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


        
        //encripta la contraseña a sha256
        public string ConvertToBase64(Stream stream)
        {
            var bytes = new Byte[(int)stream.Length];

            stream.Seek(0, SeekOrigin.Begin);
            stream.Read(bytes, 0, (int)stream.Length);

            return Convert.ToBase64String(bytes);
        }

        //metodo que actualiza los cambios realizados en la foto de perfil
        private async void SubirFotoNueva()
        {
            RequestFotoUpd foto = new RequestFotoUpd();

            foto.id_Usuario = usr.id_Usuario;
            foto.Foto = usr.Foto;


                var endPoint = RestService.For<IUsuario>(new HttpClient(new AuthenticatedHttpClientHandler(token)) { BaseAddress = new Uri(BaseAddress) });

                var request = await endPoint.UpdatePhoto(foto);


                if (request.StatusCode == HttpStatusCode.OK)
                {
                    var jsonString = await request.Content.ReadAsStringAsync();

                    await DisplayAlert("Atención", jsonString, "Aceptar");

                    RecargarDatosUsuario(usr.id_Usuario);
                }
        }

        private void validacionesCampos(int tipo)
        {
            if (nombre.Text==null ||  nombre.Text.Trim()==string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "El Nombre no puede ir en blanco";
            }

            else if (apellido.Text == null || apellido.Text.Trim() == string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "El Apellido no puede ir en blanco";
            }

            else if (rut.Text == null || nombre.Text.Trim() == string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "El rut no puede ir en blanco";
            }
            else if (rut.Text.Length < 7 || rut.Text.Length > 8 )
            {
                estadoValidacion = false;
                mensajeValidaciones = "El rut debe contener 7 u 8 dígitos";
            }

            else if (digito.Text == null || digito.Text.Trim() == string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "El dígito verificador no puede ir en blanco";
            }
            else if (digito.Text != "K" && digito.Text != "k" && !digito.Text.ToCharArray().All(Char.IsDigit))
            {
                estadoValidacion = false;
                mensajeValidaciones = "El dígito verificador debe contener un dígito o una K";
            }
           

            else if (digito.Text == null || digito.Text.Trim() == string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "El dígito verificador no puede ir en blanco";
            }

            else if (Rut.ValidaRut(rut.Text, digito.Text) == false)
            {
                estadoValidacion = false;
                mensajeValidaciones = "Ingrese un rut valido en chile";
            }

            else if (celular.Text == null || celular.Text.Trim() == string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "El teléfono no puede ir en blanco";
            }

            else if (direccion.Text == null || direccion.Text.Trim() == string.Empty)
            {
                estadoValidacion = false;
                mensajeValidaciones = "La dirección no puede ir en blanco";
            }

            //si le paso un 1 ademas de todo lo que esta validando valida la clave
            else if(tipo==1)
            {
                if (clave.Text.Trim()==clave2.Text.Trim())
                {
                    if (clave.Text == null || clave.Text.Trim() == string.Empty)
                    {
                        estadoValidacion = false;
                        mensajeValidaciones = "La clave no puede ir en blanco";
                    }
                    else
                    {
                        estadoValidacion = true;
                    }
                }
                else
                {
                    estadoValidacion = false;
                    mensajeValidaciones = "Las contraseñas no coinciden";
                }
                
               
            }
            

        }

    }
}