using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Models;

namespace VeciHelpAPK.Interface
{
    public interface IAdministrador
    {
        [Get("/user/GetListaVecinoId?id={id}")]
        [Headers("Authorization: Bearer")]
        Task<List<Usuario>> GetListaVecinos(int id);

        [Get("/admin/GetListaVecinoByCorreo?correo={correo}")]
        [Headers("Authorization: Bearer")]
        Task<List<Usuario>> GetListaVecinosByCorreo(string correo);


        [Post("/admin/EnrolarUsr")]
        [Headers("Authorization: Bearer")]
        Task<string> EnrolarUsuario(Administrador admin);


        //falta completarlo
        [Delete("/admin/EliminarUsuario?idUsuario={idUsuario}")]
        [Headers("Authorization: Bearer")]
        Task<string> EliminarUsuario(int idUsuario);


        [Get("/admin/GetUsuarios?idUsuario={idUsuario}")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> GetUsuarios(int idUsuario);


        [Post("/admin/InsAsocVecino")]
        [Headers("Authorization: Bearer")]
        Task<string> AsociarVecinos(RequestAsoc asoc);

        [Post("/admin/DelAsocVecino")]
        [Headers("Authorization: Bearer")]
        Task<string> EliminarAsociacion(RequestAsoc rasq);


        [Get("/admin/GetUsuarioByCorreo?correo={correo}")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> GetUsuarioByCorreo(string correo);
    }
}
