using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcollapvanbanthoathuanhddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TrangThaiPhatHanh",
                table: "HoaDonDienTus",
                newName: "TrangThaiQuyTrinh");

            migrationBuilder.AddColumn<bool>(
                name: "IsLapVanBanThoaThuan",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'IntCanhBaoKhiKhongLapVBDTTT' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('IntCanhBaoKhiKhongLapVBDTTT', N'Cảnh báo khi không lập văn bản điện tử thỏa thuận trước khi lập hóa đơn điều chỉnh, hóa đơn thay thế cho hóa đơn đã lập có sai sót', 1)
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsLapVanBanThoaThuan",
                table: "HoaDonDienTus");

            migrationBuilder.RenameColumn(
                name: "TrangThaiQuyTrinh",
                table: "HoaDonDienTus",
                newName: "TrangThaiPhatHanh");
        }
    }
}
