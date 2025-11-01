using HMS.Models.Enums;

namespace HMS.Contracts.Entities
{
    public abstract class BaseUser : BaseEntity
    {
        public DateTime DateOfBirth { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
