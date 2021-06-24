namespace WebStore.Domain.Entitys.Base.Interfaces
{
    public interface IOrderedEntity : IEntity
    {
        int Order { get; }
    }
}
