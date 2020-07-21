using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BethanysPieShop.Auth
{
    public class MinimumOrderAgeRequirement : IAuthorizationRequirement 
    {
        public int minimumOrderAge { get; }

        public MinimumOrderAgeRequirement(int age)
        {
            minimumOrderAge = age;
        }

    }
}
