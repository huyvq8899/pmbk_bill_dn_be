using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addlinkpostURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosCustomerURL",
                table: "HoaDonDienTus",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CertificateDN",
                table: "ChungThuSoSuDungs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PosCustomerURL",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "CertificateDN",
                table: "ChungThuSoSuDungs");
        }
    }
}
