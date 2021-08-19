using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class addtruongdulieubs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung1Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung2Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung3Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung4Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung5Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung6Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung7Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung8Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung9Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet10Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet1Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet2Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet3Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet4Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet5Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet6Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet7Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet8Id");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet9Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet10Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet1Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet2Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet3Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet4Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet5Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet6Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet7Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet8Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets",
                column: "TruongMoRongChiTiet9Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung10Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung1Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung2Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung3Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung4Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung5Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung6Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung7Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung8Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus",
                column: "TruongThongTinBoSung9Id",
                principalTable: "TruongDuLieuMoRongs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTuChiTiets_TruongDuLieuMoRongs_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonDienTus_TruongDuLieuMoRongs_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung10Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung1Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung2Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung3Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung4Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung5Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung6Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung7Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung8Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTus_TruongThongTinBoSung9Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonDienTuChiTiets_TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung10Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung1Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung2Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung3Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung4Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung5Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung6Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung7Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung8Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongThongTinBoSung9Id",
                table: "HoaDonDienTus");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet10Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet1Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet2Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet3Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet4Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet5Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet6Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet7Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet8Id",
                table: "HoaDonDienTuChiTiets");

            migrationBuilder.DropColumn(
                name: "TruongMoRongChiTiet9Id",
                table: "HoaDonDienTuChiTiets");
        }
    }
}
