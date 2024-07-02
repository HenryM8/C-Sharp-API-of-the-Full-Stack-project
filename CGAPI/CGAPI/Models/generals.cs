using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;

namespace CGAPI.Models
{
    public class generals
    {
        public string CreatePassword()
        {
            int length = 8;
            const string valid = "0123456789";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }

        public void SendEmail(string destinatario, string asunto, string mensaje)
        {
            string CuentaEmail = ConfigurationManager.AppSettings["CuentaEmail"].ToString();
            string PasswordEmail = ConfigurationManager.AppSettings["PasswordEmail"].ToString();

            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(destinatario));
            msg.From = new MailAddress(CuentaEmail);
            msg.Subject = asunto;
            msg.Body = mensaje;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(CuentaEmail, PasswordEmail);
            client.Port = 587;
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Send(msg);
        }
    }
}