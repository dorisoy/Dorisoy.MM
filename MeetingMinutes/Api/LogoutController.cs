using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MeetingMinutes.Models;
using System.Net.Http;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using System.Net.Mail;
using System.Web.Configuration;
using System.IO;
using System.Data.SqlClient;
using AuthorizeAttribute = System.Web.Mvc.AuthorizeAttribute;
using RoutePrefixAttribute = System.Web.Mvc.RoutePrefixAttribute;
using RouteAttribute = System.Web.Mvc.RouteAttribute;
using HttpPostAttribute = System.Web.Mvc.HttpPostAttribute;
using AllowAnonymousAttribute = System.Web.Mvc.AllowAnonymousAttribute;
using HttpGetAttribute = System.Web.Mvc.HttpGetAttribute;

namespace MeetingMinutes.Api.SuperAdmin
{
    [Authorize]
    [RoutePrefix("api/Logout")]
    public class LogoutController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;

        public LogoutController()
        {
        }

        public LogoutController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        //http://localhost:59604/api/Logout/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
            return Ok(webaddress);
        }



        /*
         {"Message":"An error has occurred.","ExceptionMessage":"Multiple actions were found that match the request: \r\nSiginOut on type MeetingMinutes.Api.SuperAdmin.LogoutController\r\nForgotPasswordSendMail on type MeetingMinutes.Api.SuperAdmin.LogoutController\r\nConfirmEmailSendMail on type MeetingMinutes.Api.SuperAdmin.LogoutController\r\nSetPassword on type MeetingMinutes.Api.SuperAdmin.LogoutController\r\nChangePassword on type MeetingMinutes.Api.SuperAdmin.LogoutController","ExceptionType":"System.InvalidOperationException","StackTrace":"   在 System.Web.Http.Controllers.ApiControllerActionSelector.ActionSelectorCacheItem.SelectAction(HttpControllerContext controllerContext)\r\n   在 System.Web.Http.Controllers.ApiControllerActionSelector.SelectAction(HttpControllerContext controllerContext)\r\n   在 System.Web.Http.ApiController.ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken)\r\n   在 System.Web.Http.Dispatcher.HttpControllerDispatcher.<SendAsync>d__15.MoveNext()"}
         */
        //forgot password
        //forgot password Send Mail
        [HttpPost]
        [AllowAnonymous]
        [Route("ForgotPasswordSendMail")]
        public async Task<IHttpActionResult> ForgotPasswordSendMail(ForgotPasswordSendMailModel data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            string ConnectString = WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            var Id = ""; var Email = ""; var UserName = "";
            int rowNumber = 0;
            using (meetingminutesEntities db = new meetingminutesEntities())
            {
                using (SqlConnection con = new SqlConnection(ConnectString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.AspNetUsers AS asp WHERE asp.Email = '" + data.Email + "'", con))
                    {
                        SqlDataReader Reader = cmd.ExecuteReader();
                        while (Reader.Read())
                        {
                            rowNumber = 1;
                            Email = Reader["Email"].ToString();
                            Id = Reader["Id"].ToString();
                            UserName = Reader["UserName"].ToString();
                        }
                    }
                }
                if (rowNumber > 0)
                {
                    var code = await UserManager.GeneratePasswordResetTokenAsync(Id);
                    var callbackUrl = new Uri(Url.Link("ConfirmEmailSendMailRoute", new { userId = Id, code = code }));

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
                    mail.From = new MailAddress(FromEmail, "Society of Chittagong IT Professionals");
                    mail.To.Add(Email);
                    mail.Subject = "Recover Your Password";
                    string link = callbackUrl.ToString();
                    mail.Body = PopulateBody(UserName, link);
                    mail.IsBodyHtml = true;
                    client.Port = Port;
                    client.Credentials = new System.Net.NetworkCredential(FromEmail, FromPassword);
                    client.EnableSsl = EnableSsl;
                    client.Send(mail);
                    rowNumber = 0;

                    var request = HttpContext.Current.Request;
                    string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
                    return Ok(webaddress);
                }
                else
                {
                    return Content(HttpStatusCode.BadRequest, "invalid email");
                }
            }
            //return Ok();
        }

        private string PopulateBody(string UserName, string callbackUrl)
        {
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
            using (StreamReader reader = new StreamReader(System.Web.Hosting.HostingEnvironment.MapPath("~/Views/Home/ForgotPasswordEmail.html")))
            {
                body = reader.ReadToEnd();
            }

            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
            if (!webaddress.EndsWith("/"))
                webaddress += "/";

            body = body.Replace("{FullName}", UserName);
            body = body.Replace("{link}", callbackUrl);
            body = body.Replace("{alternateLink}", callbackUrl);
            body = body.Replace("{footeryear}", year);
            body = body.Replace("{logo}", webaddress);
            return body;
        }


        //Confirm Mail For Forgot Password
        [AllowAnonymous]
        [HttpGet]
        [Route("ConfirmEmailSendMail", Name = "ConfirmEmailSendMailRoute")]
        public IHttpActionResult ConfirmEmailSendMail(string userId = "", string code = "")
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                ModelState.AddModelError("", "User Id and Code are required");
                return BadRequest(ModelState);
            }

            //string url = "http://localhost:57091/Admin/CreateNewPassword?userId=" + userId + "&code=" + code;
            //string webaddress = System.Web.Configuration.WebConfigurationManager.AppSettings["SiteAddress"];
            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;
            if (!webaddress.EndsWith("/"))
                webaddress += "/";
            string url = webaddress + "Home/CreateNewPassword?userId=" + userId + "&code=" + code;


            System.Uri uri = new System.Uri(url);
            return Redirect(uri);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await UserManager.ResetPasswordAsync(model.userId, model.Code, model.NewPassword);

            if (!result.Succeeded)
            {
                return Content(HttpStatusCode.BadRequest, result.Errors);
            }
            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;

            return Ok(webaddress);
        }

        // POST api/Logout/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            var request = HttpContext.Current.Request;
            string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;

            return Ok(webaddress);
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
                        ModelState.AddModelError("error", error);
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
    }
}