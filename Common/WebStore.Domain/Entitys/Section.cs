using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entitys.Base;
using WebStore.Domain.Entitys.Base.Interfaces;

namespace WebStore.Domain.Entitys
{
    public class Section : NamedEntity, IOrderedEntity
    {
        //public int Order { get; set; }

        //public int? ParentId { get; set; }

        //[ForeignKey(nameof(ParentId))]
        //public Section Parent { get; set; }

        //public ICollection<Product> Products { get; set; }
        public int Order { get; set; }

        public int? ParentId { get; set; }

        [ForeignKey(nameof(ParentId))]
        public Section Parent { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}
