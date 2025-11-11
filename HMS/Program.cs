using HMS.Identity;
using HMS.Implementation.Repositories;
using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Scan(scan => scan
    .FromApplicationDependencies(a => a.FullName!.StartsWith("HMS"))
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Service")))
        .AsImplementedInterfaces()
        .WithScopedLifetime()
    .AddClasses(c => c.Where(t => t.Name.EndsWith("Repository")))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

//builder.Services.AddScoped<IBaseRepository, BaseRespository>()
//    .AddScoped<IPatientRepository, PatientRepository>()
//    .AddScoped<IDoctorRepository, DoctorRepository>()
//    .AddScoped<IRoleRepository, RoleRepository>()
//    .AddScoped<ISpecialityRepository, SpecialityRepository>()
//    .AddScoped<IUserRepository, UserRepository>()
//    .AddScoped<IPatientRepository, PatientRepository>();

//builder.Services.AddScoped<IPatientService, PatientService>()
//    .AddScoped<IRoleService, RoleService>()
//    .AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add Database
//builder.Services.AddDbContext<HmsContext>(options =>
//    options.UseMySql(builder.Configuration.GetConnectionString("HMSContext"),
//        new MySqlServerVersion(new Version(9, 0, 0))
//    ));




var connectionString = builder.Configuration.GetConnectionString("HmsConnection");

// Detect if it’s a Render-style URL (postgres://...) and convert
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');

    var npgsqlBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port,
        Username = userInfo[0],
        Password = userInfo.Length > 1 ? userInfo[1] : "",
        Database = uri.AbsolutePath.Trim('/'),
        SslMode = SslMode.Require,
        TrustServerCertificate = true
    };

    connectionString = npgsqlBuilder.ConnectionString;
}

builder.Services.AddDbContext<HmsContext>(options =>
    options.UseNpgsql(connectionString));



//builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserStore<User>, HMS.Identity.UserStore>();
builder.Services.AddScoped<IRoleStore<Role>, RoleStore>();
builder.Services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
  .AddCookie(config =>
  {
      config.LoginPath = "/User/login";
      config.Cookie.Name = "HMS";
      config.LogoutPath = "/User/logout";
      config.ExpireTimeSpan = TimeSpan.FromMinutes(15);
      config.SlidingExpiration = true;
  });
builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (builder.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}")
    .WithStaticAssets();



app.Run();
