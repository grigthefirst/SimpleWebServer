using SimpleWebServer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using SimpleWebServer.ExtentionMethods;
using System.IO;
using System.Web;
using System.Collections.Specialized;

namespace SimpleWebServer.Controllers
{
    public class GuestBookController
    {
        IDataContext<PostDataItem> dataContext;

        private GuestBookController()
        {
            string dataStorageMethod = ConfigurationManager.AppSettings["DataStorageMethod"];
            if (dataStorageMethod == "Xml")
                dataContext = new XmlDataContext<PostDataItem>(ConfigurationManager.AppSettings["PostDataItemsXmlLocation"]);
            else if (dataStorageMethod == "Sql")
                dataContext = new SqlPostDataContext(ConfigurationManager.ConnectionStrings["PostDataItemsSqlConnectionString"].ConnectionString);
            else
                throw new Exception("DataStorageMethod is setted incorrectly.");
        }

        public void GetAll(HttpListenerContext context)
        {
            IEnumerable<PostDataItem> items = dataContext.GetAll();
            context.Response.WriteJson(items);
        }

        public void Add(HttpListenerContext context)
        {
            try
            {
                if (!context.Request.HasEntityBody)
                {
                    return;
                }
                using (Stream body = context.Request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(body, context.Request.ContentEncoding))
                    {
                        NameValueCollection queryValues = HttpUtility.ParseQueryString(reader.ReadToEnd());

                        string user = queryValues["user"];
                        string message = queryValues["message"];
                        if (string.IsNullOrEmpty(user))
                            throw new BusinessLogicException("User is empty");
                        if (string.IsNullOrEmpty(message))
                            throw new BusinessLogicException("Message is empty");
                        dataContext.Add(new PostDataItem(user, message));
                        context.Response.WriteJson(new { Status = "OK", Error = "" });
                    }
                }



                
            }
            catch (BusinessLogicException ex)
            {
                context.Response.WriteJson(new { Status = "Error", Error = ex.ToString() });
            }
            
        }

        private static GuestBookController instance;
        public static GuestBookController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuestBookController();
                }
                return instance;
            }
        }
    }
}
