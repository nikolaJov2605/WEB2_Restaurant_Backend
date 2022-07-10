using MimeKit;
using MailKit.Net.Smtp;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Configuration;
using PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Interfaces;
using System.IO;

namespace PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT.Services
{
    public class MailService : IMail
    {
        private readonly EmailConfiguration _emailConfiguration;
        public MailService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        private MimeMessage GenerateEmailInfo(string recieverEmailAddress, string purpouse)
        {
            MimeMessage email = new MimeMessage();
            email.From.Add(new MailboxAddress(string.Empty, _emailConfiguration.Sender));
            email.To.Add(new MailboxAddress(string.Empty, recieverEmailAddress));
            email.Subject = "Verifikacija naloga dostavljača";
            
            string mailText = "";

            string patternFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\PR76_2018_Nikola_Jovicevic_WEB_PROJEKAT\\Data\\EmailPatterns\\";
            if (purpouse == "approved")
                patternFilePath += "VerificationAccepted.txt";
            else if (purpouse == "denied")
                patternFilePath += "VerificationDenied.txt";
            else
                patternFilePath += "VerificationUnverified.txt";

            using (StreamReader sr = new StreamReader(patternFilePath))
            {
                while (!sr.EndOfStream)
                {
                    string row = sr.ReadLine();
                    mailText += row + "\n";
                }
            }

            email.Body = new TextPart(MimeKit.Text.TextFormat.Text)
            {
                Text = mailText
            };

            return email;

        }


        private void SendEmail(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfiguration.Smtp, _emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        public void SendVerificationMail(string recieverEmailAddress, string purpouse)
        {
            MimeMessage mess = GenerateEmailInfo(recieverEmailAddress, purpouse);
            SendEmail(mess);
        }
    }
}
