using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Entity
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public String UserFirstName { get; set; }
        public String UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string UserPhone { get; set; }

    }
}
