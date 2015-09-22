using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SimpleWebServer.ExtentionMethods;
using SimpleWebServer.Controllers;

namespace SimpleWebServer
{
    public class RouterService
    {
        public IEnumerable<RouterRule> Rules { get; private set; }
        public Action<HttpListenerContext> DefaultBehavior { get; private set; }

        private RouterService()
        {
            /* Config router here */
            this.Rules = new List<RouterRule>()
            {
                { new RouterRule() { HttpMethods = new List<string>(){"GET"}, Urls = new List<string>(){"Guestbook"}, Action = (a => GuestBookController.Instance.GetAll(a)) } },
                { new RouterRule() { HttpMethods = new List<string>(){"POST"}, Urls = new List<string>(){"Guestbook"}, Action = (a => GuestBookController.Instance.Add(a)) } },

                { new RouterRule() { HttpMethods = new List<string>(){"GET"}, Urls = new List<string>(){"Proxy"}, Action = (a => ProxyController.Instance.ProxyPage(a)) } },
            };
            this.DefaultBehavior = (a =>
            {
                //TODO: пропускать картинки и и скрипты
                a.Response.WriteString("Hello world!");
            });
        }

        public void Route(HttpListenerContext context)
        {
            Console.WriteLine(context.Request.HttpMethod + " " + context.Request.Url.AbsoluteUri);

            string url = context.Request.Url.Segments.Last().Replace("/","").ToLower();
            IEnumerable<Action<HttpListenerContext>> actions = Rules.Where(r => r.Urls.Any(u => u.ToLower() == url) && r.HttpMethods.Contains(context.Request.HttpMethod)).Select(a => a.Action);
            foreach (Action<HttpListenerContext> action in actions)
            {
                try
                {
                    action.Invoke(context);
                }
                catch(Exception ex)
                {
                    context.Response.WriteString("Error happend: " + ex.ToString());
                }
            }
            if (actions.Count() == 0)
                this.DefaultBehavior.Invoke(context);
        }

        private static RouterService instance;
        public static RouterService Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new RouterService();
                }
                return instance;
            }
        }
    }
}
