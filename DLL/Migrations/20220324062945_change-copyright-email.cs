using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class changecopyrightemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE ConfigNoiDungEmails
                SET NoiDungEmail = REPLACE(NoiDungEmail, '2020 PHAN', ' 2021 - 2022 PHAN');
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
