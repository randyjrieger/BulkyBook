using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class UserController : Controller
    {
        //using a different way than respositoy pattern...
        //this is directly using the DbContext

       // private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            //this will include company with the user, if FK is populated
            var userList = _db.ApplicationUsers.Include(u=>u.Company).ToList();
            //all roles
            var roles = _db.Roles.ToList();
            //all user roles
            var userRole = _db.UserRoles.ToList();

            foreach(var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(r => r.Id == roleId).Name;
                //if no company, then blank company name out
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        //it would throw an expection if null, unless we give a default
                        Name = ""
                    };
                }
            }

            return Json(new { data = userList });
        }

        #endregion
    }
}