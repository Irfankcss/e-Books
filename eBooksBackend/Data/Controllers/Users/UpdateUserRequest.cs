namespace eBooksBackend.Data.Controllers.Users
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public DateTime BirthDate { get; set; }= DateTime.MinValue;
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public string? Role { get; set; }
        public string Photo {  get; set; }
    }
}
