using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.IntegrationTests;

public sealed class DeclineTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<DeclineTests>()
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

    [Test, Category("Decline")]
    [Arguments("4000300011112220", "999", "Generic decline")]
    [Arguments("4000300211112228", "999", "Do not Honor (code 05)")]
    [Arguments("4000300611112224", "999", "Insufficient funds (code 51)")]
    [Arguments("4000300811112222", "999", "Transaction not permitted (code 57)")]
    [Arguments("4000300911112221", "999", "Restricted card (code 62)")]
    [Arguments("4000301311112225", "999", "CVV failure (code 97)")]
    public async Task Sale_DeclineCard_ReturnsDeclined(string cardNumber, string cvv, string description)
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            AccountHolder = "Test Decline",
            FirstName = "Test",
            LastName = "Decline",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = cardNumber,
            Expiration = "0929",
            Cvc = cvv
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("D");
    }
}
