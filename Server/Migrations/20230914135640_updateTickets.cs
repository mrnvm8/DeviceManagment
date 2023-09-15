using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeviceManagment.Server.Migrations
{
    public partial class updateTickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeviceTypeId",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DeviceTypeId",
                table: "Tickets",
                column: "DeviceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_DeviceTypes_DeviceTypeId",
                table: "Tickets",
                column: "DeviceTypeId",
                principalTable: "DeviceTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_DeviceTypes_DeviceTypeId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_DeviceTypeId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "DeviceTypeId",
                table: "Tickets");
        }
    }
}
