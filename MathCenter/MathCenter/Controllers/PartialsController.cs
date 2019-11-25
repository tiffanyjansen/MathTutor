using MathCenter.Models;
using MathCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace MathCenter.Controllers
{
    public class PartialsController : Controller
    {
        public PartialViewResult _Errors(string error)
        {
            ViewBag.Error = error;
            return PartialView();
        }

        public PartialViewResult _Progress(int progress)
        {
            ViewBag.Progress = progress;
            return PartialView();
        }

        public PartialViewResult _FilterDropdown(string name, List<Class> classes, int attribute)
        {
            ViewBag.Name = name;
            ViewBag.Classes = classes;
            ViewBag.Attribute = attribute;
            return PartialView();
        }

        public PartialViewResult _ClassDataTable(List<Class> data, string table_id)
        {
            ViewBag.TableID = table_id;
            ViewBag.Data = data;
            ViewBag.First = data.FirstOrDefault();
            return PartialView();
        }

        public PartialViewResult _DataDataTable(List<SignIn> data, string table_id)
        {
            ViewBag.TableID = table_id;
            ViewBag.Data = data;
            ViewBag.First = data.FirstOrDefault();
            return PartialView();
        }
    }
}