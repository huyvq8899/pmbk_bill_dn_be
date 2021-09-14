using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthietlaptruongdulieutable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropTable(
                name: "ThietLapTruongDuLieuMoRongs");

            migrationBuilder.DropTable(
                name: "TruongDuLieuHoaDons");

            migrationBuilder.DropTable(
                name: "TruongDuLieuMoRongs");

            migrationBuilder.DropTable(
                name: "TruongMoRongHoaDons");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung9");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung8");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung7");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung6");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung5");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung4");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung3");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung2");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung10");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung1");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                newName: "XuatBanPhi");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet9");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet8");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet7");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet6");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet5");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet4");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet3");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet2");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet10");

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung9",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung8",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung7",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung6",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung5",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung4",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung3",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung2",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung10",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung1",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "XuatBanPhi",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet9",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet8",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet7",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet6",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet5",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet4",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet3",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet2",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet10",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaQuyCach",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet1",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ThietLapTruongDuLieus",
                columns: table => new
                {
                    ThietLapTruongDuLieuId = table.Column<string>(nullable: false),
                    MaTruong = table.Column<string>(nullable: true),
                    TenCot = table.Column<string>(nullable: true),
                    TenTruong = table.Column<string>(nullable: true),
                    TenTruongHienThi = table.Column<string>(nullable: true),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    LoaiTruongDuLieu = table.Column<int>(nullable: false),
                    KieuDuLieu = table.Column<int>(nullable: false),
                    GhiChu = table.Column<string>(nullable: true),
                    DoRong = table.Column<int>(nullable: true),
                    STT = table.Column<int>(nullable: false),
                    HienThi = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietLapTruongDuLieus", x => x.ThietLapTruongDuLieuId);
                });

            migrationBuilder.Sql(@"delete from ThietLapTruongDuLieus");

            migrationBuilder.InsertData(
                table: "ThietLapTruongDuLieus",
                columns: new[] { "ThietLapTruongDuLieuId", "DoRong", "GhiChu", "HienThi", "KieuDuLieu", "LoaiHoaDon", "LoaiTruongDuLieu", "MaTruong", "STT", "TenCot", "TenTruong", "TenTruongHienThi" },
                values: new object[,]
                {
                    { "595b65d0-c2a6-4cb6-8ec7-35a81006fbde", 100, null, true, 4, 0, 0, null, 1, "NgayHoaDon", "Ngày hóa đơn", "Ngày hóa đơn" },
                    { "6231eef5-7815-4799-a125-3b88cb443228", null, null, true, 1, 2, 1, null, 9, "TruongThongTinBoSung9", "Trường thông tin bổ sung 9", "Trường thông tin bổ sung 9" },
                    { "f083b08d-4dae-4f4c-b957-b0a3053d39bc", null, null, true, 1, 2, 1, null, 8, "TruongThongTinBoSung8", "Trường thông tin bổ sung 8", "Trường thông tin bổ sung 8" },
                    { "ea0679f6-e98b-4c44-bd7e-8cdf0b7857d8", null, null, true, 1, 2, 1, null, 7, "TruongThongTinBoSung7", "Trường thông tin bổ sung 7", "Trường thông tin bổ sung 7" },
                    { "00fdbc33-19fb-4fc9-b577-203922ca9f8c", null, null, true, 1, 2, 1, null, 6, "TruongThongTinBoSung6", "Trường thông tin bổ sung 6", "Trường thông tin bổ sung 6" },
                    { "f4e8b240-080b-45f3-a85d-c4f46ee81c42", null, null, true, 1, 2, 1, null, 5, "TruongThongTinBoSung5", "Trường thông tin bổ sung 5", "Trường thông tin bổ sung 5" },
                    { "6bca970b-546d-4e24-b9b5-076433e1888d", null, null, true, 1, 2, 1, null, 4, "TruongThongTinBoSung4", "Trường thông tin bổ sung 4", "Trường thông tin bổ sung 4" },
                    { "7cb85b80-2e26-44b9-b685-382c3076399c", null, null, true, 1, 2, 1, null, 3, "TruongThongTinBoSung3", "Trường thông tin bổ sung 3", "Trường thông tin bổ sung 3" },
                    { "99d0e62e-58fd-474f-a2cb-ce3347cdd52b", null, null, true, 1, 2, 1, null, 2, "TruongThongTinBoSung2", "Trường thông tin bổ sung 2", "Trường thông tin bổ sung 2" },
                    { "5771621a-87af-4c14-a348-935f3fcb8b9f", null, null, true, 1, 2, 1, null, 1, "TruongThongTinBoSung1", "Trường thông tin bổ sung 1", "Trường thông tin bổ sung 1" },
                    { "0321f639-792e-4a21-aeff-b24981e5ce23", 100, null, false, 1, 1, 2, "HHDV 36", 36, "TruongMoRongChiTiet10", "Trường mở rộng chi tiết số 10", "Trường mở rộng chi tiết số 10" },
                    { "f1f0d194-e940-4cba-b5a1-e96543496557", 100, null, false, 1, 1, 2, "HHDV 35", 35, "TruongMoRongChiTiet9", "Trường mở rộng chi tiết số 9", "Trường mở rộng chi tiết số 9" },
                    { "f40cc7ac-7452-42dd-b436-3d46f54e6c45", 100, null, false, 1, 1, 2, "HHDV 34", 34, "TruongMoRongChiTiet8", "Trường mở rộng chi tiết số 8", "Trường mở rộng chi tiết số 8" },
                    { "5e775ea3-6790-4ed9-8c4b-eab394324857", 100, null, false, 1, 1, 2, "HHDV 33", 33, "TruongMoRongChiTiet7", "Trường mở rộng chi tiết số 7", "Trường mở rộng chi tiết số 7" },
                    { "ca5bb37b-a4ca-481c-aaf6-00637ca7677b", 100, null, false, 1, 1, 2, "HHDV 32", 32, "TruongMoRongChiTiet6", "Trường mở rộng chi tiết số 6", "Trường mở rộng chi tiết số 6" },
                    { "0777a6c8-9cc4-4633-a6d2-361028f9e701", 100, null, false, 1, 1, 2, "HHDV 31", 31, "TruongMoRongChiTiet5", "Trường mở rộng chi tiết số 5", "Trường mở rộng chi tiết số 5" },
                    { "ceee5ac9-4172-4eac-ae27-f5baedd12076", 100, null, false, 1, 1, 2, "HHDV 30", 30, "TruongMoRongChiTiet4", "Trường mở rộng chi tiết số 4", "Trường mở rộng chi tiết số 4" },
                    { "23d6e60a-49e8-4722-8f1a-f0eb60039959", 100, null, false, 1, 1, 2, "HHDV 29", 29, "TruongMoRongChiTiet3", "Trường mở rộng chi tiết số 3", "Trường mở rộng chi tiết số 3" },
                    { "601e1558-510e-43a7-a51d-605a1fbdb109", 100, null, false, 1, 1, 2, "HHDV 28", 28, "TruongMoRongChiTiet2", "Trường mở rộng chi tiết số 2", "Trường mở rộng chi tiết số 2" },
                    { "d164540a-1fcf-4f0d-ace3-1b9f4bb6e464", 100, null, false, 1, 1, 2, "HHDV 27", 27, "TruongMoRongChiTiet1", "Trường mở rộng chi tiết số 1", "Trường mở rộng chi tiết số 1" },
                    { "de93d1cc-1b79-4fa4-8581-ae6c2a334e49", 120, null, false, 1, 1, 2, "HHDV 26", 26, "TenNhanVien", "Tên nhân viên", "Tên nhân viên" },
                    { "30f63f09-ed9d-4deb-b0fd-a59379fe8c51", 100, null, false, 1, 1, 2, "HHDV 25", 25, "MaNhanVien", "Mã nhân viên", "Mã nhân viên" },
                    { "3e40afc4-3930-4212-8cc1-aee2f7a547fe", 100, null, false, 1, 1, 2, "HHDV 24", 24, "GhiChu", "Ghi chú", "Ghi chú" },
                    { "7de323c7-6e72-4017-b5c2-27a7942cf672", 100, null, false, 1, 1, 2, "HHDV 23", 23, "XuatBanPhi", "Xuất bản phí", "Xuất bản phí" },
                    { "3ddd6a03-36f2-482c-b2d9-b02ca17768db", 100, null, false, 1, 1, 2, "HHDV 22", 22, "SoMay", "Số máy", "Số máy" },
                    { "d4f4cb39-33e2-41db-a2d5-07ed35998731", 100, null, false, 1, 1, 2, "HHDV 21", 21, "SoKhung", "Số khung", "Số khung" },
                    { "80c7812d-8673-465e-8606-ccc5675e38d2", 100, null, false, 4, 1, 2, "HHDV 20", 20, "HanSuDung", "Hạn sử dụng", "Hạn sử dụng" },
                    { "2477de25-a8aa-4e25-a039-ee8f9f845b44", 100, null, false, 1, 1, 2, "HHDV 19", 19, "SoLo", "Số lô", "Số lô" },
                    { "5bc5e0b7-566d-466f-bac5-37c89dc71eb3", null, null, true, 1, 2, 1, null, 10, "TruongThongTinBoSung10", "Trường thông tin bổ sung 10", "Trường thông tin bổ sung 10" },
                    { "570c6a54-9faf-42d7-a6ed-d28fa38ff77f", 50, null, true, 3, 2, 2, "HHDV 1", 1, "STT", "STT", "STT" },
                    { "5663cfd3-848c-48d2-9820-559b4363d638", 100, null, true, 1, 2, 2, "HHDV 2", 2, "MaHang", "Mã hàng", "Mã hàng" },
                    { "ccf7a0c1-ce77-4327-bd08-cf98d8a553b4", 150, null, true, 1, 2, 2, "HHDV 3", 3, "TenHang", "Tên hàng", "Tên hàng hóa, dịch vụ" },
                    { "239c078b-6617-4e86-83ff-8dd443058e62", 100, null, false, 1, 2, 2, "HHDV 34", 34, "TruongMoRongChiTiet8", "Trường mở rộng chi tiết số 8", "Trường mở rộng chi tiết số 8" },
                    { "84879b42-f3ab-4eb8-a5ae-e91018dd4bd9", 100, null, false, 1, 2, 2, "HHDV 33", 33, "TruongMoRongChiTiet7", "Trường mở rộng chi tiết số 7", "Trường mở rộng chi tiết số 7" },
                    { "63eaa42a-4d8d-4aa6-a027-441942abdfcf", 100, null, false, 1, 2, 2, "HHDV 32", 32, "TruongMoRongChiTiet6", "Trường mở rộng chi tiết số 6", "Trường mở rộng chi tiết số 6" },
                    { "5bf18e63-5461-40d2-8dc0-f3df7579d513", 100, null, false, 1, 2, 2, "HHDV 31", 31, "TruongMoRongChiTiet5", "Trường mở rộng chi tiết số 5", "Trường mở rộng chi tiết số 5" },
                    { "d5712bf9-447b-4a02-865a-76e1234afe7c", 100, null, false, 1, 2, 2, "HHDV 30", 30, "TruongMoRongChiTiet4", "Trường mở rộng chi tiết số 4", "Trường mở rộng chi tiết số 4" },
                    { "061a4bd9-cd08-4065-a2ff-c44a67c76d94", 100, null, false, 1, 2, 2, "HHDV 29", 29, "TruongMoRongChiTiet3", "Trường mở rộng chi tiết số 3", "Trường mở rộng chi tiết số 3" },
                    { "2c74f3c4-bc54-4e42-9c26-6aa193bebaec", 100, null, false, 1, 2, 2, "HHDV 28", 28, "TruongMoRongChiTiet2", "Trường mở rộng chi tiết số 2", "Trường mở rộng chi tiết số 2" },
                    { "cca734b3-5574-4219-913b-846c55917565", 100, null, false, 1, 2, 2, "HHDV 27", 27, "TruongMoRongChiTiet1", "Trường mở rộng chi tiết số 1", "Trường mở rộng chi tiết số 1" },
                    { "c8884ac1-0bc8-4964-94cc-e3f3ea497b64", 120, null, false, 1, 2, 2, "HHDV 26", 26, "TenNhanVien", "Tên nhân viên", "Tên nhân viên" },
                    { "bb25070c-b9e0-45e0-92f2-c34cffaa5000", 100, null, false, 1, 2, 2, "HHDV 25", 25, "MaNhanVien", "Mã nhân viên", "Mã nhân viên" },
                    { "427f4cbf-0dc0-4c2d-9a74-399bbb388242", 100, null, false, 1, 2, 2, "HHDV 24", 24, "GhiChu", "Ghi chú", "Ghi chú" },
                    { "5c8276c1-8973-498b-8c80-b4979c11297d", 100, null, false, 1, 2, 2, "HHDV 23", 23, "XuatBanPhi", "Xuất bản phí", "Xuất bản phí" },
                    { "aababb6c-741f-416a-919c-6415cce63bcb", 100, null, false, 1, 2, 2, "HHDV 22", 22, "SoMay", "Số máy", "Số máy" },
                    { "cde2627c-c582-4207-b7ad-cef772095bf5", 120, null, false, 2, 1, 2, "HHDV 18", 18, "TienThueGTGTQuyDoi", "Tiền thuế GTGT quy đổi", "Tiền thuế GTGT quy đổi" },
                    { "d9e32f0b-ea62-4215-a80a-662d1cd8dfdd", 100, null, false, 1, 2, 2, "HHDV 21", 21, "SoKhung", "Số khung", "Số khung" },
                    { "717f08b4-1716-4b87-9235-46aedbdd2fdc", 100, null, false, 1, 2, 2, "HHDV 19", 19, "SoLo", "Số lô", "Số lô" },
                    { "c263c289-a972-478e-8036-70a87c997426", 120, null, false, 2, 2, 2, "HHDV 15", 15, "TienChietKhauQuyDoi", "Tiền chiết khấu quy đổi", "Tiền chiết khấu quy đổi" },
                    { "bb565f5a-56f0-47e9-8f82-5a774d387f4c", 120, null, true, 2, 2, 2, "HHDV 14", 14, "TienChietKhau", "Tiền chiết khấu", "Tiền chiết khấu" },
                    { "d08089e9-16b0-4a5f-bd80-9d01e9f2ea1b", 100, null, true, 5, 2, 2, "HHDV 13", 13, "TyLeChietKhau", "Tỷ lệ chiết khấu", "Tỷ lệ chiết khấu" },
                    { "f035d849-c667-4855-86de-612951428dbd", 150, null, false, 2, 2, 2, "HHDV 12", 12, "ThanhTienQuyDoi", "Thành tiền quy đổi", "Thành tiền quy đổi" },
                    { "ba97792e-5e90-43f5-81d1-cbc1f64326e7", 150, null, true, 2, 2, 2, "HHDV 11", 11, "ThanhTien", "Thành tiền", "Thành tiền" },
                    { "183ea35b-bdaf-47f3-9189-bdc9672b3a8e", 150, null, false, 2, 2, 2, "HHDV 10", 10, "ThanhTienSauThue", "Thành tiền sau thuế", "Thành tiền sau thuế" },
                    { "1cb9bbc8-3d57-49ae-baf1-b1ad63fabea4", 120, null, true, 2, 2, 2, "HHDV 9", 9, "DonGia", "Đơn giá", "Đơn giá" },
                    { "82d7b449-b200-483a-8b46-8d7696a904ec", 120, null, false, 2, 2, 2, "HHDV 8", 8, "DonGiaSauThue", "Đơn giá sau thuế", "Đơn giá sau thuế" },
                    { "76adac6b-2715-42d2-a112-a699c7a338ca", 100, null, true, 3, 2, 2, "HHDV 7", 7, "SoLuong", "Số lượng", "Số lượng" },
                    { "80c7554a-03f2-47e7-be6b-ee9acc123d28", 100, null, true, 1, 2, 2, "HHDV 6", 6, "DonViTinhId", "ĐVT", "ĐVT" },
                    { "c795881d-25b6-4922-8e05-91d3c005652f", 100, null, false, 1, 2, 2, "HHDV 5", 5, "MaQuyCach", "Mã quy cách", "Mã quy cách" },
                    { "4c007062-06d3-4e91-91e7-7ee68975559d", 120, null, true, 6, 2, 2, "HHDV 4", 4, "HangKhuyenMai", "Hàng khuyến mại", "Hàng khuyến mại" },
                    { "e1cdbeb8-bacb-421a-ad72-c7f4b2d11b1a", 100, null, false, 4, 2, 2, "HHDV 20", 20, "HanSuDung", "Hạn sử dụng", "Hạn sử dụng" },
                    { "acfc23df-f21d-44ae-81ff-1a4216368756", 100, null, false, 1, 2, 2, "HHDV 35", 35, "TruongMoRongChiTiet9", "Trường mở rộng chi tiết số 9", "Trường mở rộng chi tiết số 9" },
                    { "91c83ead-afb8-4d03-b5d1-e8fcfbb686c0", 120, null, true, 2, 1, 2, "HHDV 17", 17, "TienThueGTGT", "Tiền thuế GTGT", "Tiền thuế GTGT" },
                    { "64c39883-e5db-48d1-9852-01690ae675ef", 120, null, false, 2, 1, 2, "HHDV 15", 15, "TienChietKhauQuyDoi", "Tiền chiết khấu quy đổi", "Tiền chiết khấu quy đổi" },
                    { "11fb3d74-2711-450c-8453-3c8d338b03eb", 100, null, false, 1, 0, 0, null, 28, "TruongThongTinBoSung2", "Trường thông tin bổ sung 2", "Trường thông tin bổ sung 2" },
                    { "e012612b-7148-40db-9179-6cfcba13a8f1", 100, null, false, 1, 0, 0, null, 27, "TruongThongTinBoSung1", "Trường thông tin bổ sung 1", "Trường thông tin bổ sung 1" },
                    { "689002c3-7f85-4485-a8e8-451459aade0c", 120, null, true, 1, 0, 0, null, 26, "TaiLieuDinhKem", "Tài liệu đính kèm", "Tài liệu đính kèm" },
                    { "28db3bb7-abd2-41a7-a715-f79890e1c0cc", 120, null, true, 1, 0, 0, null, 25, "NguoiLap", "Người lập", "Người lập" },
                    { "6f195e38-c4dc-486b-85cb-021d89e96816", 100, null, true, 4, 0, 0, null, 24, "NgayLap", "Ngày lập", "Ngày lập" },
                    { "cae70dee-fae0-4e01-b418-3281ae13935b", 120, null, true, 1, 0, 0, null, 23, "LoaiChungTu", "Loại chứng từ", "Loại chứng từ" },
                    { "d9e65870-0c4f-4901-9bab-9c4d26449fb9", 150, null, true, 1, 0, 0, null, 22, "LyDoXoaBo", "Lý do xóa bỏ", "Lý do xóa bỏ" },
                    { "4ca93f29-4767-413a-b5c3-7af4b04ea6d9", 180, null, true, 3, 0, 0, null, 21, "SoLanChuyenDoi", "Số lần chuyển thành hóa đơn giấy", "Số lần chuyển thành hóa đơn giấy" },
                    { "65e7c7b5-45b0-4b45-9550-e61ebaefdd57", 180, null, true, 6, 0, 0, null, 20, "KhachHangDaNhan", "Khách hàng đã nhận hóa đơn", "Khách hàng đã nhận hóa đơn" },
                    { "f696a80d-a1e3-4207-878c-279111b99e3d", 120, null, false, 1, 0, 0, null, 19, "SoDienThoaiNguoiNhanHD", "Số điện thoại người nhận", "Số điện thoại người nhận" },
                    { "89a8a691-f13c-4eaf-a4e1-f88e5a982dc7", 120, null, false, 1, 0, 0, null, 18, "EmailNguoiNhanHD", "Email người nhận", "Email người nhận" },
                    { "c9316139-9120-4865-a095-9714064fcbdc", 120, null, false, 1, 0, 0, null, 17, "HoTenNguoiNhanHD", "Tên người nhận", "Tên người nhận" },
                    { "d543ca78-4347-42f6-8bf1-3a6dd309b1fc", 120, null, true, 1, 0, 0, null, 16, "TrangThaiGuiHoaDon", "Trạng thái gửi hóa đơn", "Trạng thái gửi hóa đơn" },
                    { "fd7b2111-0c6e-4fe9-abb3-20cfa34c591f", 100, null, true, 1, 0, 0, null, 15, "MaTraCuu", "Mã tra cứu", "Mã tra cứu" },
                    { "ea6eb5fa-7625-4c4e-b8a2-dad6a30ca375", 120, null, true, 1, 0, 0, null, 14, "TrangThaiPhatHanh", "Trạng thái phát hành", "Trạng thái phát hành" },
                    { "33d64a21-12c3-4524-a6b6-29aad40f95f9", 120, null, true, 1, 0, 0, null, 13, "TrangThai", "Trạng thái hóa đơn", "Trạng thái hóa đơn" },
                    { "e3817656-f4d2-4b4e-943a-55a929dfafcc", 120, null, true, 1, 0, 0, null, 12, "LoaiHoaDon", "Loại hóa đơn", "Loại hóa đơn" },
                    { "58fcc96a-f94f-4d00-a7d5-d523b3f0cbd5", 150, null, true, 2, 0, 0, null, 11, "TongTienThanhToan", "Tổng tiền thanh toán", "Tổng tiền thanh toán" },
                    { "b78287fb-c455-49f4-9917-34c69add1de7", 120, null, false, 1, 0, 0, null, 10, "TenNhanVienBanHang", "NV bán hàng", "NV bán hàng" },
                    { "53a67712-d429-46ac-96d0-28a03ac91fd2", 120, null, false, 1, 0, 0, null, 9, "HoTenNguoiMuaHang", "Người mua hàng", "Người mua hàng" },
                    { "ca8db479-9598-44c4-98c3-86548981859c", 100, null, true, 1, 0, 0, null, 8, "MaSoThue", "Mã số thuế", "Mã số thuế" },
                    { "21e07f70-47a6-445d-b941-4c1172c4210e", 200, null, true, 1, 0, 0, null, 7, "DiaChi", "Địa chỉ", "Địa chỉ" },
                    { "feed82e5-7b0b-4532-b228-3dba3f876a6b", 180, null, true, 1, 0, 0, null, 6, "TenKhachHang", "Tên khách hàng", "Tên khách hàng" },
                    { "68ba176b-b6e2-4cc7-b642-5ac672406682", 100, null, false, 1, 0, 0, null, 5, "MaKhachHang", "Mã khách hàng", "Mã khách hàng" },
                    { "1254283b-65d9-45c2-9a8f-f429ddec21c6", 100, null, true, 1, 0, 0, null, 4, "KyHieu", "Ký hiệu hóa đơn", "Ký hiệu hóa đơn" },
                    { "61770c1c-d686-46bb-9f04-62a16550bcfa", 150, null, true, 1, 0, 0, null, 3, "MauSo", "Ký hiệu mẫu số hóa đơn", "Ký hiệu mẫu số hóa đơn" },
                    { "9e80be70-155b-4c0f-8dca-b7282940fc97", 100, null, true, 1, 0, 0, null, 2, "SoHoaDon", "Số hóa đơn", "Số hóa đơn" },
                    { "a25570be-70b1-48ec-91c9-48b845598c90", 100, null, false, 1, 0, 0, null, 29, "TruongThongTinBoSung3", "Trường thông tin bổ sung 3", "Trường thông tin bổ sung 3" },
                    { "d281be37-96e3-4d0c-82db-0ac759302c31", 100, null, false, 1, 0, 0, null, 30, "TruongThongTinBoSung4", "Trường thông tin bổ sung 4", "Trường thông tin bổ sung 4" },
                    { "48f996bc-f5a2-4311-af4c-2438ede16462", 100, null, false, 1, 0, 0, null, 31, "TruongThongTinBoSung5", "Trường thông tin bổ sung 5", "Trường thông tin bổ sung 5" },
                    { "72e7030c-7c50-48a8-86a7-5caa7c9318c8", 100, null, false, 1, 0, 0, null, 32, "TruongThongTinBoSung6", "Trường thông tin bổ sung 6", "Trường thông tin bổ sung 6" },
                    { "56d6054e-27d0-4c69-a498-c60682d4d55a", 120, null, true, 2, 1, 2, "HHDV 14", 14, "TienChietKhau", "Tiền chiết khấu", "Tiền chiết khấu" },
                    { "4b0750d6-d31e-44a3-8097-7c079ae2dc55", 100, null, true, 5, 1, 2, "HHDV 13", 13, "TyLeChietKhau", "Tỷ lệ chiết khấu", "Tỷ lệ chiết khấu" },
                    { "15f17db7-7008-48b2-a09a-3321a62340cd", 150, null, false, 2, 1, 2, "HHDV 12", 12, "ThanhTienQuyDoi", "Thành tiền quy đổi", "Thành tiền quy đổi" },
                    { "67f11dfd-4c43-44e2-bbc2-11fed8541aee", 150, null, true, 2, 1, 2, "HHDV 11", 11, "ThanhTien", "Thành tiền", "Thành tiền" },
                    { "27a5dfc4-9a41-4b89-b931-46ddf92d6713", 150, null, false, 2, 1, 2, "HHDV 10", 10, "ThanhTienSauThue", "Thành tiền sau thuế", "Thành tiền sau thuế" },
                    { "f9bbc4a3-5a97-47a5-90e8-9ee244ab8c3f", 120, null, true, 2, 1, 2, "HHDV 9", 9, "DonGia", "Đơn giá", "Đơn giá" },
                    { "7e158d84-7472-4fea-8445-af4d4122be9b", 120, null, false, 2, 1, 2, "HHDV 8", 8, "DonGiaSauThue", "Đơn giá sau thuế", "Đơn giá sau thuế" },
                    { "fc272b0a-5b75-4598-afe8-d27187ca6d82", 100, null, true, 3, 1, 2, "HHDV 7", 7, "SoLuong", "Số lượng", "Số lượng" },
                    { "f91e2ac2-2f97-433e-a0d0-71138de0dbda", 100, null, true, 1, 1, 2, "HHDV 6", 6, "DonViTinhId", "ĐVT", "ĐVT" },
                    { "284474a0-daa1-4f3e-ac39-71bb7df03df5", 100, null, false, 1, 1, 2, "HHDV 5", 5, "MaQuyCach", "Mã quy cách", "Mã quy cách" },
                    { "627cc1d4-8b8a-4345-b8b0-6a21baadee95", 120, null, true, 6, 1, 2, "HHDV 4", 4, "HangKhuyenMai", "Hàng khuyến mại", "Hàng khuyến mại" },
                    { "7ba85e12-82bb-40d2-bd46-23fe9f8e8f36", 150, null, true, 1, 1, 2, "HHDV 3", 3, "TenHang", "Tên hàng", "Tên hàng hóa, dịch vụ" },
                    { "ffa07117-d5d5-4b02-85fc-6263b0fc1c41", 100, null, true, 1, 1, 2, "HHDV 2", 2, "MaHang", "Mã hàng", "Mã hàng" },
                    { "d8015c3b-6c8e-4e69-814c-33bade7991c6", 100, null, true, 5, 1, 2, "HHDV 16", 16, "ThueGTGT", "% Thuế GTGT", "% Thuế GTGT" },
                    { "58b4d6e6-6c76-4ce6-8825-00223166fe06", 50, null, true, 3, 1, 2, "HHDV 1", 1, "STT", "STT", "STT" },
                    { "d63bbe50-5a20-41ea-a490-11d9a13d77a5", null, null, true, 1, 0, 1, null, 9, "TruongThongTinBoSung9", "Trường thông tin bổ sung 9", "Trường thông tin bổ sung 9" },
                    { "b435cd9c-d2fc-45c9-9823-05fe5676adcf", null, null, true, 1, 0, 1, null, 8, "TruongThongTinBoSung8", "Trường thông tin bổ sung 8", "Trường thông tin bổ sung 8" },
                    { "4897fc0a-23ce-45db-b064-ac9b6992b601", null, null, true, 1, 0, 1, null, 7, "TruongThongTinBoSung7", "Trường thông tin bổ sung 7", "Trường thông tin bổ sung 7" },
                    { "d4e0665f-5365-4278-9ca7-936acea4efae", null, null, true, 1, 0, 1, null, 6, "TruongThongTinBoSung6", "Trường thông tin bổ sung 6", "Trường thông tin bổ sung 6" },
                    { "909dcbb1-d0d8-41d2-b3e5-df80cc2ca1b9", null, null, true, 1, 0, 1, null, 5, "TruongThongTinBoSung5", "Trường thông tin bổ sung 5", "Trường thông tin bổ sung 5" },
                    { "4b97ef52-d4df-4e19-aa02-8cacbfdee842", null, null, true, 1, 0, 1, null, 4, "TruongThongTinBoSung4", "Trường thông tin bổ sung 4", "Trường thông tin bổ sung 4" },
                    { "9515cb61-e7b1-4cc7-be82-8990bebc5386", null, null, true, 1, 0, 1, null, 3, "TruongThongTinBoSung3", "Trường thông tin bổ sung 3", "Trường thông tin bổ sung 3" },
                    { "918354f1-8b1c-447a-9c51-6c88d13083ad", null, null, true, 1, 0, 1, null, 2, "TruongThongTinBoSung2", "Trường thông tin bổ sung 2", "Trường thông tin bổ sung 2" },
                    { "ecb82130-ac10-4b52-b479-14b647a5abdd", null, null, true, 1, 0, 1, null, 1, "TruongThongTinBoSung1", "Trường thông tin bổ sung 1", "Trường thông tin bổ sung 1" },
                    { "a8b32fa1-e209-4dd3-a604-63c063364ad8", 100, null, false, 1, 0, 0, null, 36, "TruongThongTinBoSung10", "Trường thông tin bổ sung 10", "Trường thông tin bổ sung 10" },
                    { "a640eef7-6b24-4803-8aac-c409e81fbfbc", 100, null, false, 1, 0, 0, null, 35, "TruongThongTinBoSung9", "Trường thông tin bổ sung 9", "Trường thông tin bổ sung 9" },
                    { "a655f644-c9f6-4025-9044-b4120dacfb58", 100, null, false, 1, 0, 0, null, 34, "TruongThongTinBoSung8", "Trường thông tin bổ sung 8", "Trường thông tin bổ sung 8" },
                    { "c1c6e9c1-fd82-4137-ab1f-37b4b1be1ad2", 100, null, false, 1, 0, 0, null, 33, "TruongThongTinBoSung7", "Trường thông tin bổ sung 7", "Trường thông tin bổ sung 7" },
                    { "160fe51c-09c7-4e39-9099-7a2081149017", null, null, true, 1, 0, 1, null, 10, "TruongThongTinBoSung10", "Trường thông tin bổ sung 10", "Trường thông tin bổ sung 10" },
                    { "a9ef1f16-b100-4e1d-8110-56ca1facfe29", 100, null, false, 1, 2, 2, "HHDV 36", 36, "TruongMoRongChiTiet10", "Trường mở rộng chi tiết số 10", "Trường mở rộng chi tiết số 10" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThietLapTruongDuLieus");

            migrationBuilder.DropColumn(
                name: "MaQuyCach",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet1",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung9",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung9Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung8",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung8Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung7",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung7Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung6",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung6Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung5",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung5Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung4",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung4Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung3",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung3Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung2",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung2Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung10",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung1Id");

            migrationBuilder.RenameColumn(
                name: "TruongThongTinBoSung1",
                table: "HoaDonDienTus",
                newName: "TruongThongTinBoSung10Id");

            migrationBuilder.RenameColumn(
                name: "XuatBanPhi",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet9Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet9",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet8Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet8",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet7Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet7",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet6Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet6",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet5Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet5",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet4Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet4",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet3Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet3",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet2Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet2",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet1Id");

            migrationBuilder.RenameColumn(
                name: "TruongMoRongChiTiet10",
                table: "HoaDonDienTuChiTiets",
                newName: "TruongMoRongChiTiet10Id");

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ThietLapTruongDuLieuMoRongs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    GhiChu = table.Column<string>(nullable: true),
                    HienThi = table.Column<bool>(nullable: false),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    STT = table.Column<int>(nullable: false),
                    TenTruong = table.Column<string>(nullable: true),
                    TenTruongHienThi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThietLapTruongDuLieuMoRongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TruongDuLieuHoaDons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Align = table.Column<string>(nullable: true),
                    Default = table.Column<bool>(nullable: false),
                    DefaultSTT = table.Column<int>(nullable: false),
                    DinhDangSo = table.Column<bool>(nullable: false),
                    GhiChu = table.Column<string>(nullable: true),
                    IsChiTiet = table.Column<bool>(nullable: false),
                    IsLeft = table.Column<bool>(nullable: false),
                    IsMoRong = table.Column<bool>(nullable: false),
                    Left = table.Column<int>(nullable: false),
                    LoaiHoaDon = table.Column<int>(nullable: false),
                    MaTruong = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: false),
                    Size = table.Column<int>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    TenTruong = table.Column<string>(nullable: true),
                    TenTruongData = table.Column<string>(nullable: true),
                    TenTruongHienThi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongDuLieuHoaDons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TruongDuLieuMoRongs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DataId = table.Column<string>(nullable: true),
                    DuLieu = table.Column<string>(nullable: true),
                    HienThi = table.Column<bool>(nullable: false),
                    TenTruong = table.Column<string>(nullable: true),
                    TenTruongHienThi = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongDuLieuMoRongs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TruongMoRongHoaDons",
                columns: table => new
                {
                    TruongMoRongHoaDonId = table.Column<string>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true),
                    HoaDonDienTuId = table.Column<string>(nullable: true),
                    MauHoaDonTuyChinhChiTietId = table.Column<string>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruongMoRongHoaDons", x => x.TruongMoRongHoaDonId);
                    table.ForeignKey(
                        name: "FK_TruongMoRongHoaDons_HoaDonDienTus_HoaDonDienTuId",
                        column: x => x.HoaDonDienTuId,
                        principalTable: "HoaDonDienTus",
                        principalColumn: "HoaDonDienTuId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung1Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung2Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung3Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung4Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung5Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung6Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung7Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung8Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung9Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet1Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet2Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet3Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet4Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet5Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet6Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet7Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet8Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet9Id");

            migrationBuilder.CreateIndex(
                name: "IX_TruongMoRongHoaDons_HoaDonDienTuId",
                table: "TruongMoRongHoaDons",
                column: "HoaDonDienTuId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet10Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet1Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet2Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet3Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet4Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet5Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet6Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet7Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet8Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet9Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung10Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung1Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung2Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung3Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung4Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung5Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung6Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung7Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung8Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung9Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
