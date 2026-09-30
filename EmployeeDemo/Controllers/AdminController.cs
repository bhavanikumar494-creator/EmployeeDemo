using EmployeeDemo.Models;
using EmployeeDemo.Data; // add if you created the DbContext in Data
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeeDemo.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdminController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;   
        }
    
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            if (product.ImageFile != null)
            {
                string folderPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "images"
                );

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string fileName = Guid.NewGuid().ToString()
                                + Path.GetExtension(product.ImageFile.FileName);

                string filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(
                    filePath,
                    FileMode.Create))
                {
                    product.ImageFile.CopyTo(stream);
                }

                product.ImagePath = "/images/" + fileName;
            }

            if (ModelState.IsValid)
            {
                product.CreatedAt = DateTime.Now;

                applicationDbContext.Products.Add(product);

                applicationDbContext.SaveChanges();

                return RedirectToAction("Index");


            }
            else
            {
                return View(product);
            }
        }
    }
}
        