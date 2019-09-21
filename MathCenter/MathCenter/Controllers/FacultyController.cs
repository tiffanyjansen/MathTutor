using MathCenter.Models;
using MathCenter.Models.ViewModels;
using MathCenter.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using OfficeOpenXml;

namespace MathCenter.Controllers
{
    public class FacultyController : Controller
    {
        //Database Connection
        private readonly MathContext db = new MathContext();
        
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
            Debug.WriteLine("Download = " + download);
            //if you press the download button, the excel sheet will be created.
            if (download == 1)
            {
                return RedirectToAction("SelectDates");
            }
            //If you press the reset button, it will redirect you to another page.
            if(download == 2)
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

        [HttpGet]
        public ActionResult SelectDates()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SelectDates(DateTime start, DateTime end)
        {
            if(start.Month > end.Month || (start.Month == end.Month && start.Day > end.Day) || start.Year > end.Year)
            {
                ViewBag.Error = "Please make sure your start date is before your end date.";
                return View();
            }

            Excel(start, end);
            return RedirectToAction("Index");
        }
        
        /*
         * This method will do the work of downloading the excel file with 'hopefully'
         * all the data.
         */ 
         public void Excel(DateTime? start, DateTime? end)
        {
            DataExcel excel = new DataExcel();
            Response.ClearContent();
            Response.BinaryWrite(excel.GenerateExcel(GetData(start, end)));
            Response.AddHeader("content-disposition", "attachment; filename=MathCenterData.xlsx");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Flush();
            Response.End();
        }
        public List<Data> GetData(DateTime? start, DateTime? end)
        {
            //Create an empty list.
            List<Data> datas = new List<Data>();

            DateTime startDate = (DateTime)start;
            DateTime endDate = (DateTime)end;

            //Go through the list of sign ins and add all the data to the list.
            foreach (var SignIn in db.SignIns.ToList())
            {
                Data data = new Data { Week = SignIn.Week, Date = SignIn.Date, Hour = SignIn.Hour, Min = SignIn.Min, VNum = SignIn.Student.VNum, FirstName = SignIn.Student.FirstName, LastName = SignIn.Student.LastName, SignedClass = db.Classes.Find(SignIn.ClassID) };                        

                //Only add if they are in the selected dates.
                if (data.Date.Month >= startDate.Month && data.Date.Day >= startDate.Day && data.Date.Year >= startDate.Year && data.Date.Month <= endDate.Month && data.Date.Day <= endDate.Day && data.Date.Year <= endDate.Year)
                {
                    datas.Add(data);
                }
                else if(startDate == null && endDate == null)
                {
                    datas.Add(data);
                }
            }

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
         * It will allow users to add classes whenever they want.
         */
        [HttpGet]
         public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    Stream fs = file.InputStream;
                    ExcelPackage package = new ExcelPackage(fs);
                    foreach (ExcelWorksheet worksheet in package.Workbook.Worksheets)
                    {
                        Class @class = new Models.Class();
                        for (int i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                        {
                            @class.CRN = Int32.Parse(worksheet.Cells[i, 1].Value.ToString());
                            @class.DeptPrefix = worksheet.Cells[i, 2].Value.ToString();
                            @class.ClassNum = Int32.Parse(worksheet.Cells[i, 3].Value.ToString());
                            @class.Time = worksheet.Cells[i, 4].Value.ToString();
                            @class.Days = worksheet.Cells[i, 5].Value.ToString();
                            @class.Instructor = worksheet.Cells[i, 6].Value.ToString();
                            db.Classes.Add(@class);
                            db.SaveChanges();
                        }
                    }
                    return RedirectToAction("Class");
                }
                else
                {
                    ViewBag.Error = "The file you uploaded has no data in it, please upload another one."; //return error message
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "The data you inputted was not added to the database, please try again."; //return error message
                return View();
            }
        }

        /*
         * The class method returns a table with all the Classes in the database.
         */ 
         [HttpGet]
         public ActionResult Class()
        {
            //Get the entire list of classes.
            var Classes = db.Classes.ToList();

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
            if(reset == 1)
            {
                Excel(null, null);
                ClearDB();
                return RedirectToAction("Complete");
            }
            else if(reset == 2)
            {
                ClearDB();            
                return RedirectToAction("Complete");
            }
            else if(reset == 3)
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
            //Delete all the StudentClasses from the DB.
            foreach(var StudentClass in db.StudentClasses.ToList())
            {
                db.StudentClasses.Remove(StudentClass);
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