using System;
using Refit;
using System.Threading.Tasks;
using VeciHelpAPK.Models;
using System.Net.Http;

namespace VeciHelpAPK.Interface
{
    public interface ILogin
    {

        [Post("/login/authenticate")]
        Task<HttpResponseMessage> PostLogin(Login log);

    }
}
