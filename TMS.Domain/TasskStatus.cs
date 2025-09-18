using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class TasskStatus
    { 
        public int?                     TASKSSTATUSID { get; set; }
        public int?                     TASKID { get; set; }
        public string? TASKSERIALNO { get; set; }
        public int?                     STATUSID { get; set; } 
        public string?                  DESCRIPTION { get; set; }
        public int?                     TASKCATEGORYID { get; set; } 
        public int?                     TASKPRIORITYID { get; set; }
        public int?                     ISDELETED { get; set; }
        public int?                     CREATEDBY { get; set; }
        public DateTime?                CREATEDON { get; set; }
        public int?                     UPDATEDBY { get; set; }
        public DateTime?                UPDATEDON { get; set; }
    }
}
