using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using day_1_MVC.Models;

namespace day_1_MVC.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly SchoolDbContext _context;

        // 1. Controller Constructor / Injection LifeCycle
        public DepartmentController(SchoolDbContext context)
        {
            _context = context;
        }

        // 2. ShowAll LifeCycle: جلب كل الأقسام وعرضها
        public IActionResult ShowAll()
        {
            var departments = _context.Departments
                .Include(d => d.Students)
                .ToList();

            return View(departments);
        }

        // 3. ShowDetails LifeCycle: جلب تفاصيل القسم بالـ ID
        public IActionResult ShowDetails(int id)
        {
            var department = _context.Departments
                .Include(d => d.Students)
                .Include(d => d.Teachers)
                .Include(d => d.Courses)
                .FirstOrDefault(d => d.Id == id);

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // 4. Add (HTTP GET) LifeCycle: فتح صفحة إضافة قسم
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        // 5. Add (HTTP POST) LifeCycle: حفظ القسم الجديد في القاعدة
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(department);
                _context.SaveChanges();
                return RedirectToAction(nameof(ShowAll));
            }

            return View(department);
        }

        // 6. Action الخاص بالـ ViewModel والـ DropDownList وتحديد حالة القسم
        public IActionResult DepartmentReport(int id)
        {
            var department = _context.Departments
                .Include(d => d.Students)
                .FirstOrDefault(d => d.Id == id);

            if (department == null) return NotFound();

            // حساب حالة القسم: Main لو أكبر من 50، و Branch لو أقل
            string state = department.Students.Count > 50 ? "Main" : "Branch";

            // فلترة الطلاب الذين تزيد أعمارهم عن 25 سنة
            var filteredStudents = department.Students
                .Where(s => s.Age > 25)
                .Select(s => new StudentInfo { Id = s.Id, Name = s.Name })
                .ToList();

            var viewModel = new DepartmentDetailsViewModel
            {
                DepartmentName = department.Name,
                DepartmentState = state,
                OlderStudents = filteredStudents
            };

            return View(viewModel);
        }
    }
}