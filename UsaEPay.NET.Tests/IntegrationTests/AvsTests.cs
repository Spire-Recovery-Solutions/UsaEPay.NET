using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.IntegrationTests;

public sealed class AvsTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<AvsTests>()
            .Build();

        _client = new UsaEPayClient(
            _config["API_URL"]!,
            _config["API_KEY"]!,
            _config["API_PIN"]!,
            _config["RANDOM_SEED"]!,
            true);
    }

    [Test, Category("AVS")]
    [Arguments("4000100011112224", "123", "YYY")]
    [Arguments("4000100211112222", "999", "NYZ")]
    [Arguments("4000100411112220", "999", "YNA")]
    [Arguments("4000100511112229", "999", "NNN")]
    [Arguments("4000100811112226", "999", "XXR")]
    [Arguments("4000100911112225", "999", "XXS")]
    public async Task Sale_AvsVariation_ReturnsExpectedCode(string cardNumber, string cvv, string expectedAvs)
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "AVS Test",
            FirstName = "AVS",
            LastName = "Test",
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
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.AVS).IsNotNull();
        await Assert.That(response.AVS!.ResultCode).IsEqualTo(expectedAvs);
    }
}
