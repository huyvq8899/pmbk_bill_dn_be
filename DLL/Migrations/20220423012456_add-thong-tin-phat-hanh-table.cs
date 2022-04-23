using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addthongtinphathanhtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ThongTinPhatHanhs",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MaSoThue = table.Column<string>(unicode: false, maxLength: 16, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThongTinPhatHanhs", x => x.Id);
                });

            migrationBuilder.Sql(@"
                IF NOT EXISTS(SELECT 1 FROM ThongTinPhatHanhs)
                BEGIN
                    INSERT INTO ThongTinPhatHanhs VALUES (NEWID(), '0200784873', 1);
                    INSERT INTO ThongTinPhatHanhs VALUES (NEWID(), '0202029650', 0);
                END
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ThongTinPhatHanhs");
        }
    }
}
