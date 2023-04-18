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
                    INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'0113D9E1-D77B-4CBF-ABB0-934D02B19935', N'70700', N'38')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'09FAADF7-2B0B-4593-817A-67AC5D18A0C4', N'22300', N'23')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'0E407CBB-4E27-4E46-9DD6-F3966E9991FE', N'11100', N'07')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'119CA432-F1DE-4504-A05C-EA799F114318', N'10100', N'01')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'12A42354-F4AC-4301-BDA5-A80A7456D738', N'40300', N'29')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'14C5FA52-C627-4344-9AA3-D62F0B335372', N'81500', N'18')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'163CE657-7ABF-433C-A442-F28AEAB905E5', N'21100', N'50')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'226319AB-FD80-495D-A05F-9B050093FB43', N'80300', N'14')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'2529027E-06E2-4BFE-A8E8-3EFF7B3296EF', N'82100', N'19')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'26494E23-B6DB-4593-949A-387D932B19CD', N'51100', N'42')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'28DECA28-BCC0-4998-9463-4BF408F38382', N'40900', N'32')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'2E1BAB37-4426-403C-9A99-5C5924794C0B', N'81100', N'13')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'32DC17AE-BCEC-4EBB-8B5D-C5C6F9BB9C12', N'10700', N'08')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'38F6B65B-C769-4BC3-BC80-D365CF3EC28E', N'81300', N'17')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'404B8F46-8289-441E-8DE1-A99D94E984A8', N'80100', N'11')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'40927D3F-4397-4CEF-8C13-63E29F1DF826', N'11500', N'10')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'41A4BD02-5AB0-4F32-8B38-08C550E54655', N'82300', N'20')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'45580F51-3143-4AE4-9608-4FC3823EC7BD', N'20300', N'48')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'5052455B-91BA-424D-84B6-9BA5DF4B271E', N'60100', N'61')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'51864534-EC6E-46B0-B663-CE042C4F696F', N'50100', N'04')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'5BDFBDE3-DBEC-4E5C-8AC2-BCF3EAFB6D22', N'30300', N'55')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'5C8537BB-1248-4EAB-ACDB-3C5FCDC92537', N'20500', N'53')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'5E7BC270-4369-401D-B033-9D1C2240B428', N'70900', N'39')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'61348159-B1EF-4265-9B82-9EB141A9E2BA', N'22100', N'24')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'62F30953-31AC-4332-9045-559B1A84284E', N'60300', N'59')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'6B5A9EC9-4EBC-4A34-94E8-15A1C17BA0F1', N'10300', N'02')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'6FF39229-E68E-4C78-AB07-0B105A214EAD', N'60500', N'60')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'717D2367-2DBD-4060-998D-5A5D4404D334', N'80700', N'12')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'75FEBA22-E529-4717-92A3-72FA38B5FEEF', N'50900', N'44')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'77ED7504-C2E1-415A-B9EF-4B56B4B7E103', N'50700', N'41')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'781154DF-AC54-4D01-91AF-150670D9A354', N'80900', N'15')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'789A9407-7EE4-474B-A78D-BA95CF64DBA6', N'30500', N'54')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'7A13F82F-E9C0-4248-B8D7-A3A254FC35B7', N'20900', N'49')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'82D8F4ED-364D-4814-AEFB-110E5B9182D3', N'11700', N'27')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'8921CD46-796E-4064-83DB-955A33FA6E29', N'50500', N'43')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'931653F9-403E-42EE-B709-F21B97694226', N'81700', N'21')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'93951E95-20F0-46E0-9AD7-EF263C9C1850', N'10900', N'09')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'93AB726F-6B6D-44E9-A9A6-E970904EA744', N'21700', N'26')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'9683CC63-8255-47A7-9049-25B31CCBC067', N'70500', N'45')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'971665DE-1B07-4662-A6D6-ADD7125EBBC1', N'30200', N'62')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'A2019DB7-64E6-45CC-8014-97B7D383E54F', N'71100', N'37')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'AEF911F3-BD02-4CFD-92DC-38398DD4B88D', N'80500', N'16')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'AF1479EC-BF16-4559-A89C-A42B8B251FC1', N'81600', N'63')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'B8AECF73-FEA7-48D6-B4BB-4CCC7C8E9015', N'21500', N'46')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'BB1F859D-3855-478B-AE8A-503B26B6F8C9', N'40700', N'31')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'BBA2AA25-5467-483B-9AE3-FA92FF002B68', N'21900', N'25')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'BDF9961F-0F1E-480A-B253-00273B510E81', N'81900', N'22')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'BFBEE854-174C-4A4C-874E-DB485DF059C3', N'60600', N'64')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'C110A835-8DD4-406A-8A3A-A11F6C6A4DEA', N'40500', N'30')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'C57F119E-15B3-432D-85B4-95821AACB4F5', N'71300', N'36')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'C6E05970-DC16-4C57-959D-047704C7C291', N'20700', N'47')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'C8F1ACC7-8B66-4DD5-A54E-27F4C0FC31CF', N'41100', N'33')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'CA817F2E-8CEE-48D2-B6B6-CE7C026725A1', N'22500', N'57')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'CC708A34-5AAF-4382-8AAE-C37987393136', N'70100', N'03')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'CD02C69D-5314-4ED2-8C1F-1A06ED7A410F', N'71500', N'34')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'D6E1EC39-BC59-4673-98AB-4A651084312F', N'20100', N'51')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'D7595F46-F878-4BFE-881E-206C62A03979', N'30100', N'56')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'E107F111-DCD1-4A53-A8E1-FD8BC953E71B', N'21300', N'52')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'E7CAFAC5-A399-4606-BFC4-70D8A7B37EF3', N'11300', N'06')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'ED0A4E01-7674-4737-BDA7-086A7CFD1A99', N'50300', N'40')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'F0D6986A-27AA-4BEC-A885-88F843E819C6', N'71700', N'35')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'F7060CF0-48D6-4DCF-AA3E-E7A9FEE475EF', N'40100', N'28')
INSERT [dbo].[CoQuanThueCapCuc_DiaDanhs] ([Id], [MaCQT], [MaDiaDanh]) VALUES (N'F8484389-5DE8-43DC-9B28-876F11A49C52', N'70300', N'58')

                ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoQuanThueCapCuc_DiaDanhs");
        }
    }
}
