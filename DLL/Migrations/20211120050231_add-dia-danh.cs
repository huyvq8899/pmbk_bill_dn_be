using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddiadanh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiaDanhs",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiaDanhs", x => x.Id);
                });

            migrationBuilder.Sql
                (@"
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'001EBF81-E607-44E8-B924-9E2808D6DFEC', N'29', N'Nghệ An')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'0B1750DB-F16A-4788-AE97-8F9BA0A17152', N'15', N'Vĩnh Long')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'0EC78F60-13AE-4918-A2D7-CE0542B7B5E9', N'26', N'Phú Thọ')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'0F349C81-3423-4AF4-9000-B75DA681FC6F', N'19', N'Bạc Liêu')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'177A0531-6E8D-4527-8920-0C006865C745', N'11', N'Long An')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'17D62172-8599-48A4-948A-269CCE8F08DE', N'60', N'Đắk Lắk')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'1DC8DC6F-8228-4AEA-A562-FF8BDE9A90AF', N'41', N'Bình Định')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'1DD2F26B-6020-4D31-B3E0-C41A344A3DF0', N'17', N'Kiên Giang')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'1E484665-3B0F-4D9B-84B3-A349E8DFD3EC', N'02', N'Hải Phòng')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'21B5D25A-E273-42B1-A16F-9A87A8350CC5', N'24', N'Bắc Giang')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'220E43C7-D278-4D1D-B108-3AF770AD1792', N'03', N'Hồ Chí Minh')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'24BE3340-5C59-4179-80D2-CC8DAF18AD05', N'56', N'Điện Biên')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'29A5D9C5-A545-4FAD-AC81-DFABF3EA2666', N'62', N'Lai Châu')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'2F206423-74E2-4DDD-B2B4-1C4A86DF06C6', N'43', N'Quảng Ngãi')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'2F5996C7-5C5D-4B4E-AE69-98B641E19934', N'25', N'Vĩnh Phúc')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'32C08977-FFC0-4565-A10A-0DCCE8B656F8', N'21', N'Trà Vinh')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'3316D48C-C4B5-417F-9737-8A8872EEB150', N'32', N'Quảng Trị')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'408F7038-23C9-412C-9476-60F1BA879AB8', N'38', N'Bình Phước')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'4FE08E4D-0C5B-45B0-B975-A0E4CD916879', N'08', N'Hải Dương')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'54677EFC-2A64-46CD-9F25-7DEAFEDAA5AF', N'49', N'Lạng Sơn')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'55E00FE1-A171-4207-88BC-3B63ED7344D7', N'59', N'Gia Lai')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'5BCFA80A-5D46-4CD3-B8B4-D3F8754C1E32', N'39', N'Tây Ninh')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'5DDA57F6-6E5F-459B-917D-B2648FB6B7AF', N'46', N'Thái Nguyên')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'61065906-CC2D-42FA-9D4A-F9C0A61F3B3F', N'18', N'Cần Thơ')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'6B8257C6-B0E0-4683-8509-A89654D2782E', N'54', N'Hoà Bình')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'7E4DA50C-BC94-4F21-916E-BEB33A7BB919', N'36', N'Đồng Nai')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'7FFDF6A6-D079-4EF5-A64F-73D51A3F34FC', N'04', N'Đà Nẵng')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'81057ACD-A4C2-4A33-821B-7A2333FE1D72', N'10', N'Thái Bình')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'826C4B35-0E5F-4C21-9D9F-B743E07D0FC0', N'34', N'Bình Thuận')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'82AD12CC-82A8-4B0E-B2E5-3F4E46DE397C', N'64', N'Đắk Nông')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'8847EBE6-2682-4555-BE55-E37A630A4458', N'33', N'Thừa Thiên - Huế')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'8A6051E1-F443-495E-B767-50E03A26453F', N'20', N'Cà Mau')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'8A85D36C-DF0C-46A9-8D71-6909EF5C5AD8', N'30', N'Hà Tĩnh')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'8A8A325E-310A-4E06-ACF3-93FF2DCC5AB7', N'47', N'Bắc Kạn')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'8D6D7722-33B3-4CB5-B7AD-AB9001DA6A0E', N'27', N'Ninh Bình')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'9076F39C-DBAC-4080-8DEF-7C22180C7DFD', N'28', N'Thanh Hóa')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'91D79BF3-834A-4D97-87CB-79E7ADCA2E6B', N'51', N'Hà Giang')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'954772CD-FCCC-4623-B1F7-19E5ABF02DCA', N'37', N'Bình Dương')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'989A6BA7-3EFB-44B2-8AE9-F47B085EF9B1', N'09', N'Hưng Yên')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'9F1F21A9-5BB4-4316-8A2C-9249C8E29119', N'44', N'Phú Yên')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'9F8BBBC2-BE22-46CC-A250-C9CAD5F8AE94', N'23', N'Bắc Ninh')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'A0510D80-5DE9-44DE-8D37-E2232211A991', N'07', N'Hà Nam')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'A122E1B8-3461-4994-AC8D-D87B3FB5AEDC', N'14', N'Đồng Tháp')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'AEEB775D-A321-4208-94D2-6E87843304D5', N'16', N'An Giang')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'AF404836-1AA8-4447-8300-620E8590D3A3', N'61', N'Kon Tum')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'B43FFC6D-FB42-43DB-9CBA-07A0AF07FE86', N'45', N'Ninh Thuận')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'B4A3D720-D7B0-49C9-B7CF-FB43CAE29E13', N'42', N'Khánh Hoà')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'B8D33C2B-6F97-4F8F-A403-B9B2488A7907', N'13', N'Bến Tre')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'B922005A-21E4-4CED-BD88-8D8319F643A6', N'57', N'Quảng Ninh')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'BCBADC93-283A-4F1E-80B3-33BEEDCAC8F5', N'22', N'Sóc Trăng')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'C7357D3F-00AA-4F27-BCA0-D53D9BDE2914', N'48', N'Cao Bằng')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'D1DFC2BB-A3A2-46B1-98E3-69AFE6E4FE90', N'01', N'Hà Nội')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'D48E232F-A140-4E0B-88BD-26BF8D699087', N'40', N'Quảng Nam')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'D8D287A1-BB39-4A40-B299-CE2D80F4FB9A', N'53', N'Lào Cai')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'EB319D16-AD89-4E78-BB1A-3D65F8245810', N'55', N'Sơn La')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'EDE9A6A6-7E56-4E76-AB45-26D125F85016', N'35', N'Vũng Tàu')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'EE855682-32FD-41A6-BB17-64016B0C063C', N'63', N'Hậu Giang')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'F524B0EC-EC6D-4C6E-8D74-CC11F043C754', N'06', N'Nam Định')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'F7AA2563-692E-4BC5-90B3-5BF8ED9152F8', N'52', N'Yên Bái')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'F7B00343-A748-4541-AFCF-1B6B2277CE2B', N'31', N'Quảng Bình')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'F9F5CCBB-94E6-4CE5-9764-696EE24C3003', N'58', N'Lâm Đồng')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'FA5EBC01-39F6-4E5E-BC53-5AD9B9D1280E', N'12', N'Tiền Giang')
                    INSERT [dbo].[DiaDanhs] ([Id], [Ma], [Ten]) VALUES (N'FC89062F-8825-4AF6-9961-E30265844E21', N'50', N'Tuyên Quang')
              
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiaDanhs");
        }
    }
}
