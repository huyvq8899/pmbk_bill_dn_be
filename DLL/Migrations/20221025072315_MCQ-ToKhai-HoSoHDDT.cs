using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class MCQToKhaiHoSoHDDT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MaCuaCQTToKhaiChapNhan'
                      AND Object_ID = Object_ID(N'BoKyHieuHoaDons'))
            BEGIN
                ALTER TABLE BoKyHieuHoaDons
                ADD MaCuaCQTToKhaiChapNhan nvarchar(23)
            END");

            //migrationBuilder.AddColumn<string>(
            //    name: "MaCuaCQTToKhaiChapNhan",
            //    table: "HoSoHDDTs",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropColumn(
            //    name: "MaCuaCQTToKhaiChapNhan",
            //    table: "HoSoHDDTs");

            migrationBuilder.Sql(@"IF EXISTS(SELECT 1 FROM sys.columns 
                      WHERE Name = N'MaCuaCQTToKhaiChapNhan'
                      AND Object_ID = Object_ID(N'BoKyHieuHoaDons'))
            BEGIN
                ALTER TABLE BoKyHieuHoaDons
                DROP COLUMN MaCuaCQTToKhaiChapNhan
            END");
        }
    }
}
