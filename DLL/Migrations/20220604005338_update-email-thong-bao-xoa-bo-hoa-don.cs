using DLL.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class updateemailthongbaoxoabohoadon : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoXoaBoHoaDon,
                column: "NoiDungEmail",
                value:
                        "<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>" + Environment.NewLine +
                        "<div class='body' style='padding:10px'>" + Environment.NewLine +
                        "<div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi thông báo xóa bỏ hóa đơn cho Quý khách</span></div>" + Environment.NewLine +
                        "<div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;" +
                            "color: red;" +
                            "margin-bottom: 10px;'>" + Environment.NewLine +
                            "<i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>" + Environment.NewLine +
                            "</div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tennguoinhan##</strong><br/>" + Environment.NewLine +
                        "##tendonvi##&nbsp;xin tr&acirc;n trọng th&ocirc;ng b&aacute;o với Qu&yacute; kh&aacute;ch: Ch&uacute;ng t&ocirc;i vừa thực hiện x&oacute;a bỏ ##loaihoadon## (h&igrave;nh thức h&oacute;a đơn điện tử) theo bi&ecirc;n bản thỏa thuận về việc x&oacute;a bỏ h&oacute;a đơn đ&atilde; được sự đồng &yacute; giữa c&aacute;c b&ecirc;n.</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>Th&ocirc;ng tin h&oacute;a đơn x&oacute;a bỏ:</div>" + Environment.NewLine +
                        "<div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>" + Environment.NewLine +
                        "<ul style = 'margin-left:25px' >" + Environment.NewLine +
                            "<li>Số hóa đơn:&nbsp;<strong>##so##</strong></li>" + Environment.NewLine +
                            "<li>Ng&agrave;y h&oacute;a đơn:&nbsp;<strong>##ngayhoadon##</strong></li>" + Environment.NewLine +
                            "<li>Ký hiệu mẫu số hóa đơn:&nbsp;<strong>##mauso##</strong></li>" + Environment.NewLine +
                            "<li>K&yacute; hiệu hóa đơn:&nbsp;<strong>##kyhieu##</strong></li>" + Environment.NewLine +
                        "</ul>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>" + Environment.NewLine +
                        "<div style = 'display: flex' >" + Environment.NewLine +
                        "<div style='width: 100px;'>L&yacute; do x&oacute;a bỏ:&nbsp;</div>" + Environment.NewLine +
                        "<div style='width: calc(100% - 100px)'>##lydoxoahoadon##</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>" + Environment.NewLine +
                        "<div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>" + Environment.NewLine +
                        "<div style = 'clear:both' > &nbsp;</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>" + Environment.NewLine +
                        "<div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>");


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
