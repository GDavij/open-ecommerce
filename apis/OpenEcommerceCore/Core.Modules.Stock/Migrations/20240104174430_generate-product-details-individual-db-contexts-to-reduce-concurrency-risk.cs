using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.Stock.Migrations
{
    public partial class generateproductdetailsindividualdbcontextstoreduceconcurrencyrisk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementDetail_MeasureUnits_MeasureUnitId",
                table: "MeasurementDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_MeasurementDetail_Products_ProductId",
                table: "MeasurementDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherDetail_MeasureUnits_MeasureUnitId",
                table: "OtherDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OtherDetail_Products_ProductId",
                table: "OtherDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalDetail_MeasureUnits_MeasureUnitId",
                table: "TechnicalDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnicalDetail_Products_ProductId",
                table: "TechnicalDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TechnicalDetail",
                table: "TechnicalDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OtherDetail",
                table: "OtherDetail");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MeasurementDetail",
                table: "MeasurementDetail");

            migrationBuilder.RenameTable(
                name: "TechnicalDetail",
                newName: "Products_TechnicalDetails");

            migrationBuilder.RenameTable(
                name: "OtherDetail",
                newName: "Products_OtherDetails");

            migrationBuilder.RenameTable(
                name: "MeasurementDetail",
                newName: "Products_MeasureDetails");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalDetail_ProductId",
                table: "Products_TechnicalDetails",
                newName: "IX_Products_TechnicalDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_TechnicalDetail_MeasureUnitId",
                table: "Products_TechnicalDetails",
                newName: "IX_Products_TechnicalDetails_MeasureUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_OtherDetail_ProductId",
                table: "Products_OtherDetails",
                newName: "IX_Products_OtherDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_OtherDetail_MeasureUnitId",
                table: "Products_OtherDetails",
                newName: "IX_Products_OtherDetails_MeasureUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_MeasurementDetail_ProductId",
                table: "Products_MeasureDetails",
                newName: "IX_Products_MeasureDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_MeasurementDetail_MeasureUnitId",
                table: "Products_MeasureDetails",
                newName: "IX_Products_MeasureDetails_MeasureUnitId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Products_TechnicalDetails",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products_TechnicalDetails",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Products_OtherDetails",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products_OtherDetails",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "Products_MeasureDetails",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products_MeasureDetails",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_TechnicalDetails",
                table: "Products_TechnicalDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_OtherDetails",
                table: "Products_OtherDetails",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products_MeasureDetails",
                table: "Products_MeasureDetails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MeasureDetails_MeasureUnits_MeasureUnitId",
                table: "Products_MeasureDetails",
                column: "MeasureUnitId",
                principalTable: "MeasureUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MeasureDetails_Products_ProductId",
                table: "Products_MeasureDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OtherDetails_MeasureUnits_MeasureUnitId",
                table: "Products_OtherDetails",
                column: "MeasureUnitId",
                principalTable: "MeasureUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_OtherDetails_Products_ProductId",
                table: "Products_OtherDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TechnicalDetails_MeasureUnits_MeasureUnitId",
                table: "Products_TechnicalDetails",
                column: "MeasureUnitId",
                principalTable: "MeasureUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_TechnicalDetails_Products_ProductId",
                table: "Products_TechnicalDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MeasureDetails_MeasureUnits_MeasureUnitId",
                table: "Products_MeasureDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_MeasureDetails_Products_ProductId",
                table: "Products_MeasureDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_OtherDetails_MeasureUnits_MeasureUnitId",
                table: "Products_OtherDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_OtherDetails_Products_ProductId",
                table: "Products_OtherDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_TechnicalDetails_MeasureUnits_MeasureUnitId",
                table: "Products_TechnicalDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_TechnicalDetails_Products_ProductId",
                table: "Products_TechnicalDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_TechnicalDetails",
                table: "Products_TechnicalDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_OtherDetails",
                table: "Products_OtherDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products_MeasureDetails",
                table: "Products_MeasureDetails");

            migrationBuilder.RenameTable(
                name: "Products_TechnicalDetails",
                newName: "TechnicalDetail");

            migrationBuilder.RenameTable(
                name: "Products_OtherDetails",
                newName: "OtherDetail");

            migrationBuilder.RenameTable(
                name: "Products_MeasureDetails",
                newName: "MeasurementDetail");

            migrationBuilder.RenameIndex(
                name: "IX_Products_TechnicalDetails_ProductId",
                table: "TechnicalDetail",
                newName: "IX_TechnicalDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_TechnicalDetails_MeasureUnitId",
                table: "TechnicalDetail",
                newName: "IX_TechnicalDetail_MeasureUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_OtherDetails_ProductId",
                table: "OtherDetail",
                newName: "IX_OtherDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_OtherDetails_MeasureUnitId",
                table: "OtherDetail",
                newName: "IX_OtherDetail_MeasureUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_MeasureDetails_ProductId",
                table: "MeasurementDetail",
                newName: "IX_MeasurementDetail_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_MeasureDetails_MeasureUnitId",
                table: "MeasurementDetail",
                newName: "IX_MeasurementDetail_MeasureUnitId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "TechnicalDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TechnicalDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "OtherDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "OtherDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "MeasurementDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "MeasurementDetail",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TechnicalDetail",
                table: "TechnicalDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OtherDetail",
                table: "OtherDetail",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeasurementDetail",
                table: "MeasurementDetail",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementDetail_MeasureUnits_MeasureUnitId",
                table: "MeasurementDetail",
                column: "MeasureUnitId",
                principalTable: "MeasureUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MeasurementDetail_Products_ProductId",
                table: "MeasurementDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OtherDetail_MeasureUnits_MeasureUnitId",
                table: "OtherDetail",
                column: "MeasureUnitId",
                principalTable: "MeasureUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OtherDetail_Products_ProductId",
                table: "OtherDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalDetail_MeasureUnits_MeasureUnitId",
                table: "TechnicalDetail",
                column: "MeasureUnitId",
                principalTable: "MeasureUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnicalDetail_Products_ProductId",
                table: "TechnicalDetail",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
