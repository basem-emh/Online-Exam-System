using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExamSystem.BLL.Services.Admin;
using OnlineExamSystem.DAL.Entities.Identity;
using OnlineExamSystem.DAL.Persistence.Data.Contexts;
using OnlineExamSystem.DAL.Persistence.Data.DataSeed;
using OnlineExamSystem.DAL.Persistence.Repositories;
using OnlineExamSystem.DAL.Persistence.Unit_Of_Work;
using OnlineExamSystem.PL.Helpers.Auto_Mapper;

namespace OnlineExamSystem.PL
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region Configure Services 
            
            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>

                options
                      .UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                      .UseLazyLoadingProxies()
             );

            builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            })
                .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
                .AddDefaultTokenProviders();
            
            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options=>
                {
                    options.LoginPath = "Account/Login";
                    options.AccessDeniedPath = "Home/Error";
                });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped(typeof(IAdminServices), typeof(AdminServices));
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MappingProfilies));

            #endregion

            var app = builder.Build();

            #region ApplyAllPending Migrations [Update-Database] and Data Seeding

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var _identityDbContext = services.GetRequiredService<ApplicationIdentityDbContext>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();
            try
            {
                await _identityDbContext.Database.MigrateAsync(); // Update-Database

                var _userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                
                await ApplicationIdentityDataSeed.SeedUserAsync(_userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "An error has been occured during appling the migration");
            }

            #endregion

            #region Connfigure Kestrel Middlewares 
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
            #endregion

            app.Run();
        }
    }
}