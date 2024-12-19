namespace eBooksBackend.Data.Controllers.Users
{
    public class CreateUserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
