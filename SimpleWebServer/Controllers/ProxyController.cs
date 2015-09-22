using SimpleWebServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimpleWebServer.ExtentionMethods;
using Newtonsoft.Json;
using System.IO;

namespace SimpleWebServer.Controllers
{
    public class ProxyController
    {
        private ProxyController()
        {

        }

        public void ProxyPage(HttpListenerContext context)
        {
            string url = context.Request.QueryString["url"];
            if (string.IsNullOrEmpty(url))
                throw new BusinessLogicException("Url is empty.");
            WebRequest request = WebRequest.Create(url);
            using (WebResponse response = request.GetResponse())
            using (Stream data = response.GetResponseStream())
            {
                context.Response.ContentType = "text/html";
                data.CopyTo(context.Response.OutputStream);
            }
        }

        private static ProxyController instance;
        public static ProxyController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProxyController();
                }
                return instance;
            }
        }
    }
}
