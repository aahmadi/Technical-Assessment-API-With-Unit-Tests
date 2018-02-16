using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ali.Planning.API.Repositories;
using Ali.Planning.API.Repositories.Interfaces;
using Entities;
using Entities.TestData;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            // Add configuration service
            services.AddSingleton(_config);

            // Register the data context as a service to be able to inject it
            services.AddDbContext<PlanningDataContext>(ServiceLifetime.Scoped);

            // Register repositories
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IPlanningRepository, PlanningRepository>();

            // Seed with sample data
            services.AddTransient<DatabaseInitializer>();

            // Register AutoMapper
            //services.AddAutoMapper();

            // Register Identity system for PlanningUser and IdentityRole
            services.AddIdentity<PlanningUser, IdentityRole>()
                .AddEntityFrameworkStores<PlanningDataContext>();

            //-----------------------

            var symKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Tokens:key").Value));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _config.GetSection("Tokens:Issuer").Value,

                ValidateAudience = true,
                ValidAudience = _config.GetSection("Tokens:Audience").Value,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = symKey,

                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = _config.GetSection("Tokens:Issuer").Value;
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

    
            //----------------
            // Add CORS policies
            services.AddCors(cfg =>
            {
                cfg.AddPolicy("Any", bldr =>
                {
                    bldr.AllowAnyHeader()
                    .AllowAnyMethod()
                    .WithOrigins("http://localhost:4200");
                });

            });

            services.AddMvc(opt =>
            {
                opt.Filters.Add(new RequireHttpsAttribute());
            })
            //avoid circular referencing.
            .AddJsonOptions(opt => {
                opt.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Seed the database
           // databaseInializer.Seed().Wait();

            // Add Identity before Mvc middleware to protect the APIs
            //app.UseIdentity();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
