using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.HumanResources.Migrations
{
    public partial class fixcaseinstateentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "States",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "States",
                newName: "name");
        }
    }
}
