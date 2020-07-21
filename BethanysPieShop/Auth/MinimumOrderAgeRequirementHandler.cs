using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanysPieShop.Auth
{
    public class MinimumOrderAgeRequirementHandler : AuthorizationHandler<MinimumOrderAgeRequirement>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public MinimumOrderAgeRequirementHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            MinimumOrderAgeRequirement requirement)
        {
            ApplicationUser user = await _userManager.GetUserAsync(context.User);
            var ageInYears = DateTime.Today.Year - user.BirthDay.Year;
            if (ageInYears >= requirement.minimumOrderAge)
            {
                context.Succeed(requirement);
            }
        }
    }
}
