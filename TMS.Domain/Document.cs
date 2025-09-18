using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class Document
    {
   
        public int?                     DOCUMENTID { get; set; } 
        public int?                     PROJECTID { get; set; }

        public string? TASKSERIALNO { get; set; }
        public int?                     TASKID { get; set; }
        public int?                     TASKSSTATUSID { get; set; }
        public string?                  DOCUMENTURL { get; set; }
        public int?                     ISDELETED { get; set; }
        public int?                     CREATEDBY { get; set; }
        public DateTime?                CREATEDON { get; set; }
        public int?                     UPDATEDBY { get; set; }
        public DateTime?                UPDATEDON { get; set; }
    }
}
