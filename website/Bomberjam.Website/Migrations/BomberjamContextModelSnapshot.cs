﻿// <auto-generated />
using System;
using Bomberjam.Website.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bomberjam.Website.Migrations
{
    [DbContext(typeof(BomberjamContext))]
    partial class BomberjamContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3");

            modelBuilder.Entity("Bomberjam.Website.Database.DbBot", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Iteration")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.HasIndex("Status");

                    b.HasIndex("Updated");

                    b.HasIndex("UserId");

                    b.ToTable("App_Bots");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGame", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("GameDuration")
                        .HasColumnType("float");

                    b.Property<double?>("InitDuration")
                        .HasColumnType("float");

                    b.Property<int>("Origin")
                        .HasColumnType("int");

                    b.Property<int>("SeasonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(1);

                    b.Property<string>("Stderr")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Stdout")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.HasIndex("Origin");

                    b.HasIndex("SeasonId");

                    b.ToTable("App_Games");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGameUser", b =>
                {
                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BotId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<float>("DeltaPoints")
                        .HasColumnType("real");

                    b.Property<string>("Errors")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rank")
                        .HasColumnType("int");

                    b.Property<int>("Score")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("GameId", "UserId");

                    b.HasIndex("BotId");

                    b.HasIndex("UserId");

                    b.ToTable("App_GameUsers");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbQueuedTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Data")
                        .HasMaxLength(1024)
                        .HasColumnType("nvarchar(1024)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.HasIndex("Status");

                    b.HasIndex("Type");

                    b.ToTable("App_Tasks");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbSeason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int?>("UserCount")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("App_Seasons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Created = new DateTime(2021, 2, 28, 19, 0, 0, 0, DateTimeKind.Local),
                            Name = "S01",
                            Updated = new DateTime(2021, 2, 28, 19, 0, 0, 0, DateTimeKind.Local)
                        });
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbSeasonSummary", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SeasonId")
                        .HasColumnType("int");

                    b.Property<int>("GlobalRank")
                        .HasColumnType("int");

                    b.Property<int>("RankedGameCount")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SeasonId");

                    b.HasIndex("SeasonId");

                    b.ToTable("App_SeasonSummaries");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("GithubId")
                        .HasColumnType("int");

                    b.Property<int>("GlobalRank")
                        .HasColumnType("int");

                    b.Property<float>("Points")
                        .HasColumnType("real");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserName")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.HasIndex("GithubId")
                        .IsUnique();

                    b.HasIndex("Points");

                    b.HasIndex("UserName")
                        .IsUnique()
                        .HasFilter("[UserName] IS NOT NULL");

                    b.ToTable("App_Users");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbWorker", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Created");

                    b.ToTable("App_Workers");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbBot", b =>
                {
                    b.HasOne("Bomberjam.Website.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGame", b =>
                {
                    b.HasOne("Bomberjam.Website.Database.DbSeason", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbGameUser", b =>
                {
                    b.HasOne("Bomberjam.Website.Database.DbBot", "Bot")
                        .WithMany()
                        .HasForeignKey("BotId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Bomberjam.Website.Database.DbGame", "Game")
                        .WithMany()
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bomberjam.Website.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Bot");

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Bomberjam.Website.Database.DbSeasonSummary", b =>
                {
                    b.HasOne("Bomberjam.Website.Database.DbSeason", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Bomberjam.Website.Database.DbUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
