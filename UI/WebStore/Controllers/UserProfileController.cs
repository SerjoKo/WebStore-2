using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebStore.Services.Interfaces;
using WebStore.ViewModels;

namespace WebStore.Controllers
{
    [Authorize]
    public class UserProfileController : Controller
    {
        public IActionResult Index() => View();

        public async Task<IActionResult> Orders([FromServices] IOrderService OrderService)
        {
            var order = await OrderService.GetUserOrder(User.Identity!.Name);

            return View(order.Select(o => new UserOrderViewModel
            { 
              Id = o.Id,
              Name = o.Name,
              Phone = o.Phone,
              Adress = o.Adress,
              TotalPrice = o.Items.Sum(item => item.TotalItemPrice)
            }));
        }
    }
}
