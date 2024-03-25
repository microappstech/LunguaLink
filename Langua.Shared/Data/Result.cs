using Langua.Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Shared.Data
{
    public class Result<T>
    {
        public bool Succeeded { get; set; }
        public T Value { get; }
        public string Error { get; }
        public LanguaException languaException { get; set; }
        public Result(bool Succeeded, T Data = default, string Error = null,LanguaException LanException =null,Exception exception = null)
        {
            this.Succeeded = Succeeded;
            Value = Data;
            this.Error = Error;
            languaException = LanException;
        }

        public bool IsSucceeded()
        {
            return Succeeded;
        }
    }
}
