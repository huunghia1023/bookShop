using bookShopSolution.Application.Catalog.Products;
using bookShopSolution.Data.EF;
using bookShopSolution.Utilities.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<BookShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));
// add DI
builder.Services.AddTransient<IPublicProductService, PublicProductService>();
// Add services to the container.
builder.Services.AddControllersWithViews();

// add swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger bookShop Solution", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger bookShopSolution v1");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
