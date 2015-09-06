using System;
using System.Net.Mail;

namespace FreshFruit.Common.ToolsUtils
{
    public class MailUtils
    {
        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="to"></param>
        /// <param name="title"></param>
        /// <param name="content"></param>
        /// <param name="IsBodyHtml"></param>
        /// <returns></returns>
        public static string SendMail(string to, string title, string content, bool IsBodyHtml)
        {
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;//指定电子邮件发送方式　　　　　　
            smtpClient.Host = "mail.pinwine.cn"; ;//指定SMTP服务器　　　　　　
            smtpClient.Credentials = new System.Net.NetworkCredential("sellerwine", "R8NaH6UJPdU5Fze");//用户名和密码　　　　　　
            MailMessage mailMessage = new MailMessage("品尚客服<sellerwine@pinwine.cn>", to);
            mailMessage.Subject = title;//主题　　　　　　
            mailMessage.Body = content;//内容　　　　　　
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;//正文编码　　　　　　
            mailMessage.IsBodyHtml = IsBodyHtml;//设置为HTML格式　　　　　　
            mailMessage.Priority = MailPriority.High;//优先级
            try
            {
                smtpClient.Send(mailMessage);
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
