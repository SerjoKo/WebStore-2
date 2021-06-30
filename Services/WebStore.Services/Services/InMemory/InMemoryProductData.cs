﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebStore.Data;
using WebStore.Domain;
using WebStore.Domain.Entitys;
using WebStore.Inerfaces.Services;

namespace WebStore.Services.Services.InMemory
{
    [Obsolete]
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Brand> GetBrands()
        {
            return TestData.Brands;
        }

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            IEnumerable<Product> query = TestData.Products;

            if (Filter?.SectionId is { } section_id)
                query = query.Where(product => product.SectionId == section_id);

            if (Filter?.BrandId is { } brand_id)
                query = query.Where(product => product.SectionId == brand_id);

            return query;
        }

        public IEnumerable<Section> GetSections()
        {
            return TestData.Sections;
        }

        public Product GetProductById(int Id)
        {
            return TestData.Products.SingleOrDefault(p => p.Id == Id);
        }

        public Section GetSection(int id)
        {
            throw new NotImplementedException();
        }

        public Brand GetBrand(int id)
        {
            throw new NotImplementedException();
        }
    }
}
