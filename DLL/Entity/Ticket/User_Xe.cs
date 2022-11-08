namespace DLL.Entity.Ticket
{
    public class User_Xe : ThongTinChung
    {
        public string UserId { get; set; }
        public string XeId { get; set; }

        public User User { get; set; }
        public Xe Xe { get; set; }
    }
}
