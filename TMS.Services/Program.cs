using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Net;
using TMS.Services;
using TMS.Services.Model;

var builder = WebApplication.CreateBuilder(args);

 

// Add services to the container. 
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
 builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add following to enable DI
builder.Services.AddDbContext<TMSDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddTransient<Project>();
builder.Services.AddTransient<Tassk>();
builder.Services.AddTransient<TasskStatus>();
builder.Services.AddTransient<Comment>();
builder.Services.AddTransient<Document>();

    // Add API versioning
    builder.Services.AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;  // Header info for clients to see the supported versions
    });


// Ensure WebRootPath is set
builder.Environment.WebRootPath ??= Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"); 
var app = builder.Build();


// Serve static files from wwwroot/uploads
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.WebRootPath ?? "wwwroot", "uploads")),
    RequestPath = "/uploads"
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
 
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
 

app.Run();
