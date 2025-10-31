namespace BusinessLogic.ViewModels
{
    // Dùng cho Edit Account
    public class AccountEditVM
    {
        public short AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int AccountRole { get; set; }
        public string AccountPassword { get; set; } = "";
    }
}
