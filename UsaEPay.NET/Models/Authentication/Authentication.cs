using System.Security.Cryptography;
using System.Text;

namespace UsaEPay.NET.Models.Authentication
{
    public class Authentication
    {
        public Authentication(string seed, string apiKey, string apiPin)
        {
            Seed = seed;
            ApiKey = apiKey;
            ApiPin = apiPin;

            var prehash = ApiKey + Seed + ApiPin;
            var apihash = "s2/" + Seed + '/' + ComputeSha256Hash(prehash);

            byte[] bytes = Encoding.ASCII.GetBytes(ApiKey + ":" + apihash);
            string base64 = Convert.ToBase64String(bytes);

            AuthKey = string.Concat("Basic ", base64);
        }

        public string Seed { get; }
        public string ApiKey { get; }
        public string ApiPin { get; }
        public string AuthKey { get; }

        private static string ComputeSha256Hash(string rawData)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToHexStringLower(bytes);
        }
    }
}
