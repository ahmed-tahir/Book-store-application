using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApplication.Data;
using BookStoreApplication.Helpers;
using BookStoreApplication.Models;
using BookStoreApplication.Repository;
using BookStoreApplication.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStoreApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // adding Identity core services
            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<BookStoreContext>()
                    .AddDefaultTokenProviders();

            // configuring Identity using IdentityOptions
            services.Configure<IdentityOptions>(options => 
            {
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;

                // allows only verified email ids to sign in
                options.SignIn.RequireConfirmedEmail = true;

                // locking out the user after a given number of failed login attempts
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromDays(1);
            });

            // setting the token life span
            services.Configure<DataProtectionTokenProviderOptions>(options => 
            {
                options.TokenLifespan = TimeSpan.FromHours(1);   
            });

            // redirect user to login page if authorization fails
            services.ConfigureApplicationCookie(config => 
            {
                config.LoginPath = _configuration["Application:LoginPath"];
            });

            // tells our application to use controllers and views.
            services.AddControllersWithViews();

            #if DEBUG
            // enables runtime compilation of razor views.
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //.AddViewOptions((options) => 
            //{
            //    // disabling client-side validations.
            //    options.HtmlHelperOptions.ClientValidationEnabled = false;
            //});

            #endif

            // enables using entity framework by proving a Dbcontext class.
            services.AddDbContext<BookStoreContext>(options => options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection")));

            // resolving dependencies at run time using Dependency Injection.
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationUserClaimsPrincipalFactory>();

            // services related to read data from configuration file
            services.Configure<SMTPModel>(_configuration.GetSection("SMTPConfig"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            // Identity core authentication feature
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();

                endpoints.MapDefaultControllerRoute();

                //endpoints.MapControllerRoute(
                //    name: "Default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "MyArea",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
