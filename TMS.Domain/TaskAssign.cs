using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class TaskAssign
    {

        public int? TASKASSIGNID { get; set; }
        public string? TASKSERIALNO { get; set; }

        public int? TASKID { get; set; }

        public int? WHOASSIGNED { get; set; }

        public int? WHICHDEVELOPERASSIGNED { get; set; }

        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }
     
    public class TaskEmail
    {
    
        public int? TASKEMAILID { get; set; }

        public string? TASKSERIALNO { get; set; }


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
}
