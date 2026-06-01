
using LekhaChitra.API.Startup;
using LekhaChitra.Application.DependencyInjection;
using LekhaChitra.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInternalDependencies(builder.Configuration);
builder.Services.AddApplication();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("LekhaChitra.Infrastructure"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();       
    app.MapScalarApiReference();
  
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
