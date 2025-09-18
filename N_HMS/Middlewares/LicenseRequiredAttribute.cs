using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using N_HMS.Database;
using N_HMS.Interfaces;
using N_HMS.Services;

namespace N_HMS.Middlewares
{
    public class LicenseRequiredAttribute : TypeFilterAttribute
    {
        public LicenseRequiredAttribute() : base(typeof(LicenseRequiredFilter)) { }
    }

    public class LicenseRequiredFilter : IAsyncActionFilter
    {
        private readonly ILicenseService _licenseService;

        public LicenseRequiredFilter(ILicenseService licenseService)
        {
            _licenseService = licenseService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var request = context.HttpContext.Request;
            if (!request.Headers.TryGetValue("X-Token", out var licenseToken))
            {
                context.Result = new UnauthorizedObjectResult(new { message = "License token missing" });
                return;
            }

            var license = _licenseService.ValidateLicenseToken(licenseToken);
            if (license == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = "Invalid or expired license" });
                return;
            }

            await next();
        }
    }
}
