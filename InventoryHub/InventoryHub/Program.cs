using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using InventoryHub;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// Add services to the container.
builder.Services.AddCors(options => {
  options.AddPolicy("AllowAll",
      policy => policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader());
});

builder.Services.AddControllersWithViews();
builder.Services.AddRazorComponents();
builder.Services.AddControllers();

// Add JSON serializer options to handle nested objects.
builder.Services.AddControllers().AddJsonOptions(options => {
  options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
