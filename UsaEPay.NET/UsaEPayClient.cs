using System.Text.Json;
using RestSharp;
using UsaEPay.NET.Models;
using UsaEPay.NET.Models.Authentication;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET
{
    public class UsaEPayClient : IDisposable
    {
        private readonly RestClient _restClient;
        private readonly Authentication _authInfo;
        private bool _disposed;

        public UsaEPayClient(string apiUrl, string apiKey, string apiPin, string randomSeed, bool useSandbox = false, int maxTimeout = 20000)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(apiUrl);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);
            ArgumentException.ThrowIfNullOrWhiteSpace(apiPin);
            ArgumentException.ThrowIfNullOrWhiteSpace(randomSeed);

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

        public async Task<T?> SendRequest<T>(IUsaEPayRequest request, CancellationToken cancellationToken = default) where T : IUsaEPayResponse
        {
            ObjectDisposedException.ThrowIf(_disposed, this);

            var restRequest = new RestRequest(request.Endpoint, request.RequestType);

            // Serialize to JSON and set as raw StringBody to avoid RestSharp double-serializing
            var requestBodyJson = JsonSerializer.Serialize(request, request.GetType(), USAePaySerializerContext.Default.Options);
            restRequest.AddStringBody(requestBodyJson, ContentType.Json);

            var response = await _restClient.ExecuteAsync(restRequest, cancellationToken).ConfigureAwait(false);

            if (!response.IsSuccessful)
            {
                throw new UsaEPayApiException(
                    $"API request to {request.Endpoint} failed with status {response.StatusCode}: {response.Content}",
                    response.StatusCode,
                    response.Content);
            }

            if (string.IsNullOrEmpty(response.Content))
            {
                throw new UsaEPayApiException(
                    $"API request to {request.Endpoint} returned empty response",
                    response.StatusCode,
                    response.Content);
            }

            var result = JsonSerializer.Deserialize<T>(response.Content, USAePaySerializerContext.Default.Options);

            if (result is null)
            {
                throw new UsaEPayApiException(
                    $"Failed to deserialize response from {request.Endpoint}",
                    response.StatusCode,
                    response.Content);
            }

            var dateHeader = response.Headers?.FirstOrDefault(f => f.Name == "Date");
            if (dateHeader != null && DateTimeOffset.TryParse(dateHeader.Value?.ToString(), out var timestamp))
            {
                ((IUsaEPayResponse)result).Timestamp = timestamp;
            }

            return result;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _restClient?.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}