using HMS.Models.Entities;
using HMS.Models.Enums;
using MassTransit;

namespace HMS.Contracts.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = NewId.Next().ToGuid();
        public DateTime DateCreated  { get; set; }
        public DateTime DateModified { get; set; }

    }
}
