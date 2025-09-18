using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TMS.DATA.Model
{
    [Table("MAILDATA")]
    public class MailData
    {
        [Key]
        public int?                 MailDataId { get; set; }
        public string               MsgId { get; set; }   
        public string?              SenderemailAddress { get; set; }
        public string?              FromemailAddress { get; set; }
        public string?              ToRecipientemailAddress { get; set; }     
        public string?              CcRecipientemailAddress { get; set; }
        public string?              Subject { get; set; }
        public string?              BodycontentType { get; set; }
        public string?              Bodycontent { get; set; }
        public int?                 ISMigrate { get; set; }
        public int?                 CREATEDBY { get; set; }
        public DateTime?            CREATEDON { get; set; }
        public int?                 UPDATEDBY { get; set; }
        public DateTime?            UPDATEDON { get; set; }
    }

    [Table("MAILATTACHMENT")]
    public class MailAttachment
    {
        public int          MailAttachmentId { get; set; }
        public string       MsgId { get; set; }  // Foreign key to MailData

        public string        URL { get; set; }  // Foreign key to MailData
        //public string       ContentType { get; set; }
        //public string      ContentBytes { get; set; }  // Base64 string or file path
        //public int          Size { get; set; }
        public int?         CREATEDBY { get; set; }
        public DateTime?    CREATEDON { get; set; }
        public int?         UPDATEDBY { get; set; }
        public DateTime?    UPDATEDON { get; set; }
    }


    //[Table("MailAttachment")]
    //public class MailAttachment
    //{
    //    public int Id { get; set; }
    //    public int EmailId { get; set; }  // Foreign key to MailData
    //    public string Name { get; set; }
    //    public string ContentType { get; set; }
    //    public string ContentBytes { get; set; }  // Base64 string or file path
    //    public int Size { get; set; }
    //    public DateTime CreatedOn { get; set; }
    //}
}
