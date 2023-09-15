using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace DeviceManagment.Server.Migrations
{
    public partial class tickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PucharsedDate",
                table: "Devices",
                newName: "PurchasedDate");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceSerialNo",
                table: "Devices",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceIMEINo",
                table: "Devices",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AddColumn<string>(
                name: "Year",
                table: "Devices",
                type: "longtext",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    TicketId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    TicketTitle = table.Column<string>(type: "longtext", nullable: false),
                    TicketIssue = table.Column<string>(type: "longtext", nullable: false),
                    TicketDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TicketSolution = table.Column<string>(type: "longtext", nullable: false),
                    TicketUpdate = table.Column<string>(type: "longtext", nullable: false),
                    IssueSolved = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Updated = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Tickets_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalTable: "Devices",
                        principalColumn: "DeviceId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DeviceId",
                table: "Tickets",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Devices");

            migrationBuilder.RenameColumn(
                name: "PurchasedDate",
                table: "Devices",
                newName: "PucharsedDate");

            migrationBuilder.AlterColumn<string>(
                name: "DeviceSerialNo",
                table: "Devices",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeviceIMEINo",
                table: "Devices",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true);
        }
    }
}
