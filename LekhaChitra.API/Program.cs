using LekhaChitra.API.Middlewares;
using LekhaChitra.API.Startup;
using LekhaChitra.Application.DependencyInjection;
using LekhaChitra.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.OpenApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddInternalDependencies(builder.Configuration);
builder.Services.AddApplication();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    //option.AddSecurityDefinition("Tenant", new OpenApiSecurityScheme
    //{55555555555555555555555555555555\\\\
    //    Name = "X-Tenant-Id",
    //    Type = SecuritySchemeType.ApiKey,
    //    In = ParameterLocation.Header,
    //    Description = "Tenant Id (Hospital Tenant)"
    //});

    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Lekha Chitra", Version = "v1" });
option.AddSecurityDefinition(
    "Bearer",
    new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    }
);
option.AddSecurityRequirement(
    new OpenApiSecurityRequirement
    {
            {
            new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        }
    );
    option.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
});
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Host.UseSerilog(Log.Logger);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
options
.UseSqlServer(connectionString, b =>
                    {
                        b.MigrationsAssembly("LekhaChitra.Infrastructure");
                        b.EnableRetryOnFailure(
                            maxRetryCount: 5,
                            maxRetryDelay: TimeSpan.FromSeconds(10),
                            errorNumbersToAdd: null);
                    }
             );
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

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();       
//    app.MapScalarApiReference(options =>
//    {
//        options
//        .WithTitle("My API")
//        .AddPreferredSecuritySchemes("Bearer")
//         .AddHttpAuthentication("Bearer", scheme =>
//         {
//             scheme.Description = "Enter JWT token";
//         });
//    });


//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var conn = config.GetConnectionString("DefaultConnection");

    if (!string.IsNullOrWhiteSpace(conn))
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
       db.Database.Migrate();
    }
}
app.Run();
