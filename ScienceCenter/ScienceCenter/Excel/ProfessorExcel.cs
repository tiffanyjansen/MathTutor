using OfficeOpenXml;
using ScienceCenter.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace ScienceCenter.Excel
{
    public class ProfessorExcel
    {
        int rowIndex = 1;
        ExcelRange cell;

        public byte[] GenerateExcel(List<ProfData> data)
        {
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "Science Center";
                excelPackage.Workbook.Properties.Title = "Science Center Data by Professor";
                var sheet = excelPackage.Workbook.Worksheets.Add("byProf");
                sheet.Name = "Science Center Report by Professor";
                //Class Info
                sheet.Column(1).Width = 15; //Instructor
                sheet.Column(2).Width = 12; //CRN
                sheet.Column(3).Width = 10; //DeptPrefix
                sheet.Column(4).Width = 10; //ClassNum
                sheet.Column(5).Width = 10; //Days
                sheet.Column(6).Width = 10; //StartTime
                //Student Info
                sheet.Column(7).Width = 15; //FirstName
                sheet.Column(8).Width = 15; //LastName    
                sheet.Column(9).Width = 10; //TimesIn

                //heading of the table
                #region Table Header

                //Heading for Instructor
                cell = sheet.Cells[rowIndex, 1];
                cell.Value = "Instructor";
                cell.Style.Font.Bold = true;

                //Heading for CRN
                cell = sheet.Cells[rowIndex, 2];
                cell.Value = "CRN";
                cell.Style.Font.Bold = true;

                //Heading for DeptPrefix
                cell = sheet.Cells[rowIndex, 3];
                cell.Value = "Class Prefix";
                cell.Style.Font.Bold = true;

                //Heading for ClassNum
                cell = sheet.Cells[rowIndex, 4];
                cell.Value = "Class Number";
                cell.Style.Font.Bold = true;

                //Heading for Days
                cell = sheet.Cells[rowIndex, 5];
                cell.Value = "Days";
                cell.Style.Font.Bold = true;

                //Heading for Start Time
                cell = sheet.Cells[rowIndex, 6];
                cell.Value = "Start Time";
                cell.Style.Font.Bold = true;

                //Heading for First Name
                cell = sheet.Cells[rowIndex, 7];
                cell.Value = "First Name";
                cell.Style.Font.Bold = true;

                //Heading for Last Name
                cell = sheet.Cells[rowIndex, 8];
                cell.Value = "Last Name";
                cell.Style.Font.Bold = true;

                //Heading for Times Came In
                cell = sheet.Cells[rowIndex, 9];
                cell.Value = "Times Came In";
                cell.Style.Font.Bold = true;

                //Go to the next row.
                rowIndex = rowIndex + 1;
                #endregion

                //The Data to be added for the table.
                #region Table Body
                if (data.Count > 0)
                {
                    foreach (ProfData dat in data)
                    {
                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 1];
                        cell.Value = dat.Instructor;

                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 2];
                        cell.Value = dat.CRN;

                        //Info for Dept Prefix
                        cell = sheet.Cells[rowIndex, 3];
                        cell.Value = dat.DeptPrefix;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 4];
                        cell.Value = dat.ClassNum;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 5];
                        cell.Value = dat.Days;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = dat.StartTime;

                        //Info for First Name
                        cell = sheet.Cells[rowIndex, 7];
                        cell.Value = dat.FirstName;

                        //Info for Last Name
                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = dat.LastName;

                        //Info for Times In
                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = dat.TimesIn;

                        //Go to the next row
                        rowIndex = rowIndex + 1;
                    }
                }
                #endregion

                //return the byte array
                return excelPackage.GetAsByteArray();
            }
        }
    }
}