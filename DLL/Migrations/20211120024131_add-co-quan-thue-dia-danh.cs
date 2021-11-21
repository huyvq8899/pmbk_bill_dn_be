using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addcoquanthuediadanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoQuanThueCapCuc_DiaDanhs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    MaCQT = table.Column<string>(nullable: true),
                    MaDiaDanh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoQuanThueCapCuc_DiaDanhs", x => x.Id);
                });

            migrationBuilder.Sql
                (@"
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'0018FFCC-8A99-41F4-B9B4-B3DB69DD8B02', N'41100', N'46')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'01591E21-C9F6-4F28-BE52-C1A61BEDBCD0', N'70700', N'70')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'023FD51A-B04C-4653-9F04-6A83F1D55C7A', N'20300', N'04')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'044B3A3D-B503-4221-B609-18B88C3DB0D6', N'60300', N'64')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'085E49D3-20CF-4F3E-8FA4-2D490E490D11', N'22100', N'24')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'0B3E201C-E3A0-40AC-AF96-20F82CA37514', N'40700', N'44')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'0C7C0FD3-50F7-40F5-BDFA-C91A24FACDB5', N'60500', N'66')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'10047AC6-11D6-4432-AB4B-D00EF6DD30CE', N'10100', N'01')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'12CF4128-B783-49B9-8F8F-0A2199AE97F1', N'70300', N'68')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'233B6F17-D893-45EA-829D-F9729D6256E2', N'20100', N'02')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'2A6C3686-B62C-40C1-825C-3123221EB52A', N'30200', N'12')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'2E133FB1-7B4F-4501-BCDC-C2F209628858', N'40500', N'42')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'2F6D61D3-1DD3-47BA-A44A-23CE9A0C4BC4', N'80100', N'80')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'304D9A47-EBB3-44E4-AB18-C3D147324589', N'81300', N'91')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'30DB55EB-3258-4470-994F-6C828D7F36B7', N'11700', N'37')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'31545BED-74B8-4C42-A546-8364255267B3', N'40300', N'40')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'33054110-F570-41BD-AF31-641860D6FEF0', N'20500', N'10')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'36A41F2C-78C1-4D96-83D8-5D8168B8767E', N'81700', N'84')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'377FB838-BDB4-4B3C-9686-EC1D79ACF603', N'80500', N'89')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'387A7FD7-2C29-4655-9266-306A5DE61E57', N'82100', N'95')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'3DACE481-E916-40F5-A453-99F07C18ADE9', N'50500', N'51')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'45E5578A-8C0A-4101-96A7-C5F0DD83508D', N'71500', N'60')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'46F4214F-0FAC-4327-A5A7-2FF387C69C3F', N'22300', N'27')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'4CF1AA2C-1596-4A2C-B726-6EF509F03555', N'71100', N'74')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'4EE2B12E-A555-4744-B3AE-205C2A70449D', N'21500', N'19')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'4F69AC41-3578-4DDC-A748-78A5EA6E940B', N'30500', N'17')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'59026562-039E-4F39-8171-17E15CD45A2B', N'71700', N'77')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'5BA13246-96F2-48A5-B032-27127F21D741', N'80700', N'82')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'5BE8A355-A69D-40F7-A0F8-AE95C6647137', N'21300', N'15')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'626D1D6B-9E4F-446E-80B5-CD410EF593F8', N'21900', N'26')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'634C3A3D-69A6-422F-9A3C-E1A7000E84DB', N'22500', N'22')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'63F8ADDC-2375-442A-8F1B-C957D412A070', N'30300', N'14')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'65513A2F-1E26-4067-B414-2C00FF716CFB', N'81100', N'83')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'6B378F24-0152-4041-985F-5409A2568C2C', N'70500', N'58')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'6DBC0498-81DD-4734-874C-48EF55BF95F2', N'50900', N'54')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'7132CC19-4BC9-4F01-BB54-53074BB19577', N'80900', N'86')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'79106E2C-D938-4206-996F-12DF36E88706', N'30100', N'11')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'7C9BECDD-9E52-4838-8FBF-C8EBC4DD3E42', N'11300', N'36')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'85341360-7C36-4EAA-90A4-955A37A5E50D', N'50700', N'52')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'90F98CAE-54ED-4A34-B0B9-7B8D2A37BF3D', N'21100', N'08')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'9712B433-6240-46BA-ACF4-AF76F7FD67D8', N'21700', N'25')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'98FC03FD-A7AE-4C83-8897-56AA72A916DC', N'70900', N'72')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'A5485A5D-109E-4779-A390-E5A966D3A578', N'70100', N'79')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'A6092D84-D376-41A9-9FB9-72F25A5002BF', N'50100', N'48')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'A60A840E-C0A0-4E49-BFAB-9076BBADE72F', N'81500', N'92')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'AC250E62-1FA1-4D68-B6B0-7A7D6A3624B9', N'10700', N'30')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'BABDDE0B-5C94-49B0-9EA8-A7038AF4F054', N'50300', N'49')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'BE39CDED-6F82-41CB-8C4D-111D72984A4E', N'80300', N'87')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'CB9336D9-F944-451B-9E7F-05DEF2106A40', N'11500', N'34')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'D5151E6B-94EE-4A75-8E4F-17F66C3598AD', N'60100', N'62')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'D65371BB-7622-4FC5-8C8A-83B2CCCD32C8', N'40900', N'45')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'D68F5AD0-89C9-44F6-B79C-AB35D37C39D0', N'20700', N'06')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'DD39DCDC-AC75-491E-A7B5-2953D8C75F9D', N'10300', N'31')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'DF5FC1BB-1A12-4474-938A-4EA09F92220A', N'51100', N'56')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'E3104891-E6B2-4DE9-94D2-51D28F34C420', N'20900', N'20')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'E55D8D27-350A-48A6-8B39-7FFC26DC19B5', N'11100', N'35')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'EA406BC4-EDE8-4FBD-91BE-4082C4B8D797', N'82300', N'96')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'EC803CF3-D7C0-49DA-9834-9A9BFCAC1EC7', N'40100', N'38')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'EE96A15F-9E59-428F-B379-16D5F14D80C1', N'71300', N'75')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'EF89AA63-48B4-4842-91CC-68AE0755F0ED', N'81600', N'93')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'F412B78E-6B65-4B5F-ABDE-1F2F8D1BB631', N'60600', N'67')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'F82FDD23-8432-481A-98E9-A711EE49F481', N'10900', N'33')
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'FB411452-3723-420B-9B16-16A14D2F7447', N'81900', N'94')
                ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoQuanThueCapCuc_DiaDanhs");
        }
    }
}
