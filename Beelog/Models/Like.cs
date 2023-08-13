using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Beelog.Models
{
    public class Like
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
