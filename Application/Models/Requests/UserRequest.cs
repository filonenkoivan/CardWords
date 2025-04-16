namespace Api.Models
{
    public class UserRequest
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? PasswordConfirm { get; set; }
    }
}
