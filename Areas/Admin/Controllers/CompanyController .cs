using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using System.Data;

using TedWeb.Utility;
using TedWeb.DataAccess.Repository.IRepository;
using TedWeb.Model;
using TedWeb.Model.Models;
using TedWeb.Model.ViewModels;

namespace TedWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {

            if (id==null || id==0)
            {
                //Create
                return View(new Company());
            }
            else
            {
                //update
                Company companyobj=_unitOfWork.Company.Get(u=>u.Id==id);
                return View(companyobj);
            }

           
        }
        [HttpPost]
        public IActionResult Upsert(Company Companyobj)
        {
  ModelState.AddModelError("name", "Thse DisaplyOrder cannot exactly match the Name");
            //}
            if (ModelState.IsValid)
            {
                if (Companyobj.Id==0)
                {
                    _unitOfWork.Company.Add(Companyobj);
                }
                else
                {
                    _unitOfWork.Company.Update(Companyobj);
                }

           
                _unitOfWork.Save();
                //_db.SaveChanges();
                TempData["Success"] = "Company Created successfully";
                return RedirectToAction("Index");
            }
            else
            {
                return View(Companyobj);
            }
        

        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    //Company CompanyFromDb=_db.Categories.Find(id);
        //    Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    //Company CompanyFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //    //Company CompanyFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();


        //    if (CompanyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Company obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Company.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["Success"] = "Company Updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View(obj);

        //}
        //public IActionResult Delete(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Company CompanyFromDb = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (CompanyFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(CompanyFromDb);
        //}
        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePost(int? id)
        //{
        //    Company obj = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    _unitOfWork.Company.Remove(obj);
        //    _unitOfWork.Save();
        //    TempData["Success"] = "Company Delete successfully";
        //    return RedirectToAction("Index");
        //}

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { objCompanyList });

        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted=_unitOfWork.Company.Get(u=>u.Id==id);
            if (CompanyToBeDeleted == null)
            {
                return Json(new {success=false,message="Error while deleting"});
            }



            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });

        }

        #endregion
    }
}
