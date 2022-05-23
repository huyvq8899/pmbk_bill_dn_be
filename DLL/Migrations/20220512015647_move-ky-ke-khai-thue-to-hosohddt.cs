using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class movekykekhaithuetohosohddt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KyKeKhaiThue",
                table: "HoSoHDDTs",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(@"
                DECLARE @kyKeKhaiThue nvarchar(MAX);
                SELECT @kyKeKhaiThue = GiaTri FROM TuyChons WHERE Ma = 'KyKeKhaiThueGTGT';
                UPDATE HoSoHDDTs SET KyKeKhaiThue = IIF(@kyKeKhaiThue = 'Thang', 0, 1);
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KyKeKhaiThue",
                table: "HoSoHDDTs");
        }
    }
}
