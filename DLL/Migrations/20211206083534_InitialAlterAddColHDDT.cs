using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class InitialAlterAddColHDDT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS(SELECT 1 FROM sys.columns 
                          WHERE Name = N'BackUpTrangThai'
                          AND Object_ID = Object_ID(N'dbo.HoaDonDienTus'))
                BEGIN
                    ALTER TABLE [HoaDonDienTus] ADD [BackUpTrangThai] int NULL;
                END

                IF NOT EXISTS(SELECT 1 FROM sys.columns 
                          WHERE Name = N'HinhThucXoabo'
                          AND Object_ID = Object_ID(N'dbo.HoaDonDienTus'))
                BEGIN
                    ALTER TABLE [HoaDonDienTus] ADD [HinhThucXoabo] int NULL;
                END
            ");
            //migrationBuilder.AddColumn<int>(
            //    name: "BackUpTrangThai",
            //    table: "HoaDonDienTus",
            //    nullable: true);

            //migrationBuilder.AddColumn<int>(
            //    name: "HinhThucXoabo",
            //    table: "HoaDonDienTus",
            //    nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackUpTrangThai",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "HinhThucXoabo",
                table: "HoaDonDienTus");
        }
    }
}
