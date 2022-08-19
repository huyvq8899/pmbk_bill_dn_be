using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtriggerupdatetrangthaiquytrinhifhasmacuacqt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"IF (OBJECT_ID('[dbo].[UpdateHoaDonDienTu]') IS NOT NULL)
                                    BEGIN
                                          DROP TRIGGER [dbo].[UpdateHoaDonDienTu];
                                    END
                                    GO
                                    CREATE TRIGGER [dbo].[UpdateHoaDonDienTu]
                                    ON [dbo].[HoaDonDienTus]
                                    AFTER UPDATE
                                    AS
                                    BEGIN
                                        IF EXISTS ( SELECT 1 FROM HoaDonDienTus Where MaCuaCQT IS NOT NULL AND TrangThaiQuyTrinh != 9 )
                                        BEGIN
                                            UPDATE HoaDonDienTus
	                                        SET TrangThaiQuyTrinh = 9
	                                        WHERE MaCuaCQT IS NOT NULL AND TrangThaiQuyTrinh != 9
                                        END
                                    END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
