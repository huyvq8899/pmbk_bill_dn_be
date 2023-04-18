using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthongbaosaisotkhongphailaphoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CanCuocCongDan_Dung",
                table: "ThongBaoSaiThongTins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanCuocCongDan_Sai",
                table: "ThongBaoSaiThongTins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai_Dung",
                table: "ThongBaoSaiThongTins",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai_Sai",
                table: "ThongBaoSaiThongTins",
                nullable: true);

            //migrationBuilder.CreateTable(
            //    name: "TuyChonHoaDons",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(nullable: false),
            //        Ma = table.Column<string>(nullable: true),
            //        RefId = table.Column<string>(nullable: true),
            //        GiaTri = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TuyChonHoaDons", x => x.Id);
            //    });

            migrationBuilder.Sql(@"IF NOT EXISTS(SELECT 1 FROM sys.objects
                    WHERE object_id = object_id(N'[dbo].[TuyChonHoaDons]')
                    AND type in (N'U'))
            BEGIN
                CREATE TABLE TuyChonHoaDons(
                    Id NVARCHAR(36) PRIMARY KEY,
                    Ma NVARCHAR(256),
                    RefId NVARCHAR(36),
                    GiaTri NVARCHAR(256)
                )
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TuyChonHoaDons");

            migrationBuilder.DropColumn(
                name: "CanCuocCongDan_Dung",
                table: "ThongBaoSaiThongTins");

            migrationBuilder.DropColumn(
                name: "CanCuocCongDan_Sai",
                table: "ThongBaoSaiThongTins");

            migrationBuilder.DropColumn(
                name: "SoDienThoai_Dung",
                table: "ThongBaoSaiThongTins");

            migrationBuilder.DropColumn(
                name: "SoDienThoai_Sai",
                table: "ThongBaoSaiThongTins");
        }
    }
}
