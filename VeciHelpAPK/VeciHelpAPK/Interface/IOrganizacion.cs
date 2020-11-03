using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Models;

namespace VeciHelpAPK.Interface
{
    public interface IOrganizacion
    {
        [Get("/organizacion/Get?idUsuario={idUsuario}")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> GetOrganizacion(int idUsuario);

        [Put("/organizacion/Put")]
        [Headers("Authorization: Bearer")]
        Task<string> ActualizarOrganizacion([Body(BodySerializationMethod.Serialized)] Organizacion org);
    }
}
