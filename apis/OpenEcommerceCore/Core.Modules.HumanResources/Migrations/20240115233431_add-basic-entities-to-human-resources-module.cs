using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Modules.HumanResources.Migrations
{
    public partial class addbasicentitiestohumanresourcesmodule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "character varying(22)", maxLength: 22, nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    Sector = table.Column<int>(type: "integer", nullable: false),
                    ProcessStep = table.Column<int>(type: "integer", nullable: false),
                    ResumeURL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uuid", nullable: false),
                    State = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ZipCode = table.Column<int>(type: "integer", nullable: false),
                    Neighbourhood = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Street = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Broken = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialLinks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CollaboratorId = table.Column<Guid>(type: "uuid", nullable: true),
                    JobApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    SocialMedia = table.Column<int>(type: "integer", nullable: false),
                    URL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialLinks_Collaborators_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Collaborators",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SocialLinks_JobApplications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ContributionYears",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    Year = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContributionYears", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContributionYears_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkHours",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ContributionYearId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Start = table.Column<TimeSpan>(type: "interval", nullable: false),
                    End = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHours", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkHours_ContributionYears_ContributionYearId",
                        column: x => x.ContributionYearId,
                        principalTable: "ContributionYears",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Address_CollaboratorId",
                table: "Address",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CollaboratorId",
                table: "Contracts",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_ContributionYears_ContractId",
                table: "ContributionYears",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_CollaboratorId",
                table: "SocialLinks",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLinks_JobApplicationId",
                table: "SocialLinks",
                column: "JobApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHours_ContributionYearId",
                table: "WorkHours",
                column: "ContributionYearId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "SocialLinks");

            migrationBuilder.DropTable(
                name: "WorkHours");

            migrationBuilder.DropTable(
                name: "JobApplications");

            migrationBuilder.DropTable(
                name: "ContributionYears");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Collaborators");
        }
    }
}
