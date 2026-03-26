using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.IntegrationTests;

public sealed class PartialAuthTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<PartialAuthTests>()
            .Build();

        _client = new UsaEPayClient(
            _config["API_URL"]!,
            _config["API_KEY"]!,
            _config["API_PIN"]!,
            _config["RANDOM_SEED"]!,
            true);
    }

    [After(Test)]
    public void Teardown()
    {
        _client?.Dispose();
    }

    [Test, Category("PartialAuth")]
    public async Task Sale_PartialAuth50Percent_ReturnsPartialApproval()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 100.00m,
            AccountHolder = "Partial Test",
            FirstName = "Partial",
            LastName = "Test",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = "4000000011112275",
            Expiration = "0929",
            Cvc = "999"
        });
        request.AmountDetail = new AmountDetail { EnablePartialAuth = true };

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("P");
        await Assert.That(response.AmountAuthorized).IsNotNull();

        var authAmount = decimal.Parse(response.AmountAuthorized!, System.Globalization.CultureInfo.InvariantCulture);
        await Assert.That(authAmount).IsEqualTo(50.00m);
    }

    [Test, Category("PartialAuth")]
    public async Task Sale_PartialAuth75Percent_ReturnsPartialApproval()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 100.00m,
            AccountHolder = "Partial Test",
            FirstName = "Partial",
            LastName = "Test",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = "4000000011112283",
            Expiration = "0929",
            Cvc = "999"
        });
        request.AmountDetail = new AmountDetail { EnablePartialAuth = true };

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("P");
        await Assert.That(response.AmountAuthorized).IsNotNull();

        var authAmount = decimal.Parse(response.AmountAuthorized!, System.Globalization.CultureInfo.InvariantCulture);
        await Assert.That(authAmount).IsEqualTo(75.00m);
    }
}
