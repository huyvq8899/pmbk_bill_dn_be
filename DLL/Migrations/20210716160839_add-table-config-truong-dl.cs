using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addtableconfigtruongdl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NghiepVus",
                columns: table => new
                {
                    NghiepVuId = table.Column<string>(nullable: false),
                    TenNghiepVu = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NghiepVus", x => x.NghiepVuId);
                });

            migrationBuilder.CreateTable(
                name: "TruongDuLieus",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    STT = table.Column<int>(nullable: false),
                    MaTruong = table.Column<string>(nullable: true),
                    TenTruong = table.Column<string>(nullable: true),
                    TenHienThi = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Default = table.Column<bool>(nullable: false),
                    NghiepVuId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongDuLieus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruongDuLieus_NghiepVus_NghiepVuId",
                        column: x => x.NghiepVuId,
                        principalTable: "NghiepVus",
                        principalColumn: "NghiepVuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TruongDuLieus_NghiepVuId",
                table: "TruongDuLieus",
                column: "NghiepVuId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TruongDuLieus");

            migrationBuilder.DropTable(
                name: "NghiepVus");
        }
    }
}
