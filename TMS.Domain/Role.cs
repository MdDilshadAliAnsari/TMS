using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class Role
    { 
        public int?                 ROLEID { get; set; }
        public string?              ROLENAME { get; set; }
        public string?              ROLEDESC { get; set; }
        public int?                 ISDELETED { get; set; }
        public int?                 CREATEDBY { get; set; }
        public DateTime?            CREATEDON { get; set; }
        public int?                 UPDATEDBY { get; set; }
        public DateTime?            UPDATEDON { get; set; }
    }

    public class TASKSPRIORITY
    {

        public int? TASKSPRIORITYID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }
    public class TASKCATEGORY
    {

        public int? TASKCATEGORYID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }
    public class STATUS
    {
        public int? STATUSID { get; set; }
        public string? NAME { get; set; }
        public string? DESCRIPTION { get; set; }
        public int? ISDELETED { get; set; }
        public int? CREATEDBY { get; set; }
        public DateTime? CREATEDON { get; set; }
        public int? UPDATEDBY { get; set; }
        public DateTime? UPDATEDON { get; set; }
    }
}
