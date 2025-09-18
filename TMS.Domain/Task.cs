using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class Tassk
    {
 
        public int? TASKID                  { get; set; }
        public string? TASKSERIALNO         { get; set; } 
        public int? PROJECTID               { get; set; }
        public string? DESCRIPTION          { get; set; } 
        public int? TASKSPRIORITYID         { get; set; } 
        public int? TASKCATEGORYID          { get; set; }
        public DateTime? DUEDATE            { get; set; } 
        public int? USERID                  { get; set; }
        public int? ISDELETED               { get; set; }
        public int? CREATEDBY               { get; set; }
        public DateTime? CREATEDON          { get; set; }
        public int? UPDATEDBY               { get; set; }
        public DateTime? UPDATEDON          { get; set; }
    }




}
