namespace ProductAPI.Entities
{
    // User class represents a user entity in the system
    namespace ProductAPI.Entities
    {
        public class User
        {
            public int UserId { get; set; }  // Primary key for User
            public string Username { get; set; }
            public string Password { get; set; }  // Store hashed password
            public string Role { get; set; }  // Role can be "Viewer", "Editor", etc.
        }
    }

}
