﻿using Common.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Services.Auth;
using Services.Interface.Auth;
using Services.Repository.Auth;
using Swagger.Configuration;
using System.Text;
using System.Web.Http;
using WebApi.Filter;

namespace WebApi
{
    public class Startup
    {
        private const string AllowAllCors = "AllowAllCors";
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }
        public Startup(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddApplicationInsightsTelemetry();

            services.AddDirectoryBrowser();
            services.AddMvc(o => o.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            // Add CORS policy
            services.AddCors(options =>
            {
                options.AddPolicy(AllowAllCors,
                builder =>
                {
                    builder.WithOrigins("https://*")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                     .AllowAnyOrigin()
                     .AllowAnyMethod()
                     .AllowAnyHeader();
                });


            });

            services.AddApiVersioning(setup =>
            {
                setup.DefaultApiVersion = new ApiVersion(1, 0);
                setup.AssumeDefaultVersionWhenUnspecified = true;
                setup.ReportApiVersions = true;
            });

            #region Autentication
            services.AddSwaggerGen(c =>
            {
                c.SchemaFilter<SwaggerExcludeSchemaFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Weelo", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                var jwtKey = Configuration[$"Settings:Jwt:Key"];
                var jwtIssuer = Configuration[$"Settings:Jwt:Issuer"];
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtIssuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });
            #endregion

            #region cors

            services.AddControllers();
            #endregion

            #region CahceSection
            services.AddMemoryCache();
            services.AddResponseCaching();
            #endregion

            #region MvcSection
            services.AddMvc(option =>
            {
                option.EnableEndpointRouting = false;
                option.Filters.Add(new ErrorHandlingFilter());
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });
            #endregion MvcSection


            //Get Settings appSettings.json
            IConfiguration configuration = Configuration;
            Settings options;
            options = configuration.GetSection("Settings").Get<Settings>();
            services.AddSingleton(options);
            // Other configurations
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //Injection dependence Services
            services.AddSingleton<IAuth, AuthService>();
            services.AddSingleton<ICustomer, CustomerService>();
            services.AddSingleton<IProduct, ProductService>();
            services.AddSingleton<ISale, SaleService>();

            //  //Injection dependence Repository
            services.AddSingleton<IAuthRepository, AuthRepository>();
            services.AddSingleton<ICustomerRepository, CustomerRepository>();
            services.AddSingleton<IProductRepository, ProductRepository>();
            services.AddSingleton<ISaleRepository, SaleRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors(AllowAllCors);
            app.UseAuthentication();
            app.UseAuthorization();

            // Also add RequireCors to the controllers:
            app.UseEndpoints(endpoints => { endpoints.MapControllers().RequireCors(AllowAllCors); });

            app.UseStatusCodePages();
            app.UseCors();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/v1/{controller}/{action}/{id?}",
                    defaults: new { id = RouteParameter.Optional });
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute(
            //        name: "Areas",
            //        template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            //    routes.MapRoute(
            //        name: "default",
            //        template: "{controller=Home}/{action=Index}/{id?}");
            //});
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(SwaggerConfiguration.EndpointUrl, SwaggerConfiguration.EndpointDescription);
            });
        }
    }
}
