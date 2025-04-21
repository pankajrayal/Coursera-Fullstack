using InventoryHub2.Components;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient("InventoryHub2", client => {
  client.BaseAddress = new Uri("https://localhost:7075/");
});
builder.Services.AddMemoryCache(); // Add memory cache
builder.Services.AddCors(options => {
  options.AddPolicy("AllowAll",
      policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
});

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if(!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Error", createScopeForErrors: true);
  app.UseHsts();
}

app.UseCors("AllowAll");
app.UseStaticFiles();
app.UseRouting();

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Minimal API Endpoint with Caching
app.MapGet("/api/productlist", (IMemoryCache cache) => {
  const string cacheKey = "ProductList";
  if(!cache.TryGetValue(cacheKey, out object? cachedProducts)) {
    var products = new[]
    {
            new
            {
                Id = 1,
                Name = "Laptop",
                Price = 1200.50,
                Stock = 25,
                Category = new { Id = 101, Name = "Electronics" }
            },
            new
            {
                Id = 2,
                Name = "Headphones",
                Price = 50.00,
                Stock = 100,
                Category = new { Id = 102, Name = "Accessories" }
            }
        };

    var cacheEntryOptions = new MemoryCacheEntryOptions()
        .SetSlidingExpiration(TimeSpan.FromMinutes(5)); // Cache for 5 minutes
    cache.Set(cacheKey, products, cacheEntryOptions);

    return products;
  }

  return cachedProducts;
});

app.Run();