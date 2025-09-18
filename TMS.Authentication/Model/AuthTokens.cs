using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using TMS.Domain;

namespace TMS.Authentication.Model
{
    [Table("AUTHTOKEN")]
    public class AuthTokens
    { 
            [Key]
            public int? AUTHTOKENID             { get; set; } 
            public string ? TOKEN               { get; set; }         //  Unique identifier for the token        
            public string? REFRESHTOKEN         { get; set; }         //  Unique identifier for the token   
            public string? TOKENTYPE            { get; set; }        // 'access', -- e.g., access, refresh, api_key
            [ForeignKey("USERID")]
            public int? USERID                  { get; set; }        //-- FK to Users table 
            public string? USERNAME             { get; set; }                   //--  
            public DateTime? ISSUEDAT           { get; set; }       // -- Token generation time
            public DateTime? EXPIREDAT          { get; set; }       //-- Expiration time
            public int? ISREVOKED               { get; set; }      //-- Indicates if manually revoked (logout, admin action)
            public string? IPADDRESS            { get; set; }      //-- IP address (IPv4/IPv6) where token was issued
            public string? USERAGENT            { get; set; }      //---Browser or device info
            public int? ISDELETED               { get; set; }
            public int? CREATEDBY               { get; set; }
            public DateTime? CREATEDON          { get; set; }
            public int? UPDATEDBY               { get; set; }
            public DateTime? UPDATEDON          { get; set; }
        
    }
}
