using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApi_ReturnTypes_StatusCodes.Models;
using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{


    //Primitive or Complex Types
    //IActionResult
    //ActionResult<T>
    //Specific Result Types
    //Task<IActionResult> or Task<ActionResult<T>>

    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

        //Khi sử dụng Primitive Types hoặc Complex Types để trả về dữ liệu trong ASP.NET Core Web API, bạn sẽ gặp một số giới hạn:
        //Không thể tùy chỉnh mã trạng thái HTTP: Các phản hồi mặc định có mã trạng thái 200, không hỗ trợ trả về các mã lỗi hoặc trạng thái khác.
        //Không hỗ trợ xử lý lỗi chi tiết: Bạn không thể trả về các mã lỗi HTTP hoặc thông báo lỗi chi tiết nếu chỉ sử dụng các kiểu dữ liệu cơ bản hoặc phức tạp.
        //Hạn chế linh hoạt: Các kiểu dữ liệu này không linh hoạt trong việc trả về các loại phản hồi khác nhau (ví dụ: thành công, lỗi, hoặc thất bại xác thực).

        //Returning Primitive Types
        // GET: api/<EmployeeController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        //Returning Complex Type:  /api/employee/details
        [HttpGet("Details")]
        public Employee GetEmployeeDetails()
        {
            return new Employee()
            {
                Id = 1001,
                Name = "Anurag",
                Age = 28,
                City = "Mumbai",
                Gender = "Male",
                Department = "IT"
            };
        }

        //Returning Collections
        //[HttpGet("All")]
        //public List<Employee> GetAllEmployees()
        //{
        //    return new List<Employee>
        //    {
        //        new Employee { Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
        //        new Employee { Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
        //        new Employee { Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR" }
        //    };
        //}

        ///api/employee/all
        //[HttpGet("All")]
        //public IEnumerable<Employee> GetAllEmployeesIEnumerable()
        //{
        //    return new List<Employee>
        //    {
        //        new Employee { Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
        //        new Employee { Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
        //        new Employee { Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR" }
        //    };
        //}



        //[HttpGet("All")]
        //public IActionResult GetAllEmployee()
        //{
        //    try
        //    {
        //        // Dữ liệu giả lập từ cơ sở dữ liệu
        //        var listEmployees = new List<Employee>()
        //        {
        //            new Employee(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
        //            new Employee(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
        //            new Employee(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR" },
        //        };

        //        if (listEmployees.Any())
        //        {
        //            return Ok(listEmployees); // Trả về mã 200 OK cùng với danh sách nhân viên
        //        }
        //        else
        //        {
        //            return NotFound(); // Trả về mã 404 Not Found nếu không có nhân viên
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while processing your request."); // Trả về mã 500 nếu có lỗi xảy ra
        //    }
        //}

        //[HttpGet("{Id}")]
        //public IActionResult GetEmployeeDetails(int Id)
        //{
        //    try
        //    {
        //        var listEmployees = new List<Employee>()
        //        {
        //            new Employee(){ Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
        //            new Employee(){ Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
        //            new Employee(){ Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR" },
        //        };

        //        var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);

        //        if (employee != null)
        //        {
        //            return Ok(employee); // Trả về thông tin nhân viên nếu tìm thấy
        //        }
        //        else
        //        {
        //            return NotFound(); // Trả về mã 404 nếu không tìm thấy nhân viên
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "An error occurred while processing your request."); // Trả về lỗi 500 nếu có sự cố
        //    }
        //}


        //Không có ProducesResponseType: Swagger chỉ hiển thị mã trạng thái HTTP mà không biết kiểu dữ liệu trả về(chỉ có 200 OK, nhưng không có thông tin về data type).

        //Với ProducesResponseType: Swagger hiển thị mã trạng thái HTTP và kiểu dữ liệu trả về chi tiết(ví dụ: 200 OK với List<Employee>, 404 Not Found, v.v.), giúp tài liệu API rõ ràng và dễ hiểu hơn.
            
        [HttpGet("All")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Employee>))]  // 200 OK với danh sách các nhân viên
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // 404 Not Found nếu không tìm thấy nhân viên
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // 500 Internal Server Error nếu có lỗi xảy ra
        public IActionResult GetAllEmployee()
        {
            try
            {
                // Danh sách nhân viên cứng
                var listEmployees = new List<Employee>()
                {
                    new Employee() { Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new Employee() { Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new Employee() { Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR" },
                };

                if (listEmployees.Any())
                {
                    return Ok(listEmployees);  // Trả về 200 OK với danh sách nhân viên
                }
                else
                {
                    return NotFound();  // Trả về 404 Not Found nếu không tìm thấy nhân viên
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong khi xử lý yêu cầu.");  // Trả về 500 nếu có lỗi
            }
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]  // 200 OK với dữ liệu nhân viên
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // 404 Not Found nếu không tìm thấy nhân viên
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]  // 500 Internal Server Error nếu có lỗi xảy ra
        public IActionResult GetEmployeeDetails(int Id)
        {
            try
            {
                // Danh sách nhân viên cứng
                var listEmployees = new List<Employee>()
                {
                    new Employee() { Id = 1001, Name = "Anurag", Age = 28, City = "Mumbai", Gender = "Male", Department = "IT" },
                    new Employee() { Id = 1002, Name = "Pranaya", Age = 28, City = "Delhi", Gender = "Male", Department = "IT" },
                    new Employee() { Id = 1003, Name = "Priyanka", Age = 27, City = "BBSR", Gender = "Female", Department = "HR" },
                };

                // Tìm nhân viên theo Id
                var employee = listEmployees.FirstOrDefault(emp => emp.Id == Id);

                if (employee != null)
                {
                    return Ok(employee);  // Trả về 200 OK với dữ liệu nhân viên
                }
                else
                {
                    return NotFound();  // Trả về 404 Not Found nếu không tìm thấy nhân viên
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Đã xảy ra lỗi trong khi xử lý yêu cầu.");  // Trả về 500 nếu có lỗi
            }
        }

    }
}
