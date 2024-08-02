using Microsoft.AspNetCore.Mvc;
using myprojajax.Data;
using myprojajax.Models;
using System.Text.Json;
using Microsoft.AspNet.SignalR;
using Microsoft.EntityFrameworkCore;

namespace myprojajax.Controllers
{
    public class DexieController : Controller
    {

        public readonly ApplicationDbContext cn;
        public DexieController(ApplicationDbContext cn)
        {
            this.cn = cn;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Index1()
        {
            return View();
        }


        public class ChatHub : Hub
        {
            // Define methods to send notifications to clients
            public void SendNotification(string message)
            {
                // Send a notification to all connected clients
                Clients.All.notify(message);
            }
        }


        [HttpPost]
        public IActionResult GetNotIds([FromBody] List<string> ids)
        {
            Console.WriteLine(ids);
            
            if (ids == null || !ids.Any())
            {
                return Ok();
            }

            var students = cn.Student
                              .Where(student => !ids.Contains(student.indexId))
                              .ToList();
            Console.WriteLine(students);
            if (students.Count > 0)
            {
                string json = JsonSerializer.Serialize(students);
                return Ok(json);
            }
            return Ok();

        }


        [HttpPost]
        public IActionResult Post([FromBody] JsonElement json)
        {
            var St = JsonSerializer.Deserialize<AddStudentModel>(json.GetRawText());
            Console.WriteLine(St);

            var addStudent = new Student();
            {
                addStudent.indexId = St.Id;
                addStudent.firstName = St.firstName;
                addStudent.lastName = St.lastName;
                Console.WriteLine(St.firstName);

            }

            var student = cn.Student.FirstOrDefault(s => s.indexId == St.Id);

            Console.WriteLine(student);
            if (student != null)
            {
                Console.WriteLine("updating");
                cn.Student.Attach(student); // Attach the entity to the context
                student.firstName = St.firstName; // Update properties
                student.lastName = St.lastName;
                cn.SaveChanges(); // Save changes to the database
                return Ok();
            }

            if (addStudent.firstName != null && addStudent.lastName != null)
            {
                Console.WriteLine("adding");

                cn.Student.Add(addStudent);
                cn.SaveChanges();
                return Ok();


            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            try
            {
                Console.WriteLine("In Delete");
                var student = cn.Student.FirstOrDefault(s => s.indexId == id);

                if (student == null)
                {
                    // Student with the given ID was not found
                    return Json(new { success = false, message = "Student not found" });
                }

                cn.Student.Remove(student);
                cn.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in Delete: " + ex.Message);
                return Json(new { success = false, message = "An error occurred while deleting the student" });
            }
        }

        [HttpPost]

        public IActionResult GetStudents()
        {
            Console.WriteLine("In getst");
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string searchValue = Request.Form["search[value]"];
            string sortColumnName = Request.Form["columns[" + Request.Form["order[0][column]"] + " ][name]"];
            string sortDirection = Request.Form["order[0][dir]"];

            List<Student> StList = new List<Student>();
            StList = cn.Student.ToList<Student>();
            int totalrows = StList.Count;
            if (!string.IsNullOrEmpty(searchValue))
            {
                StList = StList.Where(x => x.firstName.ToLower().Contains(searchValue.ToLower()) || x.lastName.ToLower().Contains(searchValue.ToLower())).ToList<Student>();
            }
            int totalrowsafterfiltering = StList.Count;
            //StList = StList.OrderBy(sortColumnName + " " +sortDirection).ToList<Student>();
            StList = StList.Skip(start).Take(length).ToList<Student>();
            return Json(new { data = StList, draw = Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering });
        }
    }
}
