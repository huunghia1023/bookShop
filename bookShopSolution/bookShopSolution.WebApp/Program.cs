using bookShopSolution.ViewModels.System.Users;
using bookShopSolution.WebApp.LocalizationResources;
using bookShopSolution.WebApp.Services;
using FluentValidation.AspNetCore;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IOrderApiClient, OrderApiClient>();

var cultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("vi")
};

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ProductCreateRequestValidator>())
    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(ops =>
{
    // When using all the culture providers, the localization process will
    // check all available culture providers in order to detect the request culture.
    // If the request culture is found it will stop checking and do localization accordingly.
    // If the request culture is not found it will check the next provider by order.
    // If no culture is detected the default culture will be used.

    // Checking order for request culture:
    // 1) RouteSegmentCultureProvider
    //      e.g. http://localhost:1234/tr
    // 2) QueryStringCultureProvider
    //      e.g. http://localhost:1234/?culture=tr
    // 3) CookieCultureProvider
    //      Determines the culture information for a request via the value of a cookie.
    // 4) AcceptedLanguageHeaderRequestCultureProvider
    //      Determines the culture information for a request via the value of the Accept-Language header.
    //      See the browsers language settings

    // Uncomment and set to true to use only route culture provider
    ops.UseAllCultureProviders = false;
    ops.ResourcesPath = "LocalizationResources";
    ops.RequestLocalizationOptions = o =>
    {
        o.SupportedCultures = cultures;
        o.SupportedUICultures = cultures;
        o.DefaultRequestCulture = new RequestCulture("en");
    };
});

// cookie principals
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.LoginPath = "/Account/Login";
                 options.AccessDeniedPath = "/User/Forbidden/";
             });

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
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
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.UseRequestLocalization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "Order en",
    pattern: "{culture}/orders", new
    {
        controller = "Order",
        action = "Index"
    });
    endpoints.MapControllerRoute(
    name: "Product category en",
    pattern: "{culture}/categories/{id}", new
    {
        controller = "Product",
        action = "Category"
    });
    endpoints.MapControllerRoute(
        name: "Product category vi",
        pattern: "{culture}/danh-muc/{id}", new
        {
            controller = "Product",
            action = "Category"
        });
    endpoints.MapControllerRoute(
        name: "Product detail en",
        pattern: "{culture}/products/{id}", new
        {
            controller = "Product",
            action = "Detail"
        });
    endpoints.MapControllerRoute(
        name: "Product detail vi",
        pattern: "{culture}/san-pham/{id}", new
        {
            controller = "Product",
            action = "Detail"
        });
    endpoints.MapControllerRoute(
        name: "Product detail vi",
        pattern: "{culture}/san-pham/{id}", new
        {
            controller = "Product",
            action = "Detail"
        });
    endpoints.MapControllerRoute(
        name: "Login",
        pattern: "{culture}/Account/Login", new
        {
            controller = "Account",
            action = "Login"
        });
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{culture=en}/{controller=Home}/{action=Index}/{id?}");
});

app.Run();