﻿// <auto-generated />
using System;
using BugTrakr.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BugTrakr.Migrations
{
    [DbContext(typeof(BugTrakrDbContext))]
    [Migration("20200616224301_CreateBugMigration")]
    partial class CreateBugMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BugTrakr.Models.Bug", b =>
                {
                    b.Property<int>("BugId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignedById")
                        .HasColumnType("int");

                    b.Property<int>("AssignedToId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BugCompletedTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("BugCreationTimeStamp")
                        .HasColumnType("datetime2");

                    b.Property<string>("BugDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(2147483647);

                    b.Property<int>("BugDifficulty")
                        .HasColumnType("int");

                    b.Property<int>("BugStatus")
                        .HasColumnType("int");

                    b.Property<string>("BugTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.HasKey("BugId");

                    b.ToTable("Bugs");
                });

            modelBuilder.Entity("BugTrakr.Models.BugAssignmentHistory", b =>
                {
                    b.Property<int>("BugAssignmentHistoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssignedToId")
                        .HasColumnType("int");

                    b.Property<int>("BugId")
                        .HasColumnType("int");

                    b.Property<int>("BugStatus")
                        .HasColumnType("int");

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("BugAssignmentHistoryId");

                    b.ToTable("BugAssignmentHistories");
                });

            modelBuilder.Entity("BugTrakr.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BugId")
                        .HasColumnType("int");

                    b.Property<string>("CommentText")
                        .HasColumnType("nvarchar(max)")
                        .HasMaxLength(2147483647);

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CommentId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("BugTrakr.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BugId")
                        .HasColumnType("int");

                    b.Property<string>("NotificationMessage")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<int>("NotificationType")
                        .HasColumnType("int");

                    b.Property<int>("ReceipientId")
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.ToTable("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
