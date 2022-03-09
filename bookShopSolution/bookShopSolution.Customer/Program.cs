using bookShopSolution.Customer.LocalizationResources;
using bookShopSolution.Customer.Services;
using bookShopSolution.ViewModels.System.Users;
using FluentValidation.AspNetCore;
using IdentityServer4.AccessTokenValidation;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden/";
    });
var cultures = new[]
           {
                new CultureInfo("en"),
                new CultureInfo("vi"),
            };
// Add services to the container.
builder.Services.AddControllersWithViews()
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
        o.DefaultRequestCulture = new RequestCulture("vi");
    };
})
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE.ENVIROMENT");
#if DEBUG
if (enviroment == Environments.Development)
{
    builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
}
#endif

//add session to store token
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});

// add DI
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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

app.UseRequestLocalization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();