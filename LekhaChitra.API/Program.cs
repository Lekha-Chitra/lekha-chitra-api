using LekhaChitra.API.Middlewares;
using LekhaChitra.API.Startup;
using LekhaChitra.Application.DependencyInjection;
using LekhaChitra.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInternalDependencies(builder.Configuration);
builder.Services.AddApplication();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LekhaChitra.Infrastructure"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        }
    );
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();       
    app.MapScalarApiReference();
  
}
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
