using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimbirHealth.Common.Migrations
{
    /// <inheritdoc />
    public partial class AddTimetableModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Timetables",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    To = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    HospitalGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DoctorGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timetables", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Timetables_Accounts_DoctorGuid",
                        column: x => x.DoctorGuid,
                        principalTable: "Accounts",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Timetables_Hospitals_HospitalGuid",
                        column: x => x.HospitalGuid,
                        principalTable: "Hospitals",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Timetables_Rooms_RoomGuid",
                        column: x => x.RoomGuid,
                        principalTable: "Rooms",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uuid", nullable: false),
                    Time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsTaken = table.Column<bool>(type: "boolean", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    TimetableGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Appointments_Timetables_TimetableGuid",
                        column: x => x.TimetableGuid,
                        principalTable: "Timetables",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_TimetableGuid",
                table: "Appointments",
                column: "TimetableGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_DoctorGuid",
                table: "Timetables",
                column: "DoctorGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_HospitalGuid",
                table: "Timetables",
                column: "HospitalGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Timetables_RoomGuid",
                table: "Timetables",
                column: "RoomGuid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Timetables");
        }
    }
}
