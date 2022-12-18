using Microsoft.AspNetCore.Mvc;
using Personal_StoreFront.UI.MVC.Models;
using System.Diagnostics;
//using System.Net.Mail;

using MailKit.Net.Smtp;//Grants access to SmtpClient
using MimeKit;//Grants access to MimeMessage

namespace Personal_StoreFront.UI.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contact(ContactViewModel cvm)
        {
            //Add credentials to appsettings.json

            //Need to build out the mailmessage with creds
            //need to send email via SMTP
            if (!ModelState.IsValid)
            {
                return View(cvm);
            }
            else
            {
                #region Email Setup Steps & Email Info

                #endregion

                string message = $"You have received an email from {cvm.Name} (reply to: {cvm.Email}).\n* Subject: {cvm.Subject}\n* Message: \n{cvm.Message}";
                var mm = new MimeMessage();
                mm.From.Add(new MailboxAddress("No Reply", _config.GetValue<string>("Credentials:Email:User")));
                mm.To.Add(new MailboxAddress("You", _config.GetValue<string>("Credentials:Email:Recipient")));

                mm.Subject = cvm.Subject;

                mm.Body = new TextPart("HTML") { Text = message };

                mm.ReplyTo.Add(new MailboxAddress(cvm.Name, cvm.Email));

                using (var client = new SmtpClient())
                {
                    client.Connect(_config.GetValue<string>("Credentials:Email:Client"));

                    client.Authenticate(
                        _config.GetValue<string>("Credentials:Email:User"),
                        _config.GetValue<string>("Credentials:Email:Password")
                        );

                    try
                    {
                        client.Send(mm);
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = $"There was an error sending the email.  Please try again later..";
                        return View(cvm);
                    }
                }



                return View("EmailConfirmation", cvm);
            }
        }
    }
}