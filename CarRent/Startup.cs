using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CarRent.Models;
using CarRent.Models.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace CarRent
{
    public class Startup
    {
        private string connString = null;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {

            connString = configuration["DefaultConnection"];

            
            services.AddTransient<CarServices>();

            services.AddDbContext<CarRentContext>(o => o.UseSqlServer(connString, x => x.UseNetTopologySuite()));
            services.AddDbContext<MyIdentityContext>(o => o.UseSqlServer(connString, x => x.UseNetTopologySuite()));
            services.AddIdentity<MyIdentityUser, IdentityRole>(o => 
            {
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 8;
            })
            .AddEntityFrameworkStores<MyIdentityContext>().AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(o => o.LoginPath = "/accounts/login");
            services.AddTransient<AccountsService>();
            services.AddTransient<HomeService>();
            services.AddSession();
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var cultureInfo = new CultureInfo("sv-SE");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseSession();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
            app.UseStaticFiles();

        }
    }
}
