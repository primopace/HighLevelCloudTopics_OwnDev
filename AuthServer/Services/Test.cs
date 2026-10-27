using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using System.Security.Cryptography.X509Certificates;

namespace AuthServer.Services
{
    public class Test
    {



        // ==========================================
        // 1. EXTRACT CREDENTIALS FROM CONFIGURATION
        // ==========================================
        // Pulling the secret credentials from appsettings.json or environment variables






        //string tenantId = builder.Configuration["AzureAd:TenantId"]!;
        //string clientId = builder.Configuration["AzureAd:ClientId"]!;
        //string clientSecret = builder.Configuration["AzureAd:ClientSecret"]!;
        //string keyVaultUrl = builder.Configuration["KeyVault:VaultUri"]!;
        //string certificateName = "identityserver-signing-cert";







        // ==========================================
        // 2. AUTHENTICATE WITH MICROSOFT ENTRA ID
        // ==========================================
        // ClientSecretCredential explicitly targets cross-cloud scenarios.
        // It wraps your credentials into a handler that knows how to talk to Entra ID.





        //var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);











        // ==========================================
        // 3. INITIALIZE KEY VAULT CLIENT
        // ==========================================
        // This client establishes a pathway to your specific Key Vault instance.
        // It passes the credential object created above to handle future handshakes.





        //var certificateClient = new CertificateClient(new Uri(keyVaultUrl), credential);






        // ==========================================
        // 4. DOWNLOAD THE SIGNING CERTIFICATE
        // ==========================================
        // This network call triggers Azure.Identity to request an access token from Entra ID,
        // attaches it to the Key Vault request, and downloads the full PFX certificate (including Private Key).


        //X509Certificate2 signingCertificate = certificateClient.DownloadCertificate(certificateName);




        // ==========================================
        // 5. REGISTER DUENDE IDENTITYSERVER
        // ==========================================




    //    builder.Services.AddIdentityServer()
    //.AddConfigurationStore(options => { /* ... DB Config ... */ })
    //.AddOperationalStore(options => { /* ... DB Operational ... */ })
    //// Injects the extracted Key Vault certificate into Duende's token engine.
    //.AddSigningCredential(signingCertificate);

    }
}
