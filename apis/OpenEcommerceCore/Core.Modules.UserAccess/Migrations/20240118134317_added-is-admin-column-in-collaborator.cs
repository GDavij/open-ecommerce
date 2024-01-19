using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.UserAccess.Migrations
{
    public partial class addedisadmincolumnincollaborator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Collaborators",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Collaborators");
        }
    }
}
