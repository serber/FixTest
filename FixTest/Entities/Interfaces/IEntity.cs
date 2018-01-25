namespace FixTest.Entities.Interfaces
{
    /// <summary>
    /// Base interface for entity
    /// </summary>
    public interface IEntity
    {
    }
    
    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}