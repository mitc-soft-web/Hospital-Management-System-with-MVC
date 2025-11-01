using HMS.Identity;
using HMS.Implementation.Repositories;
using HMS.Interfaces.Repositories;
using HMS.Models.Entities;
using HMS.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddDbContext<HmsContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("HMSContext"),
        new MySqlServerVersion(new Version(9, 0, 0))
    ));

//builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IUserStore<User>, HMS.Identity.UserStore>();
builder.Services.AddScoped<IRoleStore<Role>, RoleStore>();
builder.Services.AddIdentity<User, Role>()
    .AddDefaultTokenProviders();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        //ValidIssuer = builder.Configuration["JwtTokenSettings:TokenIssuer"],
        //ValidAudience = builder.Configuration["JwtTokenSettings:TokenIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtTokenSettings:TokenKey"]))
    };
    options.RequireHttpsMetadata = false;
});
builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
