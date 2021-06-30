using System.Collections.Generic;
using System.Linq;
using WebStore.Domain.Entitys;

namespace WebStore.Domain.DTO
{
    public static class BrandMapperDTO
    {
        public static BrandDTO ToDTO(this Brand Brand) => Brand is null
            ? null
            : new BrandDTO
            {
                Id = Brand.Id,
                Name = Brand.Name,
                Order = Brand.Order,
            };

        public static Brand FromDTO(this BrandDTO Brand) => Brand is null
            ? null
            : new Brand
            {
                Id = Brand.Id,
                Name = Brand.Name,
                Order = Brand.Order,
            };

        public static IEnumerable<BrandDTO> ToDTO(this IEnumerable<Brand> Brands) => Brands.Select(ToDTO);
        public static IEnumerable<Brand> FromDTO(this IEnumerable<BrandDTO> Brands) => Brands.Select(FromDTO);
    }
}