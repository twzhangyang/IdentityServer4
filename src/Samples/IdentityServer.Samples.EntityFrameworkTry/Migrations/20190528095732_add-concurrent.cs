using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Samples.EntityFrameworkTry.Migrations
{
    public partial class addconcurrent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Version",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Orders");
        }
    }
}
