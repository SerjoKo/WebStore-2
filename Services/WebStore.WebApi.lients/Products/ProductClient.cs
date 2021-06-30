using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
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
            return Get<Brand>($"{Address}/brand{id}");
        }

        public IEnumerable<Brand> GetBrands()
        {
            return Get<IEnumerable<Brand>>($"{Address}/brandss");
        }

        public Product GetProductById(int id)
        {
            return Get<Product>($"{Address}/product{id}");
        }

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            var reesponse = Post(Address, Filter);
            var product = reesponse.Content.ReadFromJsonAsync<IEnumerable<Product>>().Result;

            return product;
        }

        public Section GetSection(int id)
        {
            return Get<Section>($"{Address}/section{id}");
        }

        public IEnumerable<Section> GetSections()
        {
            return Get<IEnumerable<Section>>($"{Address}/sections");
        }
    }
}
