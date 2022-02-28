using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class InsertRowDiaDanhs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'FC89062F-8825-4AF6-9961-E30265845E22', N'65', N'Cục Thuế Doanh nghiệp lớn')");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
