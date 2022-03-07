using CastingProject.Models;
using Microsoft.AspNetCore.Mvc;
using QuickMailer;

namespace CastingProject.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult Send()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Send(MailMessage mailMessage)
        {
            try
            {
                Email email = new Email();
                email.SendEmail(mailMessage.To, Credential.Email, Credential.Password, mailMessage.Subject, mailMessage.Body);
                return View();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }

        }
    }
}
