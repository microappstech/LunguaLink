using Langua.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using Langua.Models;

namespace Langua.Repositories.Services
{
    public class MailService:IMailService
    {
        IConfiguration configuration;
        public MailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public bool SendMail(string subject, string body,string ToMail,string ToName)
        {
            try
            {
                var stmpSettings = configuration.GetSection("MailSettings");
                using(MimeMessage emailMsg = new MimeMessage())
                {
                    emailMsg.From.Add(new MailboxAddress(stmpSettings["SenderName"], stmpSettings["SenderEmail"]));
                    emailMsg.To.Add(new MailboxAddress(ToName,ToMail));
                    emailMsg.Subject = subject;
                    emailMsg.Body = new TextPart(subtype: "plain")
                    {
                        Text = body
                    };
                    using(var client = new MailKit.Net.Smtp.SmtpClient())
                    {
                        client.Connect(stmpSettings["Server"], int.Parse(stmpSettings["Port"]),false);
                        client.Authenticate(stmpSettings["Username"], stmpSettings["Password"]);
                        client.Send(emailMsg);
                        client.Disconnect(true);

                    }

                }

                return true;
            }catch
            {
                return false;
            }
        }
    }
}
