﻿// <auto-generated />
using System;
using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HighRiskDiseaseSurvellance.Persistence.Migrations
{
    [DbContext(typeof(SurveillanceContext))]
    [Migration("20220610045356_RecordAddScore")]
    partial class RecordAddScore
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("utf8_general_ci")
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.HasCharSet(modelBuilder, "utf8");

            modelBuilder.Entity("HighRiskDiseaseSurvellance.Domain.Models.OfficeUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("Salt")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("OfficeUsers");
                });

            modelBuilder.Entity("HighRiskDiseaseSurvellance.Domain.Models.Order", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OrderNumber")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("OrderPayStatus")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(65,30)");

                    b.Property<string>("UserId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("HighRiskDiseaseSurvellance.Domain.Models.SurveillanceRecord", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("OrderId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("Score")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("SurveillanceContent")
                        .HasColumnType("longtext");

                    b.Property<string>("SurveillanceTypeName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("UserId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Records");
                });

            modelBuilder.Entity("HighRiskDiseaseSurvellance.Domain.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(2048)
                        .HasColumnType("varchar(2048)");

                    b.Property<DateTime>("CreateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("DistributorId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("DistributorQrCode")
                        .HasMaxLength(1024)
                        .HasColumnType("varchar(1024)");

                    b.Property<bool>("IsDistributor")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("ModifiedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NickName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("WeChatOpenId")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("PhoneNumber");

                    b.HasIndex("WeChatOpenId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HighRiskDiseaseSurvellance.Domain.Models.Order", b =>
                {
                    b.OwnsOne("HighRiskDiseaseSurvellance.Domain.Models.ValueObjects.UserInfo", "UserInfo", b1 =>
                        {
                            b1.Property<string>("OrderId")
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("NickName")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("PhoneNumber")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("WeChatOpenId")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.HasKey("OrderId");

                            b1.ToTable("Orders");

                            b1.WithOwner()
                                .HasForeignKey("OrderId");
                        });

                    b.Navigation("UserInfo");
                });

            modelBuilder.Entity("HighRiskDiseaseSurvellance.Domain.Models.SurveillanceRecord", b =>
                {
                    b.OwnsOne("HighRiskDiseaseSurvellance.Domain.Models.ValueObjects.UserInfo", "UserInfo", b1 =>
                        {
                            b1.Property<string>("SurveillanceRecordId")
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("NickName")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("PhoneNumber")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.Property<string>("WeChatOpenId")
                                .HasMaxLength(255)
                                .HasColumnType("varchar(255)");

                            b1.HasKey("SurveillanceRecordId");

                            b1.ToTable("Records");

                            b1.WithOwner()
                                .HasForeignKey("SurveillanceRecordId");
                        });

                    b.Navigation("UserInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
