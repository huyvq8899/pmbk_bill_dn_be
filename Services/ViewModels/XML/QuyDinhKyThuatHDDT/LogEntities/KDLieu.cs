namespace Services.ViewModels.XML.QuyDinhKyThuatHDDT.LogEntities
{
    /// <summary>
    /// Kỳ dữ liệu
    /// </summary>
    /// 1.	Danh mục các loại kỳ:
    ////    STT Giá trị Mô tả
    ////     1		T Kỳ theo tháng
    ////     2		Q Kỳ theo quý
    ////     3		N Kỳ theo ngày
    ////    2.	Định dạng trường kỳ theo tháng, quý: N1N2/Y1Y2Y3Y4
    ////    Trong đó: 
    ////    -	N1N2 là 2 số chỉ tháng nếu loại kỳ là T hoặc là 2 số tối đa chỉ quý nếu loại kỳ là Q.
    ////    -	Y1Y2Y3Y4 là 4 số chỉ kỳ năm.
    ////    Ví dụ 1: Kỳ tính thuế tháng 12 năm 2022 được biểu diễn bằng các thẻ: LKTThue = T; KTThue = 12/2022
    ////    Ví dụ 2: Kỳ tính thuế quý 3 năm 2022 được biểu diễn bằng các thẻ: LKTThue = Q; KTThue = 3/2022
    ////    3.	Định dạng trường kỳ theo ngày: N1N2/N3N4/Y1Y2Y3Y4
    ////    Trong đó: 
    ////    -	N1N2 là 2 số chỉ ngày.
    ////    -	N3N4 là 2 số chỉ tháng.
    ////    -	Y1Y2Y3Y4 là 4 số chỉ năm.
    ////    Ví dụ: Kỳ dữ liệu ngày 15 tháng 12 năm 2022 được biểu diễn bằng các thẻ: LKDLieu = N; KDLieu = 15/12/2022

    public class KDLieu
    {

    }
}
