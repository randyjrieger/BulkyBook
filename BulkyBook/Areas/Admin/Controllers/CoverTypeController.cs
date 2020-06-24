using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        public readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: CoverType
        public ActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType covertype = new CoverType();
            if (id == null)
            {
                //this is for create
                return View(covertype);
            }
            //this is for edit
            covertype = _unitOfWork.CoverType.Get(id.GetValueOrDefault());
            if (covertype == null)
            {
                return NotFound();
            }
            return View(covertype);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType covertype)
        {
            if (ModelState.IsValid)
            {
                if (covertype.Id == 0)
                {
                    _unitOfWork.CoverType.Add(covertype);

                }
                else
                {
                    _unitOfWork.CoverType.Update(covertype);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(covertype);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.CoverType.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.CoverType.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.CoverType.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}