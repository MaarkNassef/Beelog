using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Beelog.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ProfilePicturePath { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<User> Following { get; set; }
        public ICollection<User> Follower { get; set; }

    }
}
