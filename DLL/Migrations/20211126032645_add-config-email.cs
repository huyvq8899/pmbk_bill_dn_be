using DLL.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace DLL.Migrations
{
    public partial class addconfigemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("Delete ConfigNoiDungEmails");
            migrationBuilder.InsertData(
    table: "ConfigNoiDungEmails",
    columns: new string[] { "Id", "LoaiEmail", "TieuDeEmail", "NoiDungEmail" },
    values: new object[,]
    {
                    {
                        Guid.NewGuid().ToString(),
                        (int)LoaiEmail.ThongBaoPhatHanhHoaDon,
                        "##tendonvi## gửi hóa đơn điện tử số ##so## cho ##tenkhachhang##",
                        "<style type='text/css'>.container { " +
                        "}" + Environment.NewLine +
                        ".container .body {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".subject {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".subject .subject-text {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .content-text {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .content-text .link {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .content-text .note {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .detail {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .detail ul {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .bt-search a {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .bt-search a:hover {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".content .signer {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        ".footer {" + Environment.NewLine +
                        "}" + Environment.NewLine +
                        "</style>" + Environment.NewLine +
                        "<div class='container' style='width:100%;margin:0;font-family:Tahoma;font-size: 14px;'>" + Environment.NewLine +
                        "<div class='body' style='padding:40px'>" + Environment.NewLine +
                        "<div style='padding:6px 0'><img src='../../assets/logo.png'/></div>" + Environment.NewLine +
                        "<div class='subject' style='background-color:#0f97f1; border-top-left-radius:4px; border-top-right-radius:4px; height:70px;line-height:70px;padding-left: 20px;'><span style = 'color:#ffffff; font-family:Tahoma; font-size:22px' >##tendonvi##&nbsp;th&ocirc;ng b&aacute;o gửi ##loaihoadon## cho Qu&yacute; kh&aacute;ch</span></div>"
                        + Environment.NewLine +
                        "<div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px; border-radius: 0 0 4px 4px;'>"
                        + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;" +
                        "color: red;" +
                        "margin-bottom: 10px;'>" + Environment.NewLine +
                        "<i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>" + Environment.NewLine +
                        "</div>--><strong>K&iacute; nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tennguoinhan##</strong><br />" + Environment.NewLine +
                        "##tendonvi##&nbsp;xin gửi cho Qu&yacute; kh&aacute;ch ##loaihoadon## (theo h&igrave;nh thức h&oacute;a đơn điện tử) với c&aacute;c th&ocirc;ng tin như sau (Chi tiết xem trong file đ&iacute;nh k&egrave;m):</div>" + Environment.NewLine +
                        "<div class='detail' style='background-color:#e1eefb; line-height:30px; padding:1px'>" + Environment.NewLine +
                        "<ul style = 'margin-left:25px' >" + Environment.NewLine +
                            "<li> Số: <strong>##so##</strong></li>" + Environment.NewLine +
                            "<li>Mẫu số:&nbsp;<strong>##mauso##</strong></li>" + Environment.NewLine +
                            "<li>K&yacute; hiệu:&nbsp;<strong>##kyhieu##</strong></li>" + Environment.NewLine +
                        "</ul>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>Để tra cứu v&agrave; k&yacute; điện tử tr&ecirc;n h&oacute;a đơn, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:<br />" + Environment.NewLine +
                        "&nbsp;" + Environment.NewLine +
                         "<div class='bt-search'><a href='##link#/tra-cuu-hoa-don/?sc=##matracuu##&amp;r=1' style='font-family: Tahoma, serif;" +
                            "background-color: #ff7500;" +
                            "color: #ebebeb;" +
                            "font-weight: 500;" +
                            "padding: 10px 50px 10px 50px;" +
                            "border-radius: 4px;" +
                            "box-shadow: 1px 1px 1px #ddd;" +
                            "border-style: none;" +
                            "cursor: pointer;" +
                            "text-decoration: none;'>TRA CỨU</a></div>" + Environment.NewLine +
                        "&nbsp;" + Environment.NewLine +

                        "<div>Hoặc truy cập v&agrave;o đường dẫn<span style='font-size:11.0pt'><span style = 'font-family:&quot;Calibri&quot;,sans-serif' ><a href='##link##/tra-cuu' style='color:blue; text-decoration:underline'><span style = 'font-size:13.0pt' > https://meinvoice.vn/tra-cuu</span></a></span></span> v&agrave; nhập m&atilde; số: <strong>##matracuu##</strong><br />" + Environment.NewLine +
                        "Qu&yacute; kh&aacute;ch vui l&ograve;ng kiểm tra, đối chiếu nội dung ghi tr&ecirc;n h&oacute;a đơn.</div>" + Environment.NewLine +
                        "<div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>" + Environment.NewLine +
                        "<div>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</div>" + Environment.NewLine +
                        "<strong>##tendonvi##</strong></div>" + Environment.NewLine +
                        "<div style = 'clear:both' > &nbsp;</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>" + Environment.NewLine +
                        "<div style='text-align:center; color:white; background-color: #0f97f1; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt='' height='0' src='##EmailTrackingHandler##' width='0' /></div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div style='color: #9b9b9b; padding: 8px 0'>Copyright ⓒ 2017- <!--##thisYear##-->2021<!--##thisYear##--> PHAN MEM BACH KHOA</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        (int)LoaiEmail.ThongBaoXoaBoHoaDon,
                        "##tendonvi## gửi thông báo xóa bỏ hóa đơn điện tử số ##so## cho ##tenkhachhang##",
                        "<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>"  + Environment.NewLine +
                        "<div class='body' style='padding:40px'>" + Environment.NewLine +
                        "<div class='subject' style='background-color:#0f97f1; border-top-left-radius:4px; border-top-right-radius:4px; padding:24px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:22px' >##tendonvi##&nbsp;th&ocirc;ng b&aacute;o x&oacute;a bỏ h&oacute;a đơn điện tử&nbsp;</span></div>" + Environment.NewLine +
                        "<div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;" +
                            "color: red;" +
                            "margin-bottom: 10px;'>" + Environment.NewLine +
                            "<i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>" + Environment.NewLine +
                            "</div>--><strong>K&iacute; nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tennguoinhan##</strong><br/>" + Environment.NewLine +
                        "##tendonvi##&nbsp;xin tr&acirc;n trọng th&ocirc;ng b&aacute;o với Qu&yacute; kh&aacute;ch: Ch&uacute;ng t&ocirc;i vừa thực hiện x&oacute;a bỏ ##loaihoadon## (h&igrave;nh thức h&oacute;a đơn điện tử) theo bi&ecirc;n bản thỏa thuận về việc x&oacute;a bỏ h&oacute;a đơn đ&atilde; được sự đồng &yacute; giữa c&aacute;c b&ecirc;n.</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>Th&ocirc;ng tin h&oacute;a đơn x&oacute;a bỏ:</div>" + Environment.NewLine +
                        "<div class='detail' style='background-color:#e1eefb; line-height:30px; padding:1px'>" + Environment.NewLine +
                        "<ul style = 'margin-left:25px' >" + Environment.NewLine +
                            "<li> Số:&nbsp;<strong>##so##</strong></li>" + Environment.NewLine +
                            "<li>Mẫu số:&nbsp;<strong>##mauso##</strong></li>" + Environment.NewLine +
                            "<li>K&yacute; hiệu:&nbsp;<strong>##kyhieu##</strong></li>" + Environment.NewLine +
                        "</ul>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>" + Environment.NewLine +
                        "<div style = 'display: flex' >" + Environment.NewLine +
                        "<div style='width: 100px;'>L&yacute; do x&oacute;a bỏ:&nbsp;</div>" + Environment.NewLine +
                        "<div style='width: calc(100% - 100px)'>##lydoxoahoadon##</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<p> Ch & uacute; ng t&ocirc;i sẽ gửi lại h&oacute;a đơn thay thế h&oacute;a đơn đ&atilde; bị x&oacute;a bỏ cho qu&yacute; kh&aacute;ch sau.</p>" + Environment.NewLine +
                        "<div class='signer' style='margin-bottom:40px; margin-top:60px'>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!<br/>" + Environment.NewLine +
                        "<strong>##tendonvi##</strong></div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon,
                        "##tendonvi## gửi biên bản hủy hóa đơn điện tử số ##so## cho ##tenkhachhang##",
                        "<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>" + Environment.NewLine +
                        "<div class='body' style='padding:40px'>" + Environment.NewLine +
                        "<div class='subject' style='background-color:#0f97f1; border-top-left-radius:4px; border-top-right-radius:4px; height:70px; line-height:70px; padding-left:20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:22px' >##tendonvi##&nbsp;th&ocirc;ng b&aacute;o gửi bi&ecirc;n bản hủy h&oacute;a đơn điện tử cho Qu&yacute; kh&aacute;ch</span></div>" + Environment.NewLine +
                        "<div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;" +
                         "color: red;" +
                         "margin-bottom: 10px;'>" +
                         "<i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>" + Environment.NewLine +
                         "</div>--><strong>K&iacute; nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tenkhachhang##</strong><br/>" + Environment.NewLine +
                        "##tendonvi##&nbsp;xin gửi cho Qu&yacute; kh&aacute;ch Bi&ecirc;n bản hủy h&oacute;a đơn điện tử của h&oacute;a đơn sau:&nbsp;</div>" + Environment.NewLine +
                        "<div class='detail' style='background-color:#e1eefb; line-height:30px; padding:1px'>" + Environment.NewLine +
                        "<ul style = 'margin-left:25px' >" + Environment.NewLine +
                            "<li> Số h&oacute;a đơn:&nbsp;<strong>##so##</strong></li>" + Environment.NewLine +
                            "<li>Mẫu số h&oacute;a đơn:&nbsp;<strong>##mauso##</strong></li>" + Environment.NewLine +
                            "<li>K&yacute; hiệu:&nbsp;<strong>##kyhieu##</strong></li>" + Environment.NewLine +
                            "<li>Ng&agrave;y h&oacute;a đơn:&nbsp;<strong>##ngayhoadon##</strong></li>" + Environment.NewLine +
                            "<li>Gi&aacute; trị h&oacute;a đơn:&nbsp;<strong>##tongtien##</strong></li>" + Environment.NewLine +
                        "</ul>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>" + Environment.NewLine +
                        "<div style='display: flex'>" + Environment.NewLine +
                        "<div style='width: 72px;'>L&yacute; do hủy:&nbsp;</div>" + Environment.NewLine +
                        "<div style = 'width: calc(100% - 72px)' >##lydohuy##</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<p> Để xem chi tiết v&agrave; thực hiện k&yacute; điện tử tr&ecirc;n bi&ecirc;n bản, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:</p>" + Environment.NewLine +
                        "<div class='bt-search'><a href = '##duongdanbienban##' style='font-family: Tahoma, serif;" +
                         "background-color: #ff7500;" +
                         "color: #ebebeb;" +
                         "font-weight: 500;" +
                         "padding: 10px 50px 10px 50px;" +
                         "border-radius: 4px;" +
                         "box-shadow: 1px 1px 1px #ddd;" +
                         "border-style: none;" +
                         "cursor: pointer;" +
                         "text-decoration: none;'>XEM CHI TIẾT BI&Ecirc;N BẢN</a></div>" + Environment.NewLine +
                        "<div class='signer' style='margin-bottom:40px; margin-top:60px'>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!<br/>" + Environment.NewLine +
                        "<strong>##tendonvi##</strong></div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>"
                    },
                    {
                        Guid.NewGuid().ToString(),
                        (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon,
                        "##tendonvi## gửi biên bản điều chỉnh hóa đơn điện tử số ##so## cho ##tenkhachhang##",
                        "<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>" + Environment.NewLine +
                        "<div class='body' style='padding:40px'>" + Environment.NewLine +
                        "<div class='subject' style='background-color:#0f97f1; border-top-left-radius:4px; border-top-right-radius:4px; height:70px; line-height:70px; padding-left:20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:22px' >##tendonvi##&nbsp;th&ocirc;ng b&aacute;o gửi bi&ecirc;n bản điều chỉnh h&oacute;a đơn điện tử cho Qu&yacute; kh&aacute;ch</span></div>" + Environment.NewLine +
                        "<div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;" +
                         "color: red;" +
                         "margin-bottom: 10px;'>" +
                         "<i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>" + Environment.NewLine +
                         "</div>--><strong>K&iacute; nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tenkhachhang##</strong><br />" + Environment.NewLine +
                        "##tendonvi##&nbsp;xin gửi cho Qu&yacute; kh&aacute;ch Bi&ecirc;n bản điều chỉnh h&oacute;a đơn điện tử của h&oacute;a đơn sau:</div>" + Environment.NewLine +
                        "<div class='detail' style='background-color:#e1eefb; line-height:30px; padding:1px'>" + Environment.NewLine +
                        "<ul style='margin-left:25px'>" + Environment.NewLine +
                            "<li> Số h&oacute;a đơn:&nbsp;<strong>##so##</strong></li>" + Environment.NewLine +
                            "<li>Mẫu số h&oacute;a đơn:&nbsp;<strong>##mauso##</strong></li>" + Environment.NewLine +
                            "<li>K&yacute; hiệu:&nbsp;<strong>##kyhieu##</strong></li>" + Environment.NewLine +
                            "<li>Ng&agrave;y h&oacute;a đơn:&nbsp;<strong>##ngayhoadon##</strong></li>" + Environment.NewLine +
                            "<li>Gi&aacute; trị h&oacute;a đơn:&nbsp;<strong>##tongtien##</strong></li>" + Environment.NewLine +
                        "</ul>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>" + Environment.NewLine +
                        "<div style='display: flex' >" + Environment.NewLine +
                        "<div style='width: 115px;'>L&yacute; do điều chỉnh:&nbsp;</div>" + Environment.NewLine +
                        "<div style='width: calc(100% - 115px)' >##lydodieuchinh##</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "<p>Để xem chi tiết v&agrave; thực hiện k&yacute; điện tử tr&ecirc;n bi&ecirc;n bản, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:</p>" + Environment.NewLine +
                        "<div class='bt-search'><a href = '##duongdanbienban##' style='font-family: Tahoma, serif;" +
                         "background-color: #ff7500;" +
                         "color: #ebebeb;" +
                         "font-weight: 500;" +
                         "padding: 10px 50px 10px 50px;" +
                         "border-radius: 4px;" +
                         "box-shadow: 1px 1px 1px #ddd;" +
                         "border-style: none;" +
                         "cursor: pointer;" +
                         "text-decoration: none;'>XEM CHI TIẾT BI&Ecirc;N BẢN</a></div>" + Environment.NewLine +
                        "<div class='signer' style='margin-bottom:40px; margin-top:60px'>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!<br/>" + Environment.NewLine +
                        "<strong>##tendonvi##</strong></div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>" + Environment.NewLine +
                        "</div>"
                    },
    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
