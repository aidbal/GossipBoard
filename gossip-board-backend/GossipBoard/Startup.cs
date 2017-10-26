using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using GossipBoard.Models;
using GossipBoard.Repository;
using GossipBoard.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;

namespace GossipBoard
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            void Configure(IApplicationBuilder app, ILoggerFactory loggerFactory)
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();

                app.UseExceptionHandler();
                app.UseAuthentication();
                

                //app.UseJwtBearerAuthentication(new JwtBearerOptions
                //{
                //    TokenValidationParameters = tokenValidationParameters
                //});

                //app.UseCookieAuthentication(new CookieAuthenticationOptions
                //{
                //    AutomaticAuthenticate = false,
                //    AutomaticChallenge = false
                //});


                app.UseMvc();
            }
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>(x =>
                {
                    x.Password.RequiredLength = 5;
                    x.Password.RequireDigit = false;
                    x.Password.RequireUppercase = false;
                    x.Password.RequireLowercase = false;
                    x.Password.RequireNonAlphanumeric = false;
                    x.User.RequireUniqueEmail = true;
                }).AddEntityFrameworkStores<GossipBoardContext>();
            // Add framework services.

            // secretKey contains a secret passphrase only your server knows
            var secretKey = Configuration.GetSection("JWTSettings:SecretKey").Value;
            var issuer = Configuration.GetSection("JWTSettings:Issuer").Value;
            var audience = Configuration.GetSection("JWTSettings:Audience").Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = issuer,

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audience
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = tokenValidationParameters;
                });
            services.AddAutoMapper();
            services.AddMvc();
            var connection = @"Server=(localdb)\mssqllocaldb;Database=GossipBoard;Trusted_Connection=True;";
            services.AddDbContext<GossipBoardContext>(options => options.UseSqlServer(connection));
            services.AddCors();
            services.AddSwaggerGen(c => c.SwaggerDoc("gossip_board_application", new Info()));
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IPostRepository, PostRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            app.UseCors(builder => builder.WithOrigins("http://localhost", "http://localhost:4200").AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials() // essential for SignalR!!!
            );
            app.UseAuthentication();
            app.UseMvc();
            var swaggerUrl = "/swagger/gossip_board_application/swagger.json";
            app.UseSwagger()
                .UseSwaggerUI(c =>
                {
                    c.DocExpansion("none");
                    c.SwaggerEndpoint(swaggerUrl, "Gossip Demo Application");
                });
        }
    }
}
