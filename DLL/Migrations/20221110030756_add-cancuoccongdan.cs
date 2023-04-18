using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcancuoccongdan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "MauSoTBaoPhanHoiCuaCQT",
            //    table: "ThongDiepChungs",
            //    nullable: true);
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MauSoTBaoPhanHoiCuaCQT'
                      AND Object_ID = Object_ID(N'ThongDiepChungs'))
            BEGIN
                ALTER TABLE ThongDiepChungs
                ADD MauSoTBaoPhanHoiCuaCQT nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "MauHoaDonId",
            //    table: "MauHoaDonXacThucs",
            //    nullable: true);
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MauHoaDonId'
                      AND Object_ID = Object_ID(N'MauHoaDonXacThucs'))
            BEGIN
                ALTER TABLE MauHoaDonXacThucs
                ADD MauHoaDonId nvarchar(36)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "MaCuaCQTToKhaiChapNhan",
            //    table: "HoSoHDDTs",
            //    nullable: true);
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MaCuaCQTToKhaiChapNhan'
                      AND Object_ID = Object_ID(N'HoSoHDDTs'))
            BEGIN
                ALTER TABLE HoSoHDDTs
                ADD MaCuaCQTToKhaiChapNhan nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "TenCoQuanThueCapCuc",
            //    table: "HoSoHDDTs",
            //    nullable: true);
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TenCoQuanThueCapCuc'
                      AND Object_ID = Object_ID(N'HoSoHDDTs'))
            BEGIN
                ALTER TABLE HoSoHDDTs
                ADD TenCoQuanThueCapCuc nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "TenCoQuanThueQuanLy",
            //    table: "HoSoHDDTs",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TenCoQuanThueQuanLy'
                      AND Object_ID = Object_ID(N'HoSoHDDTs'))
            BEGIN
                ALTER TABLE HoSoHDDTs
                ADD TenCoQuanThueQuanLy nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "CanCuocCongDan",
            //    table: "HoaDonDienTus",
            //    nullable: true);
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'CanCuocCongDan'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD CanCuocCongDan nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "EmailNhanKemTheo",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'EmailNhanKemTheo'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD EmailNhanKemTheo nvarchar(max)
            END");

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsKemGuiEmail",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'IsKemGuiEmail'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD IsKemGuiEmail bit
            END");

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsNopThueTheoThongTu1032014BTC",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'IsNopThueTheoThongTu1032014BTC'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD IsNopThueTheoThongTu1032014BTC bit
            END");

            //migrationBuilder.AddColumn<int>(
            //    name: "IsPos",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'IsPos'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD IsPos int
            END");

            //migrationBuilder.AddColumn<int>(
            //    name: "LoaiThue",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'LoaiThue'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD LoaiThue int
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "MCCQT",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MCCQT'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD MCCQT nvarchar(23)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "TenNguoiNhanKemTheo",
            //    table: "HoaDonDienTus",
            //    nullable: true);
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TenNguoiNhanKemTheo'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                ADD TenNguoiNhanKemTheo nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "ThongDiepChungId",
            //    table: "DuLieuGuiHDDTs",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'ThongDiepChungId'
                      AND Object_ID = Object_ID(N'DuLieuGuiHDDTs'))
            BEGIN
                ALTER TABLE DuLieuGuiHDDTs
                ADD ThongDiepChungId nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "CanCuocCongDan",
            //    table: "DoiTuongs",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'CanCuocCongDan'
                      AND Object_ID = Object_ID(N'DoiTuongs'))
            BEGIN
                ALTER TABLE DoiTuongs
                ADD CanCuocCongDan nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "MaCuaCQTToKhaiChapNhan",
            //    table: "BoKyHieuHoaDons",
            //    maxLength: 23,
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MaCuaCQTToKhaiChapNhan'
                      AND Object_ID = Object_ID(N'BoKyHieuHoaDons'))
            BEGIN
                ALTER TABLE BoKyHieuHoaDons
                ADD MaCuaCQTToKhaiChapNhan nvarchar(23)
            END");

            //migrationBuilder.AddColumn<long>(
            //    name: "SoBatDauCMCQT",
            //    table: "BoKyHieuHoaDons",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'SoBatDauCMCQT'
                      AND Object_ID = Object_ID(N'BoKyHieuHoaDons'))
            BEGIN
                ALTER TABLE BoKyHieuHoaDons
                ADD SoBatDauCMCQT nvarchar(max)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "DVTTe",
            //    table: "BangTongHopDuLieuHoaDons",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'DVTTe'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDons'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDons
                ADD DVTTe nvarchar(max)
            END");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "TGTKhac",
            //    table: "BangTongHopDuLieuHoaDonChiTiets",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TGTKhac'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDonChiTiets'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDonChiTiets
                ADD TGTKhac nvarchar(max)
            END");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "TGia",
            //    table: "BangTongHopDuLieuHoaDonChiTiets",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TGia'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDonChiTiets'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDonChiTiets
                ADD TGia decimal(18,2)
            END");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "TTPhi",
            //    table: "BangTongHopDuLieuHoaDonChiTiets",
            //    nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TTPhi'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDonChiTiets'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDonChiTiets
                ADD TTPhi decimal(18,2)
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MauSoTBaoPhanHoiCuaCQT'
                      AND Object_ID = Object_ID(N'ThongDiepChungs'))
            BEGIN
                ALTER TABLE ThongDiepChungs
                DROP COLUMN MauSoTBaoPhanHoiCuaCQT
            END");
            //migrationBuilder.DropColumn(
            //    name: "MauSoTBaoPhanHoiCuaCQT",
            //    table: "ThongDiepChungs");

            //migrationBuilder.DropColumn(
            //    name: "MauHoaDonId",
            //    table: "MauHoaDonXacThucs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MauHoaDonId'
                      AND Object_ID = Object_ID(N'MauHoaDonXacThucs'))
            BEGIN
                ALTER TABLE MauHoaDonXacThucs
                DROP COLUMN MauHoaDonId
            END");

            //migrationBuilder.DropColumn(
            //    name: "MaCuaCQTToKhaiChapNhan",
            //    table: "HoSoHDDTs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MaCuaCQTToKhaiChapNhan'
                      AND Object_ID = Object_ID(N'HoSoHDDTs'))
            BEGIN
                ALTER TABLE HoSoHDDTs
                DROP COLUMN MaCuaCQTToKhaiChapNhan
            END");

            //migrationBuilder.DropColumn(
            //    name: "TenCoQuanThueCapCuc",
            //    table: "HoSoHDDTs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TenCoQuanThueCapCuc'
                      AND Object_ID = Object_ID(N'HoSoHDDTs'))
            BEGIN
                ALTER TABLE HoSoHDDTs
                DROP COLUMN TenCoQuanThueCapCuc
            END");


            //migrationBuilder.DropColumn(
            //    name: "TenCoQuanThueQuanLy",
            //    table: "HoSoHDDTs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TenCoQuanThueQuanLy'
                      AND Object_ID = Object_ID(N'HoSoHDDTs'))
            BEGIN
                ALTER TABLE HoSoHDDTs
                DROP COLUMN TenCoQuanThueQuanLy
            END");

            //migrationBuilder.DropColumn(
            //    name: "CanCuocCongDan",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'CanCuocCongDan'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN CanCuocCongDan
            END");

            //migrationBuilder.DropColumn(
            //    name: "EmailNhanKemTheo",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'EmailNhanKemTheo'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN EmailNhanKemTheo
            END");

            //migrationBuilder.DropColumn(
            //    name: "IsKemGuiEmail",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'IsKemGuiEmail'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN IsKemGuiEmail
            END");

            //migrationBuilder.DropColumn(
            //    name: "IsNopThueTheoThongTu1032014BTC",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'IsNopThueTheoThongTu1032014BTC'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN IsNopThueTheoThongTu1032014BTC
            END");

            //migrationBuilder.DropColumn(
            //    name: "IsPos",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'IsPos'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN IsPos
            END");

            //migrationBuilder.DropColumn(
            //    name: "LoaiThue",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'LoaiThue'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN LoaiThue
            END");

            //migrationBuilder.DropColumn(
            //    name: "MCCQT",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MCCQT'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN MCCQT
            END");

            //migrationBuilder.DropColumn(
            //    name: "TenNguoiNhanKemTheo",
            //    table: "HoaDonDienTus");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TenNguoiNhanKemTheo'
                      AND Object_ID = Object_ID(N'HoaDonDienTus'))
            BEGIN
                ALTER TABLE HoaDonDienTus
                DROP COLUMN TenNguoiNhanKemTheo
            END");

            //migrationBuilder.DropColumn(
            //    name: "ThongDiepChungId",
            //    table: "DuLieuGuiHDDTs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'ThongDiepChungId'
                      AND Object_ID = Object_ID(N'DuLieuGuiHDDTs'))
            BEGIN
                ALTER TABLE DuLieuGuiHDDTs
                DROP COLUMN ThongDiepChungId
            END");

            //migrationBuilder.DropColumn(
            //    name: "CanCuocCongDan",
            //    table: "DoiTuongs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'CanCuocCongDan'
                      AND Object_ID = Object_ID(N'DoiTuongs'))
            BEGIN
                ALTER TABLE DoiTuongs
                DROP COLUMN CanCuocCongDan
            END");

            //migrationBuilder.DropColumn(
            //    name: "MaCuaCQTToKhaiChapNhan",
            //    table: "BoKyHieuHoaDons");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MaCuaCQTToKhaiChapNhan'
                      AND Object_ID = Object_ID(N'BoKyHieuHoaDons'))
            BEGIN
                ALTER TABLE BoKyHieuHoaDons
                DROP COLUMN MaCuaCQTToKhaiChapNhan
            END");

            //migrationBuilder.DropColumn(
            //    name: "SoBatDauCMCQT",
            //    table: "BoKyHieuHoaDons");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'SoBatDauCMCQT'
                      AND Object_ID = Object_ID(N'BoKyHieuHoaDons'))
            BEGIN
                ALTER TABLE BoKyHieuHoaDons
                DROP COLUMN SoBatDauCMCQT
            END");

            //migrationBuilder.DropColumn(
            //    name: "DVTTe",
            //    table: "BangTongHopDuLieuHoaDons");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'DVTTe'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDons'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDons
                DROP COLUMN DVTTe
            END");

            //migrationBuilder.DropColumn(
            //    name: "TGTKhac",
            //    table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TGTKhac'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDonChiTiets'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDonChiTiets
                DROP COLUMN TGTKhac
            END");

            //migrationBuilder.DropColumn(
            //    name: "TGia",
            //    table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TGia'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDonChiTiets'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDonChiTiets
                DROP COLUMN TGia
            END");

            //migrationBuilder.DropColumn(
            //    name: "TTPhi",
            //    table: "BangTongHopDuLieuHoaDonChiTiets");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'TTPhi'
                      AND Object_ID = Object_ID(N'BangTongHopDuLieuHoaDonChiTiets'))
            BEGIN
                ALTER TABLE BangTongHopDuLieuHoaDonChiTiets
                DROP COLUMN TTPhi
            END");
        }
    }
}
