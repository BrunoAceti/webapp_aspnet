using Microsoft.AspNetCore.Mvc.ViewFeatures;
using webAPP_ASPNET.Data;
using webAPP_ASPNET.Services;

namespace webAPP_ASPNET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            // ✅ ESSENCIAL
            ApiSettings.ApiBaseURL = Configuration["ApiSettings:BaseUrl"];
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddDistributedMemoryCache();
            services.AddSingleton<ITempDataProvider, SessionStateTempDataProvider>();

            services.AddSession(options =>
            {
                options.Cookie.Name = "Session";
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });

            // HttpClient padrão (LoginController usa)
            services.AddHttpClient();

            // HttpClient tipado (PedidoService usa)
            services.AddHttpClient<PedidoService>(client =>
            {
                client.BaseAddress = new Uri(ApiSettings.ApiBaseURL);
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseSession();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}