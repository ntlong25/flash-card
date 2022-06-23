using Microsoft.EntityFrameworkCore.Migrations;

namespace flash_card.data.Migrations
{
    public partial class custom_baseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DOB",
                table: "User",
                newName: "Dob");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dob",
                table: "User",
                newName: "DOB");
        }
    }
}
