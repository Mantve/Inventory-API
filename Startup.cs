using Inventory_API.Data;
using Inventory_API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Inventory_API.Data.Repositories;
using Microsoft.OpenApi.Models;

namespace Inventory_API
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new OpenApiInfo { Title = "My API" });
            }); 
            services.AddCors();
            services.AddDbContext<RestContext>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddTransient<IRoomRepository, RoomRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<JwtService>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })                        // Adding Jwt Bearer
                        .AddJwtBearer(options =>
                        {
                            options.SaveToken = true;
                            options.RequireHttpsMetadata = false;
                            options.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    context.Token = context.Request.Cookies["jwt"];
                                    return Task.CompletedTask;
                                }
                            };
                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
                                ValidateIssuer = false,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("JwtSecret")))
                            };
                        });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, RestContext restContext)
        {
            restContext.Database.Migrate();
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseCors((options => options
                .WithOrigins(new[] { "http://localhost:4200", "https://stuff.mantve.eu/", "https://mantve.github.io/Inventory-webapp" })
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
            ));
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
            });
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; private set; }
    }
}
