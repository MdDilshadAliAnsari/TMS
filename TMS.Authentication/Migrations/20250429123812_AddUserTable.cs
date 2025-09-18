using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TMS.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           // var tableExists = migrationBuilder.Sql(@"SELECT COUNT(*) FROM all_tables WHERE table_name IN( 'ROLE','USERS') AND owner = 'TMS'").ToString();

            //if (tableExists == "0")
            //{


                migrationBuilder.EnsureSchema(
                name: "TMS");

                migrationBuilder.CreateTable(
                    name: "ROLE",
                    schema: "TMS",
                    columns: table => new
                    {
                        ROLEID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        ROLENAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ROLEDESC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_ROLE", x => x.ROLEID);
                    });

                migrationBuilder.CreateTable(
                    name: "USERS",
                    schema: "TMS",
                    columns: table => new
                    {
                        USERID = table.Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        PWD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        ROLEID = table.Column<int>(type: "int", nullable: true),
                        FIRSTNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        LASTNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        EMAILID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        MOBILENO = table.Column<string>(type: "nvarchar(max)", nullable: true),
                        PWDEXPIRTYDATE = table.Column<DateTime>(type: "datetime2", nullable: true),
                        INCORRECTLOGINS = table.Column<int>(type: "int", nullable: true),
                        LOGINTIME = table.Column<DateTime>(type: "datetime2", nullable: true),
                        DORMANTSTATUS = table.Column<int>(type: "int", nullable: true),
                        ISDELETED = table.Column<int>(type: "int", nullable: true),
                        CREATEDBY = table.Column<int>(type: "int", nullable: true),
                        CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                        UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                        UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("PK_USERS", x => x.USERID);
                    });

                migrationBuilder.InsertData(
                    schema: "TMS",
                    table: "ROLE",
                    columns: new[] { "ROLEID", "CREATEDBY", "CREATEDON", "ISDELETED", "ROLEDESC", "ROLENAME", "UPDATEDBY", "UPDATEDON" },
                    values: new object[,]
                    {
                    { 1, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "It has all access right", "Super Admin", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "It has all access right.", "Admin", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Customer access right to see their task and progress and create ticket as well.", "Customer", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "Developer access right to see their task.", "Developer", 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                    });

                migrationBuilder.InsertData(
                    schema: "TMS",
                    table: "USERS",
                    columns: new[] { "USERID", "CREATEDBY", "CREATEDON", "DORMANTSTATUS", "EMAILID", "FIRSTNAME", "INCORRECTLOGINS", "ISDELETED", "LASTNAME", "LOGINTIME", "MOBILENO", "PWD", "PWDEXPIRTYDATE", "ROLEID", "UPDATEDBY", "UPDATEDON", "USERNAME" },
                    values: new object[,]
                    {
                    { 1, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "dilshadali.md@navayugainfotech.com", "Dilshad", 0, 0, "ANSARI", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "9911859088", "123", new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dilshad123" },
                    { 2, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "venkataramana.tilla@navayugainfotech.com", "Venkat", 0, 0, "Tilla", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "9911834023", "123", new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Venkat123" },
                    { 3, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "manohar.panjala@navayugainfotech.com", "Venkat", 0, 0, "Tilla", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "9911675433", "123", new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Manu123" },
                    { 4, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 0, "karunababu.kondisetti@navayugainfotech.com", "Karuna", 0, 0, "Babu", new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "9911834023", "123", new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, 1, new DateTime(2025, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Venkat123" }
                    });
            //}
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ROLE",
                schema: "TMS");

            migrationBuilder.DropTable(
                name: "USERS",
                schema: "TMS");
        }
    }
}
