using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirHealth.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddHistoryModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Histories",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Data = table.Column<string>(type: "text", nullable: false),
                    PacientGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    HospitalGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Histories", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Histories_Accounts_DoctorGuid",
                        column: x => x.DoctorGuid,
                        principalTable: "Accounts",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Accounts_PacientGuid",
                        column: x => x.PacientGuid,
                        principalTable: "Accounts",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Hospitals_HospitalGuid",
                        column: x => x.HospitalGuid,
                        principalTable: "Hospitals",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Histories_Rooms_RoomGuid",
                        column: x => x.RoomGuid,
                        principalTable: "Rooms",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Histories_DoctorGuid",
                table: "Histories",
                column: "DoctorGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_HospitalGuid",
                table: "Histories",
                column: "HospitalGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_PacientGuid",
                table: "Histories",
                column: "PacientGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Histories_RoomGuid",
                table: "Histories",
                column: "RoomGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Histories");
        }
    }
}
