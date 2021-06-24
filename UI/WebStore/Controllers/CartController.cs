using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebStore.Domain.ViewModels;
using WebStore.Inerfaces.Services;
using WebStore.Servicess.Interfaces;

namespace WebStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _CartService;

        public CartController(ICartService CartService)
        {
            _CartService = CartService;
        }

        public IActionResult Index()
        {
            return View(new CartOrderViewModel { Cart = _CartService.GetCartViewModel() });
        }

        public IActionResult Add(int id)
        {
            _CartService.Add(id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int id)
        {
            _CartService.Remove(id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Decrement(int id)
        {
            _CartService.Decrement(id);
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Clear()
        {
            _CartService.Clear();
            return RedirectToAction("Index", "Cart");
        }

        [Authorize, HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), new CartOrderViewModel
                {
                    Cart = _CartService.GetCartViewModel(),
                    Order = OrderModel
                });

            var order = await OrderService.CreateOrder(
                User.Identity!.Name,
                _CartService.GetCartViewModel(),
                OrderModel);

            _CartService.Clear();

            return RedirectToAction(nameof(OrderConfirmed), new { order.Id });
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
