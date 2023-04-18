﻿using DLL.Data;
using DLL.Entity.Config;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class adddatathietlaptruongdulieu : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM ThietLapTruongDuLieus");

            var query = new ThietLapTruongDuLieuData().QueryInsertData();
            migrationBuilder.Sql(query);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
