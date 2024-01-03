using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.Stock.Migrations
{
    public partial class fixbranddescriptionnonnullableconstraint : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Distributors_Addresses_AddressId",
                table: "Distributors");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTag_TagsId",
                table: "ProductProductTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSupplier_Distributors_SuppliersId",
                table: "ProductSupplier");

            migrationBuilder.DropTable(
                name: "ProductImage");

            migrationBuilder.DropTable(
                name: "ProductTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Distributors",
                table: "Distributors");

            migrationBuilder.RenameTable(
                name: "Distributors",
                newName: "Suppliers");

            migrationBuilder.RenameIndex(
                name: "IX_Distributors_AddressId",
                table: "Suppliers",
                newName: "IX_Suppliers_AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Brands",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(384)", maxLength: 384, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTags_TagsId",
                table: "ProductProductTag",
                column: "TagsId",
                principalTable: "ProductTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSupplier_Suppliers_SuppliersId",
                table: "ProductSupplier",
                column: "SuppliersId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Suppliers_Addresses_AddressId",
                table: "Suppliers",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductProductTag_ProductTags_TagsId",
                table: "ProductProductTag");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductSupplier_Suppliers_SuppliersId",
                table: "ProductSupplier");

            migrationBuilder.DropForeignKey(
                name: "FK_Suppliers_Addresses_AddressId",
                table: "Suppliers");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "Distributors");

            migrationBuilder.RenameIndex(
                name: "IX_Suppliers_AddressId",
                table: "Distributors",
                newName: "IX_Distributors_AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Brands",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Distributors",
                table: "Distributors",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ProductImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Url = table.Column<string>(type: "character varying(384)", maxLength: 384, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImage_Products_Id",
                        column: x => x.Id,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTag",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TagName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTag", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Distributors_Addresses_AddressId",
                table: "Distributors",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductProductTag_ProductTag_TagsId",
                table: "ProductProductTag",
                column: "TagsId",
                principalTable: "ProductTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductSupplier_Distributors_SuppliersId",
                table: "ProductSupplier",
                column: "SuppliersId",
                principalTable: "Distributors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
