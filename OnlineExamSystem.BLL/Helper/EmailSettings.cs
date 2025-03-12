using OnlineExamSystem.BLL.Common.Helper;
using System.Net;
using System.Net.Mail;

namespace OnlineExamSystem.BLL.Common.Helper
{
    public static class EmailSettings
    {
        public static void SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("basememad1907@gmail.com", "xjdy ldfx ofzl ozrp\r\n");
            client.Send("basememad1907@gmail.com", email.To, email.Subject, email.Body);
        }

    }
}
