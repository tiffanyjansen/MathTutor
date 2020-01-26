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
                ViewBag.Error = "Please select a week number.";
            }
            else
            {
                ViewBag.Error = "Incorrect password. Please try again.";
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(WelcomeViewModel model)
        {
            if (ModelState.IsValid)
            {
                _v_number = model.VNum;
                if (db.Students.Find(_v_number) != null)
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
            Student student = db.Students.Find(_v_number);
            if (student == null)
            {
                student = new Student { VNum = _v_number };
            }
            return View(student);
        }

        [HttpPost]
        public ActionResult Name(Student student)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    student.FirstName = student.CapitalizeName(student.FirstName);
                    student.LastName = student.CapitalizeName(student.LastName);
                    db.Students.Add(student);

                    db.SaveChanges();
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error adding you to the database. Please ask a tutor for help.";
                    return View(student);
                }
                return RedirectToAction("Class", "Student");
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
            ViewBag.OtherClasses = getOtherClasses();
            ViewBag.CCClasses = getCommunityCollegeClasses();
            ViewBag.CCClassList = getCommunityClassList();
            return View(getCommonClasses());
        }

        [HttpPost]
        public ActionResult Class(int[] ClassID)
        {
            Student currentStudent = db.Students.Find(_v_number);
            if(ClassID.Length > 0 && currentStudent != null)
            {
                createSignIns(ClassID, currentStudent);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Finish", "Student");
                }
                catch (Exception)
                {
                    ViewBag.Error = "There was an error adding your classes. Please try again.";
                }
            }

            ViewBag.OtherClasses = getOtherClasses();
            ViewBag.CCClasses = getCommunityCollegeClasses();
            ViewBag.CCClassList = getCommunityClassList();
            return View(getCommonClasses());
        }

        public ActionResult Finish()
        {
            _v_number = null; //reset VNumber so if something weird happens we don't sign someone in twice.
            return View();
        }

        [HttpGet]
        public ActionResult Done()
        {
            Student student = db.Students.Find(_v_number);
            if(student.FirstName == null)
            {
                return RedirectToAction("Name", "Student");
            }
            if(student.StudentClasses.Count == 0)
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

        public List<Class> getOtherClasses()
        {
            return db.Classes.Where(c => c.Other != null).Select(c => c).ToList();
        }

        public List<Class> getCommunityCollegeClasses()
        {
            var CCColleges = Models.Class.CCCollegeStrings.Values;
            return db.Classes.Where(c => CCColleges.Contains(c.Instructor)).Select(c => c).ToList();
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
            var CCColleges = Models.Class.CCCollegeStrings.Values;
            return db.Classes.Where(c => c.Other == null).Where(c => !CCColleges.Contains(c.Instructor)).Select(c => c).ToList();
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