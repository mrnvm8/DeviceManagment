using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManagment.Server.Migrations
{
    public partial class updatedtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TicketUpdate",
                table: "Tickets",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "TicketSolution",
                table: "Tickets",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<DateTime>(
                name: "ArangedDate",
                table: "Tickets",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "FixedDate",
                table: "Tickets",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TicketYear",
                table: "Tickets",
                type: "longtext",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArangedDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FixedDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "TicketYear",
                table: "Tickets");

            migrationBuilder.AlterColumn<string>(
                name: "TicketUpdate",
                table: "Tickets",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TicketSolution",
                table: "Tickets",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
