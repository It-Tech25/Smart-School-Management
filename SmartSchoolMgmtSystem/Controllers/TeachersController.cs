
using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 
﻿using Microsoft.AspNetCore.Mvc; 
using SmartSchool.Models.DTO;
using SmartSchool.Service;
using Microsoft.AspNetCore.Authorization;

namespace SmartSchool.Controllers
{
    public class TeachersController : Controller
    {
        private readonly ITeachersServices _service; 
        private readonly MyDbContext _context;

        public TeachersController(ITeachersServices service, MyDbContext context)
        {
            _service = service;
            _context = context;
        }

        [Authorize(Policy = "School Admin")]
        public async Task<ActionResult> TeachersIndex()
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var school = _context.schools
    .Where(a => a.userid == loggedInUser.userId && a.IsDeleted == false)
    .FirstOrDefault();

            ViewBag.SchoolLogo = school.Logo;
            int id = _context.userTypeEntites.Where(a => a.CreatedBy == loggedInUser.userId && a.UserTypeName == "Teacher").Select(a => a.UserTypeId).FirstOrDefault();
            ViewBag.Users = _context.userEntity.Where(u => u.IsDeleted == false && u.UserTypeId == id && u.CreatedBy == loggedInUser.userId).ToList();
            ViewBag.Subjects = _context.subjectEntity.Where(s => s.IsDeleted == false && s.CreatedBy == loggedInUser.userId).ToList();
            ViewBag.Classes = _context.classEntity.Where(c => c.IsDeleted == false && c.CreatedBy == loggedInUser.userId).ToList();
            var schools = _context.schools.FirstOrDefault(s => s.userid == loggedInUser.userId && s.IsDeleted == false);

            ViewBag.SchoolName = schools.Name;
            var teachers = _service.GetAllAsync(loggedInUser.userId);
            return View(teachers);

        }
        [HttpGet]
        public IActionResult GetClassesById(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var duration = _service.GetDurations(loggedInUser.userId);
            var result = _service.GetClassesById(loggedInUser.userId);
            return Json(new { entries = result, durations = duration });
        }
     


        [HttpPost]    
        [ValidateAntiForgeryToken]
        public IActionResult Create(TeachersDto dto)
        {
            GenericResponse response = new GenericResponse();
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            response = _service.AddAsync(dto, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            var teacher = await _service.GetByIdAsync(id);
            if (teacher == null)
                return NotFound();

            return View(TeachersIndex);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            return View();
        }

        public async Task<ActionResult> Edit(int id)
        {
            var teacher = await _service.GetByIdAsync(id);
            if (teacher == null)
                return NotFound();


            return View(TeachersIndex);
        }

        // POST: Teachers/Edit/5 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(TeachersDto dto)
        { 
            GenericResponse response = new GenericResponse();
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            response = _service.UpdateAsync(dto, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            }
        }



        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            GenericResponse response = new GenericResponse();
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            
            response = _service.DeleteAsync(id, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false });
            } 
            
        }

       

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction("TeachersIndex"); 
        }
        public IActionResult LoadTeacherList()
        {
            GenericResponse response = new GenericResponse();
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var teachers = _service.GetAllAsync(loggedInUser.userId);

            return PartialView("_TeacherList", teachers);
        }

    }
}
