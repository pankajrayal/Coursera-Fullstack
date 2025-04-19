using Microsoft.EntityFrameworkCore;
using UserManagementAPI.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS policy
builder.Services.AddCors(options => {
  options.AddPolicy("AllowAllOrigins",
      builder => {
        builder.AllowAnyOrigin()
                 .AllowAnyMethod()
                 .AllowAnyHeader();
      });
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("");
app.UseAuthorization();
app.MapControllers();

app.Run();