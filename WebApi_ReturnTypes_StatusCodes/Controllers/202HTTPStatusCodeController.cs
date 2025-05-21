using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_ReturnTypes_StatusCodes.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class _202HTTPStatusCodeController : ControllerBase
    {

        // POST: api/Job/CreateJobAsyncWithoutData
        [HttpPost]
        public async Task<IActionResult> CreateJobAsyncWithoutData()
        {
            // Bắt đầu quá trình xử lý không đồng bộ mà không chặn luồng chính.
            // Điều này mô phỏng một tác vụ nền dài (ví dụ: gửi email, xử lý hình ảnh).
            // LongRunningTask sẽ mất 120 giây để hoàn thành.
            LongRunningTask();

            // Trả về 202 Accepted mà không có bất kỳ dữ liệu bổ sung nào trong phản hồi.
            // Client được thông báo rằng yêu cầu đã được chấp nhận, nhưng không có kết quả ngay lập tức.
            return Accepted();
        }

        // POST: api/Job/CreateJobAsyncWithData
        [HttpPost]
        public async Task<IActionResult> CreateJobAsyncWithData()
        {
            // Bắt đầu quá trình xử lý không đồng bộ.
            LongRunningTask();

            // Tạo một đối tượng trạng thái tài nguyên để trả về như một phần của nội dung phản hồi.
            // Đối tượng này cung cấp thông tin ban đầu về trạng thái của tác vụ.
            var resourceStatus = new
            {
                Status = "Processing",
                EstimatedCompletionTime = "2 hours"
            };

            // Trả về 202 Accepted với dữ liệu trạng thái tài nguyên trong nội dung phản hồi.
            // Điều này cung cấp cho client ngữ cảnh ban đầu về tác vụ được chấp nhận.
            return Accepted(resourceStatus);
        }

        // POST: api/Job/CreateJobWithLocation
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocation()
        {
            // Bắt đầu quá trình xử lý không đồng bộ.
            LongRunningTask();

            // Giả định JobId đang xử lý là 123. Đây là ID mà client có thể dùng để kiểm tra trạng thái.
            var processingJobId = 123;

            // Tạo URI động để client có thể kiểm tra trạng thái tác vụ.
            // Url.Action tạo một URL đến phương thức hành động GetStatus với JobId cụ thể.
            var locationUrl = Url.Action("GetStatus", new { JobId = processingJobId });

            // Kiểm tra lỗi nếu không thể tạo URL trạng thái.
            if (string.IsNullOrEmpty(locationUrl))
            {
                return BadRequest("Không thể tạo URL trạng thái.");
            }

            // Tạo một đối tượng Uri từ URL để đảm bảo định dạng đúng.
            var locationUri = new Uri(locationUrl, UriKind.RelativeOrAbsolute);

            // Trả về 202 Accepted với URI trong Location header.
            // Client biết nơi để kiểm tra trạng thái của tác vụ đã chấp nhận.
            return Accepted(locationUri);
        }

        // POST: api/Job/CreateJobWithLocationAndData
        [HttpPost]
        public async Task<IActionResult> CreateJobWithLocationAndData()
        {
            // Bắt đầu quá trình xử lý không đồng bộ.
            LongRunningTask();

            // Giả định JobId đang xử lý là 123.
            var processingJobId = 123;

            // Tạo đối tượng trạng thái tài nguyên với các chi tiết về tác vụ.
            var resourceStatus = new
            {
                Status = "Processing",
                EstimatedCompletionTime = "2 hours",
                JobId = processingJobId
            };

            // Tạo URI động để kiểm tra trạng thái tác vụ.
            var locationUrl = Url.Action("GetStatus", new { JobId = processingJobId });

            // Kiểm tra lỗi.
            if (string.IsNullOrEmpty(locationUrl))
            {
                return BadRequest("Không thể tạo URL trạng thái.");
            }

            // Tạo một đối tượng Uri từ URL.
            var locationUri = new Uri(locationUrl, UriKind.RelativeOrAbsolute);

            // Trả về 202 Accepted với URI trong Location header và dữ liệu trạng thái trong nội dung phản hồi.
            // Đây là cách toàn diện nhất, cung cấp cả nơi kiểm tra trạng thái và thông tin ban đầu.
            return Accepted(locationUri, resourceStatus);
        }

        // GET: api/Job/GetStatus/123
        [HttpGet("{JobId}")] // Định nghĩa một endpoint GET để lấy trạng thái dựa trên JobId
        public IActionResult GetStatus(int JobId)
        {
            // Trong ứng dụng thực tế, bạn sẽ truy vấn trạng thái thực tế của công việc từ database hoặc hệ thống khác.
            // Dưới đây là dữ liệu giả lập để minh họa.
            var resourceStatus = new { Status = "Processing", EstimatedCompletionTime = "2 hours" };

            // Trả về 200 OK với dữ liệu trạng thái công việc.
            return Ok(resourceStatus);
        }

        // Phương thức nội bộ mô phỏng một hoạt động tốn thời gian.
        private async Task LongRunningTask()
        {
            // Mô phỏng một tác vụ chạy dài bằng cách chờ 120 giây (2 phút).
            await Task.Delay(TimeSpan.FromSeconds(120));
            // Logic tác vụ thực tế (ví dụ: xử lý dữ liệu, gọi dịch vụ khác) sẽ nằm ở đây.
        }


    }
}
