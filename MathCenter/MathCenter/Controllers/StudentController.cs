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
                return RedirectToAction("SelectClass", "Student");
            }
            else
            {
                ViewBag.Error = "Looks like you inputted something that does not look like a name. Please try again.";
                return View(student);
            }
        }
    }
}  