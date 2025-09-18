using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;
using System.Threading.Tasks;

namespace TMS.Services.Model
{
    /// <summary>
    /// Tasks to Projects           : One-to-many (A project can have multiple tasks)
    //  Tasks to Users              : Many-to-one (Multiple tasks can be assigned to one user)
    /// </summary>
    /// 
    #region Master Table
    [Table("TASKSPRIORITY")]
    public class TASKSPRIORITY
    {
        [Key]
        public int? TASKSPRIORITYID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    [Table("TASKCATEGORY")]
    public class TASKCATEGORY
    {
        [Key]
        public int? TASKCATEGORYID { get; set; }
        public string? CATCODE { get; set; }

        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    [Table("STATUS")]
    public class STATUS
    {
        [Key]
        public int? STATUSID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    #endregion

    [Table("PROJECTS")]
    public class Project
    {
        [Key]
        public int? PROJECTID { get; set; }
        public string? PROJECTNAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public DateTime? STARTDATE { get; set; }
        public DateTime? ENDDATE { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    [Table("TASKS")]
    public class Tassk
    {
        [Key]
        public int? TASKID { get; set; }

        public string? TASKSERIALNO { get; set; }

        [ForeignKey("PROJECTID")]
        public int? PROJECTID { get; set; }
        public string? DESCRIPTION { get; set; }

        [ForeignKey("TASKSPRIORITYID")]
        public int? TASKSPRIORITYID { get; set; }

        [ForeignKey("TASKCATEGORYID")]
        public int? TASKCATEGORYID { get; set; }
        public DateTime? DUEDATE { get; set; }
        [ForeignKey("USERID")]
        public int? USERID { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    [Table("TASKSTATUS")]
    public class TasskStatus
    {
        [Key]
        public int? TASKSSTATUSID { get; set; }

        public string? TASKSERIALNO { get; set; }

        [ForeignKey("TASKID")]
        public int? TASKID { get; set; }

        [ForeignKey("STATUSID")]
        public int? STATUSID { get; set; }
        /// <summary>
        /// (e.g., "Open", "In Progress", "Completed")
        /// </summary>
        public string? DESCRIPTION { get; set; }

        [ForeignKey("TASKCATEGORYID")]
        public int? TASKCATEGORYID { get; set; }

        [ForeignKey("TASKPRIORITYID")]
        public int? TASKPRIORITYID { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    [Table("COMMENTS")]
    public class Comment
    {
        [Key]
        public int? COMMENTID { get; set; }

        public string? TASKSERIALNO { get; set; }

        [ForeignKey("TASKID")]
        public int? TASKID { get; set; }
        [ForeignKey("USERID")]
        public int? USERID { get; set; }

        public string? COMMENTTEXT { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }

    [Table("DOCUMENTS")]
    public class Document
    {
        [Key]
        public int? DOCUMENTID { get; set; }

        /// <summary>
        /// All three are foreign Key which is used ofr tracking the document which is uploaded by enduser
        /// </summary>
        [ForeignKey("PROJECTID")]
        public int? PROJECTID { get; set; }

        public string? TASKSERIALNO { get; set; }

        [ForeignKey("TASKID")]
        public int? TASKID { get; set; }

        [ForeignKey("TASKSSTATUSID")]
        public int? TASKSSTATUSID { get; set; }

        public string? DOCUMENTURL { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }



    [Table("ASSIGNOPERATION")]
    public class TaskAssign
    {
        [Key]
        public int? TASKASSIGNID { get; set; }

        public string? TASKSERIALNO { get; set; }

        [ForeignKey("TASKID")]
        public int? TASKID { get; set; }

        public int? WHOASSIGNED { get; set; }

        public int? WHICHDEVELOPERASSIGNED { get; set; }

        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }


    [Table("TASKEMAIL")]
    public class TaskEmail
    {
        [Key]
        public int? TASKEMAILID { get; set; }

        public string? TASKSERIALNO { get; set; }

        [ForeignKey("TASKID")]
        public int? TASKID { get; set; }


        public string? SENDEREMAILADDRESS { get; set; }

        public string? FROMEMAILADDRESS { get; set; }

        public string? TORECIPIENTEMAILADDRESS { get; set; }

        public string? CCRECIPIENTEMAILADDRESS { get; set; }
        public string? SUBJECT { get; set; }

        public string? BODYCONTENTTYPE { get; set; }

        public string? BODYCONTENT { get; set; }

        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }


    [Table("TASKEMAILATTACHMENT")]
    public class TASKEMAILATTACHMENT
    {
        [Key]
        public int? TASKEMAILATTACHMENTID { get; set; }
        public string? TASKSERIALNO { get; set; }
        [ForeignKey("TASKID")]
        public int? TASKID { get; set; }
        public string? URL { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }



    public class AssignmentDetailDto
    {
        public string AssignmentDetails { get; set; }
    }

    public class UserIdResultDTO
    {
        public int UserId { get; set; }
    }

    public class DeleteTaskIdDTO
    {
        public int TaskId { get; set; }
    }

    public class UserNameResultDTO
    {
        public string UserName { get; set; }
    }

    public class UserEmailResultDTO
    {
        public string UserEmail { get; set; }
    }

    #region dashboard part
    public class DashboardPart1tDTO
    {
        public string FirstResponseTime { get; set; }
        public string CustomerSatisfaction { get; set; }
        public string FullResoltuionTime { get; set; }
        public string ResoltuionInFirstCall { get; set; }
        public string TodayTicket { get; set; }
        public string New { get; set; }
        public string InProgress { get; set; }
        public string Resolved { get; set; }
        public string Closed { get; set; }
    }
    public class DashboardPart1TO4tDTO
    {
        public string Title { get; set; }
    }

    public class DashboardPart5DTO
    {
        public string labels { get; set; }
    }

    public class DashboardPart6DTO
    {
        public string? ticketsSubmitted { get; set; }
    }
    public class DashboardPart7DTO
    {
        public string? ticketsSolved { get; set; }
    }
    #endregion
    #region Analytics part 
    public class AnalyticsPart1tDTO
    {
        public int? NoofTask { get; set; }
        public string? AvgTimeResolve { get; set; }
    }
    public class AnalyticsPart2tDTO
    {
        public int? New { get; set; }
        public int? Inprocess { get; set; }
        public int? Resolved { get; set; }
        public int? Hold { get; set; }
        public int? Closed { get; set; }
    }

    public class AnalyticsPart3tDTO
    {  
         public string? Data { get; set; }
    }

    public class AnalyticsPart5tDTO
    {
        public string? labels { get; set; }
        public string? ticketsunResolved { get; set; }
        public string? ticketSolved { get; set; }
    }
    #endregion
    #region HQ & TeamFeed
    public class HQDTO
    { 
        public string? Time { get; set; }
        public string? Incoming { get; set; }
        public string? Outgoing { get; set; }
        public string? LasthourIncoming { get; set; }
        public string? LasthourOutgoing { get; set; }
        public string? UnassignedData { get; set; }
        public string? DeveloperData { get; set; }
        public string? CustomerData { get; set; }
        public string? LatestTaskData { get; set; }
    }
    public class TeamFeed
    {
        public string? Data { get; set; } 
    }
    #endregion
    [Table("PROJECTASSIGNMENT")]
    public class ProjectAssignment
    {
        [Key]
        public int?         PROJECTASSIGNMENTID { get; set; }
        public int?         PROJECTID { get; set; }
        public int?         USERID { get; set; }
        public int?         ISDELETED { get; set; }
        public int?         CREATEDBY { get; set; }
        public DateTime?    CREATEDON { get; set; }
        public int?         UPDATEDBY { get; set; }
        public DateTime?    UPDATEDON { get; set; }
    }
}


