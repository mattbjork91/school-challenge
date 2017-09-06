using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolChallenge.Models;
using System.Data.SqlClient;
using System.Data;

namespace SchoolChallenge.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolManagementContext _context;

        public StudentsController(SchoolManagementContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstSortParm"] = String.IsNullOrEmpty(sortOrder) ? "First_desc" : "";
            ViewData["LastSortParm"] = sortOrder == "Last" ? "Last_desc" : "Last";
            ViewData["NumberSortParm"] = sortOrder == "Number" ? "Number_desc" : "Number";
            ViewData["ScholarSortParm"] = sortOrder == "Scholar" ? "Scholar_desc" : "Scholar";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var students = from s in _context.Student
                               select s;

            if(!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.Contains(searchString)
                               || s.FirstName.Contains(searchString));
            }
                switch (sortOrder)
                {
                    case "First_desc":
                        students = students.OrderByDescending(s => s.FirstName);
                        break;
                    case "Last":
                        students = students.OrderBy(s => s.LastName);
                        break;
                    case "Last_desc":
                        students = students.OrderByDescending(s => s.LastName);
                        break;
                    case "Number":
                        students = students.OrderBy(s => s.StudentNumber);
                        break;
                    case "Number_desc":
                        students = students.OrderByDescending(s => s.StudentNumber);
                        break;
                    case "Scholar":
                        students = students.OrderBy(s => s.HasScholarship);
                        break;
                    case "Scholar_desc":
                        students = students.OrderByDescending(s => s.HasScholarship);
                        break;
                default:
                        students = students.OrderBy(s => s.FirstName);
                        break;
                }

            int pageSize = 3;
            return View(await PaginatedList<Student>.CreateAsync(students.AsNoTracking(), page ?? 1, pageSize));

        }





        // GET: Students/Details
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,StudentNumber,FirstName,LastName,HasScholarship")] Student student)
        {
            var connection = @"Server=DESKTOP-DI37240\SQLEXPRESS;Database=SchoolManagement;Trusted_Connection=True;";
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT TOP 1 [Student ID] FROM Student ORDER BY [Student ID] DESC";
                cmd.CommandType = CommandType.Text;
                student.StudentId = Int16.Parse(cmd.ExecuteScalar().ToString()) + 1;
                con.Close();              
            }
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.SingleOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,StudentNumber,FirstName,LastName,HasScholarship")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .SingleOrDefaultAsync(m => m.StudentId == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.SingleOrDefaultAsync(m => m.StudentId == id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
