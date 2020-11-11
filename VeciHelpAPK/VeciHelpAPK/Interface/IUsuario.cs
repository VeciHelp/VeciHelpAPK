using System;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeciHelpAPK.Models;
using System.Net.Http;

namespace VeciHelpAPK.Interface
{
    public interface IUsuario
    {
        [Get("/user/GetListaVecinoId?id={id}")]
        [Headers("Authorization: Bearer")]
        Task<List<Usuario>> GetListaVecinos(int id);

        [Get("/user/GetUserId?idUsuario={idUsuario}")]
        [Headers("Authorization: Bearer")]
        Task<Usuario> GetUserId(int idUsuario);


        [Get("/user/validarcodigo")]
        Task<HttpResponseMessage> ValidarCodigo([Body(BodySerializationMethod.Serialized)] Usuario usr);


        [Post("/user/CrearUser")]
        Task<HttpResponseMessage> RegistrarUsuario([Body(BodySerializationMethod.Serialized)] Usuario usr);

        [Put("/user/UpdateUser")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> ActualizarPerfil([Body(BodySerializationMethod.Serialized)] Usuario usr);


        [Put("/user/UpdatePhoto")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> UpdatePhoto([Body(BodySerializationMethod.Serialized)] RequestFotoUpd foto);

        [Put("/user/UpdatePass")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> UpdatePass(RequestPass password);


        //no pide token
        [Put("/user/RecuperarClave?correo={correo}")]

        Task<string> RecuperarClave(string correo);


    }
}
