using System.Text.Json;

namespace WebApi_ReturnTypes_StatusCodes.Models
{
    // Khai báo lớp HttpMethodMiddleware
    public class HttpMethodMiddleware
    {
        // Khai báo một trường riêng để lưu trữ delegate middleware tiếp theo
        private readonly RequestDelegate _next;
        // Khai báo một trường riêng để lưu trữ các phương thức HTTP được phép
        private readonly string[] _allowedMethods;

        // Constructor để khởi tạo middleware với delegate tiếp theo và các phương thức được phép.
        public HttpMethodMiddleware(RequestDelegate next, string[] allowedMethods)
        {
            _next = next;  // Lưu trữ delegate tiếp theo trong pipeline.
            _allowedMethods = allowedMethods;  // Lưu trữ các phương thức HTTP được phép.
        }

        // Phương thức bất đồng bộ xử lý từng yêu cầu HTTP.
        public async Task InvokeAsync(HttpContext context)
        {
            // Kiểm tra xem phương thức HTTP hiện tại có nằm trong danh sách các phương thức được phép hay không.
            if (!_allowedMethods.Contains(context.Request.Method))
            {
                // Đặt mã trạng thái thành 405.
                context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                // Đặt kiểu nội dung phản hồi thành JSON.
                context.Response.ContentType = "application/json";
                // Tạo một đối tượng ẩn danh để chứa chi tiết lỗi.
                var customResponse = new
                {
                    Code = 405,  // Mã trạng thái HTTP cho "Method Not Allowed".
                    Message = "HTTP Method not allowed"  // Thông báo lỗi tùy chỉnh.
                };
                // Tuần tự hóa đối tượng phản hồi tùy chỉnh thành JSON.
                var responseJson = JsonSerializer.Serialize(customResponse);
                // Ghi JSON đã tuần tự hóa vào phản hồi HTTP.
                await context.Response.WriteAsync(responseJson);
                return; // Ngắt ngắn pipeline để ngăn chặn xử lý thêm.
            }
            // Nếu phương thức được phép, chuyển ngữ cảnh đến middleware tiếp theo trong pipeline.
            await _next(context);
        }
    }
}
