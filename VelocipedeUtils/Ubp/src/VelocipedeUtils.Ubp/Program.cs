using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using VelocipedeUtils.Ubp.Core.DbContexts;
using VelocipedeUtils.Ubp.Core.Domain.DatasetGenerators;
using VelocipedeUtils.Ubp.Core.Domain.Filtering;
using VelocipedeUtils.Ubp.Core.Models.Configurations;
using VelocipedeUtils.Ubp.Core.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Get configurations.
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.{environment}.json").Build();
var appsettings = configuration.GetSection("AppSettings").Get<AppSettings>()
    ?? throw new Exception($"Cannot start the application: '{nameof(AppSettings)}' section is not specified in the config file");

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.LoginPath = "/Auth/SignIn";
    });

// Add dependencies.
builder.Services.AddSingleton(appsettings);
builder.Services.AddSingleton<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<DatasetGenerator>();
builder.Services.AddTransient<ICommonDataFilter, CommonDataFilter>();
builder.Services.AddDbContext<UbpDbContext>(options => options.UseNpgsql(appsettings.ConnectionString));

var app = builder.Build();

// Initialize datasets.
var datasetGenerator = app.Services.GetService<DatasetGenerator>()
    ?? throw new Exception($"Cannot start the application: '{nameof(DatasetGenerator)}' is not initialized");
datasetGenerator.Initialize();

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
app.UseAuthorization();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
