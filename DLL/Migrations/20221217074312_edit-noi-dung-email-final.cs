﻿using DLL.Enums;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DLL.Migrations
{
    public partial class editnoidungemailfinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoBienBanDieuChinhHoaDon,
                column: "NoiDungEmail",
                value: @"<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>  <div class='body' style='padding:10px'>  <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi biên bản điều chỉnh hóa đơn cho Quý khách</span></div>  <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;color: red;margin-bottom: 10px;'><i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>  </div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tenkhachhang##</strong><br />  ##tendonvi## xin trân trọng gửi cho Quý khách Biên bản điều chỉnh hóa đơn điện tử với các thông tin hóa đơn sau:</div>  <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>  
                                    <ul style='margin-left:25px'>  
                                        <li>Ký hiệu mẫu số h&oacute;a đơn:&nbsp;<strong>##mauso##</strong></li>  
                                        <li>K&yacute; hiệu h&oacute;a đơn:&nbsp;<strong>##kyhieu##</strong></li>
                                        <li>Số h&oacute;a đơn:&nbsp;<strong>##so##</strong></li>  
                                        <li>Ng&agrave;y h&oacute;a đơn:&nbsp;<strong>##ngayhoadon##</strong></li>
                                        <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li>  </ul>  </div>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>  <div style='display: flex' >  <div style='width: 115px;'>L&yacute; do điều chỉnh:&nbsp;</div>  <div style='width: calc(100% - 115px)' >##lydodieuchinh##</div>  </div>  <div>Để xem chi tiết v&agrave; thực hiện k&yacute; điện tử tr&ecirc;n bi&ecirc;n bản, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:</div><br />  <div class='bt-search'><a href = '##duongdanbienban##' style='font-family: Tahoma, serif;background-color: #ed7d31;color: #ebebeb;font-weight: 500;padding: 10px 50px 10px 50px;border-radius: 4px;box-shadow: 1px 1px 1px #ddd;border-style: none;cursor: pointer;text-decoration: none;'>XEM CHI TIẾT BI&Ecirc;N BẢN</a></div><br>  <div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>  <div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>  <div style = 'clear:both' > &nbsp;</div>  </div>  <img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>  <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>  </div>  <div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>  </div>  </div>  </div>  </div>  </div>  </div>  </div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoBienBanDieuChinhPXK,
                column: "NoiDungEmail",
                value: @"<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>  <div class='body' style='padding:10px'>  <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi biên bản điều chỉnh phiếu xuất kho cho Quý khách</span></div>  <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;color: red;margin-bottom: 10px;'><i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>  </div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tenkhachhang##</strong><br />  ##tendonvi## xin trân trọng gửi cho Quý khách Biên bản điều chỉnh phiếu xuất kho điện tử với các thông tin phiếu xuất kho sau:</div>  <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>  
                        <ul style='margin-left:25px'>   
                            <li>Ký hiệu mẫu số phiếu xuất kho:&nbsp;<strong>##mauso##</strong></li>  
                            <li>K&yacute; hiệu phiếu xuất kho:&nbsp;<strong>##kyhieu##</strong></li>
                            <li>Số phiếu xuất kho:&nbsp;<strong>##so##</strong></li>  
                            <li>Ng&agrave;y phiếu xuất kho:&nbsp;<strong>##ngayhoadon##</strong></li> 
                            <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li>  </ul>  </div>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>  <div style='display: flex' >  <div style='width: 115px;'>L&yacute; do điều chỉnh:&nbsp;</div>  <div style='width: calc(100% - 115px)' >##lydodieuchinh##</div>  </div>  <div>Để xem chi tiết v&agrave; thực hiện k&yacute; điện tử tr&ecirc;n bi&ecirc;n bản, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:</div><br />  <div class='bt-search'><a href = '##duongdanbienban##' style='font-family: Tahoma, serif;background-color: #ed7d31;color: #ebebeb;font-weight: 500;padding: 10px 50px 10px 50px;border-radius: 4px;box-shadow: 1px 1px 1px #ddd;border-style: none;cursor: pointer;text-decoration: none;'>XEM CHI TIẾT BI&Ecirc;N BẢN</a></div><br>  <div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>  <div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>  <div style = 'clear:both' > &nbsp;</div>  </div>  <img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>  <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>  </div>  <div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>  </div>  </div>  </div>  </div>  </div>  </div>  </div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoXoaBoHoaDon,
                column: "NoiDungEmail",
                value: @"<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>  <div class='body' style='padding:10px'>  <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi thông báo xóa bỏ hóa đơn cho Quý khách</span></div>  <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 0px 20px'><!--<div class='note' style='font-size: 14px;color: red;margin-bottom: 10px;'>  <i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>  </div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tennguoinhan##</strong><br/> ##tendonvi## xin trân trọng thông báo với Quý khách: Chúng tôi đã thực hiện xóa bỏ hóa đơn điện tử với các thông tin như sau:</div> 
                        <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>  
                            <ul style = 'margin-left:25px' >    
                                <li>Ký hiệu mẫu số hóa đơn:&nbsp;<strong>##mauso##</strong></li>  
                                <li>K&yacute; hiệu hóa đơn:&nbsp;<strong>##kyhieu##</strong></li>  
                                <li>Số hóa đơn:&nbsp;<strong>##so##</strong></li>  
                                <li>Ng&agrave;y h&oacute;a đơn:&nbsp;<strong>##ngayhoadon##</strong></li>
                                <li>Tổng tiền thanh toán: <strong>##tongtien##</strong></li>  
                            </ul>  </div>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>  <div style = 'display: flex' >  <div style='width: 100px;'>L&yacute; do x&oacute;a bỏ:&nbsp;</div>  <div style='width: calc(100% - 100px)'>##lydoxoahoadon##</div>  </div>  <div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>  <div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>  <div style = 'clear:both' > &nbsp;</div>  </div>  <img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>  <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>  </div>  <div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>  </div>  </div>  </div>  </div>  </div>  </div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoXoaBoPXK,
                column: "NoiDungEmail",
                value: @"<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>  <div class='body' style='padding:10px'>  <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi thông báo xóa bỏ phiếu xuất kho cho Quý khách</span></div>  <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 0px 20px'><!--<div class='note' style='font-size: 14px;color: red;margin-bottom: 10px;'>  <i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>  </div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tennguoinhan##</strong><br/> ##tendonvi## xin trân trọng thông báo với Quý khách: Chúng tôi đã thực hiện xóa bỏ phiếu xuất kho điện tử với các thông tin như sau:</div>  
                        <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>  
                                <ul style='margin-left:25px' >  
                                    <li>Ký hiệu mẫu số phiếu xuất kho:&nbsp;<strong>##mauso##</strong></li>  
                                    <li>K&yacute; hiệu phiếu xuất kho:&nbsp;<strong>##kyhieu##</strong></li>  
                                    <li>Số phiếu xuất kho:&nbsp;<strong>##so##</strong></li>  
                                    <li>Ng&agrave;y phiếu xuất kho:&nbsp;<strong>##ngayhoadon##</strong></li>
                                    <li>Tổng tiền thanh toán: <strong>##tongtien##</strong></li>  
                                </ul>  </div>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>  <div style = 'display: flex' >  <div style='width: 100px;'>L&yacute; do x&oacute;a bỏ:&nbsp;</div>  <div style='width: calc(100% - 100px)'>##lydoxoahoadon##</div>  </div>  <div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>  <div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>  <div style = 'clear:both' > &nbsp;</div>  </div>  <img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>  <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>  </div>  <div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>  </div>  </div>  </div>  </div>  </div>  </div>");


            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoBienBanHuyBoHoaDon,
                column: "NoiDungEmail",
                value: @"<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>  <div class='body' style='padding:10px'>  <div class='subject' style='background-color:#0070C0;border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi biên bản hủy hóa đơn cho Quý khách</span></div>  <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;color: red;margin-bottom: 10px;'><i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>  </div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tenkhachhang##</strong><br/>##tendonvi## xin trân trọng gửi cho Quý khách Biên bản hủy hóa đơn điện tử với các thông tin như sau:</div>  <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>  
                                    <ul style = 'margin-left:25px' >  
                                        <li>Ký hiệu mẫu số h&oacute;a đơn:&nbsp;<strong>##mauso##</strong></li>  
                                        <li>K&yacute; hiệu hóa đơn:&nbsp;<strong>##kyhieu##</strong></li>  
                                        <li> Số h&oacute;a đơn:&nbsp;<strong>##so##</strong></li>  
                                        <li>Ng&agrave;y h&oacute;a đơn:&nbsp;<strong>##ngayhoadon##</strong></li>
                                        <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li>  
                                    </ul>  </div>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>  <div style='display: flex'>  <div style='width: 72px;'>L&yacute; do hủy:&nbsp;</div>  <div style = 'width: calc(100% - 72px)' >##lydohuy##</div>  </div>  <div> Để xem chi tiết v&agrave; thực hiện k&yacute; điện tử tr&ecirc;n bi&ecirc;n bản, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:</div><br />  <div class='bt-search'><a href = '##duongdanbienban##' style='font-family: Tahoma, serif;background-color: #ed7d31;color: #ebebeb;font-weight: 500;padding: 10px 50px 10px 50px;border-radius: 4px;box-shadow: 1px 1px 1px #ddd;border-style: none;cursor: pointer;text-decoration: none;'>XEM CHI TIẾT BI&Ecirc;N BẢN</a></div><br>  <div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>  <div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>  <div style = 'clear:both' > &nbsp;</div>  </div>  <img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>  <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>  </div>  <div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>  </div>  </div>  </div>  </div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoBienBanHuyBoPXK,
                column: "NoiDungEmail",
                value: @"<div class='container' style='font-family:Tahoma; font-size:14px; margin-bottom:0px; margin-left:0px; margin-right:0px; margin-top:0px; width:100%'>  <div class='body' style='padding:10px'>  <div class='subject' style='background-color:#0070C0;border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px'><span style = 'color:#ffffff; font-family:Tahoma; font-size:20px' >##tendonvi##<br>Gửi biên bản hủy phiếu xuất kho cho Quý khách</span></div>  <div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px'>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'><!--<div class='note' style='font-size: 14px;color: red;margin-bottom: 10px;'><i><b>Chú ý</b>: Đây là email tự động từ hệ thống, vui lòng không phản hồi (reply) lại email này</i>  </div>--><strong>K&iacute;nh gửi: Qu&yacute; kh&aacute;ch&nbsp;##tenkhachhang##</strong><br/>##tendonvi## xin trân trọng gửi cho Quý khách Biên bản hủy phiếu xuất kho điện tử với các thông tin như sau:</div>  <div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>  
                        <ul style = 'margin-left:25px' >  
                            <li>Ký hiệu mẫu số phiếu xuất kho:&nbsp;<strong>##mauso##</strong></li>  
                            <li>K&yacute; hiệu phiếu xuất kho:&nbsp;<strong>##kyhieu##</strong></li> 
                            <li>Số phiếu xuất kho:&nbsp;<strong>##so##</strong></li>  
                            <li>Ng&agrave;y phiếu xuất kho:&nbsp;<strong>##ngayhoadon##</strong></li> 
                            <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li>  
                        </ul>  </div>  <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 10px 20px'>  <div style='display: flex'>  <div style='width: 72px;'>L&yacute; do hủy:&nbsp;</div>  <div style = 'width: calc(100% - 72px)' >##lydohuy##</div>  </div>  <div> Để xem chi tiết v&agrave; thực hiện k&yacute; điện tử tr&ecirc;n bi&ecirc;n bản, Qu&yacute; kh&aacute;ch vui l&ograve;ng nhấn n&uacute;t:</div><br />  <div class='bt-search'><a href = '##duongdanbienban##' style='font-family: Tahoma, serif;background-color: #ed7d31;color: #ebebeb;font-weight: 500;padding: 10px 50px 10px 50px;border-radius: 4px;box-shadow: 1px 1px 1px #ddd;border-style: none;cursor: pointer;text-decoration: none;'>XEM CHI TIẾT BI&Ecirc;N BẢN</a></div><br>  <div><strong>Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong></div>  <div class='signer' style='margin-bottom:40px; margin-top:60px; text-align:center; float:left;'>  <div style = 'clear:both' > &nbsp;</div>  </div>  <img alt ='' height='0' src='##EmailTrackingHandler##' width='0'/>  <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'><strong>##tendonvi##</strong> <img alt ='' height='0' src='##EmailTrackingHandler##' width='0' /></div>  </div>  <div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ2021 - 2022 PHAN MEM BACH KHOA</div>  </div>  </div>  </div>  </div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiPXK,
                column: "NoiDungEmail",
                value: @"<div class='container' style='width:100%;margin:0;font-family:Tahoma;font-size: 14px;'> <div class='body' style='padding:10px'> <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px;'> <span style='color:#ffffff; font-family:Tahoma; font-size:20px; white-space: pre-wrap!important;text-align: justify!important;'>##tendonvi##<br/>Gửi thông báo phiếu xuất kho có thông tin sai sót không phải lập lại phiếu xuất kho cho Quý khách</span> </div><div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px; border-radius: 0 0 4px 4px;'> <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 0px 20px'> <strong>Kính gửi: Quý khách&nbsp;##tennguoinhan##</strong><br/> ##tendonvi##&nbsp;xin trân trọng thông báo với Quý khách: Chúng tôi nhận thấy phiếu xuất kho điện tử đã lập và đã gửi đến <strong>Quý khách</strong> có thông tin sai sót. </div><div style='padding:5px 20px 10px 20px'> Thông tin phiếu xuất kho có thông tin sai sót không phải lập lại phiếu xuất kho theo quy định tại điểm a, khoản 2, điều 19 của Nghị định số 123/2020/NĐ-CP: </div><div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>
                            <ul style='margin-left:25px'>  
                                <li>Ký hiệu mẫu số:&nbsp;<strong>##mauso##</strong></li>
                                <li>Ký hiệu:&nbsp;<strong>##kyhieu##</strong></li>
                                <li>Số: <strong>##so##</strong></li>
                                <li>Ngày:&nbsp;<strong>##ngayhoadon##</strong></li>
                                <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li>
                            </ul> <div style='margin: 6px 20px 6px 20px;'> 
                            <table style='border-collapse: collapse; width: 100%;'>
                                <thead> 
                                    <tr style='background-color: #0070C0;color: #fff'> 
                                        <th style='border: 1px solid #0070C0; width: 100%'>Thông tin giải trình sai sót</th> 
                                    </tr>
                                </thead> 
                                <tbody> 
                                    <tr style='display: table-row'> 
                                        <td style='border: 1px solid #0070C0; padding: 0px 2px 0px 2px'>##thongtingiaitrinhsaisot##</td>                                   
                                    </tr>
                                </tbody> </table> </div></div><br><div style='padding:0px 20px 0px 20px;color:black'><strong>&nbsp;Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong> </div><img alt='' height='0' src='##EmailTrackingHandler##' width='0'/> <div style='padding:5px 20px 0px 20px'> <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'> <strong>##tendonvi##</strong> </div></div><div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ 2021 - 2022 PHAN MEM BACH KHOA </div></div></div></div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiPXK,
                column: "TieuDeEmail",
                value: "##tendonvi##<br>Gửi thông báo phiếu xuất kho có thông tin sai sót không phải lập lại phiếu xuất kho cho Quý khách");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
                column: "NoiDungEmail",
                value: @"<div class='container' style='width:100%;margin:0;font-family:Tahoma;font-size: 14px;'> <div class='body' style='padding:10px'> <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px;'> <span style='color:#ffffff; font-family:Tahoma; font-size:20px; white-space: pre-wrap!important;text-align: justify!important;'>##tendonvi##<br/>Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn cho Quý khách</span> </div><div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px; border-radius: 0 0 4px 4px;'> <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 0px 20px'> <strong>Kính gửi: Quý khách&nbsp;##tennguoinhan##</strong><br/> ##tendonvi##&nbsp;xin trân trọng thông báo với Quý khách: Chúng tôi nhận thấy hóa đơn điện tử đã lập và đã gửi đến <strong>Quý khách</strong> có thông tin sai sót. </div><div style='padding:5px 20px 10px 20px'> Thông tin hóa đơn có thông tin sai sót không phải lập lại hóa đơn theo quy định tại điểm a, khoản 2, điều 19 của Nghị định số 123/2020/NĐ-CP: </div><div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>
                                        <ul style='margin-left:25px'>
                                            <li>Ký hiệu mẫu số:&nbsp;<strong>##mauso##</strong></li>
                                            <li>Ký hiệu:&nbsp;<strong>##kyhieu##</strong></li>
                                            <li>Số: <strong>##so##</strong></li>
                                            <li>Ngày:&nbsp;<strong>##ngayhoadon##</strong></li>
                                            <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li></ul> <div style='margin: 6px 20px 6px 20px;'> <table style='border-collapse: collapse; width: 100%;'> <thead> <tr style='background-color: #0070C0;color: #fff'> <th style='border: 1px solid #0070C0;border-right-color: #F0F0F0; width: 22%'>Thông tin</th> <th style='border: 1px solid #0070C0;border-right-color: #F0F0F0; width: 39%'>Thông tin sai sót</th> <th style='border: 1px solid #0070C0; width: 39%'>Thông tin đúng</th> </tr></thead> <tbody> <tr style='display: ##displayhotennguoimuahang##'> <td style='border: 1px solid #0070C0; padding: 0px 2px 0px 2px'>Họ và tên người mua hàng </td><td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##hotennguoimuahang_sai## </td><td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##hotennguoimuahang_dung## </td></tr><tr style='display: ##displaytendonvi##'> <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>Tên đơn vị</td><td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##tendonvi_sai##</td><td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##tendonvi_dung##</td></tr><tr style='display: ##displaydiachi##'> <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>Địa chỉ</td><td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##diachi_sai##</td><td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##diachi_dung##</td></tr></tbody> </table> </div></div><br><div style='padding:0px 20px 0px 20px;color:black'><strong>&nbsp;Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong> </div><img alt='' height='0' src='##EmailTrackingHandler##' width='0'/> <div style='padding:5px 20px 0px 20px'> <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'> <strong>##tendonvi##</strong> </div></div><div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ 2021 - 2022 PHAN MEM BACH KHOA </div></div></div></div>");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDon,
                column: "TieuDeEmail",
                value: "##tendonvi##<br>Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn cho Quý khách");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDonHinhThucCoMaTuMTT,
                column: "TieuDeEmail",
                value: "##tendonvi##<br>Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn cho Quý khách");

            migrationBuilder.UpdateData(
                table: "ConfigNoiDungEmails",
                keyColumn: "LoaiEmail",
                keyValue: (int)LoaiEmail.ThongBaoSaiThongTinKhongPhaiLapLaiHoaDonHinhThucCoMaTuMTT,
                column: "NoiDungEmail",
                value: @"<div class='container' style='width:100%;margin:0;font-family:Tahoma;font-size: 14px;'> <div class='body' style='padding:10px'> <div class='subject' style='background-color:#0070C0; border-top-left-radius:4px; border-top-right-radius:4px; padding: 10px 20px 10px 20px;'> <span style='color:#ffffff; font-family:Tahoma; font-size:20px; white-space: pre-wrap!important;text-align: justify!important;'>##tendonvi##<br/>Gửi thông báo hóa đơn có thông tin sai sót không phải lập lại hóa đơn cho Quý khách</span> </div><div class='content' style='background-color:#fafafa; border-color:#e1e1e1; border-style:none solid solid; border-width:1px; border-radius: 0 0 4px 4px;'> <div class='content-text' style='color:#030303; font-family:Tahoma,serif; line-height:26px; padding:10px 20px 0px 20px'> <strong>Kính gửi: Quý khách&nbsp;##tennguoinhan##</strong><br/> ##tendonvi##&nbsp;xin trân trọng thông báo với Quý khách: Chúng tôi nhận thấy hóa đơn điện tử đã lập và đã gửi đến <strong>Quý khách</strong> có thông tin sai sót. </div><div style='padding:5px 20px 10px 20px'> Thông tin hóa đơn có thông tin sai sót không phải lập lại hóa đơn theo quy định tại điểm a, khoản 2, điều 19 của Nghị định số 123/2020/NĐ-CP: </div><div class='detail' style='background-color:#FFF2CC; line-height:30px; padding:1px'>
                            <ul style='margin-left:25px'>  
                                <li>Ký hiệu mẫu số:&nbsp;<strong>##mauso##</strong></li>
                                <li>Ký hiệu:&nbsp;<strong>##kyhieu##</strong></li>
                                <li>Số: <strong>##so##</strong></li>
                                <li>Ngày:&nbsp;<strong>##ngayhoadon##</strong></li>
                                <li>Tổng tiền thanh toán:&nbsp;<strong>##tongtien##</strong></li></ul> 
                            <div style='margin: 6px 20px 6px 20px;'> 
                                <table style='border-collapse: collapse; width: 100%;'> 
                                    <thead> 
                                        <tr style='background-color: #0070C0;color: #fff'> 
                                            <th style='border: 1px solid #0070C0;border-right-color: #F0F0F0; width: 22%'>Thông tin</th> 
                                            <th style='border: 1px solid #0070C0;border-right-color: #F0F0F0; width: 39%'>Thông tin sai sót</th> 
                                            <th style='border: 1px solid #0070C0; width: 39%'>Thông tin đúng</th> 
                                        </tr>
                                    </thead> 
                                    <tbody> 
                                        <tr style='display: ##displayten##'>
                                            <td style='border: 1px solid #0070C0; padding: 0px 2px 0px 2px'>Tên </td>
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##ten_sai## </td>
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##ten_dung## </td>
                                        </tr>
                                        <tr style='display: ##displaysodienthoai##'> 
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>Số điện thoại</td>
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##sodienthoai_sai##</td>
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##sodienthoai_dung##</td>
                                        </tr>
                                        <tr style='display: ##displaycancuoccongdan##'> 
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>Căn cước công dân</td>
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##cancuoccongdan_sai##</td>
                                            <td style='border: 1px solid #0070C0;padding: 0px 2px 0px 2px'>##cancuoccongdan_dung##</td>
                                        </tr>
                                    </tbody> 
                                </table> 
                                </div></div><br><div style='padding:0px 20px 0px 20px;color:black'><strong>&nbsp;Tr&acirc;n trọng k&iacute;nh ch&agrave;o!</strong> </div><img alt='' height='0' src='##EmailTrackingHandler##' width='0'/> <div style='padding:5px 20px 0px 20px'> <div style='text-align:center; color:white; background-color: #0070C0; padding: 6px; border-radius: 0 0 4px 4px;'> <strong>##tendonvi##</strong> </div></div><div style='color: #9b9b9b; padding: 8px 0; text-align: center'>Copyright ⓒ 2021 - 2022 PHAN MEM BACH KHOA </div></div></div></div>"
                );


            migrationBuilder.Sql("UPDATE ConfigNoiDungEmails SET NoiDungEmail = REPLACE(NoiDungEmail, 'padding:24px', 'padding: 10px 20px 10px 20px') WHERE LoaiEmail = 0;");
            migrationBuilder.Sql("UPDATE ConfigNoiDungEmails SET NoiDungEmail = REPLACE(NoiDungEmail, 'font-size:22px', 'font-size:20px') WHERE LoaiEmail = 0;");
            migrationBuilder.Sql("UPDATE ConfigNoiDungEmails SET NoiDungEmail = REPLACE(NoiDungEmail, 'padding:40px', 'padding:10px') WHERE LoaiEmail = 0;");

            migrationBuilder.Sql("UPDATE ConfigNoiDungEmails SET NoiDungEmail = STUFF(NoiDungEmail, CHARINDEX('Copyright', NoiDungEmail) + 11, 0, ' ');");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
