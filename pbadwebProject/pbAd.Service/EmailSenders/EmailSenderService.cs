#region Usings
using pbAd.Core.Filters;
using pbAd.Core.Utilities;
using pbAd.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace pbAd.Service.EmailSenders
{
    public class EmailSenderService : IEmailSenderService
    {
        #region Private Members
        private readonly pbAdContext db;
        #endregion

        #region Ctor
        public EmailSenderService(pbAdContext db)
        {
            this.db = db;
        }
        #endregion

        #region Public Methods

        public async Task<BaseResponse> SendEmail(EmailSenderModel model)
        {
            try
            {
                string to = model.ToEmail; //To address    
                string from = model.FromEmail; //From address    
                MailMessage message = new MailMessage(from, to);

                string mailbody = model.Body;
                message.Subject = model.Subject;
                message.Body = mailbody;
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient(model.SMTPMailServer, model.SMTPMailServerPort);//Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential(model.MailNetworkUsername, model.MailNetworkPassword);
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                }

                catch (Exception ex)
                {
                    throw ex;
                }
                //SmtpClient smtpClient = new SmtpClient(model.SMTPMailServer, model.SMTPMailServerPort);
                //smtpClient.Credentials = new System.Net.NetworkCredential(model.MailNetworkUsername, model.MailNetworkPassword);
                //smtpClient.UseDefaultCredentials = true;
                //smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                //smtpClient.EnableSsl = true;
                //var mail = new MailMessage();
                //mail.From = new MailAddress(model.FromEmail, model.FromEmailName);
                //mail.To.Add(new MailAddress(model.ToEmail));
                //mail.Subject =model.Subject;
                //mail.Body = model.Body; 

                //await smtpClient.SendMailAsync(mail);
                //smtpClient.Dispose();               
            }
            catch (Exception ex)
            {
                return new BaseResponse { IsSuccess = true, Message = "Warning! Error sending email." };
            }

            return new BaseResponse {IsSuccess=true,Message="Success! Email Sent." };
        }

        #endregion

        #region Private Methods

        #endregion
    }
}
