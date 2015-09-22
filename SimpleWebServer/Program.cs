using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SimpleWebServer.Models;

namespace SimpleWebServer
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer server = new HttpServer(ConfigurationManager.AppSettings["ServerHost"], ConfigurationManager.AppSettings["ServerPort"]);
        }
    }
}
