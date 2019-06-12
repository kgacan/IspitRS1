using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ispit.Data.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KorisnickiNalog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KorisnickoIme = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KorisnickiNalog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AutorizacijskiToken",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IpAdresa = table.Column<string>(nullable: true),
                    KorisnickiNalogId = table.Column<int>(nullable: false),
                    Vrijednost = table.Column<string>(nullable: true),
                    VrijemeEvidentiranja = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutorizacijskiToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AutorizacijskiToken_KorisnickiNalog_KorisnickiNalogId",
                        column: x => x.KorisnickiNalogId,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Nastavnik",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImePrezime = table.Column<string>(nullable: true),
                    KorisnickiNalogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nastavnik", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Nastavnik_KorisnickiNalog_KorisnickiNalogId",
                        column: x => x.KorisnickiNalogId,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImePrezime = table.Column<string>(nullable: true),
                    KorisnickiNalogId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Student_KorisnickiNalog_KorisnickiNalogId",
                        column: x => x.KorisnickiNalogId,
                        principalTable: "KorisnickiNalog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dogadjaj",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumOdrzavanja = table.Column<DateTime>(nullable: false),
                    NastavnikID = table.Column<int>(nullable: true),
                    Opis = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dogadjaj", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dogadjaj_Nastavnik_NastavnikID",
                        column: x => x.NastavnikID,
                        principalTable: "Nastavnik",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Obaveza",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DogadjajID = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(nullable: true),
                    NotifikacijaDanaPrijeDefault = table.Column<int>(nullable: false),
                    NotifikacijeRekurizivnoDefault = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obaveza", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Obaveza_Dogadjaj_DogadjajID",
                        column: x => x.DogadjajID,
                        principalTable: "Dogadjaj",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OznacenDogadjaj",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumDodavanja = table.Column<DateTime>(nullable: false),
                    DogadjajID = table.Column<int>(nullable: false),
                    IsProcitano = table.Column<bool>(nullable: false),
                    StudentID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OznacenDogadjaj", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OznacenDogadjaj_Dogadjaj_DogadjajID",
                        column: x => x.DogadjajID,
                        principalTable: "Dogadjaj",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OznacenDogadjaj_Student_StudentID",
                        column: x => x.StudentID,
                        principalTable: "Student",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StanjeObaveze",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumIzvrsenja = table.Column<DateTime>(nullable: false),
                    IsZavrseno = table.Column<bool>(nullable: false),
                    IzvrsenoProcentualno = table.Column<float>(nullable: false),
                    NotifikacijaDanaPrije = table.Column<int>(nullable: false),
                    NotifikacijeRekurizivno = table.Column<bool>(nullable: false),
                    ObavezaID = table.Column<int>(nullable: false),
                    OznacenDogadjajID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StanjeObaveze", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StanjeObaveze_Obaveza_ObavezaID",
                        column: x => x.ObavezaID,
                        principalTable: "Obaveza",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StanjeObaveze_OznacenDogadjaj_OznacenDogadjajID",
                        column: x => x.OznacenDogadjajID,
                        principalTable: "OznacenDogadjaj",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PoslataNotifikacija",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumSlanja = table.Column<DateTime>(nullable: false),
                    StanjeObavezeID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoslataNotifikacija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PoslataNotifikacija_StanjeObaveze_StanjeObavezeID",
                        column: x => x.StanjeObavezeID,
                        principalTable: "StanjeObaveze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutorizacijskiToken_KorisnickiNalogId",
                table: "AutorizacijskiToken",
                column: "KorisnickiNalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Dogadjaj_NastavnikID",
                table: "Dogadjaj",
                column: "NastavnikID");

            migrationBuilder.CreateIndex(
                name: "IX_Nastavnik_KorisnickiNalogId",
                table: "Nastavnik",
                column: "KorisnickiNalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Obaveza_DogadjajID",
                table: "Obaveza",
                column: "DogadjajID");

            migrationBuilder.CreateIndex(
                name: "IX_OznacenDogadjaj_DogadjajID",
                table: "OznacenDogadjaj",
                column: "DogadjajID");

            migrationBuilder.CreateIndex(
                name: "IX_OznacenDogadjaj_StudentID",
                table: "OznacenDogadjaj",
                column: "StudentID");

            migrationBuilder.CreateIndex(
                name: "IX_PoslataNotifikacija_StanjeObavezeID",
                table: "PoslataNotifikacija",
                column: "StanjeObavezeID");

            migrationBuilder.CreateIndex(
                name: "IX_StanjeObaveze_ObavezaID",
                table: "StanjeObaveze",
                column: "ObavezaID");

            migrationBuilder.CreateIndex(
                name: "IX_StanjeObaveze_OznacenDogadjajID",
                table: "StanjeObaveze",
                column: "OznacenDogadjajID");

            migrationBuilder.CreateIndex(
                name: "IX_Student_KorisnickiNalogId",
                table: "Student",
                column: "KorisnickiNalogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutorizacijskiToken");

            migrationBuilder.DropTable(
                name: "PoslataNotifikacija");

            migrationBuilder.DropTable(
                name: "StanjeObaveze");

            migrationBuilder.DropTable(
                name: "Obaveza");

            migrationBuilder.DropTable(
                name: "OznacenDogadjaj");

            migrationBuilder.DropTable(
                name: "Dogadjaj");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "Nastavnik");

            migrationBuilder.DropTable(
                name: "KorisnickiNalog");
        }
    }
}
