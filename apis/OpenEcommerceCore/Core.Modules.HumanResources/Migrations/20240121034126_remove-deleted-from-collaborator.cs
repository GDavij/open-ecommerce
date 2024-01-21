using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.HumanResources.Migrations
{
    public partial class removedeletedfromcollaborator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Address_Collaborators_CollaboratorId",
                table: "Address");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_States_StateId",
                table: "Address");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Collaborators");

            migrationBuilder.RenameTable(
                name: "Address",
                newName: "Addresses");

            migrationBuilder.RenameIndex(
                name: "IX_Address_StateId",
                table: "Addresses",
                newName: "IX_Addresses_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Address_CollaboratorId",
                table: "Addresses",
                newName: "IX_Addresses_CollaboratorId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Collaborators",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Collaborators_CollaboratorId",
                table: "Addresses",
                column: "CollaboratorId",
                principalTable: "Collaborators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_States_StateId",
                table: "Addresses",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Collaborators_CollaboratorId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_States_StateId",
                table: "Addresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "Address");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_StateId",
                table: "Address",
                newName: "IX_Address_StateId");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_CollaboratorId",
                table: "Address",
                newName: "IX_Address_CollaboratorId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Collaborators",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Collaborators",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Collaborators_CollaboratorId",
                table: "Address",
                column: "CollaboratorId",
                principalTable: "Collaborators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_States_StateId",
                table: "Address",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
