using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.DATA.Migrations
{
    /// <inheritdoc />
    public partial class EmailData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "TMS");

            migrationBuilder.CreateTable(
                name: "MAILDATA",
                schema: "TMS",
                columns: table => new
                {
                    MailDataId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MsgId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderemailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FromemailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ToRecipientemailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CcRecipientemailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BodycontentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Bodycontent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISMigrate = table.Column<int>(type: "int", nullable: true),
                    CREATEDBY = table.Column<int>(type: "int", nullable: true),
                    CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                    UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAILDATA", x => x.MailDataId);
                });


            migrationBuilder.CreateTable(
                name: "MAILATTACHMENT",
                schema: "TMS",
                columns: table => new
                {
                    MailAttachmentId    = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MsgId               = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url                 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    //ContentType         = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    //ContentBytes        = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    //Size                = table.Column<int>(type: "int", nullable: true), 
                    CREATEDBY           = table.Column<int>(type: "int", nullable: true),
                    CREATEDON           = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UPDATEDBY           = table.Column<int>(type: "int", nullable: true),
                    UPDATEDON           = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MAILATTACHMENT", x => x.MailAttachmentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MAILDATA",
                schema: "TMS");
        }
    }
}
