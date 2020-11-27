using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Business.Providers.HTTP
{
    class ISmsProvider
    {
        private Task<HttpResponseMessage> GetApiResponse(HttpMethod method, string subsidiaryUrl, object model = null);
    }
}
