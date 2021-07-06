using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtablethongbaophathanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChoPhepPhatHanhMax",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "ChoPhepPhatHanhMin",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "TuNhap",
                table: "MauHoaDons");

            migrationBuilder.RenameColumn(
                name: "TenMauSo",
                table: "MauHoaDons",
                newName: "TenBoMau");

            migrationBuilder.RenameColumn(
                name: "DienGiai",
                table: "MauHoaDons",
                newName: "Ten");

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KyHieu",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModifyBy",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifyDate",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoThuTu",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ThongBaoPhatHanhs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    NgayPhatHanh = table.Column<DateTime>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true),
                    ChoPhepPhatHanhMin = table.Column<int>(nullable: false),
                    ChoPhepPhatHanhMax = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongBaoPhatHanhs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThongBaoPhatHanhs_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ThongBaoPhatHanhs_MauHoaDonId",
                table: "ThongBaoPhatHanhs",
                column: "MauHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongBaoPhatHanhs");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "KyHieu",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "ModifyBy",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "ModifyDate",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "SoThuTu",
                table: "MauHoaDons");

            migrationBuilder.RenameColumn(
                name: "TenBoMau",
                table: "MauHoaDons",
                newName: "TenMauSo");

            migrationBuilder.RenameColumn(
                name: "Ten",
                table: "MauHoaDons",
                newName: "DienGiai");

            migrationBuilder.AddColumn<int>(
                name: "ChoPhepPhatHanhMax",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ChoPhepPhatHanhMin",
                table: "MauHoaDons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "TuNhap",
                table: "MauHoaDons",
                nullable: true);
        }
    }
}
