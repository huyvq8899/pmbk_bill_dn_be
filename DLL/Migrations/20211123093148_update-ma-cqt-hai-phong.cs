using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class Updatemacqthaiphong : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
            @"
                UPDATE CoQuanThues SET Ma='3101' WHERE Ma='10300';
                UPDATE CoQuanThues SET Ma='3102' WHERE Ma='10301';
                UPDATE CoQuanThues SET Ma='3103' WHERE Ma='10303';
                UPDATE CoQuanThues SET Ma='3104' WHERE Ma='10304';
                UPDATE CoQuanThues SET Ma='3105' WHERE Ma='10305';
                UPDATE CoQuanThues SET Ma='3106' WHERE Ma='10307';
                UPDATE CoQuanThues SET Ma='3107' WHERE Ma='10309';
                UPDATE CoQuanThues SET Ma='3108' WHERE Ma='10311';
                UPDATE CoQuanThues SET Ma='3109' WHERE Ma='10313';
                UPDATE CoQuanThues SET Ma='3110' WHERE Ma='10315';
                UPDATE CoQuanThues SET Ma='3111' WHERE Ma='10317';
                UPDATE CoQuanThues SET Ma='3112' WHERE Ma='10319';
                UPDATE CoQuanThues SET Ma='3113' WHERE Ma='10321';
                UPDATE CoQuanThues SET Ma='3114' WHERE Ma='10323';
                UPDATE CoQuanThues SET Ma='3115' WHERE Ma='10327';
                UPDATE CoQuanThues SET MaCQTCapCuc='3101' WHERE MaCQTCapCuc='10300';

                UPDATE CoQuanThueCapCuc_DiaDanhs SET MaCQT = '3101' WHERE MaCQT = '10300';

                UPDATE HoSoHDDTs SET CoQuanThueCapCuc='3101' WHERE CoQuanThueCapCuc='10300';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3101' WHERE CoQuanThueQuanLy='10300';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3102' WHERE CoQuanThueQuanLy='10301';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3103' WHERE CoQuanThueQuanLy='10303';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3104' WHERE CoQuanThueQuanLy='10304';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3105' WHERE CoQuanThueQuanLy='10305';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3106' WHERE CoQuanThueQuanLy='10307';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3107' WHERE CoQuanThueQuanLy='10309';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3108' WHERE CoQuanThueQuanLy='10311';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3109' WHERE CoQuanThueQuanLy='10313';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3110' WHERE CoQuanThueQuanLy='10315';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3111' WHERE CoQuanThueQuanLy='10317';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3112' WHERE CoQuanThueQuanLy='10319';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3113' WHERE CoQuanThueQuanLy='10321';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3114' WHERE CoQuanThueQuanLy='10323';
                UPDATE HoSoHDDTs SET CoQuanThueQuanLy='3115' WHERE CoQuanThueQuanLy='10327';
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
