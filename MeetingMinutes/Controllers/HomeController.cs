using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MeetingMinutes.Controllers
{
    public class YourCustomAuthorize : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            // If they are authorized, handle accordingly
            if (this.AuthorizeCore(filterContext.HttpContext))
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                // Otherwise redirect to your specific authorized area
                // string webaddress = System.Web.Configuration.WebConfigurationManager.AppSettings["SiteAddress"];
                var request = HttpContext.Current.Request;
                string webaddress = request.Url.Scheme + "://" + request.ServerVariables["HTTP_HOST"] + request.ApplicationPath;

                //filterContext.Result = new RedirectResult("/Home/Login");
                filterContext.Result = new RedirectResult(webaddress);
            }
        }
    }
    public class HomeController : Controller
    {
        //[Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        //[Authorize]
        public ActionResult Login()
        {
            return View();
        }
        //[Authorize]
        public ActionResult Registration()
        {
            return View();
        }
        //[Authorize]
        public ActionResult RegistrationSuccessful()
        {
            return View();
        }
        //[Authorize]
        public ActionResult RecoverPassword()
        {
            return View();
        }
        //[Authorize]
        public ActionResult CreateNewPassword()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult Meeting()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult CreateMeeting()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult CreateAgenda()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult MeetingInvited()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult MeetingDetails()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult Task()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult TaskDetails()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult UserProfile()
        {
            return View();
        }
        [YourCustomAuthorize]
        public ActionResult ChangeEmail()
        {
            return View();
        }
    }
}
