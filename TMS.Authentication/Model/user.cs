using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TMS.Authentication.Model
{
    [Table("USERS")]
    public class user
    {
        [Key]
        public int?             USERID                  { get; set; }
        public string?          USERNAME                { get; set; }
        public string?          PWD                     { get; set; }

        [ForeignKey("ROLEID")]
        public int?             ROLEID                  { get; set; }
        public string?          FIRSTNAME               { get; set; }
        public string?          LASTNAME                { get; set; }
        public string?          EMAILID                 { get; set; }
        public string?          MOBILENO                { get; set; }
        public DateTime?        PWDEXPIRTYDATE          { get; set; }
        public int?             INCORRECTLOGINS         { get; set; }
        public DateTime?        LOGINTIME               { get; set; }
        public int?             DORMANTSTATUS           { get; set; }
        public int?             ISDELETED               { get; set; }
        public int?             CREATEDBY               { get; set; }
        public DateTime?        CREATEDON               { get; set; }
        public int?             UPDATEDBY               { get; set; }
        public DateTime?        UPDATEDON               { get; set; }

    }
    public class JWTTokenResponse
    { 
        public string? Token { get; set; }
        public string? USERNAME { get; set; }
    }
    public class TokenApiModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
