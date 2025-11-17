using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookingApp.Infrastructures.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Dicusar_Add_booking_times : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CanceledAt",
                schema: "room-booking",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ConfirmedAt",
                schema: "room-booking",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                schema: "room-booking",
                table: "Bookings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CanceledAt",
                schema: "room-booking",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "ConfirmedAt",
                schema: "room-booking",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "room-booking",
                table: "Bookings");
        }
    }
}
