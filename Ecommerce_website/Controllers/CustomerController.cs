using Ecommerce_website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_website.Controllers
{
    public class CustomerController : Controller
    {

        private readonly Dbcontext context;
        
        public CustomerController(Dbcontext context)
        {
            this.context = context;
            
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("customer_session") == null)
            {
                return RedirectToAction("signupsigin");
               
            }
            else
            {
                List<Category> category = context.category_tbl.ToList();
                ViewData["categories"] = category;
                ViewBag.checksession = HttpContext.Session.GetString("customer_session");
                return View();
            }
        }
        public IActionResult signupsigin()
        {
            
            return View();
        }
        [HttpPost]
        public IActionResult Login(string customer_email,string customer_password)
        {
            var customer = context.customer_tbl.FirstOrDefault(x => x.customer_email==customer_email);
            if (customer != null && customer.customer_password==customer_password)
            {
                HttpContext.Session.SetString("customer_session", customer.customer_id.ToString());
                return RedirectToAction("Index", "Customer");

            }
            else
            {
                ViewBag.message = "incorrect email and password.";
                return View("signupsigin");

            }
            
        }
        [HttpPost]
        public IActionResult Register(Customer cus)
        {
           
            context.customer_tbl.Add(cus);
            context.SaveChanges();

            ViewBag.registersuccess = "Registration successful!";
            return View("signupsigin");
        }



    }
}
