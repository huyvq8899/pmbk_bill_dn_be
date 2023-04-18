using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtendonvitinhchohoadonchitiet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TenDonViTinh",
                table: "HoaDonDienTuChiTiets",
                maxLength: 50,
                nullable: true);

            migrationBuilder.Sql("update HoaDonDienTuChiTiets set TenDonViTinh = (select Ten from DonViTinhs where DonViTinhId = HoaDonDienTuChiTiets.DonViTinhId)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenDonViTinh",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
