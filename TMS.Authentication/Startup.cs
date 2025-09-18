using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TMS.Authentication.Model;
using Microsoft.EntityFrameworkCore;
using TMS.Authentication.Authenticate;
using Microsoft.IdentityModel.Logging;

namespace TMS.Authentication
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
            IdentityModelEventSource.ShowPII = true;
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            }); 
            services.AddMvc();
            services.AddHttpClient();
            services.AddControllers();
            services.AddHttpClient();
            services.AddHttpClient<ProxyController>(); 
            // Register TokenService with DI 
            services.AddScoped<IAuthenticationUserNew, NewAuthUser>();
            //services.AddTransient<IAuthenticationUser, AuthenticationUser>();
            services.AddDbContext<TMSDbContext>();
            services.AddDbContext<TMSDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DbConnection")));

        }
 
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment()|| env.IsProduction())
            {
                app.UseDeveloperExceptionPage(); 
                IdentityModelEventSource.ShowPII = true;
               
            }
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // or MapDefaultControllerRoute();
            });
            app.UseAuthentication(); app.Run(context => {
                var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

                var document = @"<!doctype html>
<html>
<body>
<strong>Hello world</strong>
</body>
</html>";

                context.Response.ContentLength = encoding.GetByteCount(document);
                context.Response.ContentType = "text/html;charset=UTF-8";

                return context.Response.WriteAsync(document, encoding, context.RequestAborted);
            });


        }
    }
}
