var builder = WebApplication.CreateBuilder(args);
// Add CORS services
builder.Services.AddCors(options => {
  options.AddPolicy("AllowAll",
      policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
});
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

app.MapGet("/api/products", () => {
  return new[]
  {
        new { Id = 1, Name = "Laptop", Price = 1200.50, Stock = 25 },
        new { Id = 2, Name = "Headphones", Price = 50.00, Stock = 100 }
    };
});

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
