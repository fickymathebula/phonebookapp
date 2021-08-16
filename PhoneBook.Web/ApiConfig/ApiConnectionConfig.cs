using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhoneBook.Web.ApiConfig
{
    public class Api_Config
    {
        public HttpClient Initial()
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:3821/api/PhoneBook/");
            return client;
        }
    }
}
