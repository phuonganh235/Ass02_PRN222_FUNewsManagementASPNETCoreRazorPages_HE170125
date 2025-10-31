namespace BusinessLogic.ViewModels
{
    public class AccountVM
    {
        public int AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int AccountRole { get; set; } // 1/2/3
        public string? AccountPassword { get; set; }
    }
}
