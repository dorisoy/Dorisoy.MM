//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MeetingMinutes.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MeetingAgenda
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MeetingAgenda()
        {
            this.Decisions = new HashSet<Decision>();
            this.MeetingTasks = new HashSet<MeetingTask>();
        }
    
        public string vAgendaID { get; set; }
        public string vMeetingID { get; set; }
        public string vAgendaName { get; set; }
        public string vAgendaDetails { get; set; }
        public int iIndex { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Decision> Decisions { get; set; }
        public virtual Meeting Meeting { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeetingTask> MeetingTasks { get; set; }
    }
}
