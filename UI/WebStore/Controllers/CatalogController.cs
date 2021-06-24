using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebStore.Domain;
using WebStore.Domain.ViewModels;
using WebStore.Inerfaces.Services;
using WebStore.Inftastructure.Mapping;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData)
        {
            _ProductData = ProductData;
        }

        public IActionResult Index(int? SectionId, int? BrandId)
        {
            var filter = new ProductFilter
            {
                BrandId = BrandId,
                SectionId = SectionId,
            };

            var products = _ProductData.GetProducts(filter);

            return View(new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products
                    .OrderBy(p => p.Order).ToView(),
            });
        }

        public IActionResult Details(int Id)
        {
            var product = _ProductData.GetProductById(Id);

            if (product is null)
                return NotFound();

            //return View(new ProductViewModel
            //{ 
            //    Id = product.Id,
            //    Price = product.Price,
            //    ImgUrl = product.ImgUrl,
            //    Name = product.Name,
            //});

            return View(product.ToView());
        }
    }
}
