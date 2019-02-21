using MathCenter.Models.ViewModels;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MathCenter.Excel
{
    public class DataExcel
    {
        int rowIndex = 1;
        ExcelRange cell;

        public byte[] GenerateExcel(List<Data> data)
        {
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "Math Center";
                excelPackage.Workbook.Properties.Title = "Math Center Data";
                var sheet = excelPackage.Workbook.Worksheets.Add("Data");
                sheet.Name = "Math Center Report";
                //Sign In Info
                sheet.Column(1).Width = 11; //Week
                sheet.Column(2).Width = 10; //Date
                sheet.Column(3).Width = 4; //Hour
                sheet.Column(4).Width = 7; //Min
                sheet.Column(5).Width = 7; //Sec
                //Student Info
                sheet.Column(6).Width = 8; //VNum
                sheet.Column(7).Width = 15; //FirstName
                sheet.Column(8).Width = 15; //LastName
                //Class Info
                sheet.Column(9).Width = 7; //CRN
                sheet.Column(10).Width = 12; //DeptPrefix
                sheet.Column(11).Width = 12; //ClassNum
                sheet.Column(12).Width = 25; //Instructor
                sheet.Column(13).Width = 5; //Days
                sheet.Column(14).Width = 10; //StartTime
                sheet.Column(15).Width = 10; //Other

                //heading of the table
                #region Table Header
                
                //Heading for Week Number
                cell = sheet.Cells[rowIndex, 1];
                cell.Value = "Week Number";
                cell.Style.Font.Bold = true;

                //Heading for Date
                cell = sheet.Cells[rowIndex, 2];
                cell.Value = "Date";
                cell.Style.Font.Bold = true;

                //Heading for Hour
                cell = sheet.Cells[rowIndex, 3];
                cell.Value = "Hour";
                cell.Style.Font.Bold = true;

                //Heading for Minute
                cell = sheet.Cells[rowIndex, 4];
                cell.Value = "Minute";
                cell.Style.Font.Bold = true;

                //Heading for Second
                cell = sheet.Cells[rowIndex, 5];
                cell.Value = "Second";
                cell.Style.Font.Bold = true;

                //Heading for V-Number
                cell = sheet.Cells[rowIndex, 6];
                cell.Value = "V-Number";
                cell.Style.Font.Bold = true;

                //Heading for First Name
                cell = sheet.Cells[rowIndex, 7];
                cell.Value = "First Name";
                cell.Style.Font.Bold = true;

                //Heading for Last Name
                cell = sheet.Cells[rowIndex, 8];
                cell.Value = "Last Name";
                cell.Style.Font.Bold = true;

                //Heading for CRN
                cell = sheet.Cells[rowIndex, 9];
                cell.Value = "CRN";
                cell.Style.Font.Bold = true;

                //Heading for Class Prefix
                cell = sheet.Cells[rowIndex, 10];
                cell.Value = "Class Prefix";
                cell.Style.Font.Bold = true;

                //Heading for Class Number
                cell = sheet.Cells[rowIndex, 11];
                cell.Value = "Class Number";
                cell.Style.Font.Bold = true;

                //Heading for Instructor
                cell = sheet.Cells[rowIndex, 12];
                cell.Value = "Instructor";
                cell.Style.Font.Bold = true;

                //Heading for Days
                cell = sheet.Cells[rowIndex, 13];
                cell.Value = "Days";
                cell.Style.Font.Bold = true;

                //Heading for Start Time
                cell = sheet.Cells[rowIndex, 14];
                cell.Value = "Start Time";
                cell.Style.Font.Bold = true;

                //Heading for Other
                cell = sheet.Cells[rowIndex, 15];
                cell.Value = "Other";
                cell.Style.Font.Bold = true;

                //Go to the next row.
                rowIndex = rowIndex + 1;
                #endregion

                //The Data to be added for the table.
                #region Table Body
                if (data.Count > 0)
                {
                    foreach (Data dat in data)
                    {
                        //Info for Week Number
                        cell = sheet.Cells[rowIndex, 1];
                        cell.Value = dat.Week;

                        //Info for Date
                        cell = sheet.Cells[rowIndex, 2];
                        cell.Value = dat.Date.ToShortDateString();

                        //Info for Hour
                        cell = sheet.Cells[rowIndex, 3];
                        cell.Value = dat.Hour;

                        //Info for Minute
                        cell = sheet.Cells[rowIndex, 4];
                        cell.Value = dat.Min;

                        //Info for Second
                        cell = sheet.Cells[rowIndex, 5];
                        cell.Value = dat.Sec;

                        //Info for V-Number
                        cell = sheet.Cells[rowIndex, 6];
                        cell.Value = dat.VNum;

                        //Info for First Name
                        cell = sheet.Cells[rowIndex, 7];
                        cell.Value = dat.FirstName;

                        //Info for Last Name
                        cell = sheet.Cells[rowIndex, 8];
                        cell.Value = dat.LastName;

                        //Info for CRN
                        cell = sheet.Cells[rowIndex, 9];
                        cell.Value = dat.CRN;

                        //Info for Class Prefix
                        cell = sheet.Cells[rowIndex, 10];
                        cell.Value = dat.DeptPrefix;

                        //Info for Class Number
                        cell = sheet.Cells[rowIndex, 11];
                        cell.Value = dat.ClassNum;

                        //Info for Instructor
                        cell = sheet.Cells[rowIndex, 12];
                        cell.Value = dat.Instructor;

                        //Info for Days
                        cell = sheet.Cells[rowIndex, 13];
                        cell.Value = dat.Days;

                        //Info for Start Time
                        cell = sheet.Cells[rowIndex, 14];
                        cell.Value = dat.StartTime;

                        //Info for Other
                        cell = sheet.Cells[rowIndex, 15];
                        cell.Value = dat.Other;

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