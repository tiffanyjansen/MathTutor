using MathCenter.DAL;
using MathCenter.Models;
using MathCenter.Models.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class StudentController : Controller
    {
        private readonly MathContext db = new MathContext(); //Access to Database
        private static int _week_number;
        private static string _v_number;

        [HttpPost]
        public ActionResult SignIn(string tutorPwd, int Week)
        {
            string tutorPass = "Math42";

            if (tutorPwd == tutorPass && Week != -1)
            {
                _week_number = Week;
                return RedirectToAction("Index", "Student");
            }
            else if (Week == -1)
            {
                TempData["Error"] = "Please select a week number.";
            }
            else
            {
                TempData["Error"] = "Incorrect password. Please try again.";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Index()
        {
            if(_week_number < 2 || _week_number > 11)
            {
                return RedirectToAction("Index", "Home");
            }
            _v_number = null; //reset VNumber.
            return View();
        }

        [HttpPost]
        public ActionResult Index(WelcomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _v_number = model.VNum;
                if (db.Students.Find(_v_number) != null && db.Students.Find(_v_number).SignIns.Count() > 0)
                {
                    return RedirectToAction("Done", "Student");
                }
                return RedirectToAction("Name", "Student");
            }
            else
            {
                ViewBag.Error = "Your V Number is invalid. It must be 8 characters. Please do not include the V.";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Name()
        {
            var ProgressPercent = 33;
            if(_v_number == null)
            {
                return RedirectToAction("Index", "Student");
            }
            Student student = db.Students.Find(_v_number);
            if (student == null)
            {
                student = new Student { VNum = _v_number };
            }
            else if (student.SignIns.Count() > 0)
            {
                ProgressPercent = 50;
            }
            ViewBag.ProgressPercent = ProgressPercent;
            return View(student);
        }

        [HttpPost]
        public ActionResult Name(Student student)
        {
            if (ModelState.IsValid)
            {
                var redirectAction = "Done";
                try
                {
                    Student currentStudent = db.Students.Find(_v_number);
                    if(currentStudent != null)
                    {
                        currentStudent = currentStudent.Update(student);
                    }
                    else
                    {
                        student = student.CapitalizeNames();
                        redirectAction = "Class";
                        db.Students.Add(student);
                    }

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error adding you to the database. Please ask a tutor for help.";
                    return View(student);
                }
                return RedirectToAction(redirectAction, "Student");
            }
            else
            {
                ViewBag.Error = "Looks like you inputted something that does not look like a name. Please try again.";
                return View(student);
            }
        }

        [HttpGet]
        public ActionResult Class()
        {
            var ProgressPercent = 66;
            Student student = db.Students.Find(_v_number);
            if(student == null)
            {
                return RedirectToAction("Index", "Student");
            }
            else if(student.StudentClasses.Count > 0)
            {
                ProgressPercent = 50;
            }
            ViewBag.ProgressPercent = ProgressPercent;
            ViewBag.OtherClasses = getOtherClasses();
            ViewBag.CCClasses = getCommunityCollegeClasses();
            ViewBag.CCClassList = getCommunityClassList();
            return View(getCommonClasses());
        }

        [HttpPost]
        public ActionResult Class(int[] ClassID)
        {
            Student currentStudent = db.Students.Find(_v_number);
            var redirectAction = "Finish";
            var ProgressPercent = 66;
            if (ClassID.Length > 0 && currentStudent != null)
            {
                if (currentStudent.StudentClasses.Count > 0)
                {
                    ProgressPercent = 50;
                    redirectAction = "Done";
                }
                createSignIns(ClassID, currentStudent);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction(redirectAction, "Student");
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error adding your classes. Please try again.";
                }
            }
            else if(ClassID.Length == 0)
            {
                ViewBag.Error = "Please select your class(es).";
            }

            ViewBag.ProgressPercent = ProgressPercent;
            ViewBag.OtherClasses = getOtherClasses();
            ViewBag.CCClasses = getCommunityCollegeClasses();
            ViewBag.CCClassList = getCommunityClassList();
            return View(getCommonClasses());
        }

        public ActionResult Finish()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Done()
        {
            Student student = db.Students.Find(_v_number);
            if(student == null)
            {
                return RedirectToAction("Index", "Student");
            }
            else if (student.FirstName == null)
            {
                return RedirectToAction("Name", "Student");
            }
            else if(student.StudentClasses.Count == 0)
            {
                return RedirectToAction("Class", "Student");
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult Done(int[] ClassID)
        {
            Student currentStudent = db.Students.Find(_v_number);
            if (ClassID.Length > 0 && currentStudent != null)
            {
                createSignIns(ClassID, currentStudent);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Finish", "Student");
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error. Please try again.";
                }
            }

            return View(currentStudent);
        }

        public ActionResult SignOut()
        {
            _week_number = 0;
            return RedirectToAction("Index", "Home");
        }

        public List<int> getStudentClasses()
        {
            List<int> list = new List<int>();

            Student student = db.Students.Find(_v_number);
            if (student != null && student.StudentClasses.Count > 0)
            {
                list = student.StudentClasses.Select(c => c.ClassID).ToList();
            }

            return list;
        }
        public List<Class> getOtherClasses()
        {
            List<int> sClasses = getStudentClasses();           
            return db.Classes.Where(c => c.Other != null).Where(c => !sClasses.Contains(c.ClassID)).Select(c => c).ToList();
        }

        public List<Class> getCommunityCollegeClasses()
        {
            List<int> sClasses = getStudentClasses();
            var CCColleges = Models.Class.CCCollegeStrings.Values;
            return db.Classes.Where(c => CCColleges.Contains(c.Instructor)).Where(c => !sClasses.Contains(c.ClassID)).Select(c => c).ToList();
        }

        public List<string> getCommunityClassList()
        {
            return db.Classes.Where(c => c.Other == null).Where(c => c.ClassNum < 300).Where(c => c.DeptPrefix == "MTH")
                .OrderBy(c => c.ClassNum)
                .GroupBy(c => c.ClassNum)
                .Select(c => ("MTH " + c.Key.Value.ToString()))
                .ToList();
        }

        public List<Class> getCommonClasses()
        {
            List<int> sClasses = getStudentClasses();
            var CCColleges = Models.Class.CCCollegeStrings.Values;
            return db.Classes.Where(c => c.Other == null).Where(c => !CCColleges.Contains(c.Instructor)).Where(c => !sClasses.Contains(c.ClassID)).Select(c => c).ToList();
        }

        public void createSignIns(int[] ClassID, Student student)
        {
            foreach (int Id in ClassID)
            {
                StudentClass studentClass = db.StudentClasses.Where(c => c.VNum == student.VNum).Where(c => c.ClassID == Id).FirstOrDefault();
                if(studentClass == null)
                {
                    db.StudentClasses.Add(new StudentClass
                    {
                        VNum = student.VNum,
                        ClassID = Id
                    });
                }
                db.SignIns.Add(new SignIn
                {
                    Week = _week_number,
                    Date = DateTime.Today,
                    Hour = DateTime.Now.Hour,
                    Min = DateTime.Now.Minute,
                    StudentID = student.VNum,
                    ClassID = Id
                });
            }
        }
    }
}  