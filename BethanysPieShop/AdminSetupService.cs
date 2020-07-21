using BethanysPieShop.Auth;
using BethanysPieShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BethanysPieShop
{
    public class AdminSetupService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AdminSetupService> _logger;

        public AdminSetupService(ILogger<AdminSetupService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                UserManager<ApplicationUser> userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                ApplicationUser user = await userManager.FindByNameAsync("admin@admin.net");
                if (user == null)
                {
                    _logger.LogWarning("Unable to find user: admin@admin.net");
                    IdentityResult result = await userManager.CreateAsync(new ApplicationUser()
                    {
                        UserName = "admin@admin.net",
                        Email = "admin@admin.net"
                    }, "24WilloW!");

                    if (!result.Succeeded)
                    {
                        _logger.LogError("Unable to create user: admin@admin.net");
                        throw new System.Exception(result.Errors.ToString());
                    }
                    _logger.LogInformation("Created user: admin@admin.net");
                    _logger.LogInformation("Set password: 24WilloW!");
                    user = await userManager.FindByNameAsync("admin@admin.net");
                }
                _logger.LogInformation("Found user: " + user.UserName);
                if (!await userManager.IsInRoleAsync(user, "Administrator"))
                {
                    _logger.LogWarning("User admin@admin.net is not an Administrator");
                    IdentityRole role = await roleManager.FindByNameAsync("Administrator");
                    if (role == null)
                    {
                        _logger.LogWarning("Unable to find role: Administrator");
                        IdentityResult result = await roleManager.CreateAsync(new IdentityRole()
                        {
                            Name = "Administrator"
                        });

                        if (!result.Succeeded)
                        {
                            _logger.LogError("Unable to create role: Administrator");
                            throw new System.Exception(result.Errors.ToString());
                        }
                        _logger.LogInformation("Created role: Administrator");
                        role = await roleManager.FindByNameAsync("Administrator");
                    }
                    _logger.LogInformation("Adding Administrator role to admin user.");
                    await userManager.AddToRoleAsync(user, role.Name);
                }
            }

        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}