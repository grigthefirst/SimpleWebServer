using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.ExtentionMethods
{
    public static class HttpListenerResponseExtentionMethods
    {
        public static void WriteString(this HttpListenerResponse response, string str)
        {
            byte[] content = Encoding.UTF8.GetBytes(str);
            response.ContentType = "text/html; charset=utf-8";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = content.Length;
            response.OutputStream.Write(content, 0, content.Length);
        }
        public static void WriteJson(this HttpListenerResponse response, object obj)
        {

            byte[] content = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));
            response.ContentType = "application/json; charset=utf-8";
            response.ContentEncoding = Encoding.UTF8;
            response.ContentLength64 = content.Length;
            response.OutputStream.Write(content, 0, content.Length);
        }
    }
}
