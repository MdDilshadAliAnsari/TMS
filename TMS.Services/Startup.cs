using TMS.Services.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace TMS.Services
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
         
            services.AddMvc();
            services.AddHttpClient();
            services.AddControllers();



            // Register TokenService with DI 
            services.AddTransient<Project>();
            services.AddTransient<Tassk>();
            services.AddTransient<TasskStatus>();
            services.AddTransient<Comment>();
            services.AddTransient<Document>();
            services.AddDbContext<TMSDbContext>();
            services.AddDbContext<TMSDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(env.WebRootPath, "uploads")),
                RequestPath = "/uploads"
            });

            //app.UseAuthentication();


        }

    }
}
