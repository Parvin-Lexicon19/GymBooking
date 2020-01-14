using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GymBooking.Data.Migrations
{
    public partial class IdentityModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdentityModelsId",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "IdentityModels",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    TimeOfRegistration = table.Column<DateTime>(nullable: false),
                    ApplicationUserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityModels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityModels");

            migrationBuilder.DropColumn(
                name: "IdentityModelsId",
                table: "AspNetUsers");
        }
    }
}
