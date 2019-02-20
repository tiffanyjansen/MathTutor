﻿using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            //Get the passwords from the outside file so the passwords are not hard-coded.
            //Hard code passwords since the outside file was being dumb.
            string tutorPass = "Math42";
                //System.Configuration.ConfigurationManager.AppSettings["TutorPass"];
            string facultyPass = "Math42";
                //System.Configuration.ConfigurationManager.AppSettings["FacultyPass"];

            //Check which button was pressed.
            if (button == "tutor")
            {
                //Check the password and make sure there is a week input.
                if (tutorPwd == tutorPass && Week != -1)
                {
                    return RedirectToAction("SignIn", new {Week});
                }
                //Return specific errors if the input is not valid.
                else if(tutorPwd != tutorPass)
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
        public ActionResult SignIn(int Week)
        {
            ViewBag.Num = Week;
            return View();
        }             
        [HttpPost]
        public ActionResult SignIn(string VNum, int Week)
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
            return RedirectToAction("NameInput", new { VNum, Week });
        }

        /*
        * This is the method for inputting their name. It will allow the user
        * to input their name and get to adding their class.
        * 
        * It will also add the student to the database and provide a placeholder
        * class so we don't have to carry around all the information.
        */
        [HttpGet]
        public ActionResult NameInput(string VNum, int Week)
        {
            //Use the VNum and Week in the View.
            ViewBag.VNum = VNum;
            ViewBag.Week = Week;

            //Return the View
            return View();
        }
        [HttpPost]
        public ActionResult NameInput(PersonWeek pWeek)
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
                student = new Student { VNum = pWeek.VNum, FirstName = pWeek.FirstName, LastName = pWeek.LastName, Class = -1 };
                db.Students.Add(student);
            }

            //Save the changes to the database.            
            db.SaveChanges();
            
            //Redirect to the select department method and slowly select the class.
            return RedirectToAction("ClassDept", new { NumWeek = pWeek.Week, Id = student.VNum});
        }

        /*
         * This is the method for selecting the class prefix for the class the
         * user is taking. (ex: MTH, FYS, PSY, etc.) It will also have an "other"
         * category for people not in any of the available classes.
         */
         [HttpGet]
         public ActionResult ClassDept(int NumWeek, string Id )
        {
            //Find all of the distict Class Prefixes and use that for the drop down.
            var ClassDepts = db.Classes
                .GroupBy(c => c.DeptPrefix)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the placeholder class from the list of options.
            var remClass = ClassDepts.Where(c => c.DeptPrefix == "NO").Select(c => c).FirstOrDefault();
            ClassDepts.Remove(remClass);

            //Remove the 'other' classes from the list of options.
            var remClass2 = ClassDepts.Where(c => c.DeptPrefix == null).Select(c => c).FirstOrDefault();
            ClassDepts.Remove(remClass2);

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = Id;
            ViewBag.NumWeek = NumWeek;

            //Return the View so students can select their DeptPrefix.
            return View(ClassDepts);
        }
        [HttpPost]
        public ActionResult ClassDept(string dept, int NumWeek, string Id)
        {
            //Check if the dept selected was other.
            if (dept == "other"){
                //Redirect to the Page to Type in the "other" info.
                return RedirectToAction("Other", new { WeekNum = NumWeek, Id });
            }
            else
            {
                //Redirect to the Page to select the Class Number
                return RedirectToAction("ClassNum", new { WeekNum = NumWeek, Id, Dept = dept });
            }
        }

        /*
         * This is the method for selecting the class number for the class the
         * user is taking. (ex: 111, 102, 344, etc.) It will also have an "other"
         * category for people not in any of the available classes.
         */
        [HttpGet]
        public ActionResult ClassNum(int WeekNum, string Id, string Dept)
        {
            //Find all of the distict Class Numbers in relation to the Prefix Given and use that for the drop down.
            var ClassNums = db.Classes
                .Where(c => c.DeptPrefix == Dept)
                .GroupBy(c => c.ClassNum)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the placeholder class from the list of options.
            var remClass = ClassNums.Where(c => c.DeptPrefix == "NO").Select(c => c).FirstOrDefault();
            ClassNums.Remove(remClass);

            //Remove the 'other' classes from the list of options.
            var remClass2 = ClassNums.Where(c => c.DeptPrefix == null).Select(c => c).FirstOrDefault();
            ClassNums.Remove(remClass2);

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = Id;
            ViewBag.WeekNum = WeekNum;
            ViewBag.Dept = Dept;

            // Return the View so students can select their Class Number.
            return View(ClassNums);
        }
        [HttpPost]
        public ActionResult ClassNum(int WeekNum, string Id, int cNum, string dept)
        {
            //Check if the number selected was other.
            if (cNum == -2)
            {
                //Redirect to the Page to Type in the "other" info.
                return RedirectToAction("Other", new { WeekNum, Id });
            }
            else
            {
                //Redirect to the Page to select the Professor
                return RedirectToAction("ChooseProf", new { NumWeek = WeekNum, Id, Num = cNum, dept });
            }
        }
        /*
         * This is the method for selecting the professor for the class the
         * user is taking. It will also have an "other" category for people not 
         * in any of the available classes.
         */
        [HttpGet]
        public ActionResult ChooseProf(int NumWeek, string Id, int Num, string dept)
        {
            //Find all of the distict Class Professors in relation to the previous Info and use that for the drop down.
            var Instructors = db.Classes
                .Where(c => c.DeptPrefix == dept)
                .Where(c => c.ClassNum == Num)                
                .GroupBy(c => c.Instructor)
                .Select(c => c.FirstOrDefault())
                .ToList();

            //Remove the placeholder class from the list of options.
            var remClass = Instructors.Where(c => c.DeptPrefix == "NO").Select(c => c).FirstOrDefault();
            Instructors.Remove(remClass);

            //Remove the 'other' classes from the list of options.
            var remClass2 = Instructors.Where(c => c.DeptPrefix == null).Select(c => c).FirstOrDefault();
            Instructors.Remove(remClass2);

            //Keep these floating around so we can easily have the stuff working. 
            ViewBag.Id = Id;
            ViewBag.WeekNum = NumWeek;
            ViewBag.cNum = Num;
            ViewBag.dept = dept;            

            // Return the View so students can select their Class Number.
            return View(Instructors);
        }
        [HttpPost]
        public ActionResult ChooseProf(int WeekNum, string Id, string Prof, int classN, string dept)
        {
            //Check if the number selected was other.
            if (Prof == "other")
            {
                //Redirect to the Page to Type in the "other" info.
                return RedirectToAction("Other", new { WeekNum, Id });
            }            
            else
            {
                //Redirect to the Page to select the Professor
                return RedirectToAction("ChooseStartTime", new { WeekNum, Id, cNum = classN, Prof, dept });
            }
        }

        /*
         * This is the method where the student selects the startTime 
         * of their class. It will include 'online' for online classes and 
         * an other category, just in case.
         */ 
         [HttpGet]
         public ActionResult ChooseStartTime(int WeekNum, int Id, int cNum, string Prof, string dept)
        {
            //Find all possible start times
            var startTimes = db.Classes
                .Where(c => c.DeptPrefix == dept)
                .Where(d => d.ClassNum == cNum)
                .Where(n => n.Instructor == Prof)
                .Select(p => p)
                .ToList();

            //Remove the placeholder class from the list of options.
            var remClass = startTimes.Where(c => c.DeptPrefix == "NO").Select(c => c).FirstOrDefault();
            startTimes.Remove(remClass);

            //Remove the 'other' classes from the list of options.
            var remClass2 = startTimes.Where(c => c.DeptPrefix == null).Select(c => c).FirstOrDefault();
            startTimes.Remove(remClass2);

            //Keep Week Number and StudentID
            ViewBag.Id = Id;
            ViewBag.WeekNum = WeekNum;

            // Return the View so students can select their Class Number.
            return View(startTimes);
        }
        [HttpPost]
        public ActionResult ChooseStartTime(int WeekNum, string Id, int classID)
        {
            Debug.WriteLine("classID = " + classID);
            if (classID == -2)
            {
                return RedirectToAction("Other", new { WeekNum, Id });
            }            
            //Set the student's class to their actual class.
            Student currentStudent = db.Students.Find(Id);
            currentStudent.Class = classID;

            //Create the SignIn to add it to the Database.
            SignIn signIn = new SignIn { Week = WeekNum, Date = DateTime.Today.Date, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, Sec = DateTime.Now.TimeOfDay.Seconds, StudentID = Id };
            db.SignIns.Add(signIn);

            //Save the database changes.
            db.SaveChanges();
            
            //Redirect to the "finish" page.
            return RedirectToAction("Finish", new { Week = WeekNum });
        }

        /*
         * The method for when the student selects Other.
         */
         [HttpGet]
         public ActionResult Other(int WeekNum, string Id)
        {                   
            //Keep the data floating.
            ViewBag.Week = WeekNum;
            ViewBag.Id = Id;

            //Return the View.
            return View();
        }
        [HttpPost]
        public ActionResult Other(int Week, string Id, string other)
        {
            //Create the class to be connected to the student.
            db.Classes.Add(new Class { Other = other });

            //Save the class into the database.
            db.SaveChanges();

            //Get the class again.
            Class sClass = db.Classes
                .Where(c => c.Other == other)
                .Select(c => c).FirstOrDefault();

            //Add the class to the current student.
            Student currentStudent = db.Students.Find(Id);
            currentStudent.Class = sClass.ClassID;

            //Create the Sign In to be added to the db.
            db.SignIns.Add(new SignIn { Week = Week, Date = DateTime.Today.Date, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, Sec = DateTime.Now.TimeOfDay.Seconds, StudentID = Id });

            //Save the Changes to the db.
            db.SaveChanges();

            //Redirect to the finish page.
            return RedirectToAction("Finish", new { Week });
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
                //Create the Sign In and add it to the database.
                SignIn signIn = new SignIn { Week = Week, Date = DateTime.Today.Date, Hour = DateTime.Now.TimeOfDay.Hours, Min = DateTime.Now.TimeOfDay.Minutes, Sec= DateTime.Now.TimeOfDay.Seconds, StudentID = VNum };
                db.SignIns.Add(signIn);
                db.SaveChanges();

                //Redirect to the "finish" page.
                return RedirectToAction("Finish", new { Week });
            }
            else
            {
                //If it's not you redirect to Sign In page.
                return RedirectToAction("NameInput", new { VNum, Week });
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