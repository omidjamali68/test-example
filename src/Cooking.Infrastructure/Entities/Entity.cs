namespace Cooking.Infrastructure.Entities
{
    public abstract class Entity
    {
    }

    public abstract class Entity<TId> : Entity
    {
        protected Entity()
        {
        }

        protected Entity(TId id)
        {
            Id = id;
        }

        public TId Id { get; }

        public override bool Equals(object obj)
        {
            return obj is Entity<TId> other && other.GetType() == GetType() && Equals(other.Id, Id);
        }

        public override int GetHashCode()
        {
            return Id != null ? Id.GetHashCode() : 0;
        }
    }
}