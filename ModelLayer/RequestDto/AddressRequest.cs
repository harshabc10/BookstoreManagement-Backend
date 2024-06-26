﻿using ModelLayer.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDto
{
    public class AddressRequest
    {
        public String address { get; set; }
        public String city { get; set; }
        public String state { get; set; }
        public AddressType type { get; set; }
        public string UserName { get; set; }  // New property
        public string UserPhone { get; set; }
    }
}
