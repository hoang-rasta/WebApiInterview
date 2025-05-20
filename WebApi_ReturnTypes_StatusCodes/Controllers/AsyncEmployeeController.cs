using Microsoft.AspNetCore.Mvc;
using WebApi_ReturnTypes_StatusCodes.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsyncEmployeeController : ControllerBase
    {
        private static readonly List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "John Doe", Gender = "Male", City = "New York", Age = 30, Department = "HR" },
            new Employee { Id = 2, Name = "Jane Smith", Gender = "Female", City = "Los Angeles", Age = 25, Department = "Finance" },
            new Employee { Id = 3, Name = "Mike Johnson", Gender = "Male", City = "Chicago", Age = 40, Department = "IT" }
        };

        [HttpGet("all")]
        //[ProducesResponseType(typeof(IEnumerable<Employee>), 200)]
        //[ProducesResponseType(500)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                return Ok(Employees);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // Read (GET all employees)
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            try
            {
                // Simulate an asynchronous operation
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Return the list of employees with a 200 OK status
                return Ok(Employees);
            }
            catch (Exception)
            {
                // Return 500 Internal Server Error in case of an exception
                return StatusCode(500, "Internal server error");
            }
        }
        // Read (GET employee by ID)
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            try
            {
                // Simulate an asynchronous operation
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Find the employee with the specified ID
                var employee = Employees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    // If the employee is not found, return a 404 Not Found status with a custom message
                    return NotFound(new { message = $"No employee found with ID {id}" });
                }
                // If the employee is found, return it with a 200 OK status
                return Ok(employee);
            }
            catch (Exception)
            {
                // Return 500 Internal Server Error in case of an exception
                return StatusCode(500, "Internal server error");
            }
        }
        // Create (POST new employee)
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee([FromBody] Employee employee)
        {
            try
            {
                // Simulate an asynchronous operation
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Validate the employee data
                if (employee == null || string.IsNullOrEmpty(employee.Name))
                {
                    // If the data is invalid, return a 400 Bad Request status with a custom message
                    return BadRequest(new { Message = "Invalid employee data" });
                }
                // Assign a new ID to the employee
                employee.Id = Employees.Count + 1;
                // Add the employee to the list
                Employees.Add(employee);
                // Return a 201 Created status with a location header pointing to the newly created employee
                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            }
            catch (Exception)
            {
                // Return 500 Internal Server Error in case of an exception
                return StatusCode(500, "Internal server error");
            }
        }
        // Update (PUT existing employee)
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, [FromBody] Employee employee)
        {
            try
            {
                // Simulate an asynchronous operation
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Validate the employee data
                if (employee == null || id != employee.Id)
                {
                    // If the data is invalid, return a 400 Bad Request status with a custom message
                    return BadRequest(new { Message = "Invalid employee data" });
                }
                // Find the existing employee with the specified ID
                var existingEmployee = Employees.FirstOrDefault(e => e.Id == id);
                if (existingEmployee == null)
                {
                    // If the employee is not found, return a 404 Not Found status
                    return NotFound();
                }
                // Update the employee properties
                existingEmployee.Name = employee.Name;
                existingEmployee.Gender = employee.Gender;
                existingEmployee.City = employee.City;
                existingEmployee.Age = employee.Age;
                existingEmployee.Department = employee.Department;
                // Return a 204 No Content status to indicate that the update was successful
                return NoContent();
            }
            catch (Exception)
            {
                // Return 500 Internal Server Error in case of an exception
                return StatusCode(500, "Internal server error");
            }
        }
        // Delete (DELETE employee)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                // Simulate an asynchronous operation
                await Task.Delay(TimeSpan.FromSeconds(1));
                // Find the employee with the specified ID
                var employee = Employees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    // If the employee is not found, return a 404 Not Found status
                    return NotFound();
                }
                // Remove the employee from the list
                Employees.Remove(employee);
                // Return a 200 OK status with no content
                return Ok();
            }
            catch (Exception)
            {
                // Return 500 Internal Server Error in case of an exception
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
