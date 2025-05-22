using SmartSchool.Models.Entity;
using SmartSchool.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchool.BAL;
using SmartSchool.Models.DTO;
using Microsoft.AspNetCore.Authorization;
namespace SmartSchool.Controllers
{
    public class AdmissionsController : Controller
    {
        private readonly IAdmissionsServices _AdmissionServices;
        private readonly IClassesService _ClassesService;
        private readonly MyDbContext _context;
        public AdmissionsController(IAdmissionsServices AdmissionsServices, IClassesService classesService, MyDbContext context)
        {
            _AdmissionServices = AdmissionsServices;
            _ClassesService = classesService;
            _context = context;
        }
        private string GenerateReceiptNumber()
        {
            return "RCPT-" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }


        [Authorize(Policy = "School Admin")]
        public IActionResult GetAdmissions(string? state)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            var states = _AdmissionServices.GetStates();
            ViewBag.State = states;
            var cities = _AdmissionServices.Getcity(state);
            ViewBag.city = cities;
            
            if (loggedInUser.userTypeName == "School Admin")
            {
                var school = _context.schools.FirstOrDefault(s => s.userid == loggedInUser.userId && s.IsDeleted == false);
                ViewBag.SchoolName = school;
                var Classes = _context.classEntity.Where(c => c.IsDeleted == false && c.CreatedBy == loggedInUser.userId).ToList();
                ViewBag.Classes = Classes;
            }
            else
            {
                int? id = _context.userEntity.Where(s => s.UserId == loggedInUser.userId && s.IsDeleted == false).Select(a => a.CreatedBy).FirstOrDefault();
                var school = _context.schools.Where(s => s.userid == id && s.IsDeleted == false).Select(a => a.Name).FirstOrDefault();
                ViewBag.SchoolName = school;
                var Classes = _context.classEntity.Where(c => c.IsDeleted == false && c.CreatedBy == id).ToList();
                ViewBag.Classes = Classes;
            }

                var res = _AdmissionServices.GetAllAdmissions();
            return View(res);
        }

        // Add a new Admissions
        [HttpPost]
        public IActionResult AddAdmissions(IFormCollection form, IFormFile Photo)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            var admissions = new AdmissionsDto
            {
                StudentName = form["StudentName"],
                School = form["School"],
                ClassId = Convert.ToInt32(form["ClassId"]),
                AdmissionFee = Convert.ToInt32(form["AdmissionFee"]),
                AdharcardNo = form["AdharcardNo"],
                Address = form["Address"],
                DOB = Convert.ToDateTime(form["DOB"]),
                Gender = form["Gender"],
                TotalFee =Convert.ToDecimal(form["TotalFee"]),



            };

            if (Photo != null && Photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(Photo.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    Photo.CopyToAsync(stream);
                }

                admissions.Photo = "/uploads/" + fileName;
            }

            GenericResponse response = _AdmissionServices.AddAdmission(admissions, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }

            else
            {
                return RedirectToAction("GetAdmissions");
            }
        }

        // Update Admissions
        [HttpPost]
        public IActionResult UpdateAdmissions(AdmissionsDto obj)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _AdmissionServices.UpdateAdmissions(obj, loggedInUser.userId);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetAdmissions");
            }
        }

        // Delete Admissions
        [HttpPost]
        public IActionResult DeleteAdmissions(int id)
        {
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }

            GenericResponse response = _AdmissionServices.DeleteAdmissions(id);
            if (response.statuCode == 1)
            {
                return Json(new { success = true });
            }
            else
            {
                return RedirectToAction("GetAdmissions");
            }
        }

        [HttpGet]
        public JsonResult GetCitiesByState(string state)
        {
            var cities = _AdmissionServices.Getcity(state);

            return Json(cities);
        }

        //        [HttpPost]
        //        public IActionResult AddAdmissionsToUsers([FromBody] AdmissionIdListDto dto)
        //        {
        //            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
        //            if (loggedInUser == null)
        //            {
        //                return RedirectToAction("Login", "Authenticate");
        //            }
        //           int utype = _context.userTypeEntites.Where(a => a.IsDeleted == false &&a.UserTypeName=="Student"&& a.CreatedBy == loggedInUser.userId).Select(a=>a.UserTypeId).FirstOrDefault();
        //            try
        //            {
        //                foreach (var id in dto.Admissions)
        //                {
        //                    var admission = _context.admissionsEntity.FirstOrDefault(a => a.AdmissionId == id &&a.CreatedBy==loggedInUser.userId);
        //                    if (admission != null)
        //                    {
        //                        StudentEntity newUser = new StudentEntity
        //                        {
        //                            StudentName = admission.StudentName,
        //                            UserTypeId= utype,
        //                            SchoolId = admission.SchoolId,
        //                            ClassID = admission.ClassId,
        //                            TotalFee = admission.TotalFee,
        //                           PaiedFee = admission.AdmissionFee,
        //                           // AcademicYear = DateTime.Now.Year.ToString();
        //                             AdharcardNo = admission.AdharcardNo,
        //                            Photo = admission.Photo,

        //                             IsDeleted = false,
        //                             IsAdded = true,
        //                            CreatedOn = DateTime.Now,
        //                             CreatedBy =loggedInUser.userId,

        //                        };

        //                        _context.studentEntity.Add(newUser);
        //                    }
        //                    admission.IsAdded = true;
        //                    _context.admissionsEntity.Update(admission);

        //                }

        //                _context.SaveChanges();
        //                return Json(new { success = true });
        //            }
        //            catch
        //            {
        //                return Json(new { success = false });
        //            }
        //        }

        //    }
        //}

        [HttpPost]
        public IActionResult AddAdmissionsToUsers([FromBody] AdmissionIdListDto dto)
        {
            int utype = 0;
            var loggedInUser = SessionHelper.GetObjectFromJson<LoginResponse>(HttpContext.Session, "loggedUser");
            if (loggedInUser == null)
            {
                return RedirectToAction("Login", "Authenticate");
            }
            if (loggedInUser.userTypeName == "School Admin")
            {
             
                 utype = _context.userTypeEntites.Where(a => a.IsDeleted == false && a.UserTypeName == "Student" && a.CreatedBy == loggedInUser.userId).Select(a => a.UserTypeId).FirstOrDefault();
            }
            else
            {
                int? id = _context.userEntity.Where(s => s.UserId == loggedInUser.userId && s.IsDeleted == false).Select(a => a.CreatedBy).FirstOrDefault();
                
                utype = _context.userTypeEntites.Where(a => a.IsDeleted == false && a.UserTypeName == "Student" && a.CreatedBy == id).Select(a => a.UserTypeId).FirstOrDefault();
            }

            


            try
            {
                foreach (var id in dto.Admissions)
                {
                    var admission = _context.admissionsEntity
                        .FirstOrDefault(a => a.AdmissionId == id && a.CreatedBy == loggedInUser.userId);
                    if (utype == 0)
                        return Json(new { success = false, error = "Student UserTypeId not found." });

                    if (admission == null)
                        return Json(new { success = false, error = $"Admission not found for id {id}." });
                    int Studentid = _context.studentEntity.OrderByDescending(a => a.StudentId).Select(a => a.StudentId).FirstOrDefault();



                    if (admission != null)
                    {
                        // Create and save StudentEntity
                        StudentEntity newUser = new StudentEntity
                        {
                           StudentId = Studentid + 1,
                            StudentName = admission.StudentName,
                            UserTypeId = utype,
                            SchoolId = admission.SchoolId,
                            ClassID = admission.ClassId,
                            TotalFee = admission.TotalFee,
                            PaiedFee = admission.AdmissionFee,
                            AdharcardNo = admission.AdharcardNo,
                            Address = admission.Address,
                            DOB  = admission.DOB,
                            Gender = admission.Gender,
                            Photo = admission.Photo,
                            IsDeleted = false,
                            IsAdded = true,
                            CreatedOn = DateTime.Now,
                            CreatedBy = loggedInUser.userId
                        };

                        _context.studentEntity.Add(newUser);
                        _context.SaveChanges(); // Save to get StudentId
                        int PaymentId = _context.feePaymentEntity.OrderByDescending(a => a.PaymentId).Select(a => a.PaymentId).FirstOrDefault();
                        // Create and save FeePaymentsEntity
                        FeePaymentsEntity fee = new FeePaymentsEntity
                        {
                            PaymentId = PaymentId+1,
                            StudentId = newUser.StudentId,
                            ItemId = 1,
                            PaymentDate = DateTime.Now,
                            Amount = admission.AdmissionFee,
                            ReceiptNumber = GenerateReceiptNumber(),
                            IsDeleted = false,
                            CreatedOn = DateTime.Now,
                            CreatedBy = loggedInUser.userId
                        };

                        _context.feePaymentEntity.Add(fee);

                        // Mark admission as added
                        admission.IsAdded = true;
                        _context.admissionsEntity.Update(admission);

                        _context.SaveChanges(); 
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.InnerException?.Message ?? ex.Message });
            }

        }
    }
    } 

