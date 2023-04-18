using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddatainitthongtinhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM QuanLyThongTinHoaDons )
                                BEGIN
                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        1,
                                        1,
                                        1,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        2,
                                        1,
                                        2,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        1,
                                        2,
                                        3,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        2,
                                        2,
                                        4,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        3,
                                        2,
                                        5,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        4,
                                        2,
                                        6,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        5,
                                        2,
                                        7,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );

                                    INSERT INTO QuanLyThongTinHoaDons (
                                        QuanLyThongTinHoaDonId,
                                        STT,
                                        LoaiThongTin,
                                        LoaiThongTinChiTiet,
                                        TrangThaiSuDung,
                                        NgayBatDauSuDung,
                                        TuNgayTamNgungSuDung,
                                        DenNgayTamNgungSuDung,
                                        NgayNgungSuDung
                                    ) VALUES (
                                        NEWID(),
                                        6,
                                        2,
                                        8,
                                        1,
                                        NULL,
                                        NULL,
                                        NULL,
                                        NULL
                                    );
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
