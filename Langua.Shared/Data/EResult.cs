using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Shared.Data
{
    public class EResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public EResult(bool Succ,string msg = null,List<string> errors = null)
        {
            Success = Succ;
            Message = msg;
            Errors = errors;
        }
        public bool IsSucced()
        {
            return Success;
        }
    }
    public class LResult : EResult
    {
        public string RedirectUrl { get; set; }
        public LResult(bool succ, string msg = "", List<string> ers = null ,string uri = ""):base(succ,msg,ers)
        {
            RedirectUrl = uri;
        }
    }
}
