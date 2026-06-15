using LekhaChitra.API.Middlewares;
using LekhaChitra.API.Startup;
using LekhaChitra.Application.DependencyInjection;
using LekhaChitra.Infrastructure.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.OpenApi;


var builder = WebApplication.CreateBuilder(args);

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

    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Signin Manager", Version = "v1" });
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
    option.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        //[new OpenApiSecuritySchemeReference("Tenant", document)] = [],
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });

    option.MapType<DateOnly>(() => new OpenApiSchema { Type = JsonSchemaType.String, Format = "date" });
});
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

if (app.Environment.IsDevelopment())
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

app.Run();
