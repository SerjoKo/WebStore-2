using System.ComponentModel.DataAnnotations.Schema;
using WebStore.Domain.Entitys.Base;
using WebStore.Domain.Entitys.Base.Interfaces;

namespace WebStore.Domain.Entitys
{
    public class Product : NamedEntity, IOrderedEntity
    {
        public int Order { get; set; }

        public int SectionId { get; set; }

        [ForeignKey(nameof(SectionId))]
        public Section Section { get; set; }

        public int? BrandId { get; set; }

        [ForeignKey(nameof(BrandId))]
        public Brand Brand { get; set; }

        public string ImgUrl { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
