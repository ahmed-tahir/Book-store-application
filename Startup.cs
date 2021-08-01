using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStoreApplication.Data;
using BookStoreApplication.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookStoreApplication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
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
            services.AddDbContext<BookStoreContext>(options => options.UseSqlServer("Server=TAHIRAHMEDT_I5;Database=BookStore;Integrated Security=True;"));

            // resolving dependencies at run time using Dependency Injection.
            services.AddScoped<BookRepository, BookRepository>();
            services.AddScoped<LanguageRepository, LanguageRepository>();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                //endpoints.MapControllerRoute(
                //    name: "Default",
                //    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
