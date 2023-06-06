using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using TedWeb.DataAccess.Repository;
using TedWeb.Model;
using TedWeb.Model.Models;

namespace TedWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            //List<Product>objProductList=_db.Categories.ToList();
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            //IEnumerable<SelectListItem> CategoryList=_unitOfWork.Category.GetAll().
            //    Select(u=>new SelectListItem
            //    {
            //        Text=u.Name,
            //        Value=u.Id.ToString(),

            //    })
            //    ;
            return View(objProductList);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().
                Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString(),

                });
            //ViewBag.CategoryList = CategoryList;
            ViewData["CategoryList"]= CategoryList;

            return View();
        }
        [HttpPost]
        public IActionResult Create(Product obj)
        {
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "Thse DisaplyOrder cannot exactly match the Name");
            //}
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "Thse DisaplyOrder cannot exactly match the Name");
            //}
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(obj);
                _unitOfWork.Save();
                //_db.SaveChanges();
                TempData["Success"] = "Product Created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            //Product ProductFromDb=_db.Categories.Find(id);
            Product productFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            //Product ProductFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Product ProductFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();


            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Product obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Update(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Product Updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product ProductFromDb = _unitOfWork.Product.Get(u => u.Id == id);
            if (ProductFromDb == null)
            {
                return NotFound();
            }
            return View(ProductFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Product obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Product Delete successfully";
            return RedirectToAction("Index");
        }
    }
}
