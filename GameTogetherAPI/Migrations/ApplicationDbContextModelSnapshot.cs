﻿// <auto-generated />
using System;
using GameTogetherAPI.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GameTogetherAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("GameTogetherAPI.Models.Chat", b =>
                {
                    b.Property<int>("ChatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("ChatId"));

                    b.Property<int?>("SessionId")
                        .HasColumnType("int");

                    b.HasKey("ChatId");

                    b.HasIndex("SessionId")
                        .IsUnique();

                    b.ToTable("Chats");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Message", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int?>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TimeStamp")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("ChatId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Profile", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("longtext");

                    b.Property<string>("Region")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Profiles");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AgeRange")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("IsEmailVerified")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.UserChat", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ChatId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "ChatId");

                    b.HasIndex("ChatId");

                    b.ToTable("UserChats");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.UserSession", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("UserId", "SessionId");

                    b.HasIndex("SessionId");

                    b.ToTable("UserSessions");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Chat", b =>
                {
                    b.HasOne("GameTogetherAPI.Models.Session", "Session")
                        .WithOne("Chat")
                        .HasForeignKey("GameTogetherAPI.Models.Chat", "SessionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("Session");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Message", b =>
                {
                    b.HasOne("GameTogetherAPI.Models.Chat", "Chat")
                        .WithMany("Messages")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameTogetherAPI.Models.User", "Sender")
                        .WithMany("SentMessages")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Chat");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Profile", b =>
                {
                    b.HasOne("GameTogetherAPI.Models.User", "User")
                        .WithOne("Profile")
                        .HasForeignKey("GameTogetherAPI.Models.Profile", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Session", b =>
                {
                    b.HasOne("GameTogetherAPI.Models.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.UserChat", b =>
                {
                    b.HasOne("GameTogetherAPI.Models.Chat", "Chat")
                        .WithMany("UserChats")
                        .HasForeignKey("ChatId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameTogetherAPI.Models.User", "User")
                        .WithMany("Chats")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Chat");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.UserSession", b =>
                {
                    b.HasOne("GameTogetherAPI.Models.Session", "Session")
                        .WithMany("Participants")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GameTogetherAPI.Models.User", "User")
                        .WithMany("JoinedSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");

                    b.Navigation("User");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Chat", b =>
                {
                    b.Navigation("Messages");

                    b.Navigation("UserChats");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.Session", b =>
                {
                    b.Navigation("Chat")
                        .IsRequired();

                    b.Navigation("Participants");
                });

            modelBuilder.Entity("GameTogetherAPI.Models.User", b =>
                {
                    b.Navigation("Chats");

                    b.Navigation("JoinedSessions");

                    b.Navigation("Profile")
                        .IsRequired();

                    b.Navigation("SentMessages");
                });
#pragma warning restore 612, 618
        }
    }
}
