using FixTest.Entities.Interfaces;

namespace FixTest.Entities.Base
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        
        public override bool Equals(object obj)
        {
            return obj is IEntity<T> other && Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}