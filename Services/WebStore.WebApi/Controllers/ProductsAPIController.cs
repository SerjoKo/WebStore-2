using Microsoft.AspNetCore.Mvc;
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
    }
}
