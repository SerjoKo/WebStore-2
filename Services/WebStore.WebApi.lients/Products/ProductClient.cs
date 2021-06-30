using System.Collections.Generic;
using System.Net.Http;
using WebStore.Domain;
using WebStore.Domain.Entitys;
using WebStore.Inerfaces;
using WebStore.Inerfaces.Services;
using WebStore.WebApi.lients.Base;

namespace WebStore.WebApi.Clients.Products
{
    public class ProductClient : BaseClient, IProductData
    {
        public ProductClient(HttpClient Client) : base(Client, WebAPIAddress.Products)
        {
        }

        public Brand GetBrand(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Brand> GetBrands()
        {
            throw new System.NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            throw new System.NotImplementedException();
        }

        public Section GetSection(int id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Section> GetSections()
        {
            throw new System.NotImplementedException();
        }
    }
}
