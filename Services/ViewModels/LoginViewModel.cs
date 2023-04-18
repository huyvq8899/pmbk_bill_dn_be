namespace Services.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string TaxCode { get; set; }
        public string DatabaseName { get; set; }

        // Trường này dùng cho kế toán bách khoa kết nối vào hóa đơn bách khoa
        // dùng mật khẩu đã mã hóa từ trước không qua giao diện login
        public bool? IsPasswordEncoded { get; set; }
    }
}
