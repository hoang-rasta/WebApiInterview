using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_ReturnTypes_StatusCodes.Models;

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class _200HTTPStatusCodeController : ControllerBase
    {
        //Data Source
        private static List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Rakesh", Age = 25, City="BBSR", Gender = "Male", Department = "IT" },
            new Employee { Id = 2, Name = "Hina", Age = 26, City="CTC", Gender = "Female", Department = "HR" },
            new Employee { Id = 3, Name = "Suresh", Age = 27, City="RKL", Gender = "Male", Department = "IT" },
        };
        //URL: GET /api/employee
        [HttpGet]
        public async Task<ActionResult> GetAllEmployees()
        {
            //Do Some IO Bound Operation 
            //Delaying the Execution for 1000 MS
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            // 200 OK with all the employees in the response body
            return Ok(Employees);

            //return StatusCode(200, listEmployees); // Manually setting 200 OK with Data
        }
        //URL: GET /api/employee/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            //Do Some IO Bound Operation 
            //Delaying the Execution for 1000 MS
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            var employee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
            {
                // 404 Not Found if the employee does not exist
                return NotFound();
            }
            // 200 OK with the employee in the response body
            return Ok(employee);
        }
        //URL:POST /api/employee
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateEmployee(Employee employee)
        {
            //Do Some IO Bound Operation 
            //Delaying the Execution for 1000 MS
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            var existingEmployee = Employees.FirstOrDefault(emp => emp.Id == employee.Id);
            if (existingEmployee != null)
            {
                // Update the existing employee
                existingEmployee.Name = employee.Name;
                existingEmployee.Age = employee.Age;
                existingEmployee.Gender = employee.Gender;
                employee.Department = employee.Department;
                // 200 OK with the updated employee
                return Ok(existingEmployee);
            }
            else
            {
                // Add a new employee
                employee.Id = Employees.Count() + 1;
                Employees.Add(employee);
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
            }
        }
        //URL:PUT /api/employee/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee employee)
        {
            //Do Some IO Bound Operation 
            //Delaying the Execution for 1000 MS
            await Task.Delay(TimeSpan.FromMilliseconds(1000));
            if (id != employee.Id)
            {
                // 400 Bad Request if the ID does not match
                return BadRequest();
            }
            var existingEmployee = Employees.FirstOrDefault(emp => emp.Id == employee.Id);
            if (existingEmployee == null)
            {
                // 404 Not Found if the employee does not exist
                return NotFound();
            }
            // Update the existing employee
            existingEmployee.Name = employee.Name;
            existingEmployee.Age = employee.Age;
            existingEmployee.Gender = employee.Gender;
            employee.Department = employee.Department;
            // 200 OK with the updated employee
            return Ok(existingEmployee);
        }
        //URL:DELETE /api/employee/1
        [HttpDelete("{id}")]
        public ActionResult<Employee> DeleteEmployee(int id)
        {
            var employee = Employees.FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
            {
                // 404 Not Found if the employee does not exist
                return NotFound();
            }
            // Delete the employee
            Employees.Remove(employee);
            return Ok(employee); // 200 OK with the deleted employee's details
        }
    }
}
