using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolChallenge.Models;
using System.Data.SqlClient;
using System.Data;

namespace SchoolChallenge.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SchoolManagementContext _context;

        public TeachersController(SchoolManagementContext context)
        {
            _context = context;
        }

        // GET: Teachers
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["FirstSortParm"] = String.IsNullOrEmpty(sortOrder) ? "First_desc" : "";
            ViewData["LastSortParm"] = sortOrder == "Last" ? "Last_desc" : "Last";
            ViewData["NumberSortParm"] = sortOrder == "Number" ? "Number_desc" : "Number";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            var teachers = from s in _context.Teacher
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                teachers = teachers.Where(s => s.LastName.Contains(searchString)
                               || s.FirstName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "First_desc":
                    teachers = teachers.OrderByDescending(s => s.FirstName);
                    break;
                case "Last":
                    teachers = teachers.OrderBy(s => s.LastName);
                    break;
                case "Last_desc":
                    teachers = teachers.OrderByDescending(s => s.LastName);
                    break;
                case "Number":
                    teachers = teachers.OrderBy(s => s.NumberOfStudents);
                    break;
                case "Number_desc":
                    teachers = teachers.OrderByDescending(s => s.NumberOfStudents);
                    break;
                default:
                    teachers = teachers.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 3;
            return View(await PaginatedList<Teacher>.CreateAsync(teachers.AsNoTracking(), page ?? 1, pageSize));

        }

            // GET: Teachers/Details
            public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .SingleOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // GET: Teachers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teachers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,LastName,NumberOfStudents")] Teacher teacher)
        {
            var connection = @"Server=DESKTOP-DI37240\SQLEXPRESS;Database=SchoolManagement;Trusted_Connection=True;";
            using (SqlConnection con = new SqlConnection(connection))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "SELECT TOP 1 [Teacher ID] FROM Teacher ORDER BY [Teacher ID] DESC";
                cmd.CommandType = CommandType.Text;
                teacher.TeacherId = Int16.Parse(cmd.ExecuteScalar().ToString()) + 1;
                con.Close();
            }
            if (ModelState.IsValid)
            {
                _context.Add(teacher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(teacher);
        }

        // GET: Teachers/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher.SingleOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FirstName,LastName,NumberOfStudents")] Teacher teacher)
        {
            if (id != teacher.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(teacher.TeacherId))
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
            return View(teacher);
        }

        // GET: Teachers/Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacher = await _context.Teacher
                .SingleOrDefaultAsync(m => m.TeacherId == id);
            if (teacher == null)
            {
                return NotFound();
            }

            return View(teacher);
        }

        // POST: Teachers/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.Teacher.SingleOrDefaultAsync(m => m.TeacherId == id);
            _context.Teacher.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherExists(int id)
        {
            return _context.Teacher.Any(e => e.TeacherId == id);
        }
    }
}
