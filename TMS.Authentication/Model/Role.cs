using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TMS.Authentication.Model
{
    [Table("ROLE")]
    public class Role
    {
        [Key]
        public int?         ROLEID               { get; set; }
        public string?      ROLENAME             { get; set; }
        public string?      ROLEDESC             { get; set; }
        public int?         ISDELETED            { get; set; }
        public int?         CREATEDBY            { get; set; }
        public DateTime?    CREATEDON            { get; set; }
        public int?         UPDATEDBY            { get; set; }
        public DateTime?    UPDATEDON            { get; set; }
    }


    public class UserIdResultDTO
    {
        public int UserId { get; set; }
    }

}
