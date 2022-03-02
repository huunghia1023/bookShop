using bookShopSolution.Application.Catalog.Products;
using bookShopSolution.Application.Common;
using bookShopSolution.Application.System.Users;
using bookShopSolution.BackendApi.IdentityServer;
using bookShopSolution.Data.EF;
using bookShopSolution.Data.Entities;
using bookShopSolution.Utilities.Constants;
using bookShopSolution.ViewModels.System.Users;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<BookShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));
//builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<BookShopDbContext>().AddDefaultTokenProviders();
// add DI
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<IPublicProductService, PublicProductService>();
builder.Services.AddTransient<IManageProductService, ManageProductService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IUserService, UserService>();
//builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();

// Add services to the container.
builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

// Identity Server 4
// add identity EF
builder.Services.AddIdentity<AppUser, AppRole>() // use custome user and role
            .AddEntityFrameworkStores<BookShopDbContext>() // declare DbContext
                                                           // add default token provider used to generate tokens for reset password,
                                                           // change mail and telephone number, operations and for 2FA token generation
            .AddDefaultTokenProviders();
// add identity server
builder.Services.AddIdentityServer(options => // custome event for identity server
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
})
    .AddInMemoryApiResources(Config.Apis) // using in-memory resources
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddAspNetIdentity<AppUser>()// declare user using identity server
    .AddDeveloperSigningCredential();
// add authenticate config for using scheme jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["AuthorityUrl"];
    options.TokenValidationParameters.ValidateAudience = false;
});

// add author
builder.Services.AddAuthorization();

// add swagger
builder.Services.AddSwaggerGen(c =>
{
    // add swagger ui
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Description = "Swagger UI for BookShop Web",
        Title = "Swagger BookShop ",
        Version = "1.0.0"
    });

    // add identity server to swagger
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = @"Sign In to get authorize",
        Name = "Authorization",
        Type = SecuritySchemeType.OAuth2,
        Scheme = "oauth2",
        Flows = new OpenApiOAuthFlows
        {
            Password = new OpenApiOAuthFlow
            {
                TokenUrl = new Uri(builder.Configuration["AuthorityUrl"] + "/connect/token"),
                Scopes = new Dictionary<string, string>
                {
                    //{ "api.BackendApi", "Backend API" }
                },
            }
        }
    });

    // add identity server to swagger
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
             },
             new List<string>{"api.BackendApi" }
        }
    });
});
//string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
//string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
//byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);
//builder.Services.AddAuthentication(opt =>
//{
//    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(option =>
//{
//    option.RequireHttpsMetadata = false;
//    option.SaveToken = true;
//    option.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = issuer,
//        ValidateAudience = true,
//        ValidAudience = issuer,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ClockSkew = System.TimeSpan.Zero,
//        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
//    };
//});

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

// security
app.UseIdentityServer();

app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.OAuthClientId("swagger");
    c.OAuthScopeSeparator(" ");
    c.OAuthClientSecret("swagger_RookiesB4_BookShopBackendApi");
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger BookShop v1");
});
//app.UseSwaggerUI(c =>
//{
//    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger bookShopSolution v1");
//});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();