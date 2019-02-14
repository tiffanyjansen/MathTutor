using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class HomeController : Controller
    {
        //Access to Database.
        MathContext db = new MathContext();


        /*
         * The "Home Page." The page for Tutors/Faculty to either access the 
         * sign in sheet or the data. (Depending on which you are.)
         */
        public ActionResult Index()
        {
            return View();
        }

        /*
         * The method for signing a student in. This page will just have the student
         * enter their V Number, then it will direct them to select their class and 
         * add them to the database.
         */
        [HttpGet]
        public ActionResult SignIn(string VNum)
        {
            //If no input, refresh page.
            if (VNum == null)
            {
                return View();
            }                  
            //If student is in the database, redirect to done page.
            if (db.Students.Find(VNum) != null)
            {
                return RedirectToAction("Done");
            }
            //Otherwise, redirect to create student page.
            return RedirectToAction("CreateStudent", new { Id = VNum });
        }

        public ActionResult Done()
        {

            return View();
        }

        /*
         * This method will finish the "creation" process of a student.
         */
        public ActionResult CreateStudent(string Id)
        {
            ViewBag.Id = Id;

            return View();
        }

        /*
         * This method will continue the "creation" process of a student.
         */
         [HttpPost]
         public ActionResult CreateStudent(string Id, string FirstName, string LastName)
        {
            Person person = new Person { VNum = Id, FirstName = FirstName, LastName = LastName };
            Debug.WriteLine(person.FirstName + ", " + person.LastName + ", " + person.VNum);
            if (person != null)
            {
                return RedirectToAction("SelectClass", new { Adult = person });
            }
            return View();
        }

        /*
         * This method will allow the student to select their class
         */
         [HttpGet]
         public ActionResult SelectClass(Person Adult)
        {
            List<Class> distictDept = db.Classes
                .GroupBy(c => c.DeptPrefix)
                .Select(d => d.FirstOrDefault())
                .ToList();

            Debug.WriteLine(distictDept.FirstOrDefault().ClassNum);

            return View(distictDept);
        }

        /*
         * This takes the input of the user (the department prefix) and uses that to go on to decide the class number.
         */
        [HttpPost]
        public ActionResult SelectClass(string classDept)
        {
            if(classDept == "other")
            {
                return RedirectToAction("Other");
            }

            Debug.WriteLine(db.Classes.Select(c => c.DeptPrefix).First() == classDept);

            List<Class> DistictNums = db.Classes
                .Where(c => classDept == c.DeptPrefix)
                .ToList();
                //.GroupBy(c => c.ClassNum)
                //.Select(n => n.FirstOrDefault())
                //.ToList();

            Debug.WriteLine(DistictNums.First().ClassNum);
            //Debug.WriteLine(DistictNums.Last().ClassNum);

            return RedirectToAction("SelectClassNum", new { NumList = DistictNums });
        }

        public ActionResult SelectClassNum(List<Class> NumList)
        {            
            return View(NumList);
        }

        /*
         * Allows user to type in their class number/class in general.
         */
        public ActionResult Other(Person Adult, string deptPrefix)
        {

            ViewBag.Person = Adult;
            if(deptPrefix != null)
            {
                ViewBag.Prefix = deptPrefix;
            }
            ViewBag.Prefix = "";

            return View();
        }

    }
}