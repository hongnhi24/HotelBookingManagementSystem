using Microsoft.AspNetCore.Mvc;

namespace WebAppCustomer.Controllers
{
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
