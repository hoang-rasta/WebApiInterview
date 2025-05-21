using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_ReturnTypes_StatusCodes.Models;

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _204HTTPStatusCodeController : ControllerBase
    {
        [Route("api/[controller]")]
        [ApiController]
        public class EmployeeController : ControllerBase
        {
            //Data Source - Đây là nguồn dữ liệu giả lập cho ví dụ
            private static List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Rakesh", Age = 25, Gender = "Male", Department = "IT" },
            new Employee { Id = 2, Name = "Hina", Age = 26, Gender = "Female", Department = "HR" },
            new Employee { Id = 3, Name = "Suresh", Age = 27, Gender = "Male", Department = "IT" },
        };

            //Một ví dụ về phương thức PUT sử dụng phản hồi 204 No Content
            //PUT: api/Employee/5
            [HttpPut("{id}")]
            public IActionResult UpdateEmployee(int id, Employee employee)
            {
                if (id != employee.Id)
                {
                    return BadRequest();
                }
                var existingEmployee = Employees.FirstOrDefault(emp => emp.Id == id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }
                existingEmployee.Name = employee.Name;
                existingEmployee.Age = employee.Age;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.Department = employee.Department;

                // Trong ứng dụng thực tế, bạn sẽ cập nhật sản phẩm trong cơ sở dữ liệu.
                // Trả về rõ ràng 204 bằng StatusCode
                return StatusCode(StatusCodes.Status204NoContent);
            }

            //Một ví dụ về phương thức DELETE sử dụng phản hồi 204 No Content
            //DELETE: api/Employee/5
            [HttpDelete("{id}")]
            public IActionResult DeleteEmployee(int id)
            {
                var existingEmployee = Employees.FirstOrDefault(emp => emp.Id == id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }
                Employees.Remove(existingEmployee);

                // Trong ứng dụng thực tế, bạn sẽ xóa sản phẩm khỏi cơ sở dữ liệu.
                // Trả về rõ ràng 204 bằng StatusCode
                return StatusCode(StatusCodes.Status204NoContent);
            }
        }
    }
}
