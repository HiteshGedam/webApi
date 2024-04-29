using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApiApp;
using WebApiApp.Data;
using WebApiApp.MyLogging;
 

var builder = WebApplication.CreateBuilder(args);

#region Serilog

Log.Logger = new LoggerConfiguration().MinimumLevel.Information()
    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

//use serilog along with built in logger
builder.Logging.AddSerilog();

//use serilog only logger 
//builder.Host.UseSerilog();
#endregion

builder.Services.AddDbContext<CollegeDBContext>(options =>
 options.UseSqlServer(builder.Configuration.GetConnectionString("CollegeAppDbConnection"))
);

// Add services to the container.

builder.Services.AddControllers(options => options.ReturnHttpNotAcceptable = true).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IMyLogger, LogToServerMemory>();

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
