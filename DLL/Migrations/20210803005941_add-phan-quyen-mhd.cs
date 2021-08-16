using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addphanquyenmhd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhanQuyenMauHoaDons",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    MauHoaDonId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhanQuyenMauHoaDons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhanQuyenMauHoaDons_MauHoaDons_MauHoaDonId",
                        column: x => x.MauHoaDonId,
                        principalTable: "MauHoaDons",
                        principalColumn: "MauHoaDonId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PhanQuyenMauHoaDons_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhanQuyenMauHoaDons_MauHoaDonId",
                table: "PhanQuyenMauHoaDons",
                column: "MauHoaDonId");

            migrationBuilder.CreateIndex(
                name: "IX_PhanQuyenMauHoaDons_RoleId",
                table: "PhanQuyenMauHoaDons",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhanQuyenMauHoaDons");
        }
    }
}
