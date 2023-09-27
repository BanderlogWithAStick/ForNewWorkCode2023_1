using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkApp1.Interfaces
{
    public interface IRepository<T> where T : IGuidIdentity
    {
        List<T> GetList();
    }
}
