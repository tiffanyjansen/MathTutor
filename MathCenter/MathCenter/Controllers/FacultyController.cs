using MathCenter.Models;
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
        MathContext db = new MathContext();
        
        /*
         * This method returns a welcome page for Faculty users.
         */ 
        [HttpGet]
        public ActionResult Index()
        {
            return View();
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
            Debug.WriteLine("Data = " + data);

            var dataList = data.Split(Environment.NewLine.ToCharArray());
            Debug.WriteLine(dataList.First());

            foreach(var row in dataList)
            {
                var rowList = row.Split(' ');
                foreach(var item in rowList)
                {
                    Debug.WriteLine("Item = " + item);
                }
            }

            return View();
        }
    }
}