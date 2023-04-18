using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class edittuychonstt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Functions
                SET STT=30
                WHERE FunctionName = 'BaoCao';

                UPDATE Functions
                SET STT=31
                WHERE FunctionName = 'TienIch';

                UPDATE Functions
                SET STT=32
                WHERE FunctionName = 'NhatKyGuiEmail';

                UPDATE Functions
                SET STT=33
                WHERE FunctionName = 'NhatKyTruyCap';

                UPDATE Functions
                SET STT=29
                WHERE FunctionName = 'PhieuXuatKho';
            ");
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
