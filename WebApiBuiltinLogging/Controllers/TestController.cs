using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiBuiltinLogging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        // A private readonly field to hold the logger instance.
        // The ILogger interface allows logging at various levels (Information, Warning, Error, etc.).
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            // Constructor dependency injection to provide an ILogger instance specific to the TestController.
            // ASP.NET Core automatically injects the appropriate logger.
            // Assigns the injected logger to the private field.
            _logger = logger;
            // Logs an informational message when the controller instance is created. 
            // Useful for confirming that the controller is up and running.
            _logger.LogInformation("TestController Started");
        }
        [HttpGet]
        public IActionResult Get()
        {
            // Logs an informational message indicating that the "Get" method is being executed. 
            // Helps in tracing the request flow through the application.
            _logger.LogInformation("Executing TestController.Get Method");
            return Ok();
        }


        [HttpGet("all-logs")]
        public IActionResult LogAllLevels()
        {
            // Ghi nhật ký ở mức Trace, cực kỳ chi tiết
            _logger.LogTrace("LogTrace: Entering the LogAllLevels endpoint with Trace-level logging.");
            // Mô phỏng một biến và ghi nhật ký ở mức Trace
            int calculation = 5 * 10;
            _logger.LogTrace("LogTrace: Calculation value is {calculation}", calculation);

            // Ghi nhật ký ở mức Debug, ít chi tiết hơn Trace nhưng vẫn phục vụ mục đích gỡ lỗi
            _logger.LogDebug("LogDebug: Initializing debug-level logs for debugging purposes.");
            // Ghi một số thông tin gỡ lỗi
            var debugInfo = new { Action = "LogAllLevels", Status = "Debugging" };
            _logger.LogDebug("LogDebug: Debug information: {@debugInfo}", debugInfo);

            // Ghi nhật ký ở mức Information, cho các sự kiện thông thường
            _logger.LogInformation("LogInformation: The LogAllLevels endpoint was reached successfully.");

            // Mô phỏng một điều kiện có thể gây ra vấn đề và ghi nhật ký ở mức Warning
            bool IsTakingMoreTime = true;
            if (IsTakingMoreTime)
            {
                _logger.LogWarning("LogWarning: External API taking More Time to Respond. Action may be required soon.");
            }

            try
            {
                // Mô phỏng một kịch bản lỗi (chia cho 0) và ghi nhật ký ở mức Error
                int x = 0;
                int result = 10 / x;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LogError: An error occurred while processing the request.");
            }

            // Ghi nhật ký một kịch bản lỗi nghiêm trọng ở mức Critical
            bool criticalFailure = true;
            if (criticalFailure)
            {
                _logger.LogCritical("LogCritical: A critical system failure has been detected. Immediate attention is required.");
            }

            return Ok("All logging levels demonstrated in this endpoint.");
        }
    }
}
