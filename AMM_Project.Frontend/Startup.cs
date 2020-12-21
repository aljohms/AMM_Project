using AMM_Project.Frontend.Models;
using AMM_Project.Frontend.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AMM_Project.Frontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie();
            services.AddRazorPages(options => { 
            options.Conventions.AuthorizePage("/index");//Like using Authorize on top of page

        });
            services.AddScoped<IBusinessService, BusinessService>();//add service for loose coupling
            services.AddScoped<IBranchService, BranchService>();//add service for loose coupling
            services.AddScoped<IBranchItemService, BranchItemService>();//add service for loose coupling
            services.AddScoped<IEmployeeService, EmployeeService>();//add service for loose coupling
            services.AddScoped<IEmployeeItemService, EmployeeItemService>();//add service for loose coupling
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            //Configuring Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()//IdentityUser has properties like username, email, and a collection of user Claims. You could also inherit from IdentityUser to add your own custom properties. Identity Role  provides authorization information, like access rights. The default class has properties like Role Name. You can also derive from it if you need to customize it. 
                .AddEntityFrameworkStores<UsersDbContext>()//Tell Identity Services to use entity framework.
                .AddDefaultTokenProviders();//Default Token Providers. These are involved in generating tokens for password reset and two-factor authentication functionality.

            //Configure Identity Options
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });
            // services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("Backend"));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();            //cookie middleware here, and notice that it's done before registering the Mvc middleware so it can redirect to the login page when Mvc detects unauthorized access. 
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
