using Refit;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VeciHelpAPK.Models;

namespace VeciHelpAPK.Interface
{
    public interface INotificaciones
    {
        [Post("/fcm/send")]
        [Headers("Authorization: Bearer")]
        Task<HttpResponseMessage> Push(string firebase);
    }
}
