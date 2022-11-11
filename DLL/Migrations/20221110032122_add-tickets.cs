using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtickets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TuyenDuongId",
                table: "MauHoaDons",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaTraCuu",
                table: "HoaDonDienTus",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaCuaCQT",
                table: "HoaDonDienTus",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenDen",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BenDi",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVeTam",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "NgungXuatVe",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoChang",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoChuyen",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoGhe",
                table: "HoaDonDienTus",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SoLuong",
                table: "HoaDonDienTus",
                type: "decimal(21,6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoTuyen",
                table: "HoaDonDienTus",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoXe",
                table: "HoaDonDienTus",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TenTuyenDuong",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ThoiGianKhoiHanh",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThueGTGT",
                table: "HoaDonDienTus",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TuyenDuongId",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XeId",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TuyenDuongs",
                columns: table => new
                {
                    TuyenDuongId = table.Column<Guid>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    TenTuyenDuong = table.Column<string>(nullable: true),
                    BenDi = table.Column<string>(nullable: true),
                    BenDen = table.Column<string>(nullable: true),
                    ThoiGianKhoiHanh = table.Column<string>(nullable: true),
                    SoXe = table.Column<string>(nullable: true),
                    SoTuyen = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyenDuongs", x => x.TuyenDuongId);
                });

            migrationBuilder.CreateTable(
                name: "Xes",
                columns: table => new
                {
                    XeId = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    MaXe = table.Column<string>(maxLength: 256, nullable: true),
                    SoXe = table.Column<string>(maxLength: 256, nullable: true),
                    LoaiXe = table.Column<string>(maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Xes", x => x.XeId);
                });

            migrationBuilder.CreateTable(
                name: "User_Xes",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    XeId = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Xes", x => new { x.UserId, x.XeId });
                    table.ForeignKey(
                        name: "FK_User_Xes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Xes_Xes_XeId",
                        column: x => x.XeId,
                        principalTable: "Xes",
                        principalColumn: "XeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MauHoaDons_TuyenDuongId",
                table: "MauHoaDons",
                column: "TuyenDuongId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_XeId",
                table: "HoaDonDienTus",
                column: "XeId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Xes_XeId",
                table: "User_Xes",
                column: "XeId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_Xes_XeId",
                table: "HoaDonDienTus",
                column: "XeId",
                principalTable: "Xes",
                principalColumn: "XeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MauHoaDons_TuyenDuongs_TuyenDuongId",
                table: "MauHoaDons",
                column: "TuyenDuongId",
                principalTable: "TuyenDuongs",
                principalColumn: "TuyenDuongId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_Xes_XeId",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_MauHoaDons_TuyenDuongs_TuyenDuongId",
                table: "MauHoaDons");

            migrationBuilder.DropTable(
                name: "TuyenDuongs");

            migrationBuilder.DropTable(
                name: "User_Xes");

            migrationBuilder.DropTable(
                name: "Xes");

            migrationBuilder.DropIndex(
                name: "IX_MauHoaDons_TuyenDuongId",
                table: "MauHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_XeId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TuyenDuongId",
                table: "MauHoaDons");

            migrationBuilder.DropColumn(
                name: "BenDen",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "BenDi",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "IsVeTam",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "NgungXuatVe",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoChang",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoChuyen",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoGhe",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoTuyen",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "SoXe",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TenTuyenDuong",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "ThoiGianKhoiHanh",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "ThueGTGT",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TuyenDuongId",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "XeId",
                table: "HoaDonDienTus");

            migrationBuilder.AlterColumn<string>(
                name: "MaTraCuu",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MaCuaCQT",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 256,
                oldNullable: true);
        }
    }
}
