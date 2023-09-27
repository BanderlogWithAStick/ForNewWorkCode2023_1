using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestWorkApp1.ProductsDB.Models.CommonDto
{
    public class SimpleGuidSelectDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool? IsActive { get; set; }
    }
}
