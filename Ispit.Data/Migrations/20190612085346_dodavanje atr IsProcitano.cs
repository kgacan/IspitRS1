using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ispit.Data.Migrations
{
    public partial class dodavanjeatrIsProcitano : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcitano",
                table: "OznacenDogadjaj");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcitano",
                table: "PoslataNotifikacija",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProcitano",
                table: "PoslataNotifikacija");

            migrationBuilder.AddColumn<bool>(
                name: "IsProcitano",
                table: "OznacenDogadjaj",
                nullable: false,
                defaultValue: false);
        }
    }
}
