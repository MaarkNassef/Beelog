namespace Beelog.Models
{
    public class Post
    {
        public int Id { get; set; }
        public User Author { get; set; }
        public string Text { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
    }
}
