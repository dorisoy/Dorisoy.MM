using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using MeetingMinutes.Models;
using MeetingMinutes.Providers;
using MeetingMinutes.Results;
using System.Text;
using System.Net.Mail;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Web.Configuration;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;

namespace MeetingMinutes.Api.SuperAdmin
{
    [Authorize]
    [RoutePrefix("api/Registration")]
    public class RegistrationController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public RegistrationController()
        {
        }

        public RegistrationController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            set
            {
                _userManager = value;
            }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }
            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }
                return BadRequest(ModelState);
                //GetResponse();
            }
            return null;
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        //registration
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(CustomRegisterModel model)
        {
            //
            string ConnectString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (SqlConnection con = new SqlConnection(ConnectString))
            {
                int record = 0; int name = 0;
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as record FROM dbo.AspNetUsers WHERE Email='" + model.Email + "'", con))
                {
                    SqlDataReader Reader = cmd.ExecuteReader();
                    while (Reader.Read())
                    {
                        record = (int)Reader["record"];
                    }
                    if (record > 0)
                    {
                        record = 0;
                        return Content(HttpStatusCode.BadRequest, "duplicate_email");
                    }
                    Reader.Close();
                }
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) as name FROM dbo.AspNetUsers WHERE UserName='" + model.Name + "'", con))
                {
                    SqlDataReader Reader = cmd.ExecuteReader();
                    while (Reader.Read())
                    {
                        name = (int)Reader["name"];
                    }
                    if (name > 0)
                    {
                        name = 0;
                        return Content(HttpStatusCode.BadRequest, "User Name is already Taken");
                    }
                    Reader.Close();
                }
            }
            var user = new ApplicationUser() { UserName = model.Name, Email = model.Email };
            IdentityResult result = await UserManager.CreateAsync(user, model.Password);



            if (!result.Succeeded)
            {
                return Content(HttpStatusCode.BadRequest, "Something Wrong!");
            }

            await ConfirmMailSent(user);
            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
            return Ok(webaddress);
        }

        // POST api/Registration/ChangeEmail
        [Route("ChangeEmail")]
        public async Task<IHttpActionResult> ChangeEmail(ChangeEmail model)
        {
            string webaddress = "";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ApplicationUser user = await UserManager.FindByEmailAsync(model.OldEmail);
            //user = await UserManager.FindAsync(user.UserName, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("error", "The user email or password is incorrect.");
                return BadRequest(ModelState);
            }
            else
            {
                bool isPassOk = UserManager.CheckPassword(user, model.Password);
                if (!isPassOk)
                {
                    ModelState.AddModelError("error", "The user email or password is incorrect.");
                    return BadRequest(ModelState);
                }
                else
                {
                    ApplicationUser user2 = await UserManager.FindByEmailAsync(model.NewEmail);
                    if (user2 != null)
                    {
                        ModelState.AddModelError("error", "The new email is already taken.");
                        return BadRequest(ModelState);
                    }
                    else if (user2 == null)
                    {
                        user.Email = model.NewEmail;
                        user.EmailConfirmed = false;
                        IdentityResult result = await UserManager.UpdateAsync(user);
                        if (!result.Succeeded)
                        {
                            return GetErrorResult(result);
                        }
                        else
                        {
                            await ConfirmMailSent(user);
                            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                            var request = HttpContext.Current.Request;
                            webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
                        }
                    }
                }
            }

            return Ok(webaddress);
        }

        public async Task ConfirmMailSent(ApplicationUser user)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
            if (!webaddress.EndsWith("/"))
                webaddress += "/";
            var callbackUrl = webaddress + "api/Registration/ConfirmEmail?userId=" + user.Id + "&code=" + System.Web.HttpUtility.UrlEncode(code) + "";

            //find employee information

            string link = String.Format("Hello, " + user.Email + "\n Please confirm your account by clicking this  \n\n" + callbackUrl + "");

            string Host = System.Web.Configuration.WebConfigurationManager.AppSettings["Host"];
            string FromEmail = System.Web.Configuration.WebConfigurationManager.AppSettings["FromEmail"];
            string FromPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["FromPassword"];
            string To = System.Web.Configuration.WebConfigurationManager.AppSettings["To"];
            string CC = System.Web.Configuration.WebConfigurationManager.AppSettings["CC"];
            string BCC = System.Web.Configuration.WebConfigurationManager.AppSettings["BCC"];
            Int32 Port = Convert.ToInt32(System.Web.Configuration.WebConfigurationManager.AppSettings["Port"]);
            bool EnableSsl = Convert.ToBoolean(System.Web.Configuration.WebConfigurationManager.AppSettings["EnableSsl"]);

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient(Host);
            mail.From = new MailAddress(FromEmail, "Meeting Minutes");
            mail.To.Add(user.Email);
            mail.Subject = "Email Verification";
            mail.Body = PopulateBody(user.Id, user.UserName, callbackUrl, callbackUrl);
            mail.IsBodyHtml = true;
            client.Port = Port;
            client.Credentials = new System.Net.NetworkCredential(FromEmail, FromPassword);
            client.EnableSsl = EnableSsl;
            client.Send(mail);
        }

        private string PopulateBody(string userID, string userName, string callbackUrl, string alternatecallbackUrl)
        {
            string ConnectString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var UserName = ""; var Email = "";

            using (SqlConnection con = new SqlConnection(ConnectString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT *  FROM AspNetUsers WHERE Id='" + userID + "'", con))
                {
                    SqlDataReader Reader = cmd.ExecuteReader();
                    while (Reader.Read())
                    {
                        Email = Reader["Email"].ToString();
                        UserName = Reader["UserName"].ToString();
                    }
                }
            }

            //string tokenizer for formating footer year date like 01-01-2017 
            var todayDate = (DateTime.Now).ToString();
            string[] words1 = todayDate.Split(' ');
            var Date = ""; var year = "";
            for (int i = 0; i < 1; i++)
            {
                if (i == 0)
                    Date = words1[i];
            }
            //now convert date for 2017
            string[] date1 = Date.Split('/');
            for (int i = 2; i <= 2; i++)
            {
                if (i == 2)
                    year = date1[i];
            }
            //end//

            string body = string.Empty;
            using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Views/Home/registration.html")))
            {
                body = reader.ReadToEnd();
            }

            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
            if (!webaddress.EndsWith("/"))
                webaddress += "/";

            body = body.Replace("{link}", callbackUrl);
            body = body.Replace("{alternateLink}", alternatecallbackUrl);
            body = body.Replace("{Email}", Email);
            body = body.Replace("{UserName}", UserName);
            body = body.Replace("{footeryear}", year);
            body = body.Replace("{logo}", webaddress);

            return body;
        }


        //Confirm Mail For Register new User
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmail", Name = "ConfirmEmailRoute")]
        public async Task<IHttpActionResult> ConfirmEmail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            IdentityResult result = await this.UserManager.ConfirmEmailAsync(userId, code);

            if (result.Succeeded)
            {
                var request = HttpContext.Current.Request;
                string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
                if (!webaddress.EndsWith("/"))
                    webaddress += "/";
                string url = webaddress + "Home/RegistrationSuccessful";

                System.Uri uri = new System.Uri(url);
                return Redirect(uri);
            }
            else
            {
                return GetErrorResult(result);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}