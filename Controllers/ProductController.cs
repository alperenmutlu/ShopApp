using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using shopapp.webui.Data;
using shopapp.webui.Models;


namespace shopapp.webui.Controllers
{
    public class ProductController:Controller
    {
        public IActionResult Index()
        {
            // Viewbag
            //Models
            // ViewData

            var product = new Product{Name ="Iphone X", Price=600,Description="Güzel Telefon"};

           // ViewData["Product"] = product;
           // ViewData["Category"] = "Telefonlar";

            //ViewBag.Product
            // ViewBag.Category = "Telefonlar";
            // ViewBag.Product = product;

            //Model olarak
            ViewBag.Category = "Telefonlar";
            return View(product);
        }

        //localhost:5000/product/list
        // product/list => Tüm ürünleri(sayfalama)
        //product/list/2 => 2 numaralı kategoriye ait ürünler
         public IActionResult List(int? id,string q,double? min_price,double? max_price)
        {
            // {controller}/{action}/{id?}
            //product/kist/3
            // RouteData.Values["controller"] => product
            // RouteData.Values["action"] => list
            // RouteData.Values["id"] => 3

            // Console.WriteLine(RouteData.Values["controller"]);
            // Console.WriteLine(RouteData.Values["action"]);
            // Console.WriteLine(RouteData.Values["id"]);

            // QUERY STRING
            // Console.WriteLine(q);
            // Console.WriteLine(HttpContext.Request.Query["q"].ToString());
            // Console.WriteLine(HttpContext.Request.Query["min_price"].ToString());

            var products = ProductRepository.Products;
        
            if(id!=null)
            {
                products = products.Where(p=>p.CategoryId==id).ToList();
            }

            if(!string.IsNullOrEmpty(q))
            {
                products = products.Where(i=>i.Name.ToLower().Contains(q.ToLower()) || i.Description.Contains(q)).ToList();
            }

            var productViewModel = new ProductViewModel()
            {
                Products = products
            };

            return View(productViewModel);
        }

         //localhost:5000/product/details/2
        [HttpGet]
        public IActionResult Details(int id)
        {
            //name: "samsunf s6"
            //price : 3000
            //description: "iyi telefon"

            // ViewBag.Name = "Samsung s6";
            // ViewBag.Price = 3000;
            // ViewBag.Description = "İyi telefon";

            return View(ProductRepository.GetProductById(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Product p)
        {
            ProductRepository.AddProduct(p);
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {   
            ViewBag.Categories = new SelectList(CategoryRepository.Categories,"CategoryId","Name");

            return View(ProductRepository.GetProductById(id));
        }

        [HttpPost]
        public IActionResult Edit(Product p )
        {
            ProductRepository.EditProduct(p);
            return RedirectToAction("List");
        }
    }
}