﻿using System.Collections.Generic;
using WebStore.Domain;
using WebStore.Domain.Entitys;

namespace WebStore.Inerfaces.Services
{
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        IEnumerable<Brand> GetBrands();

        IEnumerable<Product> GetProducts(ProductFilter Filter = null);

        Product GetProductById(int id);
    }
}
