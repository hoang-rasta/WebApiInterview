
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using WebApiBasicAuthentication.Models;
using WebApiBasicAuthentication.Services;
using static System.Net.WebRequestMethods;

namespace WebApiBasicAuthentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            // 1. Bỏ qua Camel Case khi Serialize JSON
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Điều này sẽ sử dụng tên thuộc tính như được định nghĩa trong model C#
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();


            // THAY ĐỔI / THÊM CẤU HÌNH SWAGGER 
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasicAuthenticationDemo API", Version = "v1" });

                // Thêm định nghĩa bảo mật cho Basic Authentication
                c.AddSecurityDefinition("BasicAuth", new OpenApiSecurityScheme
                {
                    Name = "Basic Authentication",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic", // Tên scheme phải khớp với AuthenticationHandler của bạn (lower case)
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                //Scheme = "basic": Đây là phần quan trọng nhất. Nó cho Swagger biết rằng prefix(tiền tố) mà client cần gửi trong header Authorization là "Basic".Lưu ý: mặc dù tên scheme trong AddScheme của bạn là "BasicAuthentication", nhưng giá trị Scheme trong OpenApiSecurityScheme này phải là giá trị mà HTTP standard yêu cầu, tức là "basic"(viết thường).


                // Yêu cầu sử dụng Basic Authentication cho các hoạt động
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "BasicAuth"
                                //Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "basic" }: Tham chiếu đến định nghĩa bảo mật mà bạn đã tạo với ID là "basic".
                            }
                        },
                        new string[] {}
                    }
                });
            });


            // 2. Thêm DbContext
            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection")));

            // 3. Đăng ký UserService
            builder.Services.AddScoped<IUserService, UserService>();

            // 4. Thêm Xác thực với BasicAuthenticationHandler của chúng ta
            builder.Services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            // Configure authorization and define role-based policies:
            //   - "AdminOnly": Requires the "Admin" role.
            //   - "UserOnly": Requires the "User" role.
            //   - "AdminOrUser": Requires either "Admin" or "User" role.
            //   - "AdminAndUser": Requires both "Admin" AND "User" roles (via a custom assertion).
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
                options.AddPolicy("AdminOrUser", policy => policy.RequireRole("Admin", "User"));
                options.AddPolicy("AdminAndUser", policy => policy.RequireAssertion(context =>
                    context.User.IsInRole("Admin") && context.User.IsInRole("User")));
            });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
