using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDto
{
    public class CartRequest
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
