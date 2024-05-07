using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class ShoppingCartEntity
    {
        public int CartId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
    }
}
