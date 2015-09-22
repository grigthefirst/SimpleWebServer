using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer
{
    public class RouterRule
    {
        public List<string> HttpMethods { get; set; }
        public List<string> Urls { get; set; }
        public Action<HttpListenerContext> Action { get; set; }
    }
}
