using EmployeeDemo.Models;
using EmployeeDemo.Data; // add if you created the DbContext in Data
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace EmployeeDemo.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public AdminController(ApplicationDbContext applicationDbContext,IWebHostEnvironment webhostEnvironment)
        {
            this.applicationDbContext = applicationDbContext;
            this.webHostEnvironment = webhostEnvironment;
        }
    
        public IActionResult Index()
        {
            List<Product> allProducts=applicationDbContext.Products.ToList();
            return View(allProducts);
        }

        public IActionResult Create()
        {
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            Product? product = await applicationDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            Product? existingProduct =
                await applicationDbContext.Products.FindAsync(product.Id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Price = product.Price;

            // If a new image is uploaded
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

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                existingProduct.ImagePath = "/images/" + fileName;
            }

            await applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
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


        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product? product =
                await applicationDbContext.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            applicationDbContext.Products.Remove(product);

            await applicationDbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
        