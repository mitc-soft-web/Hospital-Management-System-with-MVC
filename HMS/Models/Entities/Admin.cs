namespace HMS.Models.Entities
{
    public class Admin : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
