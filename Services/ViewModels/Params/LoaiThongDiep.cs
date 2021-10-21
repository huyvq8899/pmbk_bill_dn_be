namespace Services.ViewModels.Params
{
    public class LoaiThongDiep
    {
        public LoaiThongDiep()
        {
            IsParent = false;
        }

        public int LoaiThongDiepId { get; set; }
        public int? MaLoaiThongDiep { get; set; }
        public string Ten { get; set; }
        public int? LoaiThongDiepChaId { get; set; }
        public int Level { get; set; }
        public bool IsParent { get; set; }
    }
}
