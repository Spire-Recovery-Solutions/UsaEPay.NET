using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.IntegrationTests;

public sealed class ErrorTests
{
    [Test, Category("Error")]
    public async Task SendRequest_DisposedClient_ThrowsObjectDisposedException()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<ErrorTests>()
            .Build();

        var client = new UsaEPayClient(
            config["API_URL"]!,
            config["API_KEY"]!,
            config["API_PIN"]!,
            config["RANDOM_SEED"]!,
            true);

        client.Dispose();

        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "Test User",
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123"
        });

        await Assert.That(async () => await client.SendRequest<UsaEPayResponse>(request))
            .ThrowsExactly<ObjectDisposedException>();
    }

    [Test, Category("Error")]
    public async Task SendRequest_CancelledToken_ThrowsOperationCanceledException()
    {
        var config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<ErrorTests>()
            .Build();

        var client = new UsaEPayClient(
            config["API_URL"]!,
            config["API_KEY"]!,
            config["API_PIN"]!,
            config["RANDOM_SEED"]!,
            true);

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "Test User",
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123"
        });

        await Assert.That(async () => await client.SendRequest<UsaEPayResponse>(request, cts.Token))
            .ThrowsException();
    }
}
