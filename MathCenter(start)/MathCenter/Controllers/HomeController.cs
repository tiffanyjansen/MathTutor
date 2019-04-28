﻿using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class HomeController : Controller
    {
        //Access to Database.
        private readonly MathContext db = new MathContext();

        /*
         * The "Home Page." The page for Tutors/Faculty to either access the 
         * sign in sheet or the data. (Depending on which you are.) It will 
         * check passwords and return the view necessary for who signed in.
         */
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string button, string tutorPwd, string facultyPwd, int? Week)
        {
            //Hard code passwords since the outside file was being dumb.
            string tutorPass = "Math42";
            string facultyPass = "Math42";

            //Check which button was pressed.
            if (button == "tutor")
            {
                //Check the password and make sure there is a week input.
                if (tutorPwd == tutorPass && Week != -1)
                {
                    return RedirectToAction("Welcome", new { Week });
                }
                //Return specific errors if the input is not valid.
                else if (tutorPwd != tutorPass)
                {
                    ViewBag.Error = "You typed in the wrong password. Please Try Again.";
                    return View();
                }
                else
                {
                    ViewBag.Error = "You did not select a week, please select a week number.";
                    return View();
                }
            }
            else
            {
                //Check the password.
                if (facultyPwd == facultyPass)
                {
                    return RedirectToAction("Index", "Faculty");
                }
                //Return specific errors if the input is not valid.
                else
                {
                    ViewBag.Error = "Please Try Again. You typed in the wrong password.";
                    return View();
                }
            }
        }

        /*
        * The method for signing a student in. This page will just have the student
        * enter their V Number, then it will direct them to select their class and 
        * add them to the database.
        */
        [HttpGet]
        public ActionResult Welcome(int Week)
        {
            ViewBag.Num = Week;
            return View();
        }
        [HttpPost]
        public ActionResult Welcome(string VNum, int Week)
        {
            //If no input, refresh page.
            if (VNum == null || VNum.Length != 8)
            {
                ViewBag.Error = "Your V Number is invalid. It must be 8 characters. Please do not include the V.";
                ViewBag.Num = Week;
                return View();
            }

            //If student is in the database, redirect to done page.
            if (db.Students.Find(VNum) != null)
            {
                return RedirectToAction("Done", new { VNum, Week });
            }
            //Otherwise, redirect to create student page.            
            return RedirectToAction("Name", new { VNum, Week });
        }

        /*
        * This is the method for inputting their name. It will allow the user
        * to input their name and get to adding their class.
        * 
        * It will also add the student to the database.
        */
        [HttpGet]
        public ActionResult Name(string VNum, int Week)
        {
            //Use the VNum and Week in the View.
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            //Return the View
            return View();
        }
        [HttpPost]
        public ActionResult Name(PersonWeek pWeek)
        {
            //Create empty student to be used later.
            Student student = null;

            //Check if Student is already in DB.
            if (db.Students.Find(pWeek.VNum) != null)
            {
                student = db.Students.Find(pWeek.VNum);
                student.FirstName = pWeek.FirstName;
                student.LastName = pWeek.LastName;
            }
            //Create a student and set the class to the "placeholder class" created in the up script.
            else
            {
                //Add the student to the database.
                student = new Student { VNum = pWeek.VNum, FirstName = pWeek.FirstName, LastName = pWeek.LastName };
                db.Students.Add(student);
            }

            try
            {
                //Save the changes to the database.            
                db.SaveChanges();
            }
            catch (Exception)
            {
                ViewBag.Error = "There was an error with the database. Please try again.";
                return View(pWeek);
            }

            //Redirect to the select department method and slowly select the class.
            return RedirectToAction("SelectClass", new { pWeek.Week, pWeek.VNum });
        }

        /*
         * This is the method for selecting the class. It will use JavaScript to do most of the functionality.
         */
        [HttpGet]
        public ActionResult SelectClass(int? Week, string VNum)
        {
            //Check for no input. This just adds extra error-handling.
            if (Week == null || VNum == null)
            {
                if (Week == null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Welcome", new { Week });
                }
            }

            //Find all of the distict Class Prefixes and use that for the drop down.
            var ClassDepts = db.Classes
                .GroupBy(c => c.DeptPrefix)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the classes with Other having something in it.
            var remClasses = ClassDepts
                .Where(c => c.Other != null)
                .Select(c => c).ToList();
            foreach (var remClass in remClasses)
            {
                ClassDepts.Remove(remClass);
            }

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = VNum;
            ViewBag.Week = Week;

            //Return the View so students can select their DeptPrefix.
            return View(ClassDepts);
        }
        [HttpPost]
        public ActionResult SelectClass(int? ClassID, int Week, string VNum, string button)
        {
            if(button == "Other")
            {
                return RedirectToAction("Other", new { VNum, Week });
            }
            else
            {
                if (ClassID == null)
                {
                    //Find all of the distict Class Prefixes and use that for the drop down.
                    var ClassDepts = db.Classes
                        .GroupBy(c => c.DeptPrefix)
                        .Select(c => c.FirstOrDefault())
                        .ToList();

                    //Remove the classes with Other having something in it.
                    var remClasses = ClassDepts
                        .Where(c => c.Other != null)
                        .Select(c => c).ToList();
                    foreach (var remClass in remClasses)
                    {
                        ClassDepts.Remove(remClass);
                    }
                    ViewBag.Id = VNum;
                    ViewBag.Week = Week;
                    return View(ClassDepts);
                }
                else
                {
                    Student currentStudent = db.Students.Find(VNum);
                    currentStudent.Classes.Add(db.Classes.Find(ClassID));
                    try { 
                        db.SaveChanges();
                    }
                    catch (Exception)
                    {
                        //There was an error.
                    }
                    return RedirectToAction("More", new { VNum, Week });
                }
            }           
        }
        [HttpGet]
        public ActionResult More(string VNum, int Week)
        {
            ViewBag.Id = VNum;
            ViewBag.Week = Week;
            return View();
        }
        [HttpPost]
        public ActionResult More(string VNum, int Week, bool addClass)
        {
            if (addClass)
            {
                return RedirectToAction("SelectClass", new { VNum, Week });
            }
            else
            {
                //Create and add the SignIn to the database
                try { 
                    db.SignIns.Add(new SignIn { Week = Week, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNum });
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error
                }
                return RedirectToAction("Finish", new { Week });
            }            
        }
        /*
         * The method for when the student selects Other.
         */
        [HttpGet]
        public ActionResult Other(int Week, string VNum)
        {
            //Keep the data floating.
            ViewBag.Week = Week;
            ViewBag.Id = VNum;

            //Return the View.
            return View();
        }
        [HttpPost]
        public ActionResult Other(int Week, string VNum, string other)
        {

            try { 
                //Create the class to be connected to the student.
                db.Classes.Add(new Class { Other = other });

                //Save the class into the database.
                db.SaveChanges();
            }
            catch (Exception)
            {
                //There was an error.
            }

            //Get the class again.
            Class sClass = db.Classes
                .Where(c => c.Other == other)
                .Select(c => c).FirstOrDefault();

            //Add the class to the current student.
            Student currentStudent = db.Students.Find(VNum);
            currentStudent.Classes.Add(sClass);

            try
            {
                //Create the Sign In to be added to the db.
                db.SignIns.Add(new SignIn { Week = Week, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNum });

                //Save the Changes to the db.
                db.SaveChanges();
            }
            catch(Exception)
            {
                //There was an error.
            }

            //Redirect to the finish page.
            return RedirectToAction("More", new { VNum, Week });
        }
        /*
         * The method for when you are already in the DB and just need to approve the sign in.
         */
        [HttpGet]
        public ActionResult Done(string VNum, int Week)
        {
            //Get the info from the Database about the current student.
            Student currentStudent = db.Students.Find(VNum);

            //Keep the data floating
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            //Return the View with the current student.
            return View(currentStudent);
        }
        [HttpPost]
        public ActionResult Done(string VNum, int Week, int approved)
        {
            if (approved == 1)
            {
                try
                {
                    //Create the Sign In and add it to the database.
                    db.SignIns.Add(new SignIn { Week = Week, Date = DateTime.Today, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, StudentID = VNum });
                    db.SaveChanges();
                }
                catch (Exception)
                {
                    //There was an error.
                }                

                //Redirect to the "finish" page.
                return RedirectToAction("Finish", new { Week });
            }
            else if (approved == 0)
            {
                //If it's not you redirect to Sign In page.
                return RedirectToAction("Name", new { Week, VNum });
            }
            else
            {
                //Redirect back to the page to input your V-Number
                return RedirectToAction("Welcome", new { Week });
            }
        }
        /*
         * The last page that gives a good message.
         */
        [HttpGet]
        public ActionResult Finish(int Week)
        {
            //The Week Continues to float.
            ViewBag.Week = Week;

            //Return the View.
            return View();
        }
    }
}