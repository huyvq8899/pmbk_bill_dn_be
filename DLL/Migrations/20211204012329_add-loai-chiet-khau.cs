using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addloaichietkhau : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.columns 
                          WHERE Name = N'LoaiChietKhau'
                          AND Object_ID = Object_ID(N'dbo.HoaDonDienTus'))
                BEGIN
                    ALTER TABLE [HoaDonDienTus] ADD [LoaiChietKhau] int DEFAULT 1;
                END");
            //migrationBuilder.AddColumn<int>(
            //    name: "LoaiChietKhau",
            //    table: "HoaDonDienTus",
            //    nullable: false,
            //    defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LoaiChietKhau",
                table: "HoaDonDienTus");
        }
    }
}
