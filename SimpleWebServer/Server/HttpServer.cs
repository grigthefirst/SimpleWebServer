using System;
using System.Net;
using System.Text;
using System.Threading;

namespace SimpleWebServer
{
    public class HttpServer
    {
        HttpListener httpListener;
        public HttpServer(string host, string port)
        {
            httpListener = new HttpListener();

            httpListener.Prefixes.Add("http://" + host + ":" + port + "/");
            httpListener.Start();

            ThreadPool.QueueUserWorkItem(s =>
            {
                while (httpListener.IsListening)
                    ThreadPool.QueueUserWorkItem(OnRequest, httpListener.GetContext());
            });

            Console.WriteLine("WebServer is up and running... Press <enter> to exit this service!");
            Console.ReadLine();
        }

        private void OnRequest(object context)
        {
            RouterService.Instance.Route((HttpListenerContext)context);
        }
    }
}
