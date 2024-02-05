using CustomPageApp.Data;
using CustomPageApp.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;

        // Validate the existence of required configuration values
        ValidateConfiguration();
    }

    private void ValidateConfiguration()
    {
        var tokenKey = Configuration["AppSettings:Token"];
        if (string.IsNullOrEmpty(tokenKey))
        {
            throw new InvalidOperationException("AppSettings:Token is missing or null in the configuration.");
        }

        // Additional validation if needed
    }

    // Other methods...
}
class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var connectionString = builder.Configuration.GetConnectionString("ASPDotNetCoreContextConnection") ?? throw new InvalidOperationException("Connection string 'ASPDotNetCoreContextConnection' not found.");
        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        // Add services to the container.
        // Retrieve token key directly from Configuration
        var tokenKey = builder.Configuration["AppSettings:Token"];

        builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.Password.RequiredUniqueChars = 0;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            })
            .AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey!)),
                    
                };
            });
        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = "AspNetCore.Identity.Application";
            options.ExpireTimeSpan = TimeSpan.FromSeconds(30);
            options.SlidingExpiration = true;
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

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
    }

}