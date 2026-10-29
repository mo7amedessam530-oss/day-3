using System.Collections.Generic;

namespace day_1_MVC.Models
{
    public class DepartmentDetailsViewModel
    {
        public string DepartmentName { get; set; } = string.Empty;

        // حالة القسم: Main لو أكبر من 50 طالب، و Branch لو أقل
        public string DepartmentState { get; set; } = string.Empty;

        // قائمة الطلاب اللي أعمارهم أكبر من 25 لعرضها في الـ DropDownList
        public List<StudentInfo> OlderStudents { get; set; } = new List<StudentInfo>();
    }

    public class StudentInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}