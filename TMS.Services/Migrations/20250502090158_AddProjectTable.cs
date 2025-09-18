using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TMS.Services.Migrations
{
  
    public partial class AddProjectTable : Migration
    {
     
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
                migrationBuilder.EnsureSchema(
                name: "TMS");

                migrationBuilder.CreateTable(
                    name: "COMMENTS",
                    schema: "TMS",
                    columns: table => new
                    {
                        COMMENTID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        TASKID = table.Column<int>(type: "int", nullable: true),
                        TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        USERID = table.Column<int>(type: "int", nullable: true),
                        COMMENTTEXT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_COMMENTS", x => x.COMMENTID);
                    });

                migrationBuilder.CreateTable(
                    name: "DOCUMENTS",
                    schema: "TMS",
                    columns: table => new
                    {
                        DOCUMENTID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        PROJECTID = table.Column<int>(type: "int", nullable: true),
                        TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        TASKID = table.Column<int>(type: "int", nullable: true),
                        TASKSSTATUSID = table.Column<int>(type: "int", nullable: true),
                        DOCUMENTURL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_DOCUMENTS", x => x.DOCUMENTID);
                    });

                migrationBuilder.CreateTable(
                    name: "PROJECTS",
                    schema: "TMS",
                    columns: table => new
                    {
                        PROJECTID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        PROJECTNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        STARTDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                        ENDDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_PROJECTS", x => x.PROJECTID);
                    });

                migrationBuilder.CreateTable(
                    name: "STATUS",
                    schema: "TMS",
                    columns: table => new
                    {
                        STATUSID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_STATUS", x => x.STATUSID);
                    });

                migrationBuilder.CreateTable(
                    name: "TASKCATEGORY",
                    schema: "TMS",
                    columns: table => new
                    {
                        TASKCATEGORYID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TASKCATEGORY", x => x.TASKCATEGORYID);
                    });

                migrationBuilder.CreateTable(
                    name: "TASKS",
                    schema: "TMS",
                    columns: table => new
                    {
                        TASKID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        PROJECTID = table.Column<int>(type: "int", nullable: true),
                        DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        TASKSPRIORITYID = table.Column<int>(type: "int", nullable: true),
                        TASKCATEGORYID = table.Column<int>(type: "int", nullable: true),
                        DUEDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                        USERID = table.Column<int>(type: "int", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TASKS", x => x.TASKID);
                    });

                migrationBuilder.CreateTable(
                    name: "TASKSPRIORITY",
                    schema: "TMS",
                    columns: table => new
                    {
                        TASKSPRIORITYID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        NAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TASKSPRIORITY", x => x.TASKSPRIORITYID);
                    });

                migrationBuilder.CreateTable(
                    name: "TASKSTATUS",
                    schema: "TMS",
                    columns: table => new
                    {
                        TASKSSTATUSID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        TASKID = table.Column<int>(type: "int", nullable: true),
                        TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        STATUSID = table.Column<int>(type: "int", nullable: true),
                        DESCRIPTION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        TASKCATEGORYID = table.Column<int>(type: "int", nullable: true),
                        TASKPRIORITYID = table.Column<int>(type: "int", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_TASKSTATUS", x => x.TASKSSTATUSID);
                    });


                migrationBuilder.CreateTable(
                   name: "ASSIGNOPERATION",
                   schema: "TMS",
                   columns: table => new
                   {
                       TASKASSIGNID = table.Column<int>(type: "int", nullable: false)
                           .Annotation("SqlServer:Identity", "1, 1"),
                       TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                       TASKID = table.Column<int>(type: "int", nullable: true),
                       WHOASSIGNED = table.Column<int>(type: "int", nullable: true),
                       WHICHDEVELOPERASSIGNED = table.Column<int>(type: "int", nullable: true),
                       ISDELETED = table.Column<int>(type: "int", nullable: true),
                       CREATEDBY = table.Column<int>(type: "int", nullable: true),
                       CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                       UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                       UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                   },
                   constraints: table =>
                   {
                       table.PrimaryKey("PK_ASSIGNOPERATION", x => x.TASKASSIGNID);
                   });


                migrationBuilder.CreateTable(
                name: "TASKEMAIL",
                schema: "TMS",
                columns: table => new
                {
                    TASKEMAILID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TASKID = table.Column<int>(type: "int", nullable: true),
                    SENDEREMAILADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FROMEMAILADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TORECIPIENTEMAILADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CCRECIPIENTEMAILADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SUBJECT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BODYCONTENTTYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BODYCONTENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISDELETED = table.Column<int>(type: "int", nullable: true),
                    CREATEDBY = table.Column<int>(type: "int", nullable: true),
                    CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                    UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASKEMAIL", x => x.TASKEMAILID);
                });



                migrationBuilder.CreateTable(
                name: "TASKEMAILATTACHMENT",
                schema: "TMS",
                columns: table => new
                {
                    TASKEMAILATTACHMENTID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TASKSERIALNO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TASKID = table.Column<int>(type: "int", nullable: true),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CREATEDBY = table.Column<int>(type: "int", nullable: true),
                    CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                    UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASKEMAILATTACHMENT", x => x.TASKEMAILATTACHMENTID);
                });


 


                migrationBuilder.InsertData(
                    schema: "TMS",
                    table: "STATUS",
                    columns: new[] { "STATUSID", "CREATEDBY", "CREATEDON", "DESCRIPTION", "ISDELETED", "NAME", "UPDATEDBY", "UPDATEDON" },
                    values: new object[,]
                    {
                    { 1, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "This task is new", 0, "New", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "This task is in Progress.", 0, "In Progress", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "This task resolved in Developement Environment", 0, "Resolved", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "This task already Closed.", 0, "Closed", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "This task on Hold.", 0, "Hold", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                    });

                migrationBuilder.InsertData(
                    schema: "TMS",
                    table: "TASKCATEGORY",
                    columns: new[] { "TASKCATEGORYID", "CREATEDBY", "CREATEDON", "DESCRIPTION", "ISDELETED", "NAME", "UPDATEDBY", "UPDATEDON" },
                    values: new object[,]
                    {
                    { 1, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "A flaw in the software or system that causes incorrect or unexpected behavior", 0, "Bug / Defect", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "A suggestion to add new functionality or improve existing features.", 0, "Feature Request", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Help with resolving technical problems, including hardware, software, or network issues", 0, "Technical Support", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "A formal request for something standard (e.g., access, install, information).", 0, "Service Request-Technical", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "A formal request for something standard (e.g., access, install, information).", 0, "Service Request-Documentation", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "An unplanned interruption to a service or a reduction in service quality.", 0, "Incident", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "User cannot access a system, file, or application.", 0, "Access / Permission Issue", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Login problems, password resets, locked accounts.", 0, "Account Issues", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "System/application is slow or unresponsive.", 0, "Performance Issue", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Non-critical feedback about the interface or usability.", 0, "UI / UX Feedback", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                    });

                migrationBuilder.InsertData(
                    schema: "TMS",
                    table: "TASKSPRIORITY",
                    columns: new[] { "TASKSPRIORITYID", "CREATEDBY", "CREATEDON", "DESCRIPTION", "ISDELETED", "NAME", "UPDATEDBY", "UPDATEDON" },
                    values: new object[,]
                    {
                    { 1, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Low Priority", 0, "LOW", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medium Priority.", 0, "Medium", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "High Priority", 0, "Hign", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Critical  Priority.", 0, "Critical", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                    });
           
               
             
        }

       
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
                migrationBuilder.DropTable(
                name: "COMMENTS",
                schema: "TMS");

                migrationBuilder.DropTable(
                    name: "DOCUMENTS",
                    schema: "TMS");

                migrationBuilder.DropTable(
                    name: "PROJECTS",
                    schema: "TMS");

                migrationBuilder.DropTable(
                    name: "STATUS",
                    schema: "TMS");

                migrationBuilder.DropTable(
                    name: "TASKCATEGORY",
                    schema: "TMS");

                migrationBuilder.DropTable(
                    name: "TASKS",
                    schema: "TMS");

                migrationBuilder.DropTable(
                    name: "TASKSPRIORITY",
                    schema: "TMS");

                migrationBuilder.DropTable(
                    name: "TASKSTATUS",
                    schema: "TMS");

                migrationBuilder.DropTable(
                  name: "ASSIGNOPERATION",
                  schema: "TMS");

                migrationBuilder.DropTable(
                   name: "TASKEMAIL",
                   schema: "TMS");

                migrationBuilder.DropTable(
                  name: "TASKEMAILATTACHMENT",
                  schema: "TMS");


 
            
        }
    }
}
