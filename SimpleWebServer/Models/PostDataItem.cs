using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Models
{
    public class PostDataItem : IDataItem
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string User { get; set; }
        public string Message { get; set; }

        public PostDataItem() { }
        public PostDataItem(string user, string message)
        {
            this.User = user;
            this.Message = message;
        }
    }
}
