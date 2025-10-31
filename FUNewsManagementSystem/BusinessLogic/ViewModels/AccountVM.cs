namespace BusinessLogic.ViewModels
{
    public class AccountVM
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; } = string.Empty;
        public string AccountEmail { get; set; } = string.Empty;
        public string AccountPassword { get; set; } = string.Empty;
        public int AccountRole { get; set; }
    }
}
