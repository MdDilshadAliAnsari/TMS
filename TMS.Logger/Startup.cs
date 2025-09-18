using Microsoft.EntityFrameworkCore;
using TMS.Logger.Model;

namespace TMS.Logger
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
           
            services.AddMvc();
            services.AddDbContext<TMSDbContext>(); 
            services.AddDbContext<TMSDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DbConnection")));
        }

        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();


        }
    }
}
