using ApoyoClinicoCol.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ApoyoClinicoCol.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Leemos la cadena de conexión llamada "DefaultConnection" desde appsettings
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            // 2. Registramos nuestro AppDbContext indicando que usará SQL Server
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

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