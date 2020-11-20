using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using AutoMapper;
using Business.DIExtension;
using Business.Mapping;
using Data.DataContext;
using Data.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using ViewModel.ServiceModel;
using Web.Helper.Cookie;
using Web.Helper.JWT;

namespace Web
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
            //JWT Auth:::
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddIdentity<AppIdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

            }).AddEntityFrameworkStores<EmployeeServiceContext>().AddDefaultTokenProviders();

            var hostingEnvironment = services.BuildServiceProvider().GetService<IHostingEnvironment>();
            services.AddDataProtection(options => options.ApplicationDiscriminator = $"{hostingEnvironment.ApplicationName}")
                .SetApplicationName($"{hostingEnvironment.ApplicationName}");

            services.AddScoped<IDataSerializer<AuthenticationTicket>, TicketSerializer>();

            var tokenValidationParam = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Issuer"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            })
            .AddCookie(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromMinutes(30);
                options.TicketDataFormat = new JwtAuthTicketFormat(tokenValidationParam, services.BuildServiceProvider().GetService<IDataSerializer<AuthenticationTicket>>(),
                services.BuildServiceProvider().GetDataProtector(new[] { $"{hostingEnvironment.ApplicationName}-Auth1" }));

                options.SlidingExpiration = true;
                options.LoginPath = "/Login";
                options.LogoutPath = "/Logout";
                options.AccessDeniedPath = options.LoginPath;
                options.ReturnUrlParameter = "returnUrl";
            });

            services.AddHttpContextAccessor();

            //========= Initilizing auto mapper =============
            services.AddAutoMapper(typeof(AutoMapperProfile));

            //========= Inject config file =============
            services.AddSingleton<IConfiguration>(Configuration);

            //========= Injected Services =============
            services.AddInjectedServices();
            services.AddTransient<ICookieHelper, CookieHelper>();

            //========= DbContext starts =============
            services.AddDbContext<EmployeeServiceContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ESSServerConnection"))).AddUnitOfWork<EmployeeServiceContext>();
            services.AddDbContext<ServiceContext>(options => options.UseSqlServer(Configuration.GetConnectionString("HRServerConnection")));

            //========= Auto validation =============
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                        );

                    return new BadRequestObjectResult(new
                    {
                        status = false,
                        message = "Invalid records. Please check for validation error",
                        errors
                    });
                };
            });

            //========= Enable HttpClient =============
            services.AddHttpClient();

            //========= Add CORS =============
            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //Enable CORS
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });
        }
    }
}
