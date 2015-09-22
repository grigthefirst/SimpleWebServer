using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer.Models
{
    public interface IDataContext<T> where T : IDataItem
    {
        T GetById(int id);
        IEnumerable<T> GetAll();
        void Add(T item);
    }
}
