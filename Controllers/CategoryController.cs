using Microsoft.AspNetCore.Mvc;

using TedWeb.Data;
using TedWeb.DataAccess.Repository;
using TedWeb.Model.Models;


namespace TedWeb.Controllers
{
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public IActionResult Index()
        {
            //List<Category>objCategoryList=_db.Categories.ToList();
            List<Category> objCategoryList = _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name==obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name","Thse DisaplyOrder cannot exactly match the Name");
            }
            //if (obj.Name == obj.DisplayOrder.ToString())
            //{
            //    ModelState.AddModelError("name", "Thse DisaplyOrder cannot exactly match the Name");
            //}
            if (ModelState.IsValid)
            {
                _categoryRepo.Add(obj);
                _categoryRepo.Save();
                //_db.SaveChanges();
                TempData["Success"]="Category Created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
   
        }

        public IActionResult Edit(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }
            //Category categoryFromDb=_db.Categories.Find(id);
            Category categoryFromDb = _categoryRepo.Get(u=>u.Id==id);
            //Category categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category categoryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();


            if (categoryFromDb==null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                _categoryRepo.Save();
                TempData["Success"] = "Category Updated successfully";
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
            Category categoryFromDb = _categoryRepo.Get(u => u.Id == id);
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            Category obj = _categoryRepo.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["Success"] = "Category Delete successfully";
            return RedirectToAction("Index");
        }

    }
}
