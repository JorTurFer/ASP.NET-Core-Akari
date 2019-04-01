using Microsoft.EntityFrameworkCore.Migrations;

namespace Akari_Net.Core.Migrations
{
    public partial class sms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSMSSended",
                table: "CalendarEvents",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSMSSended",
                table: "CalendarEvents");
        }
    }
}
