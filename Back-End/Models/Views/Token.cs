namespace Services.Models
{
    public class Token
    {
        public string SessionId { get; set; }
        public string _Token { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
