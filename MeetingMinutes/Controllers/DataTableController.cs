using MeetingMinutes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Data.Entity.SqlServer;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Configuration;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Globalization;
using System.Collections;


namespace MeetingMinutes.Controllers
{
    public class DataTableController : Controller
    {
        private ApplicationUserManager _userManager;
        //string vCompanyID = null;

        public DataTableController()
        {

        }

        public DataTableController(ApplicationUserManager userManager,
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
            set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }



        [HttpPost]
        public async Task<ActionResult> GetMeetingList()
        {
            IdentityUser userInfo = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            using (meetingminutesEntities db = new meetingminutesEntities())
            {
                try
                {
                    var meetinglist = (from asp in db.AspNetUsers
                                       join mt in db.Meetings on asp.Id equals mt.Id
                                       where asp.Id == userInfo.Id
                                       select new
                                       {
                                           mt.vMeetingID,
                                           mt.vTitle,
                                           Date = SqlFunctions.DateName("month", mt.dDate) + " " + SqlFunctions.DateName("day", mt.dDate) + ", " + SqlFunctions.DateName("year", mt.dDate),
                                           mt.tStartTime,
                                           mt.tEndTime,
                                           mt.vLocation
                                       }).AsEnumerable()
                                       .Select(x=> new {
                                           x.vMeetingID,
                                           x.vTitle,
                                           x.Date,
                                           startTime = DateTime.Today.Add(x.tStartTime).ToString("h:mm tt"),
                                           endTime = DateTime.Today.Add(x.tEndTime).ToString("h:mm tt"),
                                           x.vLocation
                                       }).ToList();


                    int totalrows = meetinglist.Count;
                    if (!string.IsNullOrEmpty(searchValue))//filter
                    {
                        meetinglist = meetinglist.
                            Where(x => x.vTitle.ToString().Contains(searchValue.ToLower()) || x.Date.ToString().Contains(searchValue.ToLower()) || x.startTime.ToString().Contains(searchValue.ToLower()) || x.endTime.ToString().Contains(searchValue.ToLower()) || x.vLocation.ToString().Contains(searchValue.ToLower())).ToList();
                    }
                    int totalrowsafterfiltering = meetinglist.Count;
                    //sorting
                    meetinglist = meetinglist.OrderBy(sortColumnName + " " + sortDirection).ToList();

                    //paging
                    meetinglist = meetinglist.Skip(start).Take(length).ToList();


                    return Json(new { data = meetinglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetMeetingInvitedList()
        {
            IdentityUser userInfo = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            using (meetingminutesEntities db = new meetingminutesEntities())
            {
                try
                {
                    var meetinglist = (from asp in db.AspNetUsers
                                       join mt in db.Meetings on asp.Id equals mt.Id
                                       join mtIn in db.MeetingInvites on mt.vMeetingID equals mtIn.vMeetingID
                                       where mtIn.Id == userInfo.Id
                                       select new
                                       {
                                           mt.vMeetingID,
                                           mt.vTitle,
                                           //mt.dDate,
                                           Date = SqlFunctions.DateName("month", mt.dDate) + " " + SqlFunctions.DateName("day", mt.dDate) + ", " + SqlFunctions.DateName("year", mt.dDate),
                                           mt.tStartTime,
                                           mt.tEndTime,
                                           mt.vLocation
                                       }).AsEnumerable()
                                       .Select(x => new {
                                           x.vMeetingID,
                                           x.vTitle,
                                           //x.dDate,
                                           x.Date,
                                           x.tStartTime,
                                           startTime = DateTime.Today.Add(x.tStartTime).ToString("h:mm tt"),
                                           endTime = DateTime.Today.Add(x.tEndTime).ToString("h:mm tt"),
                                           x.vLocation
                                       }).ToList();


                    int totalrows = meetinglist.Count;
                    if (!string.IsNullOrEmpty(searchValue))//filter
                    {
                        meetinglist = meetinglist.
                            Where(x => x.vTitle.ToString().Contains(searchValue.ToLower()) || x.Date.ToString().Contains(searchValue.ToLower()) || x.startTime.ToString().Contains(searchValue.ToLower()) || x.endTime.ToString().Contains(searchValue.ToLower()) || x.vLocation.ToString().Contains(searchValue.ToLower())).ToList();
                    }
                    int totalrowsafterfiltering = meetinglist.Count;
                    //sorting
                    meetinglist = meetinglist.OrderBy(sortColumnName + " " + sortDirection).ToList();

                    //paging
                    meetinglist = meetinglist.Skip(start).Take(length).ToList();


                    return Json(new { data = meetinglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> GetTaskList()
        {
            IdentityUser userInfo = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            int start = Convert.ToInt32(Request["start"]);
            int length = Convert.ToInt32(Request["length"]);
            string searchValue = Request["search[value]"];
            string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
            string sortDirection = Request["order[0][dir]"];


            using (meetingminutesEntities db = new meetingminutesEntities())
            {
                try
                {
                    var meetinglist = (from asp in db.AspNetUsers
                                       join mt in db.Meetings on asp.Id equals mt.Id
                                       join mtAg in db.MeetingAgendas on mt.vMeetingID equals mtAg.vMeetingID
                                       join mtTa in db.MeetingTasks on mtAg.vAgendaID equals mtTa.vAgendaID
                                       join mtTaAs in db.TaskAssigns on mtTa.vTaskID equals mtTaAs.vTaskID
                                       where mtTaAs.Id == userInfo.Id
                                       select new
                                       {
                                           mt.vMeetingID,
                                           mt.vTitle,
                                           //mt.dDate,
                                           Date = SqlFunctions.DateName("month", mt.dDate) + " " + SqlFunctions.DateName("day", mt.dDate) + ", " + SqlFunctions.DateName("year", mt.dDate),
                                           mt.tStartTime,
                                           mt.tEndTime,
                                           mt.vLocation
                                       }).AsEnumerable()
                                       .Select(x => new {
                                           x.vMeetingID,
                                           x.vTitle,
                                           //x.dDate,
                                           x.Date,
                                           x.tStartTime,
                                           startTime = DateTime.Today.Add(x.tStartTime).ToString("h:mm tt"),
                                           endTime = DateTime.Today.Add(x.tEndTime).ToString("h:mm tt"),
                                           x.vLocation
                                       }).Distinct().ToList();


                    int totalrows = meetinglist.Count;
                    if (!string.IsNullOrEmpty(searchValue))//filter
                    {
                        meetinglist = meetinglist.
                            Where(x => x.vTitle.ToString().Contains(searchValue.ToLower()) || x.Date.ToString().Contains(searchValue.ToLower()) || x.startTime.ToString().Contains(searchValue.ToLower()) || x.endTime.ToString().Contains(searchValue.ToLower()) || x.vLocation.ToString().Contains(searchValue.ToLower())).ToList();
                    }
                    int totalrowsafterfiltering = meetinglist.Count;
                    //sorting
                    meetinglist = meetinglist.OrderBy(sortColumnName + " " + sortDirection).ToList();

                    //paging
                    meetinglist = meetinglist.Skip(start).Take(length).ToList();


                    return Json(new { data = meetinglist, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception e)
                {
                    //基础提供程序在打开时失败
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
    }
}