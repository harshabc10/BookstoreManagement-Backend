using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.ResponseDto
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public List<int> BookIds { get; set; }
        public int AddressId { get; set; }
    }
}
