using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.HumanResources.Migrations
{
    public partial class addstateentityplussomeminimalmodifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                table: "Address");

            migrationBuilder.AddColumn<decimal>(
                name: "MonthlySalary",
                table: "Contracts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Sector",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "StateId",
                table: "Address",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    ShortName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_StateId",
                table: "Address",
                column: "StateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_States_StateId",
                table: "Address",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_States_StateId",
                table: "Address");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropIndex(
                name: "IX_Address_StateId",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "MonthlySalary",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "StateId",
                table: "Address");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "Address",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                defaultValue: "");
        }
    }
}
