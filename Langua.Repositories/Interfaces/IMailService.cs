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
        /// <summary>
        /// send Email to list of mails
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="ToName_Mails"></param>
        /// <returns></returns>
        bool SendMails(string subject, string body, Dictionary<string, string> ToName_Mails);
        bool SendVerificationCode(string mail, string name , string code);
    }
}
