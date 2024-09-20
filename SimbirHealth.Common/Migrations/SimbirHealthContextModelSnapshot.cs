﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SimbirHealth.Common;

#nullable disable

namespace SimbirHealth.Common.Migrations
{
    [DbContext(typeof(SimbirHealthContext))]
    partial class SimbirHealthContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("SimbirHealth.Data.Models.Account.Account", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Guid = new Guid("9dd5f073-265b-4b57-8268-d0a53355b7e7"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(417),
                            FirstName = "admin",
                            LastName = "default",
                            Password = "21232F297A57A5A743894A0E4A801FC3",
                            Username = "admin"
                        },
                        new
                        {
                            Guid = new Guid("dfa3ea95-21e1-44c6-9393-5ab531d39acd"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(436),
                            FirstName = "manager",
                            LastName = "default",
                            Password = "1D0258C2440A8D19E716292B231E3190",
                            Username = "manager"
                        },
                        new
                        {
                            Guid = new Guid("2018473b-0ec4-4702-bbaf-667e4843a48a"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(465),
                            FirstName = "doctor",
                            LastName = "default",
                            Password = "F9F16D97C90D8C6F2CAB37BB6D1F1992",
                            Username = "doctor"
                        },
                        new
                        {
                            Guid = new Guid("c6645389-3937-4d85-80e2-05437a15241b"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(479),
                            FirstName = "user",
                            LastName = "default",
                            Password = "EE11CBB19052E40B07AAC0CA060C23EE",
                            Username = "user"
                        });
                });

            modelBuilder.Entity("SimbirHealth.Data.Models.Account.AccountToRole", b =>
                {
                    b.Property<Guid>("AccountGuid")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RoleGuid")
                        .HasColumnType("uuid");

                    b.HasKey("AccountGuid", "RoleGuid");

                    b.HasIndex("RoleGuid");

                    b.ToTable("AccountToRole");

                    b.HasData(
                        new
                        {
                            AccountGuid = new Guid("9dd5f073-265b-4b57-8268-d0a53355b7e7"),
                            RoleGuid = new Guid("816dca08-d141-4fd1-8f34-7d7a4322a53d")
                        },
                        new
                        {
                            AccountGuid = new Guid("dfa3ea95-21e1-44c6-9393-5ab531d39acd"),
                            RoleGuid = new Guid("ac5328b1-acec-4739-b570-90bf511a3e02")
                        },
                        new
                        {
                            AccountGuid = new Guid("2018473b-0ec4-4702-bbaf-667e4843a48a"),
                            RoleGuid = new Guid("929a852e-4d8e-4595-9fee-00076e7a8a7b")
                        },
                        new
                        {
                            AccountGuid = new Guid("c6645389-3937-4d85-80e2-05437a15241b"),
                            RoleGuid = new Guid("803f5318-c437-47ce-8781-97719a4095ba")
                        });
                });

            modelBuilder.Entity("SimbirHealth.Data.Models.Account.Role", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateCreate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Guid");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Guid = new Guid("816dca08-d141-4fd1-8f34-7d7a4322a53d"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(202),
                            RoleName = "Admin"
                        },
                        new
                        {
                            Guid = new Guid("ac5328b1-acec-4739-b570-90bf511a3e02"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(208),
                            RoleName = "Manager"
                        },
                        new
                        {
                            Guid = new Guid("929a852e-4d8e-4595-9fee-00076e7a8a7b"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(210),
                            RoleName = "Doctor"
                        },
                        new
                        {
                            Guid = new Guid("803f5318-c437-47ce-8781-97719a4095ba"),
                            DateCreate = new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(211),
                            RoleName = "User"
                        });
                });

            modelBuilder.Entity("SimbirHealth.Data.Models.Account.AccountToRole", b =>
                {
                    b.HasOne("SimbirHealth.Data.Models.Account.Account", "Account")
                        .WithMany("AccountToRoles")
                        .HasForeignKey("AccountGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SimbirHealth.Data.Models.Account.Role", "Role")
                        .WithMany("AccountToRoles")
                        .HasForeignKey("RoleGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("SimbirHealth.Data.Models.Account.Account", b =>
                {
                    b.Navigation("AccountToRoles");
                });

            modelBuilder.Entity("SimbirHealth.Data.Models.Account.Role", b =>
                {
                    b.Navigation("AccountToRoles");
                });
#pragma warning restore 612, 618
        }
    }
}
