using BethanysPieShop.Auth;
using BethanysPieShop.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Threading.Tasks;

namespace BethanysPieShop
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();

            services.AddScoped<IPieRepository, PieRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddHttpContextAccessor();
            services.AddSession();

            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})
            //.AddCookie()
            //.AddOpenIdConnect("Auth0", options =>
            //{
            //IConfigurationSection Auth0Section = Configuration.GetSection("Authentication:Auth0");

            //// Set the authority to your Auth0 domain
            //options.Authority = $"https://{Auth0Section["Domain"]}";

            //// Configure the Auth0 Client ID and Client Secret
            //options.ClientId = Auth0Section["ClientId"];
            //options.ClientSecret = Auth0Section["ClientSecret"];


            //    // Set response type to code
            //    options.ResponseType = OpenIdConnectResponseType.Code;

            //    // Configure the scope
            //    options.Scope.Add("openid");

            //    // Set the callback path, so Auth0 will call back to http://localhost:3000/callback
            //    // Also ensure that you have added the URL as an Allowed Callback URL in your Auth0 dashboard
            //    options.CallbackPath = new PathString("/callback");

            //    // Configure the Claims Issuer to be Auth0
            //    options.ClaimsIssuer = "Auth0";


            //    // Saves tokens to the AuthenticationProperties
            //    //options.SaveTokens = true;

            //    options.Events = new OpenIdConnectEvents
            //    {
            //        // handle the logout redirection 
            //        OnRedirectToIdentityProviderForSignOut = (context) =>
            //        {
            //            var logoutUri = $"https://{Configuration["Auth0:Domain"]}/v2/logout?client_id={Configuration["Auth0:ClientId"]}";

            //            var postLogoutUri = context.Properties.RedirectUri;
            //            if (!string.IsNullOrEmpty(postLogoutUri))
            //            {
            //                if (postLogoutUri.StartsWith("/"))
            //                {
            //                    // transform to absolute
            //                    var request = context.Request;
            //                    postLogoutUri = request.Scheme + "://" + request.Host + request.PathBase + postLogoutUri;
            //                }
            //                logoutUri += $"&returnTo={ Uri.EscapeDataString(postLogoutUri)}";
            //            }

            //            context.Response.Redirect(logoutUri);
            //            context.HandleResponse();

            //            return Task.CompletedTask;
            //        }
            //    };
            //});

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddScoped<IAuthorizationHandler, MinimumOrderAgeRequirementHandler>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdministratorOnly", policy => policy.RequireRole("Administrator"));
                options.AddPolicy("DeletePie", policy => policy.RequireClaim("Delete Pie", "Delete Pie"));
                options.AddPolicy("AddPie", policy => policy.RequireClaim("Add Pie", "Add Pie"));
                options.AddPolicy("MinimumOrderAge", policy => policy.Requirements.Add(new MinimumOrderAgeRequirement(18)));
            });

            services.AddHostedService<AdminSetupService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoints.MapRazorPages();
            });
        }
    }
}
