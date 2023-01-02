using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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

        public string Seed { get; set; }
        public string ApiKey { get; set; }
        public string ApiPin { get; set; }
        public string AuthKey { get; set; }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
