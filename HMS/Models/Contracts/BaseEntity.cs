using HMS.Models.Enums;
using MassTransit;

namespace HMS.Models.Contracts
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = NewId.Next().ToGuid();
        public DateTime DateCreated  { get; set; }
        public DateTime DateModified { get; set; }

    }
}
