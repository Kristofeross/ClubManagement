﻿// <auto-generated />
using System;
using ClubManagement.ApplicationDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ClubManagement.Migrations
{
    [DbContext(typeof(ClubDbContext))]
    partial class ClubDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ClubManagement.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CoachId")
                        .HasColumnType("int");

                    b.Property<int?>("FootballerId")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("ClubManagement.Models.Coach", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("KindOfCoach")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.ToTable("Coaches");
                });

            modelBuilder.Entity("ClubManagement.Models.Footballer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("AgeCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<float?>("Growth")
                        .HasColumnType("real");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PlayerNumber")
                        .HasColumnType("int");

                    b.Property<string>("Position")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float?>("Weight")
                        .HasColumnType("real");

                    b.Property<string>("WhichFoot")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId")
                        .IsUnique()
                        .HasFilter("[AccountId] IS NOT NULL");

                    b.ToTable("Footballers");
                });

            modelBuilder.Entity("ClubManagement.Models.GroupTraining", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AgeCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfTraining")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTraining")
                        .HasColumnType("datetime2");

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTraining")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("TimeOfTraining")
                        .HasColumnType("time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GroupTraining");
                });

            modelBuilder.Entity("ClubManagement.Models.IndividualTraining", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CoachId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateOfTraining")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("EndTraining")
                        .HasColumnType("datetime2");

                    b.Property<int>("FootballerId")
                        .HasColumnType("int");

                    b.Property<string>("Place")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StartTraining")
                        .HasColumnType("datetime2");

                    b.Property<TimeSpan>("TimeOfTraining")
                        .HasColumnType("time");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CoachId");

                    b.HasIndex("FootballerId");

                    b.ToTable("IndividualTraining");
                });

            modelBuilder.Entity("ClubManagement.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Enemy")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsCancelledOrPostponed")
                        .HasColumnType("bit");

                    b.Property<string>("Place")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Score")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("ClubManagement.Models.Statistics", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Assists")
                        .HasColumnType("int");

                    b.Property<int?>("FootballerId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("Goals")
                        .HasColumnType("int");

                    b.Property<int>("Match")
                        .HasColumnType("int");

                    b.Property<int>("Minutes")
                        .HasColumnType("int");

                    b.Property<int>("RedCards")
                        .HasColumnType("int");

                    b.Property<int>("YellowCards")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FootballerId")
                        .IsUnique();

                    b.ToTable("Statistics");
                });

            modelBuilder.Entity("ClubManagement.Models.Transfer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("Transfers");
                });

            modelBuilder.Entity("CoachGroupTraining", b =>
                {
                    b.Property<int>("CoachesId")
                        .HasColumnType("int");

                    b.Property<int>("GroupTrainingsId")
                        .HasColumnType("int");

                    b.HasKey("CoachesId", "GroupTrainingsId");

                    b.HasIndex("GroupTrainingsId");

                    b.ToTable("CoachGroupTraining", (string)null);
                });

            modelBuilder.Entity("CoachMatch", b =>
                {
                    b.Property<int>("CoachesId")
                        .HasColumnType("int");

                    b.Property<int>("MatchesId")
                        .HasColumnType("int");

                    b.HasKey("CoachesId", "MatchesId");

                    b.HasIndex("MatchesId");

                    b.ToTable("CoachMatch", (string)null);
                });

            modelBuilder.Entity("FootballerGroupTraining", b =>
                {
                    b.Property<int>("FootballersId")
                        .HasColumnType("int");

                    b.Property<int>("GroupTrainingsId")
                        .HasColumnType("int");

                    b.HasKey("FootballersId", "GroupTrainingsId");

                    b.HasIndex("GroupTrainingsId");

                    b.ToTable("FootballerGroupTraining", (string)null);
                });

            modelBuilder.Entity("FootballerMatch", b =>
                {
                    b.Property<int>("FootballersId")
                        .HasColumnType("int");

                    b.Property<int>("MatchesId")
                        .HasColumnType("int");

                    b.HasKey("FootballersId", "MatchesId");

                    b.HasIndex("MatchesId");

                    b.ToTable("FootballerMatch", (string)null);
                });

            modelBuilder.Entity("ClubManagement.Models.Coach", b =>
                {
                    b.HasOne("ClubManagement.Models.Account", "Account")
                        .WithOne("Coach")
                        .HasForeignKey("ClubManagement.Models.Coach", "AccountId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ClubManagement.Models.Footballer", b =>
                {
                    b.HasOne("ClubManagement.Models.Account", "Account")
                        .WithOne("Footballer")
                        .HasForeignKey("ClubManagement.Models.Footballer", "AccountId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Account");
                });

            modelBuilder.Entity("ClubManagement.Models.IndividualTraining", b =>
                {
                    b.HasOne("ClubManagement.Models.Coach", "Coach")
                        .WithMany("IndividualTrainings")
                        .HasForeignKey("CoachId");

                    b.HasOne("ClubManagement.Models.Footballer", "Footballer")
                        .WithMany("IndividualTrainings")
                        .HasForeignKey("FootballerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coach");

                    b.Navigation("Footballer");
                });

            modelBuilder.Entity("ClubManagement.Models.Statistics", b =>
                {
                    b.HasOne("ClubManagement.Models.Footballer", "Footballer")
                        .WithOne("Statistics")
                        .HasForeignKey("ClubManagement.Models.Statistics", "FootballerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Footballer");
                });

            modelBuilder.Entity("CoachGroupTraining", b =>
                {
                    b.HasOne("ClubManagement.Models.Coach", null)
                        .WithMany()
                        .HasForeignKey("CoachesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubManagement.Models.GroupTraining", null)
                        .WithMany()
                        .HasForeignKey("GroupTrainingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoachMatch", b =>
                {
                    b.HasOne("ClubManagement.Models.Coach", null)
                        .WithMany()
                        .HasForeignKey("CoachesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubManagement.Models.Match", null)
                        .WithMany()
                        .HasForeignKey("MatchesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FootballerGroupTraining", b =>
                {
                    b.HasOne("ClubManagement.Models.Footballer", null)
                        .WithMany()
                        .HasForeignKey("FootballersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubManagement.Models.GroupTraining", null)
                        .WithMany()
                        .HasForeignKey("GroupTrainingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("FootballerMatch", b =>
                {
                    b.HasOne("ClubManagement.Models.Footballer", null)
                        .WithMany()
                        .HasForeignKey("FootballersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ClubManagement.Models.Match", null)
                        .WithMany()
                        .HasForeignKey("MatchesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ClubManagement.Models.Account", b =>
                {
                    b.Navigation("Coach");

                    b.Navigation("Footballer");
                });

            modelBuilder.Entity("ClubManagement.Models.Coach", b =>
                {
                    b.Navigation("IndividualTrainings");
                });

            modelBuilder.Entity("ClubManagement.Models.Footballer", b =>
                {
                    b.Navigation("IndividualTrainings");

                    b.Navigation("Statistics")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
