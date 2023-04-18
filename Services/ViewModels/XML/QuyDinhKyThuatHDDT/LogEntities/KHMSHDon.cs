using System;
using System.Collections.Generic;
using System.Text;

namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities
{ //Ký hiệu mẫu số hóa đơn
  ////    1.	Đối với hóa đơn điện tử theo Nghị định số 123/2020/NĐ-CP thì sử dụng bảng danh mục sau:
  ////             STT Ký hiệu mẫu số hóa đơn Tên/Mô tả
  ////              1		1	Phản ánh loại Hóa đơn điện tử giá trị gia tăng
  ////              2		2	Phản ánh loại Hóa đơn điện tử bán hàng
  ////              3		3	Phản ánh loại Hóa đơn điện tử bán tài sản công
  ////              4		4	Phản ánh loại Hóa đơn điện tử bán hàng dự trữ quốc gia
  ////              5		5	Phản ánh các loại hóa đơn điện tử khác là tem điện tử, vé điện tử, thẻ điện tử, phiếu thu điện tử hoặc các chứng từ điện tử có tên gọi khác nhưng có nội dung của hóa đơn điện tử theo quy định tại Nghị định số 123/2020/NĐ-CP
  ////              6		6	Phản ánh các chứng từ điện tử được sử dụng và quản lý như hóa đơn gồm phiếu xuất kho kiêm vận chuyển nội bộ điện tử, phiếu xuất kho hàng gửi bán đại lý điện tử
  ////    2.	Đối với hóa đơn theo Nghị định số 51/2010/NĐ-CP và Nghị định số 04/2014/NĐ-CP thì theo hướng dẫn sau: 
  ////          Ký hiệu mẫu số hóa đơn bao gồm 11 ký tự, có cấu trúc như sau:
  ////          -	2 ký tự đầu thể hiện loại hóa đơn
  ////          -	Tối đa 4 ký tự tiếp theo thể hiện tên hóa đơn
  ////          -	01 ký tự tiếp theo thể hiện số liên của hóa đơn(đối với hoá đơn điện tử số liên là 0)
  ////          -	01 ký tự tiếp theo là “/” để phân biệt số liên với số thứ tự của mẫu trong một loại hóa đơn.
  ////          -	03 ký tự tiếp theo là số thứ tự của mẫu trong một loại hóa đơn.
  ////          Bảng ký hiệu 6 ký tự đầu của mẫu hóa đơn:
  ////             STT Mẫu số Loại hóa đơn
  ////              1		01GTKT Hóa đơn giá trị gia tăng
  ////              2		02GTTT Hóa đơn bán hàng
  ////              3		03XKNB Phiếu xuất kho kiêm vận chuyển nội bộ
  ////              4		04HGDL Phiếu xuất kho hàng gửi bán đại lý
  ////         Ví dụ: Ký hiệu 01GTKT0/001 được hiểu là: Mẫu thứ nhất của loại hóa đơn giá trị gia tăng.
  ////         Số thứ tự mẫu trong một loại hóa đơn thay đổi khi có một trong các tiêu chí trên mẫu hóa đơn đã thông báo phát hành thay đổi như: một trong các nội dung bắt buộc; kích thước của hóa đơn; nhu cầu sử dụng hóa đơn đến từng bộ phận sử dụng nhằm phục vụ công tác quản lý...
  ////          -	Đối với tem, vé, thẻ: Bắt buộc ghi 3 ký tự đầu để phân biệt tem, vé, thẻ thuộc loại hóa đơn giá trị gia tăng hay hóa đơn bán hàng.Các thông tin còn lại do tổ chức, cá nhân tự quy định nhưng không vượt quá 11 ký tự.
  ////          Cụ thể:	
  ////          -	Ký hiệu 01/: đối với tem, vé, thẻ thuộc loại hóa đơn GTGT
  ////          -	Ký hiệu 02/: đối với tem, vé, thẻ thuộc loại hóa đơn bán hàng
  ////    3.	Hoá đơn giấy theo quy định tại Nghị định số 123/2020/NĐ-CP thì theo hướng dẫn sau: 
  ////      a) Ký hiệu mẫu số hóa đơn do cơ quan thuế đặt in là một nhóm gồm 11 ký tự thể hiện các thông tin về: tên loại hoá đơn, số liên, số thứ tự mẫu trong một loại hoá đơn(một loại hoá đơn có thể có nhiều mẫu), cụ thể như sau:
  ////          Sáu(06) ký tự đầu tiên thể hiện tên loại hóa đơn:
  ////          - 01GTKT: Hóa đơn giá trị gia tăng.
  ////          - 02GTTT: Hóa đơn bán hàng.
  ////          - 07KPTQ: Hóa đơn bán hàng dành cho tổ chức, cá nhân trong khu phi thuế quan.
  ////          - 03XKNB: Phiếu xuất kho kiêm vận chuyển nội bộ.
  ////          - 04HGDL: Phiếu xuất kho hàng gửi bán đại lý.
  ////      Một (01) ký tự tiếp theo là các số tự nhiên 1, 2, 3, 4, 5, 6, 7, 8, 9 thể hiện số liên hóa đơn.
  ////      Một(01) ký tự tiếp theo là “/” để phân cách.
  ////      Ba(03) ký tự tiếp theo là số thứ tự của mẫu trong một loại hóa đơn, bắt đầu bằng 001 và tối đa đến 999.
  ////      b) Ký hiệu mẫu số hóa đơn là tem, vé, thẻ do cơ quan thuế đặt in
  ////          03 ký tự đầu để phân biệt tem, vé, thẻ thuộc loại hóa đơn giá trị gia tăng hay hóa đơn bán hàng như sau:
  ////          - Ký hiệu 01/: đối với tem, vé, thẻ thuộc loại hóa đơn GTGT;
  ////          - Ký hiệu 02/: đối với tem, vé, thẻ thuộc loại hóa đơn bán hàng.
  ////      Các thông tin còn lại do cơ quan thuế quy định nhưng không vượt quá 11 ký tự

    class KHMSHDon
    {
    }
}
