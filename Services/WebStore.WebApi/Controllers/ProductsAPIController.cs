using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Inerfaces;
using WebStore.Inerfaces.Services;

namespace WebStore.WebApi.Controllers
{
    [Route(WebAPIAddress.Products)]
    [ApiController]
    public class ProductsAPIController : ControllerBase
    {
        private readonly IProductData _ProductData;

        public ProductsAPIController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        [HttpGet("sections")]
        public IActionResult GetSections() => Ok(_ProductData.GetSections());

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSection(int id) => Ok(_ProductData.GetSection(id));

        [HttpGet("brands")]
        public IActionResult GetBrands () => Ok(_ProductData.GetBrands());

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrand(int id) => Ok(_ProductData.GetBrand(id));

        [HttpPost]
        public IActionResult GetProducts(ProductFilter Filter = null) => Ok(_ProductData.GetProducts(Filter));

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) => Ok(_ProductData.GetProductById(id));
    }
}
