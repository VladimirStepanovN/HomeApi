using System.Reflection;
using FluentValidation.AspNetCore;
using HomeApi.Configuration;
using HomeApi.Contracts.Validation;
using HomeApi.Data;
using HomeApi.Data.Repos;
using Microsoft.EntityFrameworkCore;

namespace HomeApi
{
    public class Program
    {
        /// <summary>
        /// �������� ������������ �� ����� Json
        /// </summary>
        private static IConfiguration _configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile("HomeOptions.json")
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json").Build();

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var assembly = Assembly.GetAssembly(typeof(MappingProfile));

            // Add services to the container.

            // ���������� �����������
            builder.Services.AddAutoMapper(assembly);

            // ���������� ������������
            builder.Configuration.AddConfiguration(_configuration);

            // ����������� ������� ����������� ��� �������������� � ����� ������
            builder.Services.AddSingleton<IDeviceRepository, DeviceRepository>();
            builder.Services.AddSingleton<IRoomRepository, RoomRepository>();
            string connection = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<HomeApiContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

            // ���������� ���������
            builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddDeviceRequestValidator>());

            // ��������� ����� ������
            builder.Services.Configure<HomeOptions>(builder.Configuration);

            // ��������� ������ ������ (��������� Json-������))
            builder.Services.Configure<Address>(builder.Configuration.GetSection("Address"));

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
