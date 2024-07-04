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
        public async Task<bool> SendMail(string subject, string body,string ToMail,string ToName)
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

                return await Task.FromResult(true);
            }catch
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<bool> SendMails(string subject, string body, Dictionary<string, string> ToName_Mails) 
        {
            try
            {
                var smtpSettings = configuration.GetSection("MailSettings");
                foreach(var recipient in ToName_Mails)
                {
                    using(MimeMessage emailMsg = new MimeMessage())
                    {
                        emailMsg.From.Add(new MailboxAddress(smtpSettings["SenderName"], smtpSettings["SenderEmail"]));
                        emailMsg.To.Add(new MailboxAddress(recipient.Key, recipient.Value));
                        emailMsg.Subject = subject;
                        emailMsg.Body = new TextPart("plain")
                        {
                            Text = body,
                        };

                        using (var smtpclient = new SmtpClient())
                        {

                            smtpclient.Connect(smtpSettings["Server"], int.Parse(smtpSettings["Port"]), false);
                            smtpclient.Authenticate(smtpSettings["Username"], smtpSettings["Password"]);
                            smtpclient.Send(emailMsg);
                            smtpclient.Disconnect(true);
                        }
                        emailMsg.Dispose();
                    }
                }
                return await Task.FromResult(true);
            }
            catch
            {
                return await Task.FromResult(false);
            }
        }
        public async Task<bool> SendVerificationCode(string Mail, string Name, string code_verification)
        {
            var sendMail = await SendMail("Email verification", $"Hello {Name}, this code verification for your applicant in Langua Link : {code_verification}", Mail, Name);
            return sendMail;
        }

        public string GenerateVerificationCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder code = new StringBuilder();
            Random random = new Random();

            for (int i = 0; i < 6; i++)
            {
                code.Append(chars[random.Next(chars.Length)]);
            }
            return code.ToString();
        }
    }
}
