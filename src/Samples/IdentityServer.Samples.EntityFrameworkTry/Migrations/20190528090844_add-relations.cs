using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentityServer.Samples.EntityFrameworkTry.Migrations
{
    public partial class addrelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                defaultValueSql: "CONVERT(date, GETDATE())",
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<byte[]>(
                name: "TStamp",
                table: "OrderDetails",
                rowVersion: true,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TStamp",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OrderDate",
                table: "Orders",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "CONVERT(date, GETDATE())");
        }
    }
}
