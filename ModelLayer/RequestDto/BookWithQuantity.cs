using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDto
{
    public class BookWithQuantity
    {
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
