﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class Order
    {
        public int OrderId { get; set; }
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public int BookId { get; set; }
    }
}
