using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myprojajax.Data;
using myprojajax.Models;
using System.Text.Json;
using System.Linq;

namespace Datatables.ServerSide.Controllers
{
 
    public class ApiController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public ApiController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpPost]
        public ActionResult GetStudents([FromBody] DTParameters param)
        {
            // Assuming you have a Student model representing your data
            // Replace 'Student' with your actual model class name.
            IQueryable<Student> students = context.Student;

            // Filtering logic based on the 'search' parameter (you can implement more complex filtering here)
            if (!string.IsNullOrEmpty(param.Search.Value))
            {
                string searchTerm = param.Search.Value.ToLower();
                students = students.Where(s =>
                    s.firstName.ToLower().Contains(searchTerm) ||
                    s.lastName.ToLower().Contains(searchTerm)
                );
            }

            // Sorting logic based on the 'order' parameter
            foreach (var order in param.Order)
            {
                if (order.Column == 0) // Assuming column 0 is 'Id'
                {
                    students = order.Dir == DTOrderDir.ASC ? students.OrderBy(s => s.Id) : students.OrderByDescending(s => s.Id);
                }
                else if (order.Column == 1) // Assuming column 1 is 'firstName'
                {
                    students = order.Dir == DTOrderDir.ASC ? students.OrderBy(s => s.firstName) : students.OrderByDescending(s => s.firstName);
                }
                else if (order.Column == 2) // Assuming column 2 is 'lastName'
                {
                    students = order.Dir == DTOrderDir.ASC ? students.OrderBy(s => s.lastName) : students.OrderByDescending(s => s.lastName);
                }
                // Add more sorting options as needed
            }

            // Slice the data based on start and length to implement pagination
            int totalRecords = students.Count();
            students = students.Skip(param.Start).Take(param.Length);

            // Prepare the response in the format expected by DataTables
            DTResult<Student> result = new DTResult<Student>
            {
                draw = param.Draw,
                recordsTotal = totalRecords,
                recordsFiltered = totalRecords, // For this example, we are not doing any filtering, so the filtered count is the same as total records.
                data = students.ToList()
            };

            return Ok(result);
        }
    }
}