using Beelog.Data;
using Beelog.Models;

namespace Beelog.UtilityClasses
{
    public class Liked
    {
        public static bool isLiked (HttpContext context, Post post) {
            return post.Likes.FirstOrDefault(l => l.UserId.ToString().Equals(context.Session.GetString("uid"))) != null;
        }
    }
}
