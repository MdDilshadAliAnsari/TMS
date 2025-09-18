using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Authentication.Migrations
{
    /// <inheritdoc />
    public partial class AuthTokensTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUTHTOKEN",
                schema: "TMS",
                columns: table => new
                {
                    AUTHTOKENID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TOKEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    REFRESHTOKEN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TOKENTYPE = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USERID = table.Column<int>(type: "int", nullable: true),
                    USERNAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISSUEDAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EXPIREDAT = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ISREVOKED = table.Column<int>(type: "int", nullable: true),
                    IPADDRESS = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    USERAGENT = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISDELETED = table.Column<int>(type: "int", nullable: true),
                    CREATEDBY = table.Column<int>(type: "int", nullable: true),
                    CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                    UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUTHTOKEN", x => x.AUTHTOKENID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUTHTOKEN",
                schema: "TMS");
        }
    }
}
