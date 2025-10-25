using HMS.Models.Enums;
using MassTransit;

namespace HMS.Models.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; } = NewId.NextGuid();
        public DateTime DateCreated  { get; set; }
        public DateTime DateModified { get; set; }

    }
}
