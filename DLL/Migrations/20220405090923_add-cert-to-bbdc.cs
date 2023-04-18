using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcerttobbdc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "CertA",
                table: "BienBanDieuChinhs",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "CertB",
                table: "BienBanDieuChinhs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertA",
                table: "BienBanDieuChinhs");

            migrationBuilder.DropColumn(
                name: "CertB",
                table: "BienBanDieuChinhs");
        }
    }
}
