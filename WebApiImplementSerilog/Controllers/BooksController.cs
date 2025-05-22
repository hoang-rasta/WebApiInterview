using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApiImplementSerilog.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiImplementSerilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        // Một danh sách static để mô phỏng một kho dữ liệu cho sách.
        private static List<Book> Books = new List<Book>()
        {
            new Book(){ Id = 1001, Title = "ASP.NET Core", Author = "Pranaya", YearPublished = 2025 },
            new Book(){ Id = 1002, Title = "SQL Server", Author = "Pranaya", YearPublished = 2030 }
        };

        // Inject ILogger cho BooksController để thu thập nhật ký
        private readonly ILogger<BooksController> _logger;

        // Dependency Injection của ILogger<T> qua constructor
        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        // POST api/books
        // Action này thêm một cuốn sách mới vào danh sách
        [HttpPost]
        public IActionResult AddBook([FromBody] Book book)
        {
            Books.Add(book); // Thêm sách vào danh sách tĩnh của chúng ta

            // Ghi nhật ký cuốn sách mới được thêm vào bằng cách sử dụng ghi nhật ký có cấu trúc.
            // Toán tử '@' yêu cầu Serilog serialize đối tượng dưới dạng dữ liệu có cấu trúc.
            // {@Book} yêu cầu Serilog thu thập tất cả các thuộc tính dưới dạng dữ liệu có cấu trúc
            _logger.LogInformation("Added a new book {@Book}", book);

            return Ok();
        }

        // GET api/books
        // Action này truy xuất tất cả các cuốn sách trong danh sách
        [HttpGet]
        public IActionResult GetBooks()
        {
            // Ghi nhật ký việc truy xuất sách và bao gồm toàn bộ bộ sưu tập dưới dạng dữ liệu có cấu trúc.
            // Ghi nhật ký danh sách sách dưới dạng dữ liệu có cấu trúc
            _logger.LogInformation("Retrieved all books. Books: {@Books}", Books);

            return Ok(Books);
        }


        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            string UniqueId = Guid.NewGuid().ToString();
            try
            {
                // First approach: include UniqueId directly in the log message
                _logger.LogInformation("{UniqueId} This is an Information log.", UniqueId);
                _logger.LogWarning("This is a Warning log. UniqueId: {UniqueId}", UniqueId);
                _logger.LogCritical("This is a Critical log, indicating a serious failure.");
                // Second approach: use ForContext to add UniqueId to the log context
                Log.ForContext("UniqueId", UniqueId).Information("Processing Request Information");
                Log.ForContext("UniqueId", UniqueId).Warning("Processing Request Warning");
                // Simulate an error
                int x = 10, y = 0;
                int z = x / y;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{UniqueId} An error occurred.", UniqueId);
                Log.ForContext("UniqueId", UniqueId).Error("Processing Request Error");
            }
            return Ok("Check your logs to see the different logging levels in action!");
        }
    }
}
