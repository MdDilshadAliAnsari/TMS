using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Domain
{
    public class user
    { 
        public int?                     USERID              { get; set; }
        public string?                  USERNAME            { get; set; }
        public string?                  PWD                 { get; set; } 
        public int?                     ROLEID              { get; set; }
        public string?                  FIRSTNAME           { get; set; }
        public string?                  LASTNAME            { get; set; }
        public string?                  EMAILID             { get; set; }
        public string?                  MOBILENO            { get; set; }
        public DateTime?                PWDEXPIRTYDATE      { get; set; }
        public int?                     INCORRECTLOGINS     { get; set; }
        public DateTime?                LOGINTIME           { get; set; }
        public int?                     DORMANTSTATUS       { get; set; }
        public int?                     ISDELETED           { get; set; }
        public int?                     CREATEDBY           { get; set; }
        public DateTime?                CREATEDON           { get; set; }
        public int?                     UPDATEDBY           { get; set; }
        public DateTime?                UPDATEDON           { get; set; }

    }
}
