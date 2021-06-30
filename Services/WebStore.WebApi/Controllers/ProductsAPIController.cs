using Microsoft.AspNetCore.Mvc;
using WebStore.Domain;
using WebStore.Domain.DTO;
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
        public IActionResult GetSections() => Ok(_ProductData.GetSections().ToDTO());

        [HttpGet("sections/{id:int}")]
        public IActionResult GetSection(int id) => Ok(_ProductData.GetSection(id).ToDTO());

        [HttpGet("brands")]
        public IActionResult GetBrands () => Ok(_ProductData.GetBrands().ToDTO());

        [HttpGet("brands/{id:int}")]
        public IActionResult GetBrand(int id) => Ok(_ProductData.GetBrand(id).ToDTO());

        [HttpPost]
        public IActionResult GetProducts(ProductFilter Filter = null) => Ok(_ProductData.GetProducts(Filter).ToDTO());

        [HttpGet("{id}")]
        public IActionResult GetProduct(int id) => Ok(_ProductData.GetProductById(id).ToDTO());
    }
}
