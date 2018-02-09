using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.DAL
{
    public static class MailSender
    {
        // Extension method
        public static void Forget(this Task task)
        {
            throw new NotImplementedException();
        }
        
        public static void Send(string from, string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };
            try
            {
                SmtpClient smtpClient = new SmtpClient() { Timeout = 10000 };
                smtpClient.Send(mailMessage);
            }
            catch (SmtpException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        public static async Task SendMailAsync(string from, string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            try
            {
                SmtpClient smtpClient = new SmtpClient() { Timeout = 5000 };
                
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (SmtpException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        public static void SendAsync(string from, string to, string subject, string body)
        {
            MailMessage mailMessage = new MailMessage(from, to, subject, body)
            {
                IsBodyHtml = true
            };

            try
            {
                SmtpClient smtpClient = new SmtpClient() { Timeout = 5000 };

                // Set callback method when send completes
                smtpClient.SendCompleted += SmtpClient_SendCompleted;

                string token = "message";
                DateTime startTime = DateTime.Now;

                smtpClient.SendAsync(mailMessage, token);

                if (DateTime.Now.Subtract(startTime).Seconds > 15 && mailSent == false)
                {
                    smtpClient.SendAsyncCancel();
                }
            }
            catch (SmtpException e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                mailMessage.Dispose();
            }
        }

        private static bool mailSent = false;
        private static void SmtpClient_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            // Get identifier for this asynchronous operation
            string token = e.UserState as string;

            if (e.Cancelled) Console.WriteLine($"{token} - Send canceled.");

            if (e.Error != null) Console.WriteLine($"{token} {e.Error.ToString()}.");
            else
            {
                Console.WriteLine("Mail sent.");
                mailSent = true;
            }
        }
    }
}
