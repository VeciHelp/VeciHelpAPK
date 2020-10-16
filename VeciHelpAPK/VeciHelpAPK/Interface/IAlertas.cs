using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Models;

namespace VeciHelpAPK.Interface
{
    public interface IAlertas
    {
        [Post("/alerta/sos")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> AlertaRobo(RequestAlerta alert);

        [Post("/alerta/emergencia")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> AlertaAyuda(RequestAlerta alert);

        [Get("/alerta/GetAll?idUsuario={idUsuario}")]
        [Headers("Authorization: Bearer")]
        Task<List<Alerta>> AlertasActivas(int IdUsuario);

        [Get("/alerta/Get?IdAlerta={IdAlerta}&idUsuario={idUsuario}")]
        [Headers("Authorization: Bearer")]
        Task<Alerta> AlertaById(int IdAlerta,int IdUsuario);

        [Post("/alerta/Acudir")]
        [Headers("Authorization: Bearer")]
        Task<string> AcudirAlerta(RequestAlerta alert);

        [Post("/alerta/sospecha")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> Sospecha(RequestAlerta alert);
    }
}
