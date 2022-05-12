using DLL.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class suanoidungemailsaisot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
                column: "NoiDungEmail",
                value:
                    @"<style type='text/css'>.container { }
                        .container.body {
                                    }
                        .subject {
                                    }
                        .subject.subject - text {
                                    }
                        .content {
                                    }
                        .content.content - text {
                                    }
                        .content.content - text.link {
                                    }
                        .content.content - text.note {
                                    }
                        .content.detail {
                                    }
                        .content.detail ul {
                                    }
                        .content.bt - search a {
                                    }
                        .content.bt - search a: hover {
                                    }
                        .content.signer {
                                    }
                        .footer {
                                    }
                        </style>
                        <div class='container' style='width:100%;margin:0;font-family:Tahoma;font-size: 14px;'>
                        <div class='body' style='padding:10px'>
	                        <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px;'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px; white-space: pre-wrap!important;text-align: justify!important;' >##tendonvi##<br/>Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn cho Quý khách</span></div>
                        <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px; border-radius: 0 0 4px 4px;'>
	                        <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><strong>Kính gửi: Quý khách&nbsp;##tennguoinhan##</strong><br />
                        ##tendonvi##&nbsp;xin trân trọng thông báo với Quý khách: Chúng tôi nhận thấy hóa đơn điện tử đã lập và đã gửi đến <strong>Quý khách</strong> có thông tin sai sót.</div>
	                        <div style = 'padding:10px 20px 10px 20px'>
	                        Thông tin hóa đơn có thông tin sai sót không phải lập lại hóa đơn theo quy định tại điểm a, khoản 2, điều 19 của Nghị định số 123/2020/NĐ-CP:
	                        </div>
                        <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>
	                        <ul style = 'margin-left:25px' >
	                        <li>Số hóa đơn: <strong>##so##</strong></li>
	                        <li>Ký hiệu mẫu số hóa đơn:&nbsp;<strong>##mauso##</strong></li>
	                        <li>Ký hiệu hóa đơn:&nbsp;<strong>##kyhieu##</strong></li>
	                        <li>Ngày hóa đơn:&nbsp;<strong>##ngayhoadon##</strong></li>
	                        <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li>
	                        </ul>
	
	                        <table style = 'border-collapse: collapse; margin: 6px' width = '98.5%'><thead><tr style = 'background-color: #0070C0;color: #fff'><th style = 'border: 1px solid #A5A5A5; width: 22%'>Thông tin</th><th style = 'border: 1px solid #A5A5A5; width: 39%'>Thông tin sai sót</th><th style = 'border: 1px solid #A5A5A5; width: 39%'>Thông tin đúng</th></tr></thead><tbody><tr style = 'display: ##displayhotennguoimuahang##'><td style = 'border: 1px solid #A5A5A5; padding: 0px 2px 0px 2px'>Họ và tên người mua hàng</td><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>##hotennguoimuahang_sai##</td><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>##hotennguoimuahang_dung##</td></tr><tr style = 'display: ##displaytendonvi##'><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>Tên đơn vị</td><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>##tendonvi_sai##</td><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>##tendonvi_dung##</td></tr><tr style = 'display: ##displaydiachi##'><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>Địa chỉ</td><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>##diachi_sai##</td><td style = 'border: 1px solid #A5A5A5;padding: 0px 2px 0px 2px'>##diachi_dung##</td></tr></tbody></table>
                        </div>
                        <br>
                        <div style = 'padding:5px 20px 5px 20px'><strong>&nbsp;Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>
                        <img alt = '' height='0' src='##EmailTrackingHandler##' width='0'/>
                        <div style = 'padding:10px 20px 0px 20px'>
                        <div style = 'text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong></div>
                        </div>
                        <div style = 'color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>
                        </div>
                        </div>
                        </div>"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
