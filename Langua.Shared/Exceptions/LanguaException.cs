using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Shared.Exceptions
{
    public class LanguaException : Exception
    {
        public LanguaException() : base("Something wrong with you scenario") { }
        public LanguaException(string Message) :base(Message){}
    }
}
