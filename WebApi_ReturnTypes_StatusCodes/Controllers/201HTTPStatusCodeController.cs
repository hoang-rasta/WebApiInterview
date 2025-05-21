using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_ReturnTypes_StatusCodes.Models;

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class _201HTTPStatusCodeController : ControllerBase
    {
        //Data Source
        private static List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Rakesh", Age = 25, Gender = "Male", Department = "IT" },
            new Employee { Id = 2, Name = "Hina", Age = 26, Gender = "Female", Department = "HR" },
            new Employee { Id = 3, Name = "Suresh", Age = 27, Gender = "Male", Department = "IT" },
        };

        [HttpPost]
        public ActionResult<Employee> CreateEmployee(Employee employee)
        {
            // Add and save employee in your database or storage
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Assuming you have an API for getting the resource
            // GetEmployee is the name of the action method which takes the employee id and return the employee
            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }



        // POST: api/_201HTTPStatusCode/CreateEmployee1
        [HttpPost]
        public ActionResult<Employee> CreateEmployee1(Employee employee)
        {
            // Thêm và lưu nhân viên vào cơ sở dữ liệu hoặc bộ nhớ của bạn
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Trả về mã trạng thái 204 mà không có Location Header và không có Response Body
            return Created();
        }

        // POST: api/_201HTTPStatusCode/CreateEmployee2
        [HttpPost]
        public ActionResult<Employee> CreateEmployee2(Employee employee)
        {
            // Thêm và lưu nhân viên vào cơ sở dữ liệu hoặc bộ nhớ của bạn
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Tạo URI cho tài nguyên mới được tạo dưới dạng chuỗi
            // var locationUri = $"https://localhost:7077/api/_201HTTPStatusCode/GetEmployee/{employee?.Id}";
            // Tạo URI cho tài nguyên mới được tạo bằng Url.Action
            // Phương thức Url.Action tạo một URL đầy đủ cho hành động được chỉ định.
            // Trong trường hợp này, nó tạo URL cho hành động GetEmployee, sử dụng id của nhân viên mới được tạo.
            var locationUri = Url.Action("GetEmployee", new { id = employee.Id });
            // Trả về phản hồi 201 Created với URI và đối tượng nhân viên
            return Created(locationUri, employee);
        }

        // POST: api/_201HTTPStatusCode/CreateEmployee3
        [HttpPost]
        public ActionResult<Employee> CreateEmployee3(Employee employee)
        {
            // Thêm và lưu nhân viên vào cơ sở dữ liệu hoặc bộ nhớ của bạn
            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            // Tạo URI cho tài nguyên mới được tạo bằng Url.Link
            // Phương thức Url.Link tạo một URI tuyệt đối dựa trên tên route được chỉ định ("GetEmployee")
            // và các giá trị route (new { id = employee.Id }).
            var locationUri = Url.Link("GetEmployee", new { id = employee.Id });
            if (locationUri == null)
            {
                // Xử lý lỗi hoặc tạo URI mặc định
                return BadRequest("Unable to generate location URI for the new resource.");
            }
            // Trả về phản hồi 201 Created với URI và đối tượng nhân viên
            return Created(new Uri(locationUri), employee);
        }


        // GET: api/_201HTTPStatusCode/GetEmployee/1
        [HttpGet("{id}", Name = "GetEmployee")]
        public ActionResult<Employee> GetEmployee(int id)
        {
            var employee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
            {
                // 404 Not Found if the employee does not exist
                return NotFound();
            }
            // 200 OK with the employee in the response body
            return Ok(employee);
        }
    }
}
