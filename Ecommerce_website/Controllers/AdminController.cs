using Ecommerce_website.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Ecommerce_website.Controllers
{
    public class AdminController : Controller
    {
        private readonly Dbcontext context;
        public IWebHostEnvironment Env { get; }
        public AdminController(Dbcontext context, IWebHostEnvironment env)
        {
            this.context = context;
            Env = env;
        }

        

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("admin_session" ) == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
            }
                
        }
        [HttpPost]
        public IActionResult Login(string adminemail, string adminpassword)
        {
            var admin = context.admin_tbl.FirstOrDefault(x => x.admin_email == adminemail);
            if (admin != null && admin.admin_password == adminpassword)
            {
                HttpContext.Session.SetString("admin_session", admin.admin_id.ToString());
                return RedirectToAction("Index", "Admin");

            }
            else
            {
                ViewBag.message = "incorrect email and password.";
                return View("signupsigin");

            }
            
        }
        
        public IActionResult Login()
        {
            return View();

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("admin_session");
            return RedirectToAction("Login", "Admin");

        }
        public IActionResult Profile()
        {
            var adminid = HttpContext.Session.GetString("admin_session");
            var admin = context.admin_tbl.FirstOrDefault(x => x.admin_id == int.Parse(adminid));
            return View(admin);

        }
        [HttpPost]
        public IActionResult Profile(Admin admin,IFormFile admin_image)
        {
            if (admin_image != null)
            {
                var ext=Path.GetExtension(admin_image.FileName);
                var size = admin_image.Length;
                if (ext ==".jpg" || ext == ".png" || ext == ".jpeg")
                {
                    if(size <= 1000000)
                    {
                        string path = Path.Combine(Env.WebRootPath, "Images", admin_image.FileName);
                        FileStream fs = new FileStream(path, FileMode.Create);
                        admin_image.CopyTo(fs);
                        admin.admin_image = admin_image.FileName;
                    }
                    else
                    {
                        TempData["sizeerror"] = "Image size must be less than 1MB";
                        return RedirectToAction("Profile");
                    }
                   

                }
                else
                {
                    TempData["exterror"] = "Only PNG JPG AND JEPG images allowed";
                    return RedirectToAction("Profile");
                }
                    
            }
            
            context.admin_tbl.Update(admin);
            context.SaveChanges();
            return RedirectToAction("index");

        }

        //crud operations on customer
        public IActionResult fetchcustomer()
        {
            return View(context.customer_tbl.ToList());
        }
        public IActionResult Details(int id)
        {
            var cus=context.customer_tbl.FirstOrDefault(x=>x.customer_id==id);
            return View(cus);
        }
        public IActionResult Edit(int id)
        {
            var cus = context.customer_tbl.FirstOrDefault(x => x.customer_id == id);
            return View(cus);
        }
        [HttpPost]
        public IActionResult Edit(Customer customer,IFormFile customer_image)
        {
            if (customer_image != null)
            {
                var ext = Path.GetExtension(customer_image.FileName);
                var size = customer_image.Length;
                if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                {
                    if (size <= 1000000)
                    {
                        string path = Path.Combine(Env.WebRootPath, "customerimages", customer_image.FileName);
                        FileStream fs = new FileStream(path, FileMode.Create);
                        customer_image.CopyTo(fs);
                        customer.customer_image = customer_image.FileName;
                    }
                    else
                    {
                        TempData["sizeerror"] = "Image size must be less than 1MB";
                        return RedirectToAction("Edit", new { id = customer.customer_id});
                    }


                }
                else
                {
                    TempData["exterror"] = "Only PNG JPG AND JEPG images allowed";
                     return RedirectToAction("Edit", new { id = customer.customer_id });
                }

            }

            context.customer_tbl.Update(customer);
            context.SaveChanges();
            return RedirectToAction("fetchcustomer");
        }

        public IActionResult Delete(int id)
        {
            var cus=context.customer_tbl.FirstOrDefault(x=>x.customer_id==id);
            context.Remove(cus);
            context.SaveChanges();
            return RedirectToAction("fetchcustomer");

        }


        //crud operations on category
        public IActionResult fetchcategory()
        {
            return View(context.category_tbl.ToList());
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category cat)
        {
            context.category_tbl.Add(cat);
            context.SaveChanges();

            return RedirectToAction("fetchcategory");
        }
        public IActionResult updateCategory(int id)
        {
            var cat=context.category_tbl.FirstOrDefault(x=> x.category_id==id);
            return View(cat);
        }
        [HttpPost]
        public IActionResult updateCategory(Category cat)
        {
            context.category_tbl.Update(cat);
            context.SaveChanges();
            return RedirectToAction("fetchcategory");
        }
        public IActionResult DeleteCategoryPermission(int id)
        {
            var cat = context.category_tbl.FirstOrDefault(x => x.category_id == id);
            return View(cat);
        }
        
        public IActionResult DeleteCategoryconfirmed(int id)
        {
            var cat = context.category_tbl.FirstOrDefault(x => x.category_id == id);
            context.Remove(cat);
            context.SaveChanges();
            return RedirectToAction("fetchcategory");

        }


        //crud operations on product
        public IActionResult fetchproducts()
        {
            return View(context.product_tbl.ToList());
        }
        public IActionResult AddProduct()
        {
            ViewData["category"] = context.category_tbl.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(Product prod, IFormFile product_image)
        {
            // 1️⃣ First: Check model validation
            //if (!ModelState.IsValid)
            //{
            //    // reload categories again because the View needs them
            //    ViewData["category"] = context.category_tbl.ToList();
            //    return View(prod);   // STAY ON SAME PAGE + SHOW ERRORS
            //}

            // 2️⃣ Image validations
            if (product_image != null)
            {
                var ext = Path.GetExtension(product_image.FileName).ToLower();
                var size = product_image.Length;

                if (ext != ".jpg" && ext != ".png" && ext != ".jpeg")
                {
                    ModelState.AddModelError("product_image", "Only JPG, PNG, JPEG allowed");
                    ViewData["category"] = context.category_tbl.ToList();
                    return View(prod);
                }

                if (size > 1000000)
                {
                    ModelState.AddModelError("product_image", "Image must be less than 1MB");
                    ViewData["category"] = context.category_tbl.ToList();
                    return View(prod);
                }

                // Save image
                string path = Path.Combine(Env.WebRootPath, "productimages", product_image.FileName);
                using var fs = new FileStream(path, FileMode.Create);
                product_image.CopyTo(fs);
                prod.product_image = product_image.FileName;
            }

            // 3️⃣ Save to DB
            context.product_tbl.Add(prod);
            context.SaveChanges();

            return RedirectToAction("fetchproducts");
        }

        public IActionResult ProductDetails(int id)
        {
            
           
            return View(context.product_tbl.Include(p=> p.category).FirstOrDefault(p=> p.product_id==id));
        }

        public IActionResult DeleteProductPermission(int id)
        {
            var prod= context.product_tbl.FirstOrDefault(x => x.product_id == id);
            return View(prod);
        }

        public IActionResult DeleteProductConfirmed(int id)
        {
            var prod = context.product_tbl.FirstOrDefault(x => x.product_id == id);
            context.Remove(prod);
            context.SaveChanges();
            return RedirectToAction("fetchproducts");

        }
        public IActionResult updateProduct(int id)
        {
            List<Category> categories = context.category_tbl.ToList();
            ViewData["category"] = categories;
            var prod = context.product_tbl.FirstOrDefault(x => x.product_id == id);
            ViewBag.selectedcategoryid = prod.cat_id;
            return View(prod);
        }
        [HttpPost]
        public IActionResult updateProduct(Product prod)
        {
            context.product_tbl.Update(prod);
            context.SaveChanges();
            return RedirectToAction("fetchproducts");
        }
        
        public IActionResult updateProductPicture(int id)
        {
            return View(context.product_tbl.FirstOrDefault(x=> x.product_id==id));
        }
        [HttpPost]
        public IActionResult updateProductPicture(Product prod, IFormFile product_image)
        {
            if (product_image != null)
            {
                var ext = Path.GetExtension(product_image.FileName);
                var size = product_image.Length;
                if (ext == ".jpg" || ext == ".png" || ext == ".jpeg")
                {
                    if (size <= 1000000)
                    {
                        string path = Path.Combine(Env.WebRootPath, "productimages", product_image.FileName);
                        FileStream fs = new FileStream(path, FileMode.Create);
                        product_image.CopyTo(fs);
                        prod.product_image = product_image.FileName;
                    }
                    else
                    {
                        TempData["sizeerror"] = "Image size must be less than 1MB";
                        return RedirectToAction("updateProductPicture", new { id = prod.product_id });
                    }


                }
                else
                {
                    TempData["exterror"] = "Only PNG JPG AND JEPG images allowed";
                    return RedirectToAction("updateProductPicture", new { id = prod.product_id });
                }

            }
            context.product_tbl.Update(prod);
            context.SaveChanges();
            return RedirectToAction("fetchproducts");
        }




























    }

}
