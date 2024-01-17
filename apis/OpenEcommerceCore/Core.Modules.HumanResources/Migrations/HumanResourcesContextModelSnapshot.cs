﻿// <auto-generated />
using System;
using Core.Modules.HumanResources.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Core.Modules.HumanResources.Migrations
{
    [DbContext(typeof(HumanResourcesContext))]
    partial class HumanResourcesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.26")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Address", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CollaboratorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Neighbourhood")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<Guid>("StateId")
                        .HasColumnType("uuid");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)");

                    b.Property<int>("ZipCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CollaboratorId");

                    b.HasIndex("StateId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Collaborator", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(22)
                        .HasColumnType("character varying(22)");

                    b.HasKey("Id");

                    b.ToTable("Collaborators");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<bool>("Broken")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Guid>("CollaboratorId")
                        .HasColumnType("uuid");

                    b.Property<bool>("Deleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("MonthlySalary")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Sector")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CollaboratorId");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.ContributionYear", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uuid");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("ContributionYears");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.JobApplication", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("Age")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("ProcessStep")
                        .HasColumnType("integer");

                    b.Property<string>("ResumeURL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Sector")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("JobApplications");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.SocialLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CollaboratorId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("JobApplicationId")
                        .HasColumnType("uuid");

                    b.Property<int>("SocialMedia")
                        .HasColumnType("integer");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CollaboratorId");

                    b.HasIndex("JobApplicationId");

                    b.ToTable("SocialLinks");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.State", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("States");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.WorkHour", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContributionYearId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<TimeSpan>("End")
                        .HasColumnType("interval");

                    b.Property<TimeSpan>("Start")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("ContributionYearId");

                    b.ToTable("WorkHours");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Address", b =>
                {
                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.Collaborator", "Collaborator")
                        .WithMany("Addresses")
                        .HasForeignKey("CollaboratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.State", "State")
                        .WithMany("Addresses")
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collaborator");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Contract", b =>
                {
                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.Collaborator", "Collaborator")
                        .WithMany("Contracts")
                        .HasForeignKey("CollaboratorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Collaborator");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.ContributionYear", b =>
                {
                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.Contract", "Contract")
                        .WithMany("ContributionYears")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Contract");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.SocialLink", b =>
                {
                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.Collaborator", "Collaborator")
                        .WithMany("SocialLinks")
                        .HasForeignKey("CollaboratorId");

                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.JobApplication", "JobApplication")
                        .WithMany("SocialLinks")
                        .HasForeignKey("JobApplicationId");

                    b.Navigation("Collaborator");

                    b.Navigation("JobApplication");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.WorkHour", b =>
                {
                    b.HasOne("Core.Modules.HumanResources.Domain.Entities.ContributionYear", "ContributionYear")
                        .WithMany("WorkHours")
                        .HasForeignKey("ContributionYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ContributionYear");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Collaborator", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Contracts");

                    b.Navigation("SocialLinks");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.Contract", b =>
                {
                    b.Navigation("ContributionYears");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.ContributionYear", b =>
                {
                    b.Navigation("WorkHours");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.JobApplication", b =>
                {
                    b.Navigation("SocialLinks");
                });

            modelBuilder.Entity("Core.Modules.HumanResources.Domain.Entities.State", b =>
                {
                    b.Navigation("Addresses");
                });
#pragma warning restore 612, 618
        }
    }
}
