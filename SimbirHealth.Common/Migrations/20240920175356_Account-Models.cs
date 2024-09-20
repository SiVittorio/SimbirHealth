using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SimbirHealth.Common.Migrations
{
    /// <inheritdoc />
    public partial class AccountModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleName = table.Column<string>(type: "text", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "AccountToRole",
                columns: table => new
                {
                    AccountGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleGuid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountToRole", x => new { x.AccountGuid, x.RoleGuid });
                    table.ForeignKey(
                        name: "FK_AccountToRole_Accounts_AccountGuid",
                        column: x => x.AccountGuid,
                        principalTable: "Accounts",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountToRole_Roles_RoleGuid",
                        column: x => x.RoleGuid,
                        principalTable: "Roles",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Guid", "DateCreate", "FirstName", "LastName", "Password", "Username" },
                values: new object[,]
                {
                    { new Guid("2018473b-0ec4-4702-bbaf-667e4843a48a"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(465), "doctor", "default", "F9F16D97C90D8C6F2CAB37BB6D1F1992", "doctor" },
                    { new Guid("9dd5f073-265b-4b57-8268-d0a53355b7e7"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(417), "admin", "default", "21232F297A57A5A743894A0E4A801FC3", "admin" },
                    { new Guid("c6645389-3937-4d85-80e2-05437a15241b"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(479), "user", "default", "EE11CBB19052E40B07AAC0CA060C23EE", "user" },
                    { new Guid("dfa3ea95-21e1-44c6-9393-5ab531d39acd"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(436), "manager", "default", "1D0258C2440A8D19E716292B231E3190", "manager" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Guid", "DateCreate", "RoleName" },
                values: new object[,]
                {
                    { new Guid("803f5318-c437-47ce-8781-97719a4095ba"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(211), "User" },
                    { new Guid("816dca08-d141-4fd1-8f34-7d7a4322a53d"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(202), "Admin" },
                    { new Guid("929a852e-4d8e-4595-9fee-00076e7a8a7b"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(210), "Doctor" },
                    { new Guid("ac5328b1-acec-4739-b570-90bf511a3e02"), new DateTime(2024, 9, 20, 17, 53, 56, 254, DateTimeKind.Utc).AddTicks(208), "Manager" }
                });

            migrationBuilder.InsertData(
                table: "AccountToRole",
                columns: new[] { "AccountGuid", "RoleGuid" },
                values: new object[,]
                {
                    { new Guid("2018473b-0ec4-4702-bbaf-667e4843a48a"), new Guid("929a852e-4d8e-4595-9fee-00076e7a8a7b") },
                    { new Guid("9dd5f073-265b-4b57-8268-d0a53355b7e7"), new Guid("816dca08-d141-4fd1-8f34-7d7a4322a53d") },
                    { new Guid("c6645389-3937-4d85-80e2-05437a15241b"), new Guid("803f5318-c437-47ce-8781-97719a4095ba") },
                    { new Guid("dfa3ea95-21e1-44c6-9393-5ab531d39acd"), new Guid("ac5328b1-acec-4739-b570-90bf511a3e02") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountToRole_RoleGuid",
                table: "AccountToRole",
                column: "RoleGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountToRole");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
