using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtuychonhoadontable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "TuyChonHoaDons",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(maxLength: 36, nullable: false),
            //        Ma = table.Column<string>(maxLength: 256, nullable: true),
            //        RefId = table.Column<string>(maxLength: 36, nullable: true),
            //        GiaTri = table.Column<string>(maxLength: 256, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_TuyChonHoaDons", x => x.Id);
            //    });

            migrationBuilder.Sql(@"IF NOT EXISTS((SELECT * 
                 FROM INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'TheSchema' 
                 AND  TABLE_NAME = 'TuyChonHoaDons'))
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
        }
    }
}
