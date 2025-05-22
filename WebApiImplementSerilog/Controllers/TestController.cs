using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiImplementSerilog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // Inject logger cho controller này
        private readonly ILogger<TestController> _logger;

        // Dependency Injection của ILogger<TestController> qua hàm tạo
        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            // Ghi một thông báo mức Trace
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint.");

            // Mô phỏng một phép tính và ghi nhật ký nó
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Calculation value is {Calculation}", calculation);

            // Ghi một thông báo mức Debug với ngữ cảnh bổ sung
            _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purposes.");
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Debug information: {@DebugInfo}", debugInfo);

            // Ghi một thông báo mức Information
            _logger.LogInformation("LogInformation: The LogAllLevels endpoint was reached successfully.");

            // Ghi một cảnh báo nếu một điều kiện nhất định được đáp ứng
            bool resourceLimitApproaching = true;
            if (resourceLimitApproaching)
            {
                _logger.LogWarning("LogWarning: Resource usage is nearing the limit.");
            }

            try
            {
                // Mô phỏng một kịch bản lỗi
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                // Ghi một thông báo mức Error với chi tiết ngoại lệ
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            // Ghi một thông báo mức Critical nếu phát hiện lỗi nghiêm trọng
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: A critical system failure has been detected.");
            }

            return Ok("All logging levels demonstrated in this endpoint.");
        }
    }
}
