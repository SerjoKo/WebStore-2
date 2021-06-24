using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entitys;
using WebStore.Domain.ViewModels;

namespace WebStore.Inftastructure.Mapping
{
    public static class ProductMapper
    {
        public static ProductViewModel ToView(this Product product)
        {
            return product is null ? null : new ProductViewModel
            {
                Id = product.Id,
                Price = product.Price,
                ImgUrl = product.ImgUrl,
                Name = product.Name,
                Section = product.Section?.Name,
                Brand = product.Brand?.Name,
            };
        }

        public static IEnumerable<ProductViewModel>
            ToView(this IEnumerable<Product> products) => products.Select(ToView);
    }
}
