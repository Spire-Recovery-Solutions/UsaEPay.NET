using Newtonsoft.Json;
using RestSharp;
using UsaEPay.NET.Models;
using UsaEPay.NET.Models.Authentication;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET
{
    public class UsaEPayClient
    {
        private readonly RestClient _restClient;
        private readonly Authentication _authInfo;

        // TODO: Technically the apiUrl is also called apiKey on the dev dashboard... need to figure out what to do here with naming.
        public UsaEPayClient(string apiUrl, string apiKey, string apiPin, string randomSeed, bool useSandbox = false, int maxTimeout = 20000)
        {
            _authInfo = new Authentication(randomSeed, apiKey.Trim(), apiPin.Trim());

            var target = useSandbox ? "sandbox" : "secure";
            var baseUrl = new Uri($"https://{target}.usaepay.com/api/{apiUrl}/");

            var restClientOptions = new RestClientOptions
            {
                MaxTimeout = maxTimeout,
                ThrowOnDeserializationError = true,
                ThrowOnAnyError = true,
                BaseUrl = baseUrl
            };

            _restClient = new RestClient(restClientOptions);
            _restClient.AddDefaultHeader("Authorization", _authInfo.AuthKey);
        }

        public async Task<T?> SendRequest<T>(IUsaEPayRequest request) where T : IUsaEPayResponse
        {
            var restRequest = new RestRequest(request.Endpoint, request.RequestType);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(request));
            var response = await _restClient.ExecuteAsync(restRequest);

            var result = JsonConvert.DeserializeObject<T>(response.Content);
            result.Timestamp = DateTimeOffset.Parse(response.Headers.FirstOrDefault(f => f.Name == "Date").Value.ToString());
            return result;
        }

    }
}