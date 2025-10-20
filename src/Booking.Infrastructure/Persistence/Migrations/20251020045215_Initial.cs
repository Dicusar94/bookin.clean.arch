using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "room-booking");

            migrationBuilder.CreateTable(
                name: "Rooms",
                schema: "room-booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                schema: "room-booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    TimeRange_Start = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeRange_End = table.Column<TimeOnly>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Booking_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "room-booking",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomSchedule",
                schema: "room-booking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false),
                    IsRecurring = table.Column<bool>(type: "boolean", nullable: false),
                    TimeRange_Start = table.Column<TimeOnly>(type: "time", nullable: false),
                    TimeRange_End = table.Column<TimeOnly>(type: "time", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomSchedule_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalSchema: "room-booking",
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Booking_RoomId",
                schema: "room-booking",
                table: "Booking",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomSchedule_RoomId_DayOfWeek_IsRecurring_Date",
                schema: "room-booking",
                table: "RoomSchedule",
                columns: new[] { "RoomId", "DayOfWeek", "IsRecurring", "Date" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking",
                schema: "room-booking");

            migrationBuilder.DropTable(
                name: "RoomSchedule",
                schema: "room-booking");

            migrationBuilder.DropTable(
                name: "Rooms",
                schema: "room-booking");
        }
    }
}
