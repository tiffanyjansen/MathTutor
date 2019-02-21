using MathCenter.Models;
using MathCenter.Models.ViewModels;
using MathCenter.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class FacultyController : Controller
    {
        //Database Connection
        MathContext db = new MathContext();
        
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
            Excel();
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
                Data data = new Data { Week = SignIn.Week, Date = SignIn.Date, Hour = SignIn.Hour, Min = SignIn.Min, Sec = SignIn.Sec, VNum = SignIn.Student.VNum, FirstName = SignIn.Student.FirstName, LastName = SignIn.Student.LastName, CRN = SignIn.Student.Class1.CRN, DeptPrefix = SignIn.Student.Class1.DeptPrefix, ClassNum = SignIn.Student.Class1.ClassNum, Days = SignIn.Student.Class1.Days, Instructor = SignIn.Student.Class1.Instructor, Other = SignIn.Student.Class1.Other, StartTime = SignIn.Student.Class1.StartTime };

                datas.Add(data);
            }

            //Remove the Placeholder class.
            var remClass = datas.Where(d => d.Other == "Placeholder").Select(d => d).FirstOrDefault();
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
            var placeholder = db.Classes.Where(c => c.Other == "Placeholder")
                .Select(c => c).FirstOrDefault();
            if (placeholder == null)
            {
                //Add the placeholder to the database.
                db.Classes.Add(new Class { Other = "Placeholder" });
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
                        //Go through the list created by above and make variables with the names.
                        int CRN = Convert.ToInt32(rowList[0]);
                        string DeptPrefix = rowList[1];
                        int ClassNum = Convert.ToInt32(rowList[2]);
                        string StartTime = rowList[3];
                        string Days = "";
                        if(StartTime != "Online")
                        {
                            Days = rowList[4];
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
            var remClass = Classes.Where(c => c.Other == "Placeholder").Select(c => c).FirstOrDefault();
            Classes.Remove(remClass);

            //Return the View with only the classes you actually want.
            return View(Classes);
        }
    }
}