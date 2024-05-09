using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Langua.Repositories.Interfaces
{
    public interface IMailService
    {
        bool SendMail(string subject, string body, string ToMail, string ToName);
        bool SendMails(string subject, string body, Dictionary<string, string> ToMails);
        bool SendVerificationCode(string mail, string name , string code);
    }
}
