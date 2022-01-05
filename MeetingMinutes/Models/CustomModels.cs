using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MeetingMinutes.Models
{
    public class CustomRegisterModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ForgotPasswordSendMailModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class CreateMeeting
    {
        public string vMeetingID { get; set; }
        public string vTitle { get; set; }
        public System.DateTime dDate { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string vLocation { get; set; }
        public List<MeetingInvite> meetingInvitesList { get; set; }
    }

    public class CreateAgenda
    {
        public string vMeetingID { get; set; }
        public List<MeetingAgenda> AgendaList { get; set; }

    }

    public class ChangeEmail
    {
        [Required]
        [Display(Name = "OldEmail")]
        public string OldEmail { get; set; }

        [Required]
        [Display(Name = "NewEmail")]
        public string NewEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}