using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetStore.Data;
using PetStore.Data.Common.Repos;
using PetStore.Data.Models;
using PetStore.Data.Repositories;
using PetStore.Services.Data;

namespace PetStore.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplication app = ConfigureServices(args);

            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
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
            app.MapRazorPages();

            app.Run();
        }

        private static WebApplication ConfigureServices(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder
                .Services
                .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            builder
                .Services
                .AddDatabaseDeveloperPageExceptionFilter();

            builder
                .Services
                .AddDefaultIdentity<IdentityUser>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder
                .Services
                .AddControllersWithViews();

            builder.Services.AddAutoMapper(typeof(Program));

            // Repositories
            builder
                .Services
                .AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            builder
                .Services
                .AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));

            //Services
            builder
                .Services
                .AddTransient<ICategoryService, CategoryService>();

            var app = builder.Build();
            return app;
        }
    }
}