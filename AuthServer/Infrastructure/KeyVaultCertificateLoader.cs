using Azure.Identity;
using Azure.Security.KeyVault.Certificates;
using Azure.Security.KeyVault.Secrets;
using System.Security.Cryptography.X509Certificates;

namespace AuthServer.Certificates
{
    public static class KeyVaultCertificateLoader
    {
        public static X509Certificate2 LoadCertificate(
            string keyVaultUrl,
            string certName)
        {
            ArgumentNullException.ThrowIfNull(keyVaultUrl);
            ArgumentNullException.ThrowIfNull(certName);
            var credential = new DefaultAzureCredential();

            var certClient = new CertificateClient(
                new Uri(keyVaultUrl),
                credential);

            var secretClient = new SecretClient(
                new Uri(keyVaultUrl),
                credential);

            var cert = certClient.GetCertificate(certName);
            var secret = secretClient.GetSecret(certName);

            var privateKeyBytes = Convert.FromBase64String(secret.Value.Value);

            //return new X509Certificate2(
            //    privateKeyBytes,
            //    (string?)null,
            //    X509KeyStorageFlags.MachineKeySet |
            //    X509KeyStorageFlags.EphemeralKeySet
            //);

            return X509CertificateLoader.LoadPkcs12(
              privateKeyBytes,
              password: null,
              keyStorageFlags:
                  X509KeyStorageFlags.MachineKeySet |
                  X509KeyStorageFlags.EphemeralKeySet);
        }
    }
}
