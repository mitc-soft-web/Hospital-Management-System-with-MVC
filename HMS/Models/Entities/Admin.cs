using HMS.Models.Contracts;

namespace HMS.Models.Entities
{
    public class Admin : BaseUser
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
