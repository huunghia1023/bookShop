using bookShopSolution.Application.Catalog.Products;
using bookShopSolution.Application.Common;
using bookShopSolution.Application.System.Users;
using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddDbContext<BookShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<BookShopDbContext>().AddDefaultTokenProviders();
// add DI
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<IPublicProductService, PublicProductService>();
builder.Services.AddTransient<IManageProductService, ManageProductService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IUserService, UserService>();
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