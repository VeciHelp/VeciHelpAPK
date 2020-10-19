using System;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using VeciHelpAPK.Models;

namespace VeciHelpAPK.Interface
{
    public interface IUsuario
    {
        [Get("/user/GetListaVecinoId?id={id}")]
        [Headers("Authorization: Bearer")]
        Task<List<Usuario>> GetListaVecinos(int id);


        [Get("/user/validarcodigo")]
        Task<string> ValidarCodigo([Body(BodySerializationMethod.Serialized)] Usuario usr);


        [Post("/user/CrearUser")]
        Task<string> RegistrarUsuario(Usuario usr);
    }
}
