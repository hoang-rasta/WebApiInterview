
using Microsoft.OpenApi.Models;

namespace WebApiProject
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

            // Thêm các dịch vụ Swagger
            builder.Services.AddEndpointsApiExplorer();  // Cho phép trích xuất metadata từ controller
            builder.Services.AddSwaggerGen();            // Tạo tài liệu Swagger từ metadata

            //Tùy chỉnh thông tin tài liệu Swagger
            builder.Services.AddSwaggerGen(c =>
            {

                // Swagger mặc định V1
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "Default API",
                    Version = "V1",
                    Description = "Default Swagger Document"
                });

                // Swagger tùy chỉnh V10
                c.SwaggerDoc("V10", new OpenApiInfo
                {
                    Title = "My Custom API",
                    Version = "V10",
                    Description = "Tài liệu mô tả các API trong hệ thống",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Bộ phận hỗ trợ",
                        Email = "support@example.com",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Giấy phép sử dụng",
                        Url = new Uri("https://example.com/license")
                    }
                });
            });



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Chỉ bật Swagger khi đang ở môi trường Development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();           // Xuất file JSON mô tả API (swagger.json)
                //app.UseSwaggerUI();         // Hiển thị UI trực quan từ file JSON trên


                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/V1/swagger.json", "Default API V1");
                    c.SwaggerEndpoint("/swagger/V10/swagger.json", "My Custom API V10");

                    // (Tùy chọn) Thiết lập tài liệu mặc định được chọn khi mở UI
                    c.RoutePrefix = "swagger"; // hoặc "" nếu bạn muốn Swagger ở root
                });

            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
