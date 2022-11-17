using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonkymem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaiKhoanSmartCAs",
                columns: table => new
                {
                    TaiKhoanSmartCAId = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    UserNameSmartCA = table.Column<string>(nullable: true),
                    PasswordSmartCA = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaiKhoanSmartCAs", x => x.TaiKhoanSmartCAId);
                });

            migrationBuilder.Sql(@"IF NOT EXISTS ( SELECT 1 FROM TuyChons Where Ma = 'IsSelectChuKiCung' )
                                BEGIN
                                    INSERT INTO TuyChons VALUES ('IsSelectChuKiCung', N'Chọn kí cứng hay kí mềm', 'KiCung')
                                END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaiKhoanSmartCAs");
        }
    }
}
