using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MeetingMinutes.Models;
using System.Data.Entity;
using System.Net;
using System.Data.Entity.SqlServer;

namespace MeetingMinutes.Api
{
    [Authorize]
    [RoutePrefix("api/Meeting")]
    public class MeetingController : ApiController
    {
        private ApplicationUserManager _userManager;
        //string userId = "";

        public MeetingController()
        {

        }

        public MeetingController(ApplicationUserManager userManager,
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


        [AllowAnonymous]
        public async Task<IHttpActionResult> Get()
        {
            using (meetingminutesEntities db = new meetingminutesEntities())
            {
                try
                {
                    IdentityUser userInfo = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                    var user = await (from asp in db.AspNetUsers
                                      select new
                                      {
                                          asp.Id,
                                          asp.UserName
                                      }).OrderBy(x => x.UserName).ToListAsync();

                    var userData = await (from asp in db.AspNetUsers
                                          where asp.Id == userInfo.Id
                                          select new
                                          {
                                              asp.Id,
                                              asp.UserName,
                                              asp.Email
                                          }).OrderBy(x => x.UserName).ToListAsync();

                    var data = new { USERDATA = userData, USER = user};
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    return BadRequest("" + ex.Message);
                }
            }
        }

        //edit data 
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetEditData(string Id)
        {
            using (meetingminutesEntities db = new meetingminutesEntities())
            {
                try
                {
                    IdentityUser userInfo = await UserManager.FindByIdAsync(User.Identity.GetUserId());

                    var meetingDataById = (from mt in db.Meetings
                                           where mt.vMeetingID == Id
                                           select new
                                           {
                                               mt.vMeetingID,
                                               mt.vTitle,
                                               DoW = SqlFunctions.DateName("dw", mt.dDate),
                                               dDate = SqlFunctions.DateName("month", mt.dDate) + " " + SqlFunctions.DateName("day", mt.dDate) + ", " + SqlFunctions.DateName("year", mt.dDate),
                                               mt.tStartTime,
                                               mt.tEndTime,
                                               mt.vLocation,
                                               meetingInvitesList = (from mtin in db.MeetingInvites
                                                                     where mtin.vMeetingID == Id
                                                                     select new
                                                                     {
                                                                         mtin.Id,
                                                                         mtin.AspNetUser.UserName,
                                                                         mtin.vMeetingInviteID,
                                                                         mtin.vMeetingID,
                                                                         mtin.iIndex
                                                                     }).OrderBy(x => x.iIndex),
                                               user = (from asp in db.AspNetUsers
                                                       select new
                                                       {
                                                           asp.Id,
                                                           asp.UserName
                                                       }).OrderBy(x => x.UserName).ToList(),
                                               AgendaList = (from ag in db.MeetingAgendas
                                                             where ag.vMeetingID == Id
                                                             select new
                                                             {
                                                                 ag.vAgendaName,
                                                                 ag.vAgendaDetails,
                                                                 ag.iIndex,
                                                                 MeetingTasks = (from mtask in db.MeetingTasks
                                                                                 where mtask.vAgendaID == ag.vAgendaID
                                                                                 select new
                                                                                 {
                                                                                     mtask.vTaskDetails,
                                                                                     mtask.iIndex,
                                                                                     TaskAssigns = (from ta in db.TaskAssigns
                                                                                                    where ta.vTaskID == mtask.vTaskID
                                                                                                    select new
                                                                                                    {
                                                                                                        ta.Id,
                                                                                                        ta.AspNetUser.UserName
                                                                                                    }).ToList()
                                                                                 }).OrderBy(x => x.iIndex).ToList(),
                                                                 Decisions = (from dec in db.Decisions
                                                                              where dec.vAgendaID == ag.vAgendaID
                                                                              select new
                                                                              {
                                                                                  dec.vDecisionDetails,
                                                                                  dec.iIndex
                                                                              }).OrderBy(x => x.iIndex).ToList()
                                                             }).OrderBy(x => x.iIndex).ToList()
                                           }).AsEnumerable().Select(x => new
                                           {
                                               x.vMeetingID,
                                               x.vTitle,
                                               x.DoW,
                                               x.dDate,
                                               startTime = DateTime.Today.Add(x.tStartTime).ToString("h:mm tt"),
                                               endTime = DateTime.Today.Add(x.tEndTime).ToString("h:mm tt"),
                                               x.vLocation,
                                               x.meetingInvitesList,
                                               x.user,
                                               x.AgendaList
                                           }).ToList();

                    var data = new { MEETINGDATABYID = meetingDataById };
                    return Ok(data);
                }
                catch (Exception ex)
                {
                    return BadRequest("" + ex.Message);
                }
            }
        }


        // POST: api/LookupType
        [Route("SaveMeeting")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SaveMeeting(CreateMeeting CrMe)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            Meeting mt = new Meeting();
            mt.Id = user.Id;
            mt.vMeetingID = CrMe.vMeetingID;
            mt.vTitle = CrMe.vTitle;
            mt.dDate = CrMe.dDate;
            mt.vLocation = CrMe.vLocation;

            DateTime st = DateTime.ParseExact(CrMe.startTime, "h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            mt.tStartTime = st.TimeOfDay;
            DateTime et = DateTime.Parse(CrMe.endTime, System.Globalization.CultureInfo.InvariantCulture);
            mt.tEndTime = et.TimeOfDay;
            List<MeetingInvite> mtinlist = CrMe.meetingInvitesList;

            using (meetingminutesEntities dbCon = new meetingminutesEntities())
            {
                if (!ModelState.IsValid)
                {
                    var errors = new List<string>();
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
                }

                if (mt.vMeetingID == null)
                {
                    using (var dbContextTransaction = dbCon.Database.BeginTransaction())
                    {
                        try
                        {
                            mt.vMeetingID = Guid.NewGuid().ToString();
                            dbCon.Meetings.Add(mt);
                            dbCon.SaveChanges();

                            foreach(MeetingInvite mtin in mtinlist)
                            {
                                mtin.vMeetingInviteID = Guid.NewGuid().ToString();
                                mtin.vMeetingID = mt.vMeetingID;
                                dbCon.MeetingInvites.Add(mtin);
                                dbCon.SaveChanges();
                            }

                            dbContextTransaction.Commit();
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            return BadRequest(ex.InnerException.InnerException.Message);
                        }
                    }
                }
                else
                {
                    using (var dbContextTransaction = dbCon.Database.BeginTransaction())
                    {
                        try
                        {
                            dbCon.Entry(mt).State = EntityState.Modified;
                            dbCon.SaveChanges();

                            dbCon.MeetingInvites.RemoveRange(dbCon.MeetingInvites.Where(x => x.vMeetingID == mt.vMeetingID));

                            foreach (MeetingInvite mtin in mtinlist)
                            {
                                mtin.vMeetingInviteID = Guid.NewGuid().ToString();
                                mtin.vMeetingID = mt.vMeetingID;
                                dbCon.MeetingInvites.Add(mtin);
                                dbCon.SaveChanges();
                            }
                            
                            dbContextTransaction.Commit();
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            return BadRequest(ex.InnerException.InnerException.Message);
                        }
                    }
                }
            }
        }

        // POST: api/LookupType
        [Route("SaveAgenda")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SaveAgenda(CreateAgenda CrAg)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            using (meetingminutesEntities dbCon = new meetingminutesEntities())
            {
                if (!ModelState.IsValid)
                {
                    var errors = new List<string>();
                    foreach (var state in ModelState)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            errors.Add(error.ErrorMessage);
                        }
                    }
                    return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, errors));
                }

                if (CrAg.vMeetingID == null)
                {
                    return Ok("error");
                }
                else
                {
                    using (var dbContextTransaction = dbCon.Database.BeginTransaction())
                    {
                        try
                        {
                            dbCon.TaskAssigns.RemoveRange(dbCon.TaskAssigns.Where(x => x.MeetingTask.MeetingAgenda.vMeetingID == CrAg.vMeetingID));
                            dbCon.MeetingTasks.RemoveRange(dbCon.MeetingTasks.Where(x => x.MeetingAgenda.vMeetingID == CrAg.vMeetingID));
                            dbCon.Decisions.RemoveRange(dbCon.Decisions.Where(x => x.MeetingAgenda.vMeetingID == CrAg.vMeetingID));
                            dbCon.MeetingAgendas.RemoveRange(dbCon.MeetingAgendas.Where(x => x.vMeetingID == CrAg.vMeetingID));
                            dbCon.SaveChanges();

                            //ADD (Agenda)
                            foreach (var ag in CrAg.AgendaList)
                            {
                                MeetingAgenda meAg = new MeetingAgenda();
                                meAg.vAgendaID = Guid.NewGuid().ToString();
                                meAg.vMeetingID = CrAg.vMeetingID;
                                meAg.vAgendaName = ag.vAgendaName;
                                meAg.vAgendaDetails = ag.vAgendaDetails;
                                meAg.iIndex = ag.iIndex;
                                dbCon.MeetingAgendas.Add(meAg);
                                dbCon.SaveChanges();

                                //ADD (MEETING TASK)
                                foreach (MeetingTask mt in ag.MeetingTasks)
                                {
                                    MeetingTask meTa = new MeetingTask();
                                    meTa.vTaskID = Guid.NewGuid().ToString();
                                    meTa.vAgendaID = meAg.vAgendaID;
                                    meTa.vTaskDetails = mt.vTaskDetails;
                                    meTa.iIndex = mt.iIndex;
                                    dbCon.MeetingTasks.Add(meTa);
                                    dbCon.SaveChanges();

                                    //ADD (TASK ASSIGN)
                                    foreach (TaskAssign ta in mt.TaskAssigns)
                                    {
                                        TaskAssign meTaAs = new TaskAssign();
                                        meTaAs.vTaskAssignID = Guid.NewGuid().ToString();
                                        meTaAs.vTaskID = meTa.vTaskID;
                                        meTaAs.Id = ta.Id;
                                        dbCon.TaskAssigns.Add(meTaAs);
                                        dbCon.SaveChanges();
                                    }
                                }

                                //ADD (DECISION)
                                foreach (Decision dec in ag.Decisions)
                                {
                                    Decision meDec = new Decision();
                                    meDec.vDecisionID = Guid.NewGuid().ToString();
                                    meDec.vAgendaID = meAg.vAgendaID;
                                    meDec.vDecisionDetails = dec.vDecisionDetails;
                                    meDec.iIndex = dec.iIndex;
                                    dbCon.Decisions.Add(meDec);
                                    dbCon.SaveChanges();
                                }
                            }

                            dbContextTransaction.Commit();
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            dbContextTransaction.Rollback();
                            return BadRequest(ex.InnerException.InnerException.Message);
                        }
                    }
                }
            }
        }

        [AllowAnonymous]
        [Route("RemoveData")]
        public async Task<IHttpActionResult> RemoveData(string id)
        {
            using (meetingminutesEntities dbCon = new meetingminutesEntities())
            {
                using (var dbContextTransaction = dbCon.Database.BeginTransaction())
                {
                    try
                    {
                        dbCon.MeetingInvites.RemoveRange(dbCon.MeetingInvites.Where(d => d.vMeetingID == id));
                        await dbCon.SaveChangesAsync();

                        dbCon.Decisions.RemoveRange(dbCon.Decisions.Where(d => d.MeetingAgenda.vMeetingID == id));
                        await dbCon.SaveChangesAsync();

                        dbCon.TaskAssigns.RemoveRange(dbCon.TaskAssigns.Where(d => d.MeetingTask.MeetingAgenda.vMeetingID == id));
                        await dbCon.SaveChangesAsync();

                        dbCon.MeetingTasks.RemoveRange(dbCon.MeetingTasks.Where(d => d.MeetingAgenda.vMeetingID == id));
                        await dbCon.SaveChangesAsync();

                        dbCon.MeetingAgendas.RemoveRange(dbCon.MeetingAgendas.Where(d => d.vMeetingID == id));
                        await dbCon.SaveChangesAsync();

                        dbCon.Meetings.RemoveRange(dbCon.Meetings.Where(d => d.vMeetingID == id));
                        await dbCon.SaveChangesAsync();

                        dbContextTransaction.Commit();
                        return Ok();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                        return BadRequest(ex.InnerException.InnerException.Message);
                    }
                }
            }
        }


    }
}