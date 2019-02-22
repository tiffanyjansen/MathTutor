using ScienceCenter.Excel;
using ScienceCenter.Models;
using ScienceCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScienceCenter.Controllers
{
    public class FacultyController : Controller
    {
        //Database Connection
        ScienceContext db = new ScienceContext();

        /*
         * This method returns a welcome page for Faculty users.
         */
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(int? download)
        {
            //if you press the download button, the excel sheet will be created.
            if (download == 1)
            {
                Excel();
            }
            //If you press the reset button, it will redirect you to another page.
            if (download == 2)
            {
                return RedirectToAction("Reset");
            }
            //If you press the Go Back Button, it will redirect you to another page.
            if (download == 3)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /*
         * This method will do the work of downloading the excel file with 'hopefully'
         * all the data.
         */
        public void Excel()
        {
            DataExcel excel = new DataExcel();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetData()));
            Response.AddHeader("content-disposition", "attachment; filename=MathCenterData.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }
        public List<Data> GetData()
        {
            //Create an empty list.
            List<Data> datas = new List<Data>();

            //Go through the list of sign ins and add all the data to the list.
            foreach (var SignIn in db.SignIns.ToList())
            {
                Data data = new Data { Week = SignIn.Week, Date = SignIn.Date, Hour = SignIn.Hour, Min = SignIn.Min, Sec = SignIn.Sec, VNum = SignIn.Student.VNum, FirstName = SignIn.Student.FirstName, LastName = SignIn.Student.LastName, CRN = SignIn.Student.Class1.CRN, DeptPrefix = SignIn.Student.Class1.DeptPrefix, ClassNum = SignIn.Student.Class1.ClassNum, Days = SignIn.Student.Class1.Days, Instructor = SignIn.Student.Class1.Instructor, StartTime = SignIn.Student.Class1.StartTime };

                datas.Add(data);
            }

            //Remove the Placeholder class.
            var remClass = datas.Where(d => d.CRN == 0).Select(d => d).FirstOrDefault();
            datas.Remove(remClass);

            //Return the list of the data.
            return datas;
        }

        /*
        * This method returns a table with all of the data from the Database.
        */
        [HttpGet]
        public ActionResult Data()
        {
            return View(db.SignIns.ToList());
        }

        /*
         * This method returns a page with a box where users can add classes to the database.
         * It will allow users to add classes whenever they want to and will always have the 
         * placeholder class automatically get added to the db.
         */
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(string data)
        {
            //Split the data by new line.
            var dataList = data.Split(Environment.NewLine.ToCharArray());

            //Check if the placeholder is in the database or not.
            var placeholder = db.Classes.Where(c => c.CRN == 0)
                .Select(c => c).FirstOrDefault();
            if (placeholder == null)
            {
                //Add the placeholder to the database.
                db.Classes.Add(new Class { CRN = 0, DeptPrefix = "NaN", ClassNum = "NaN", Instructor = "NaN", Days = "None", StartTime = "NaN" });
                db.SaveChanges();
            }

            //Try all this stuff.
            try
            {
                //Get every row in the list created above.
                foreach (var row in dataList)
                {
                    //Split each row by space or tab.
                    var rowList = row.Split();
                    if (rowList.Length >= 8)
                    {
                        int counter = 0;                        
                        //Go through the list created by above and make variables with the names.
                        int CRN = Convert.ToInt32(rowList[counter]);
                        counter++;
                        string DeptPrefix = rowList[counter];
                        counter++;
                        string ClassNum = rowList[counter];
                        counter++;
                        if(rowList[counter] == "")
                        {
                            counter++;
                        }
                        string StartTime = rowList[counter];
                        counter++;
                        string Days = "";
                        if (StartTime != "ONLINE")
                        {
                            Days = rowList[counter];
                        }
                        string Instructor = rowList[rowList.Length - 3] + " " + rowList[rowList.Length - 2];

                        //Add the class to the database with the info above.
                        db.Classes.Add(new Class { CRN = CRN, DeptPrefix = DeptPrefix, ClassNum = ClassNum, StartTime = StartTime, Days = Days, Instructor = Instructor });
                    }
                }
                //After going through all the rows, save changes.
                db.SaveChanges();
            }
            //Catch any exception that comes through.
            catch (Exception)
            {
                //Return an error message if an exception was thrown.
                ViewBag.Error = "The data you inputted was not added to the database, please try again.";
                return View();
            }
            //If everything worked, redirect to the classes page where all the classes that are in the database is shown on a page.
            return RedirectToAction("Class");
        }

        /*
         * The class method returns a table with all the Classes in the database.
         */
        [HttpGet]
        public ActionResult Class()
        {
            //Get the entire list of classes.
            var Classes = db.Classes.ToList();

            //Remove the Placeholder Class
            var remClass = Classes.Where(c => c.CRN == 0).Select(c => c).FirstOrDefault();
            Classes.Remove(remClass);

            //Return the View with only the classes you actually want.
            return View(Classes);
        }

        /*
         * This method takes you to a page to reset the data. 
         * (That way the button doesn't feel so scary)
         */
        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Reset(int? reset)
        {
            if (reset == 1)
            {
                Excel();
                ClearDB();
                return RedirectToAction("Complete");
            }
            else if (reset == 2)
            {
                ClearDB();
                return RedirectToAction("Complete");
            }
            else if (reset == 3)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        private void ClearDB()
        {
            //Delete all the SignIns from the DB.
            foreach (var SignIn in db.SignIns.ToList())
            {
                db.SignIns.Remove(SignIn);
            }
            //Delete all the Students from the DB.
            foreach (var Student in db.Students.ToList())
            {
                db.Students.Remove(Student);
            }
            //Delete all the Classes from the DB.
            foreach (var Class in db.Classes.ToList())
            {
                db.Classes.Remove(Class);
            }
            //Save changes to Database.
            db.SaveChanges();
        }

        /*
         * This page let's you know that the database was wiped.
         */
        [HttpGet]
        public ActionResult Complete()
        {
            return View();
        }
    }
}