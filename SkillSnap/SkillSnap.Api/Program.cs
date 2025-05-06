using Microsoft.EntityFrameworkCore;
using SkillSnap.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Configure MSSQL Server
builder.Services.AddDbContext<SkillSnapContext>(options => {
  options.UseSqlServer(builder.Configuration.GetConnectionString("SkillSnapDb"));
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
