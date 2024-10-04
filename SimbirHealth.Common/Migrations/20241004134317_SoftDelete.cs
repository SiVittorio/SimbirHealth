using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirHealth.Common.Migrations
{
    /// <inheritdoc />
    public partial class SoftDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Roles",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "RefreshTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AccountToRole",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Accounts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AccountToRole",
                keyColumns: new[] { "AccountGuid", "RoleGuid" },
                keyValues: new object[] { new Guid("2018473b-0ec4-4702-bbaf-667e4843a48a"), new Guid("929a852e-4d8e-4595-9fee-00076e7a8a7b") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "AccountToRole",
                keyColumns: new[] { "AccountGuid", "RoleGuid" },
                keyValues: new object[] { new Guid("9dd5f073-265b-4b57-8268-d0a53355b7e7"), new Guid("816dca08-d141-4fd1-8f34-7d7a4322a53d") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "AccountToRole",
                keyColumns: new[] { "AccountGuid", "RoleGuid" },
                keyValues: new object[] { new Guid("c6645389-3937-4d85-80e2-05437a15241b"), new Guid("803f5318-c437-47ce-8781-97719a4095ba") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "AccountToRole",
                keyColumns: new[] { "AccountGuid", "RoleGuid" },
                keyValues: new object[] { new Guid("dfa3ea95-21e1-44c6-9393-5ab531d39acd"), new Guid("ac5328b1-acec-4739-b570-90bf511a3e02") },
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Guid",
                keyValue: new Guid("2018473b-0ec4-4702-bbaf-667e4843a48a"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Guid",
                keyValue: new Guid("9dd5f073-265b-4b57-8268-d0a53355b7e7"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Guid",
                keyValue: new Guid("c6645389-3937-4d85-80e2-05437a15241b"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Accounts",
                keyColumn: "Guid",
                keyValue: new Guid("dfa3ea95-21e1-44c6-9393-5ab531d39acd"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Guid",
                keyValue: new Guid("803f5318-c437-47ce-8781-97719a4095ba"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Guid",
                keyValue: new Guid("816dca08-d141-4fd1-8f34-7d7a4322a53d"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Guid",
                keyValue: new Guid("929a852e-4d8e-4595-9fee-00076e7a8a7b"),
                column: "IsDeleted",
                value: false);

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Guid",
                keyValue: new Guid("ac5328b1-acec-4739-b570-90bf511a3e02"),
                column: "IsDeleted",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AccountToRole");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Accounts");
        }
    }
}
