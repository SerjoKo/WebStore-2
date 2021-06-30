using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using WebStore.Domain;
using WebStore.Domain.DTO;
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
            return Get<BrandDTO>($"{Address}/brand{id}").FromDTO();
        }

        public IEnumerable<Brand> GetBrands()
        {
            return Get<IEnumerable<BrandDTO>>($"{Address}/brandss").FromDTO();
        }

        public Product GetProductById(int id)
        {
            return Get<ProductDTO>($"{Address}/product{id}").FromDTO();
        }

        public IEnumerable<Product> GetProducts(ProductFilter Filter = null)
        {
            var reesponse = Post(Address, Filter);
            var product = reesponse.Content.ReadFromJsonAsync<IEnumerable<ProductDTO>>().Result;

            return product.FromDTO();
        }

        public Section GetSection(int id)
        {
            return Get<SectionDTO>($"{Address}/section{id}").FromDTO();
        }

        public IEnumerable<Section> GetSections()
        {
            return Get<IEnumerable<SectionDTO>>($"{Address}/sections").FromDTO();
        }
    }
}
