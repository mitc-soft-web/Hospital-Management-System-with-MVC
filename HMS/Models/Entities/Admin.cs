using HMS.Models.Contracts;
using HMS.Models.Enums;

namespace HMS.Models.Entities
{
    public class Admin : BaseEntity, IBaseUser
    {
       
        public Guid UserId { get; set; }
        public User User { get; set; }
        public DateTime DateOfBirth { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string FirstName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string LastName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Gender Gender { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Address { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string PhoneNumber { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
