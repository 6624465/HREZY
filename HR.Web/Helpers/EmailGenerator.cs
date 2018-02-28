using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using HR.Web.BusinessObjects.Operation;
using System.Configuration;
using System.IO;
using System.Text;
using HR.Web.Models;
using HR.Web.Controllers;


namespace HR.Web.Helpers
{
    public class EmailGenerator : BaseController
    {
        EmployeeWorkDetailBO empworkdetailsBo = null;
        EmployeeLeaveListBO employeeLeaveListBO = null;
        public EmailGenerator()
        {
            empworkdetailsBo = new EmployeeWorkDetailBO(SESSIONOBJ);
            employeeLeaveListBO = new EmployeeLeaveListBO(SESSIONOBJ);
        }
        public bool SendMail(MailMessage msg)
        {
            //var settings = new EmailSettingsBO().GetList().FirstOrDefault();
            msg.From = new MailAddress("cit.dc@ezy-corp.com");
            
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.office365.com";
            client.Port = 587;
            client.EnableSsl = true;
            client.Timeout = 900000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("cit.dc@ezy-corp.com", "Say33125");
            try
            {
                client.Send(msg);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


       
        public bool ConfigMail(string to, string cc, string bcc, bool isHtml, string subject, string body, string[] attachments)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(to));
            msg.CC.Add(new MailAddress(cc));
            msg.Bcc.Add(new MailAddress(bcc));
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = isHtml;

            return SendMail(msg);

        }
        public bool ConfigMail(bool isHtml, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            EmployeeWorkDetail workdetail = empworkdetailsBo.GetByProperty(x => x.EmployeeId == EMPLOYEEID);
            msg.To.Add(workdetail.SendMailsTo);
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = isHtml;

            return SendMail(msg);
        }

        public bool ConfigMail(string to, bool isHtml, string subject, string body)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(new MailAddress(to));
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = isHtml;
            return SendMail(msg);
        }

        public bool ConfigMail(string to, string bcc, bool isHtml, string subject, string body)
        {
            MailMessage msg = new MailMessage();

            msg.To.Add(new MailAddress(to));
            msg.Bcc.Add(new MailAddress(bcc));
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = isHtml;
            return SendMail(msg);
        }

        public bool ConfigMail(string to, string bcc, bool isHtml, string subject, string body, string[] attachments)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(to));

            msg.Bcc.Add(new MailAddress(bcc));
            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = isHtml;

            return SendMail(msg);

        }

        public bool ConfigMail(string to, bool isHtml, string cc, string subject, string body, string[] attachments)
        {
            MailMessage msg = new MailMessage();
            msg.To.Add(new MailAddress(to));
            msg.CC.Add(new MailAddress(cc));

            msg.Subject = subject;
            msg.Body = body;
            msg.BodyEncoding = UTF8Encoding.UTF8;
            msg.IsBodyHtml = isHtml;
            return SendMail(msg);

        }
    }
}