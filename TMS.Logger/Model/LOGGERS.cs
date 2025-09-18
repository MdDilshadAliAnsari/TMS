using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.Logger.Model
{
    [Table("LOGGERS")]
    public class LOGGERS
    { 
        [Key]
        public int?             LoggerId                { get; set; }
        public string?          UserName                { get; set; }
        public string?          ExceptionType           { get; set; }
        public string?          ExceptionMessage        { get; set; }
        public string?          ControllerName          { get; set; }
        public string?          ActionName              { get; set; } 
        public string?          IP                      { get; set; }
        public string?          ExceptionStackTrace     { get; set; }
        public DateTime?        LogTime                 { get; set; }

    }
}
