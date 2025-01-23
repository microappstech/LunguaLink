using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Langua.Repositories.Services.Validation
{
    public class Validation
    {
        public (bool,string) ValidateMail(string email)
        {

            string pattern = @"^(?i)[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$";
            bool isValid = true;
            if (string.IsNullOrEmpty(email))
                return (false, "Please enter a valid email");
            if(!Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase))
                return (false,"The email is not valid");

            if (email.ToLower().Contains("@gmail.com"))
                return (false, "The account should be a google account");
            var dotindex = email.LastIndexOf(".");
            var atindex =  email.IndexOf("@");
            var host_name = email.Substring(email.IndexOf("@")+1, (email.LastIndexOf(".") - email.IndexOf("@")));

            return (true,"");
        }
    }
}
