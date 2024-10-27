using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirHealth.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountGuid",
                table: "Appointments",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AccountGuid",
                table: "Appointments",
                column: "AccountGuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Accounts_AccountGuid",
                table: "Appointments",
                column: "AccountGuid",
                principalTable: "Accounts",
                principalColumn: "Guid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Accounts_AccountGuid",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AccountGuid",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "AccountGuid",
                table: "Appointments");
        }
    }
}
