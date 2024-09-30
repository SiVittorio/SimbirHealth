using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirHealth.Common.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRefreshTModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateCreate",
                table: "RefreshTokens",
                newName: "ExpiredDate");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountGuid",
                table: "RefreshTokens",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccountGuid",
                table: "RefreshTokens",
                column: "AccountGuid",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Accounts_AccountGuid",
                table: "RefreshTokens",
                column: "AccountGuid",
                principalTable: "Accounts",
                principalColumn: "Guid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Accounts_AccountGuid",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_AccountGuid",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "AccountGuid",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "ExpiredDate",
                table: "RefreshTokens",
                newName: "DateCreate");
        }
    }
}
