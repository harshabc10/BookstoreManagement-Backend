using ModelLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ResponseDto
{
    public class OrderDetailsResponse
    {
        public Order Order { get; set; }
        public List<BookWithOrderId> Books { get; set; }
    }
}
