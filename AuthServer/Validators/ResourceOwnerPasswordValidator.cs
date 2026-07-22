using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthServer.Validators
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            // TEMP: hardcoded user for learning
            if (context.UserName == "test" && context.Password == "1234")
            {
                context.Result = new GrantValidationResult(
                    subject: "1",
                    authenticationMethod: "password",
                    claims: new[]
                    {
                    new Claim("username", context.UserName),
                    new Claim("scope", "api.read"),
                    new Claim("role", "Admin"),       
                    // new Claim("role", "User"),       
                    });

                return Task.CompletedTask;
            }

            context.Result = new GrantValidationResult(
                TokenRequestErrors.InvalidGrant,
                "Invalid credentials");

            return Task.CompletedTask;
        }
    }
}

