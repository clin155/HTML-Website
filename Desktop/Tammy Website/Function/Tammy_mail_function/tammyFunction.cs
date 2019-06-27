using System;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Tammy_mail_function
{
    public static class Function1
    {
        [FunctionName("tammyFunction")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string firstName = req.Query["firstname"];
            string lastName = req.Query["lastname"];
            string email = req.Query["Email"];
            string message = req.Query["message"];
            bool checkEmailFormat = false;
            string firstNameNoWhiteSpace = string.Join(firstName, firstName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            string lastNameNoWhiteSpace = string.Join(lastName, lastName.Split(default(string[]), StringSplitOptions.RemoveEmptyEntries));
            for (int i = 0; i < (email.Length); i++)
            {
                char character = email[i];
                if (character == '@')
                {
                    checkEmailFormat = true;
                    break;
                }
                else
                {
                    checkEmailFormat = false;
                }
            }

            if (checkEmailFormat == true)
            {
                if (firstName != "" && lastName != "" && email != "" && message != "")
                {
                    try
                    {
                        MailAddress fromAddress = new MailAddress("tammymessages75@gmail.com");
                        MailAddress toAddress = new MailAddress("tammymessages75@gmail.com");
                        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                        smtpClient.EnableSsl = true;
                        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtpClient.UseDefaultCredentials = false;
                        smtpClient.Credentials = new NetworkCredential("tammymessages75@gmail.com", "d789Twgh");
                        MailMessage mailMessage = new MailMessage();
                        mailMessage.To.Add(toAddress);
                        mailMessage.From = fromAddress;

                        mailMessage.Subject = String.Format("From {0} {1}, email: {2}", firstNameNoWhiteSpace, lastNameNoWhiteSpace, email);
                        mailMessage.Body = message;
                        smtpClient.Send(mailMessage);
                        return (ActionResult)new OkObjectResult(String.Format("Sent Your Message: {0}, email: {1}, name: {2} {3}", message, email, firstNameNoWhiteSpace, lastNameNoWhiteSpace));


                    }

                    catch (Exception ex)
                    {
                        return new BadRequestObjectResult("Could not send Mail, exception: " + Convert.ToString(ex));
                    }
                }
                else
                {
                    return new BadRequestObjectResult("Please fill in all the fields.");
                }
            }
            else
            {
                return new BadRequestObjectResult("Please format email correctly.");
            }
        }
    }
}
