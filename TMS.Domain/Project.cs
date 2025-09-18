using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class Project
    { 
        public int?                 PROJECTID { get; set; }
        public string?              PROJECTNAME { get; set; }
        public string?              DESCRIPTION { get; set; }
        public DateTime?            STARTDATE { get; set; }
        public DateTime?            ENDDATE { get; set; }
        public int?                 ISDELETED { get; set; }
        public int?                 CREATEDBY { get; set; }
        public DateTime?            CREATEDON { get; set; }
        public int?                 UPDATEDBY { get; set; }
        public DateTime?            UPDATEDON { get; set; }
    }
}
