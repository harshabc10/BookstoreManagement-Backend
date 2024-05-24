using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDto
{
    public class OrderRequest
    {
        public int AddressId { get; set; }
        public List<int> BookIds { get; set; }
    }
}
