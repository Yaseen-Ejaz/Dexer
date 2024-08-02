using Microsoft.AspNetCore.Mvc;

using myprojajax.Data;
using myprojajax.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http;
using System.Text.Json;
using System.Text;

namespace myprojajax.Controllers
{
    
    public class StudentController : Controller
    {
        public readonly ApplicationDbContext cn;
        public StudentController(ApplicationDbContext cn)
        {
            this.cn = cn;
        }

       public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get()
        {
            Console.WriteLine("InGet");

            var students = cn.Student.ToList();
            string json = JsonSerializer.Serialize(students);

            return Ok(json);
        }


        [HttpPost]
        public IActionResult Post([FromBody] JsonElement json)
        {
            var St = JsonSerializer.Deserialize<AddStudentModel>(json.GetRawText());

            var addStudent = new Student();
            {
                Console.WriteLine("assigning");

                addStudent.firstName = St.firstName;
                addStudent.lastName = St.lastName;

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
            int totalrows=StList.Count;
            if(!string.IsNullOrEmpty(searchValue))
            {
                StList = StList.Where(x => x.firstName.ToLower().Contains(searchValue.ToLower()) || x.lastName.ToLower().Contains(searchValue.ToLower())).ToList<Student>();
            }
            int totalrowsafterfiltering = StList.Count;
            //StList = StList.OrderBy(sortColumnName + " " +sortDirection).ToList<Student>();
            StList=StList.Skip(start).Take(length).ToList<Student>();
            return Json(new { data = StList, draw = Request.Form["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering });
        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            Console.WriteLine("In Delete");
            var St =  cn.Student.Find(id);

            if(St!=null)
            {
                cn.Student.Remove(St);
                cn.SaveChanges();
            }
            
            return Ok();
        }
    }
}
