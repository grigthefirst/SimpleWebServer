using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Models
{
    public interface IDataItem
    {
        int Id { get; set; }
        DateTime DateCreated { get; set; }
    }
}
