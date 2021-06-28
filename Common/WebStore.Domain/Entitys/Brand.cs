using System.Collections.Generic;
using WebStore.Domain.Entitys.Base;
using WebStore.Domain.Entitys.Base.Interfaces;

namespace WebStore.Domain.Entitys
{
    //[Table("Brand_test")] Атрибут названия таблицы
    public class Brand : NamedEntity, IOrderedEntity
    {
        //[Column("OrderTest")]  ... поля БД
        public int Order { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
