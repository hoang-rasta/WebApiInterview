using Azure.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace WebApiBasicAuthentication.Services
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        // Một tham chiếu đến dịch vụ người dùng, được dùng để xác thực thông tin đăng nhập của người dùng.
        private readonly IUserService _userService;

        // Constructor (Hàm tạo) cho BasicAuthenticationHandler.
        // Các tham số:
        // - IOptionsMonitor<AuthenticationSchemeOptions> options: Theo dõi các thay đổi đối với tùy chọn của lược đồ xác thực.
        // - ILoggerFactory logger: Factory để tạo các instance logger (ghi log).
        // - UrlEncoder encoder: Mã hóa URL để đảm bảo an toàn.
        // - IUserService userService: Dịch vụ được dùng để xác thực thông tin đăng nhập của người dùng.
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            IUserService userService)
            // Truyền các tham số options, logger, và encoder cho constructor của lớp cơ sở.
            : base(options, logger, encoder)
        {
            // Gán IUserService được cung cấp vào trường private để sử dụng trong logic xác thực.
            _userService = userService;
        }

        // Phương thức chính chịu trách nhiệm xử lý xác thực.
        // Phương thức này được ghi đè từ AuthenticationHandler<TOptions> và được gọi khi một yêu cầu cần được xác thực.
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 1. Kiểm tra sự hiện diện của Authorization Header
            // Kiểm tra xem header "Authorization" có tồn tại trong yêu cầu hay không.
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                // Nếu thiếu header "Authorization", xác thực thất bại với thông báo phù hợp.
                return AuthenticateResult.Fail("Missing Authorization Header");
            }

            // 2. Phân tích cú pháp Authorization Header
            // Lấy giá trị của header "Authorization".
            var authorizationHeader = Request.Headers["Authorization"].ToString();

            // Cố gắng phân tích cú pháp header "Authorization" thành đối tượng AuthenticationHeaderValue có cấu trúc.
            if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
            {
                // Nếu quá trình phân tích cú pháp thất bại, header được coi là không hợp lệ và xác thực thất bại.
                return AuthenticateResult.Fail("Invalid Authorization Header");
            }

            // 3. Xác minh lược đồ xác thực là "Basic"
            // Xác minh rằng lược đồ xác thực là "Basic".
            if (!"Basic".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                // Nếu lược đồ không phải là "Basic", xác thực thất bại với thông báo liên quan.
                return AuthenticateResult.Fail("Invalid Authorization Scheme");
            }

            // 4. Giải mã và tách thông tin đăng nhập
            // Giải mã thông tin đăng nhập được mã hóa Base64 từ tham số của header ủy quyền.
            // Điều này tạo ra một chuỗi "username:password" sau đó được tách bởi dấu hai chấm.
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(headerValue.Parameter)).Split(':', 2);

            // Kiểm tra xem việc tách thông tin đăng nhập có cho ra đúng hai thành phần (tên người dùng và mật khẩu) hay không.
            if (credentials.Length != 2)
            {
                // Nếu không, thông tin đăng nhập không hợp lệ và xác thực thất bại.
                return AuthenticateResult.Fail("Invalid Basic Authentication Credentials");
            }

            // Trích xuất email (tên người dùng) và mật khẩu từ thông tin đăng nhập đã giải mã.
            var email = credentials[0];
            var password = credentials[1];

            // 5. Xác thực người dùng thông qua UserService
            try
            {
                // Sử dụng IUserService để xác thực thông tin đăng nhập của người dùng.
                var user = await _userService.ValidateUserAsync(email, password);

                if (user == null)
                {
                    // Nếu không có người dùng nào khớp với thông tin đăng nhập được cung cấp, xác thực thất bại.
                    return AuthenticateResult.Fail("Invalid Username or Password");
                }


                // *********** THAY ĐỔI MỚI CHO ROLE-BASED AUTHENTICATION ***********
                // Lấy các vai trò liên kết với người dùng này để xây dựng các claim dựa trên vai trò.
                var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

                // Tạo một danh sách các Claim sẽ đại diện cho danh tính của người dùng đã xác thực.
                var claims = new List<Claim>
                {
                    // Claim này định danh duy nhất người dùng (sử dụng ID của họ).
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                    // Claim này chứa địa chỉ email của người dùng.
                    new Claim(ClaimTypes.Name, user.Email)
                };

                // Lặp qua từng vai trò của người dùng và thêm một claim vai trò tương ứng.
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                // ***************************************************************


                // Tạo một ClaimsIdentity với các claims đã chỉ định và lược đồ xác thực.
                // ClaimsIdentity nhóm các claims đó và chỉ định loại xác thực (ví dụ: "Basic", "Cookies", "Bearer", v.v.),
                // cho biết một danh tính duy nhất mà người dùng có.
                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);

                // Tạo một ClaimsPrincipal dựa trên ClaimsIdentity.
                // ClaimsPrincipal là một container có thể chứa một hoặc nhiều đối tượng ClaimsIdentity,
                // cho phép nhiều cách mà người dùng có thể được xác thực.
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                // AuthenticationTicket là đối tượng được ASP.NET Core sử dụng để lưu trữ và
                // theo dõi ClaimsPrincipal của người dùng đã xác thực trong một phiên xác thực.
                var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

                // Chỉ ra rằng xác thực đã thành công và trả về ticket.
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch
            {
                // Nếu có bất kỳ ngoại lệ nào xảy ra trong quá trình xác thực, xác thực thất bại với thông báo lỗi chung.
                return AuthenticateResult.Fail("Error occurred during authentication");
            }
        }
    }
}
