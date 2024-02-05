namespace CustomPageApp.ViewModels
{
    public class GetData
    {
        public string ? Discriminator { get; set; }
        public string  ? Name { get; set; }
        public string ? Address { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? LockoutEnabled { get; set; }
    }
}
