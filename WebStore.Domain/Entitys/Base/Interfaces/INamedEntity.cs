namespace WebStore.Domain.Entitys.Base.Interfaces
{
    public interface INamedEntity : IEntity
    {
        string Name { get; }
    }
}
