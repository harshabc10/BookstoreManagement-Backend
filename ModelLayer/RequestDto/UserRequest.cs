using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.RequestDto
{
    public class UserRequest
    {
        public String FirstName { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public string Phone { get; set; }
    }
}
