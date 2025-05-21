
using WebApi_ReturnTypes_StatusCodes.Models;

namespace WebApi_ReturnTypes_StatusCodes
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

            var app = builder.Build();

            //Register the Middleware
            // Các phương thức được phép
            var allowedMethods = new[] { "GET", "POST", "DELETE" };

            // Sử dụng một lambda để khởi tạo middleware thủ công
            app.Use(async (context, next) =>
            {
                var middleware = new HttpMethodMiddleware(next, allowedMethods);
                await middleware.InvokeAsync(context);
            });



            // Registering the Filter Globally:
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new HttpMethodFilter(new[] { "GET", "POST", "DELETE" }));
            });


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
