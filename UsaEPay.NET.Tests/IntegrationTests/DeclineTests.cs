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
        _client.Dispose();
    }

    [Test, Category("Decline")]
    [Arguments("4000300011112220", "999", 10127, "Card Declined")]
    [Arguments("4000300211112228", "999", 10205, "Do not Honor")]
    [Arguments("4000300611112224", "999", 10251, "Insufficient funds")]
    [Arguments("4000300811112222", "999", 10257, "Transaction not permitted")]
    [Arguments("4000300911112221", "999", 10262, "Restricted Card")]
    [Arguments("4000301311112225", "999", 10297, "Declined for CVV failure")]
    public async Task Sale_DeclineCard_ReturnsDeclinedWithErrorCode(string cardNumber, string cvv, long expectedErrorCode, string expectedErrorContains)
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

        // Core decline assertions
        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("D");
        await Assert.That(response.Result).IsEqualTo("Declined");

        // Error code — the key field for programmatic decline handling
        await Assert.That(response.ErrorCode).IsEqualTo(expectedErrorCode);

        // Error text — human-readable decline reason
        await Assert.That(response.Error).IsNotNull();
        await Assert.That(response.Error!).Contains(expectedErrorContains);

        // Decline responses still get a key and refnum
        await Assert.That(response.Key).IsNotNull();
        await Assert.That(response.ReferenceNumber).IsNotNull();
    }
}
