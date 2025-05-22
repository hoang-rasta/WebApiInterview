
using Serilog;
using WebApiImplementSerilog.Models;

namespace WebApiImplementSerilog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //Clear Log
            //SQL Agent Job: Thiết lập một job SQL được lên lịch để xóa các bản log cũ.
            //DELETE FROM Logs WHERE TimeStamp < DATEADD(day, -30, GETDATE());

            //Background Service: Triển khai một hosted service trong ASP.NET Core chạy định kỳ và xóa các bản ghi cũ hơn một ngày cụ thể.
            //Registering the Background service
            builder.Services.AddHostedService<LogCleanupService>();

            // Cấu hình Serilog:
            // - Đọc cấu hình từ appsettings.json sử dụng builder.Configuration.
            // - Thiết lập ghi log ra console và vào một file.
            //Log.Logger = new LoggerConfiguration()             // Tạo một cấu hình logger Serilog mới
            //    .ReadFrom.Configuration(builder.Configuration) // Đọc cài đặt từ appsettings.json
            //    .WriteTo.Console()                             // Ghi log ra console
            //    .WriteTo.File("logs/MyAppLog.txt")             // Ghi log ra một file
            //    .CreateLogger();                               // Xây dựng logger

            // Thay thế nhà cung cấp ghi log mặc định bằng Serilog
            //builder.Host.UseSerilog(); // Điều này đảm bảo Serilog xử lý tất cả các hoạt động ghi log



            //Structured Logging Centralize Configuration

            // Cấu hình host để sử dụng Serilog làm nhà cung cấp ghi nhật ký trước khi xây dựng host.
            // Cài đặt này đọc tất cả các cài đặt Serilog từ appsettings.json.
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                // context: Một thể hiện của HostBuilderContext cung cấp quyền truy cập vào cấu hình và môi trường của ứng dụng,
                //          chẳng hạn như biến môi trường hoặc giá trị appsettings.json.
                // services: IServiceProvider của ứng dụng, được sử dụng cho dependency injection.
                // configuration: Một thể hiện của LoggerConfiguration được sử dụng để cấu hình Serilog.

                // Đọc cài đặt cấu hình cho Serilog từ tệp appsettings.json hoặc bất kỳ nguồn cấu hình nào khác
                // Điều này cho phép đặt các tùy chọn như mức nhật ký, sink và định dạng đầu ra trực tiếp từ các tệp cấu hình.
                configuration.ReadFrom.Configuration(context.Configuration);

                // Tích hợp với container dependency injection, cho phép các sink sử dụng các dịch vụ đã đăng ký khác.
                // Điều này hữu ích nếu bất kỳ sink ghi nhật ký nào yêu cầu các phụ thuộc như cơ sở dữ liệu hoặc ngữ cảnh HTTP.
                configuration.ReadFrom.Services(services);
            });



            // Asynchronous
            // Configure the host to use Serilog as the logging provider .
            builder.Host.UseSerilog((context, services, configuration) =>
            {
                // Read Serilog settings from the appsettings.json file.
                configuration.ReadFrom.Configuration(context.Configuration);
                // Integrate with dependency injection for any sinks that require additional services.
                configuration.ReadFrom.Services(services);
                // Optionally, enable asynchronous logging for additional sinks defined in code.
                configuration.WriteTo.Async(a =>
                {
                    a.Console();  // Wrap Console sink asynchronously.
                    a.File("logs/log-.txt",
                           rollingInterval: RollingInterval.Day,
                           retainedFileCountLimit: 30);  // Wrap File sink asynchronously.
                });
            });




            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
