using AuthServer.Config;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Seed
{
    public static class IdentityServerSeeder
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var configDb = scope.ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();

            configDb.Database.Migrate();

            // ---- Identity Resources ----
            if (!configDb.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerConfig.IdentityResources)
                {
                    configDb.IdentityResources.Add(resource.ToEntity());
                }
                configDb.SaveChanges();
            }

            // ---- API Scopes ----
            if (!configDb.ApiScopes.Any())
            {
                foreach (var scopeItem in IdentityServerConfig.ApiScopes)
                {
                    configDb.ApiScopes.Add(scopeItem.ToEntity());
                }
                configDb.SaveChanges();
            }

            // ---- API Resources ----
            if (!configDb.ApiResources.Any())
            {
                foreach (var api in IdentityServerConfig.ApiResources)
                {
                    configDb.ApiResources.Add(api.ToEntity());
                }
                configDb.SaveChanges();
            }

            // ---- Clients ----
            if (!configDb.Clients.Any())
            {
                foreach (var client in IdentityServerConfig.Clients)
                {
                    configDb.Clients.Add(client.ToEntity());
                }
                configDb.SaveChanges();
            }
        }
    }
}
