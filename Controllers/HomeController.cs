using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProdsAndCats.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;


namespace ProdsAndCats.Controllers
{
    public class HomeController : Controller
    {
        private MyContext dbContext;

        public HomeController(MyContext context)
        {
            dbContext = context;
        }

        [HttpGet("products")]
        public IActionResult Products()
        {
            List<Product> allProducts = dbContext.Products.ToList();
            ViewBag.AllProducts = allProducts;
            return View();
        }

        [HttpPost("CreateNewProduct")]
        public IActionResult CreateNewProduct(Product product)
        {
            dbContext.Add(product);
            dbContext.SaveChanges();
            return RedirectToAction("Products");
        }

        [HttpGet("categories")]
        public IActionResult Categories()
        {
            List<Category> allCategories = dbContext.Categories.ToList();
            ViewBag.AllCategories = allCategories;
            return View();
        }

        [HttpPost("CreateNewCategory")]
        public IActionResult CreateNewCategory(Category category)
        {
            dbContext.Add(category);
            dbContext.SaveChanges();
            return RedirectToAction("Categories");
        }

        [HttpGet("product/{productId}")]
        public IActionResult ProductInfo(int productId)
        {
            HttpContext.Session.SetString("Source", "Product");
            Product product = dbContext.Products
                .Include(p => p.Associations)
                .ThenInclude(a => a.Category)
                .FirstOrDefault(d => d.ProductId == productId);

            ProdCatAssViewModel PCAVM = new ProdCatAssViewModel()
            {
                AProduct = product
            };

            List<Category> notIncluded = dbContext.Categories.Where(c => !product.Associations.Select(a => a.CategoryId).Contains(c.CategoryId)).ToList();
            // var notInCategory = Category.Where(c => !product .Contains(c.Id));
            ViewBag.Categories = notIncluded;
            return View(PCAVM);
        }

        [HttpGet("category/{categoryId}")]
        public IActionResult CategoryInfo(int categoryId)
        {
            HttpContext.Session.SetString("Source", "Category");
            Category category = dbContext.Categories
                .Include(c => c.Products)
                .ThenInclude(a => a.Product)
                .FirstOrDefault(d => d.CategoryId == categoryId);
            
            ProdCatAssViewModel PCAVM = new ProdCatAssViewModel()
            {
                ACategory = category
            };

            List<Product> notIncluded = dbContext.Products.Where(p => !category.Products.Select(a => a.ProductId).Contains(p.ProductId)).ToList();
            ViewBag.Products = notIncluded;
            return View(PCAVM);
        }

        [HttpPost]
        public IActionResult AddOneToTheOther(ProdCatAssViewModel viewModel)
        {
            // not checking for validity since the selection is given
            dbContext.Add(viewModel.AAssociation);
            dbContext.SaveChanges();
            if (HttpContext.Session.GetString("Source") == "Product")
                return RedirectToAction("ProductInfo", new{productId=viewModel.AAssociation.ProductId});
            else if (HttpContext.Session.GetString("Source") == "Category")
                return RedirectToAction("CategoryInfo", new{categoryId = viewModel.AAssociation.CategoryId});
            else
                return View("Categories");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
