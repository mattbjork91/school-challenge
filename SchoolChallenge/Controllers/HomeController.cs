using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SchoolChallenge.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace SchoolChallenge.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Index(string postedFile)
        {

            string filePath = string.Empty;

            if (postedFile != null)
            {

                string path = Path.Combine(Environment.CurrentDirectory, @"SampleData\", postedFile);

                //Create a DataTable.
                if (postedFile.Contains("Student") || postedFile.Contains("student"))
                {

                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[5] { new DataColumn("Id", typeof(int)),
                                new DataColumn("Student Number", typeof(string)),
                                new DataColumn("First Name",typeof(string)),
                                new DataColumn("Last Name",typeof(string)),
                                new DataColumn("Has Scholarship",typeof(string)) });

                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(path);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n').Skip(1))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            dt.Rows.Add();
                            int i = 0;

                            //Execute a loop over the columns.
                            foreach (string cell in row.Split(','))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell;
                                i++;
                            }
                        }
                    }

                    var connection = @"Server=DESKTOP-DI37240\SQLEXPRESS;Database=SchoolManagement;Trusted_Connection=True;";
                    using (SqlConnection con = new SqlConnection(connection))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {

                            using (SqlCommand cmd = new SqlCommand("Update_Student"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@tblStudent", dt);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
                if (postedFile.Contains("Teacher") || postedFile.Contains("teacher"))
                {

                    DataTable dt = new DataTable();
                    dt.Columns.AddRange(new DataColumn[4] { new DataColumn("Id", typeof(int)),
                                new DataColumn("First Name", typeof(string)),
                                new DataColumn("Last Name",typeof(string)),
                                new DataColumn("Number of Students",typeof(int)) });

                    //Read the contents of CSV file.
                    string csvData = System.IO.File.ReadAllText(path);

                    //Execute a loop over the rows.
                    foreach (string row in csvData.Split('\n').Skip(1))
                    {
                        if (!string.IsNullOrEmpty(row))
                        {
                            dt.Rows.Add();
                            int i = 0;

                            //Execute a loop over the columns.
                            foreach (string cell in row.Split(','))
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell;
                                if (dt.Rows[dt.Rows.Count - 1][3] == null || Convert.ToString(dt.Rows[dt.Rows.Count - 1][3]) == "")
                                {
                                    dt.Rows[dt.Rows.Count - 1][3] = 0;
                                }
                                i++;
                            }
                        }
                    }

                    var connection = @"Server=DESKTOP-DI37240\SQLEXPRESS;Database=SchoolManagement;Trusted_Connection=True;";
                    using (SqlConnection con = new SqlConnection(connection))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                        {

                            using (SqlCommand cmd = new SqlCommand("Update_Teacher"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@tblTeacher", dt);
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                    }
                }
            }

            return View();

        }

     }
}