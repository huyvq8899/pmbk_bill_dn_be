using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Functions",
                columns: table => new
                {
                    FunctionId = table.Column<string>(maxLength: 36, nullable: false),
                    FunctionName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    Type = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functions", x => x.FunctionId);
                });

            migrationBuilder.CreateTable(
                name: "KyKeToans",
                columns: table => new
                {
                    KyKeToanId = table.Column<string>(maxLength: 36, nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    ModifyBy = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: true),
                    NgayChungTu = table.Column<DateTime>(nullable: true),
                    DenNgay = table.Column<DateTime>(nullable: true),
                    GhiChu = table.Column<string>(nullable: true),
                    DaKhoaSo = table.Column<bool>(nullable: true),
                    DaKhoaNhapSoDuBanDau = table.Column<bool>(nullable: true),
                    LoaiThongTu = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KyKeToans", x => x.KyKeToanId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<string>(maxLength: 36, nullable: false),
                    PermissionName = table.Column<string>(nullable: true),
                    Status = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<string>(maxLength: 36, nullable: false),
                    RoleName = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "ThaoTacs",
                columns: table => new
                {
                    ThaoTacId = table.Column<string>(nullable: false),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThaoTacs", x => x.ThaoTacId);
                });

            migrationBuilder.CreateTable(
                name: "TuyChons",
                columns: table => new
                {
                    Ma = table.Column<string>(nullable: false),
                    Ten = table.Column<string>(nullable: true),
                    GiaTri = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TuyChons", x => x.Ma);
                });

            migrationBuilder.CreateTable(
                name: "ViewThaoTacs",
                columns: table => new
                {
                    ThaoTacId = table.Column<string>(nullable: false),
                    PemissionId = table.Column<string>(nullable: true),
                    FunctionId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    FTID = table.Column<string>(nullable: true),
                    UTID = table.Column<string>(nullable: true),
                    Ma = table.Column<string>(nullable: true),
                    Ten = table.Column<string>(nullable: true),
                    STT = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewThaoTacs", x => x.ThaoTacId);
                });

            migrationBuilder.CreateTable(
                name: "Function_Roles",
                columns: table => new
                {
                    FRID = table.Column<string>(maxLength: 36, nullable: false),
                    FunctionId = table.Column<string>(nullable: true),
                    RoleId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function_Roles", x => x.FRID);
                    table.ForeignKey(
                        name: "FK_Function_Roles_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Roles_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Roles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<string>(maxLength: 36, nullable: false),
                    Password = table.Column<string>(maxLength: 200, nullable: true),
                    ConfirmPassword = table.Column<string>(maxLength: 200, nullable: true),
                    UserName = table.Column<string>(maxLength: 200, nullable: true),
                    Email = table.Column<string>(maxLength: 200, nullable: true),
                    FullName = table.Column<string>(maxLength: 200, nullable: true),
                    Gender = table.Column<int>(nullable: true),
                    Avatar = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(maxLength: 200, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(maxLength: 100, nullable: true),
                    ModifyDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<bool>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: true),
                    IsNodeAdmin = table.Column<bool>(nullable: true),
                    IsOnline = table.Column<bool>(nullable: true),
                    LoginCount = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Function_ThaoTacs",
                columns: table => new
                {
                    FTID = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    FunctionId = table.Column<string>(nullable: true),
                    ThaoTacId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function_ThaoTacs", x => x.FTID);
                    table.ForeignKey(
                        name: "FK_Function_ThaoTacs_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Function_ThaoTacs_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Function_ThaoTacs_ThaoTacs_ThaoTacId",
                        column: x => x.ThaoTacId,
                        principalTable: "ThaoTacs",
                        principalColumn: "ThaoTacId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Function_Users",
                columns: table => new
                {
                    FUID = table.Column<string>(maxLength: 36, nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    FunctionId = table.Column<string>(nullable: true),
                    PermissionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Function_Users", x => x.FUID);
                    table.ForeignKey(
                        name: "FK_Function_Users_Functions_FunctionId",
                        column: x => x.FunctionId,
                        principalTable: "Functions",
                        principalColumn: "FunctionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Users_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Function_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User_Roles",
                columns: table => new
                {
                    URID = table.Column<string>(maxLength: 36, nullable: false),
                    RoleId = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User_Roles", x => x.URID);
                    table.ForeignKey(
                        name: "FK_User_Roles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_User_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Function_Roles_FunctionId",
                table: "Function_Roles",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Roles_PermissionId",
                table: "Function_Roles",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Roles_RoleId",
                table: "Function_Roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_FunctionId",
                table: "Function_ThaoTacs",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_PermissionId",
                table: "Function_ThaoTacs",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_ThaoTacs_ThaoTacId",
                table: "Function_ThaoTacs",
                column: "ThaoTacId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Users_FunctionId",
                table: "Function_Users",
                column: "FunctionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Users_PermissionId",
                table: "Function_Users",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_Function_Users_UserId",
                table: "Function_Users",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_RoleId",
                table: "User_Roles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Roles_UserId",
                table: "User_Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Function_Roles");

            migrationBuilder.DropTable(
                name: "Function_ThaoTacs");

            migrationBuilder.DropTable(
                name: "Function_Users");

            migrationBuilder.DropTable(
                name: "KyKeToans");

            migrationBuilder.DropTable(
                name: "TuyChons");

            migrationBuilder.DropTable(
                name: "User_Roles");

            migrationBuilder.DropTable(
                name: "ViewThaoTacs");

            migrationBuilder.DropTable(
                name: "ThaoTacs");

            migrationBuilder.DropTable(
                name: "Functions");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
