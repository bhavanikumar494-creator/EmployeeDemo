using EmployeeDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeDemo.Controllers
{
    public class ProductController : Controller
    {
         public readonly IPaymentServices _paymentServices;
        public ProductController(IPaymentServices paymentServices)
        {
            _paymentServices = paymentServices;
        }
        public IActionResult Index()
        { 
            return View();
        }

        public IActionResult About()
        {
            return View();
        }



        public string Privacy()
        {
            string s = _paymentServices.payment(100);


            return s;
        }
    }
}
