using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.CustomException
{
    public class PasswordMissmatchException : Exception
    {
        public PasswordMissmatchException()
        {

        }
        public PasswordMissmatchException(string message) : base(message)
        {

        }
    }
}
