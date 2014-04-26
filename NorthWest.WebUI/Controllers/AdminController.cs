using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NorthWest.Domain.Abstract;
using NorthWest.Domain.Entities;

namespace NorthWest.WebUI.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        private IProductRepository repository;
        public AdminController(IProductRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Products);
        }

        public ViewResult Edit(int id)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                repository.SaveProduct(product);
                TempData["message"] = string.Format("{0} has been saved", product.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // there is something wrong with the data values
                return View(product);
            }
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ViewResult Details(int id)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == id);
            return View(product);
        }

        public ActionResult Delete(int id)
        {
            Product deletedProduct = repository.DeleteProduct(id);
            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("{0} was deleted",
                deletedProduct.Name);
            }
            return RedirectToAction("Index");
        }
    }
}
