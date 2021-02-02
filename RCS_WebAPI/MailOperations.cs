using System.Net;
using System.Net.Mail;

namespace RCS_WebAPI
{
    public class MailOperations
    {
        public MailAddress FromMailAddress
        {
            get { return new MailAddress(GlobalProperties.FromEmailAddress, GlobalProperties.FromEmailName); }
        }
        public bool TestEmail()
        {
            MailAddress toAddress = new MailAddress("adnan.rehman322@gmail.com", "Adnan Rehman");

            string body = @"<b>Hi " + toAddress.DisplayName.ToUpper() + "</b>," +
                "<br><br>" +
                "Welcome to Al Fazal Rent A Car Service Test Email Service." +
                "<br><br>" +
                "Thank You<br><b>Al Fazal Rent A Car Service</b>";

            const string subject = "SignUp Verification Email - Al Fazal Rent A Car Service";
            using (var message = new MailMessage(FromMailAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                GetSmtpClient(FromMailAddress).Send(message);
                return true;
            }
        }

        public bool SendSignInEmail(MailAddress toAddress)
        {
            if (toAddress.Address.Contains("test"))
                toAddress = new MailAddress("adnan.rehman322@gmail.com", "Adnan Rehman");

            string body = @"<b>Hi " + toAddress.DisplayName.ToUpper() + "</b>," +
                "<br><br>" +
                "Welcome to Al Fazal Rent A Car Service." +
                "<br><br>" +
                "Your login activity has been observed at : " + System.DateTime.Now.ToString() +
                "<br><br>" +
                "Thank You<br><b>Al Fazal Rent A Car Service</b>";

            const string subject = "SignIn Activity Email - Al Fazal Rent A Car Service";

            using (var message = new MailMessage(FromMailAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                GetSmtpClient(FromMailAddress).Send(message);
                return true;
            }
        }

        public bool SendSignUpEmail(MailAddress toAddress, string OTP)
        {
            if (toAddress.Address.Contains("test"))
                toAddress = new MailAddress("adnan.rehman322@gmail.com", "Adnan Rehman");

            string URL = "https://localhost:44375";

            string body = @"<b>Hi " + toAddress.DisplayName.ToUpper() + "</b>," +
                "<br><br>" +
                "Welcome to Al Fazal Rent A Car Service. Thank you for SignUp to use our services." +
                "<br><br>" +
                "<a href='" + URL + "/SignInSignUp/VerifySignUp?OTP=" + OTP + "&EmailAddress=" + toAddress.Address.ToString() + "'>" +
                "Please click here to verify your account and complete SignUp Process." +
                "</a><br><br>" +
                "Thank You<br><b>Al Fazal Rent A Car Service</b>";


            const string subject = "SignUp Verification Email - Al Fazal Rent A Car Service";
            using (var message = new MailMessage(FromMailAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                GetSmtpClient(FromMailAddress).Send(message);
                return true;
            }
        }

        public bool SendCreateUserEmail(MailAddress toAddress, string OTP)
        {
            if (toAddress.Address.Contains("test"))
                toAddress = new MailAddress("adnan.rehman322@gmail.com", "Adnan Rehman");

            string URL = "https://localhost:44375";

            string body = @"<b>Hi " + toAddress.DisplayName.ToUpper() + "</b>," +
                "<br><br>" +
                "Welcome to Al Fazal Rent A Car Service. You are registered at Al Fazal Rent A Car Service." +
                "<br><br>" +
                "<a href='" + URL + "/SignInSignUp/VerifySignUp?OTP=" + OTP + "&EmailAddress=" + toAddress.Address.ToString() + "'>" +
                "Please click here to verify your account and complete User Verification Process." +
                "</a><br><br>" +
                "Thank You<br><b>Al Fazal Rent A Car Service</b>";

            const string subject = "User Created - Al Fazal Rent A Car Service.";
            using (var message = new MailMessage(FromMailAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                GetSmtpClient(FromMailAddress).Send(message);
                return true;
            }
        }

        public bool SendVerificationEmail(MailAddress toAddress, string OTP)
        {
            if (toAddress.Address.Contains("test"))
                toAddress = new MailAddress("adnan.rehman322@gmail.com", "Adnan Rehman");

            string URL = "https://localhost:44375";

            string body = @"<b>Hi " + toAddress.DisplayName.ToUpper() + "</b>," +
                "<br><br>" +
                "Welcome to Al Fazal Rent A Car Service. Thank you for SignUp to use our services." +
                "<br><br>" +
                "Your Account has been verified and you may now login your account with first time system generated password \"" + OTP + "\"" +
                "<br>" +
                "We are in process now to Activate your account so that you may use it to make Order Booking." +
                "<br><br>" +
                "Thank You<br><b>Al Fazal Rent A Car Service</b>";

            const string subject = "Account Verified - Al Fazal Rent A Car Service";
            using (var message = new MailMessage(FromMailAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
            {
                GetSmtpClient(FromMailAddress).Send(message);
                return true;
            }
        }

        private SmtpClient GetSmtpClient(MailAddress fromAddress)
        {
            string fromPassword = GlobalProperties.FromEmailPassword;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            return smtp;
        }
    }
}