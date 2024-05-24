using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDto
{
    public class WishlistRequest
    {
        public int BookId { get; set; }
        //public int UserId { get; set; } // This will be set in the controller
    }
}
