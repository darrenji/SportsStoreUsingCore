using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SportsStoreUsingCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace SportsStoreUsingCore
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup(IHostingEnvironment env)
        {
            //首先配置
            Configuration = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json").Build(); 
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //EF上下文放到IO容器，读配置文件
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration["Data:SportStoreProducts:ConnectionString"]));


            //依赖设置
            //每一次请求过来到达控制器都会生出一个实例来
            services.AddTransient<IProductRepository, FakeRepository>();

            //有关购物车
            //在同一个请求生成同一个实例，不同请求生成不同实例
            services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
            //所有请求都是同一个实例
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            

            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            //app.UseMvcWithDefaultRoute();
            app.UseSession();

            app.UseMvc(routes =>
            {
                // /Soccer/Page2
                routes.MapRoute(
                    name:null,
                    template:"{category}/Page{page:int}",
                    defaults:new { controller="Product", action="List"});

                // /page2
                routes.MapRoute(
                    name: null,
                    template:"Page{page:int}",
                    defaults: new { controller="Product", action="List", page=1});

                // /Soccer
                routes.MapRoute(
                    name:null,
                    template:"{category}",
                    defaults: new { controller="Product", action="List", page=1});

                // /
                routes.MapRoute(
                    name:null,
                    template:"",
                    defaults:new { controller="Product", action="List", page=1});

                routes.MapRoute(
                    name: null,
                    template:"{controller}/{action}/{id?}");
            });


            //种子数据
            SeedData.EnsurePopulated(app);
        }
    }
}
