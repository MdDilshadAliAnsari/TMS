using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Services.Migrations
{
    /// <inheritdoc />
    public partial class AddProjAssignTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.EnsureSchema(
            name: "TMS");

            migrationBuilder.CreateTable(
                 name: "PROJECTASSIGNMENT",
                 schema: "TMS",
                 columns: table => new
                 {
                     PROJECTASSIGNMENTID = table.Column<int>(type: "int", nullable: false)
                         .Annotation("SqlServer:Identity", "1, 1"),
                     PROJECTID = table.Column<int>(type: "int", nullable: true),
                     USERID = table.Column<int>(type: "int", nullable: true),
                     ISDELETED = table.Column<int>(type: "int", nullable: true),
                     CREATEDBY = table.Column<int>(type: "int", nullable: true),
                     CREATEDON = table.Column<DateTime>(type: "datetime2", nullable: true),
                     UPDATEDBY = table.Column<int>(type: "int", nullable: true),
                     UPDATEDON = table.Column<DateTime>(type: "datetime2", nullable: true)
                 },
                 constraints: table =>
                 {
                     table.PrimaryKey("PK_PROJECTASSIGNMENT", x => x.PROJECTASSIGNMENTID);
                 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
           name: "PROJECTASSIGNMENT",
           schema: "TMS");

        }
    }
}
