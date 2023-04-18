using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class updatedataphanquyenmauhoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE  pq
                                    SET     pq.MauHoaDonIds = bkh.MauHoaDonId
                                    FROM    PhanQuyenMauHoaDons pq
                                            INNER JOIN BoKyHieuHoaDons bkh
                                                ON pq.BoKyHieuHoaDonId = bkh.BoKyHieuHoaDonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
