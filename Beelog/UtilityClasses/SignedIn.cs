using Beelog.Data;
using Beelog.Models;

namespace Beelog.UtilityClasses
{
    public class SignedIn
    {
        public static bool isSignedIn(HttpContext context)
        {
            var uid = context.Session.GetString("uid");
            if (uid == null)
            {
                return false;
            }
            return true;
        }
        public static User GetCurrentUser(HttpContext context, ApplicationDbContext dbContext)
        {
            return dbContext.Users.FirstOrDefault(u => u.Id.ToString().Equals(context.Session.GetString("uid")));
        }
    }
}
