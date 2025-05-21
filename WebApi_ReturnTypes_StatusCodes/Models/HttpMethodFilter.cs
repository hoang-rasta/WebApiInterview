using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_ReturnTypes_StatusCodes.Models
{
    // Khai báo một lớp triển khai giao diện IActionFilter để chặn các hành động trong MVC.
    public class HttpMethodFilter : IActionFilter
    {
        // Trường riêng để chứa các phương thức HTTP được phép.
        private readonly string[] _allowedMethods;

        // Constructor để khởi tạo bộ lọc với danh sách các phương thức được phép.
        public HttpMethodFilter(string[] allowedMethods)
        {
            // Lưu trữ các phương thức HTTP được phép được truyền trong quá trình khởi tạo.
            _allowedMethods = allowedMethods;
        }

        // Phương thức được gọi trước khi một phương thức hành động thực thi.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Kiểm tra xem phương thức HTTP của yêu cầu hiện tại có nằm trong danh sách được phép hay không.
            if (!_allowedMethods.Contains(context.HttpContext.Request.Method))
            {
                // Tạo một đối tượng ẩn danh để đại diện cho phản hồi lỗi tùy chỉnh.
                var customResponse = new
                {
                    Code = 405,  // Mã trạng thái HTTP cho "Method Not Allowed".
                    Message = "HTTP Method not allowed"  // Thông báo lỗi tùy chỉnh.
                };
                // Đặt kết quả hành động thành một ObjectResult với phản hồi tùy chỉnh và mã trạng thái.
                context.Result = new ObjectResult(customResponse)
                {
                    StatusCode = StatusCodes.Status405MethodNotAllowed // Đặt rõ ràng mã trạng thái HTTP thành 405.
                };
            }
        }

        // Phương thức được gọi sau khi một phương thức hành động thực thi.
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Phương thức này là một phần của giao diện IActionFilter nhưng không được sử dụng ở đây.
            // Bạn có thể triển khai logic hậu xử lý ở đây nếu cần.
            // Phương thức này cố ý để trống vì không cần xử lý sau hành động.
        }
    }
}
