using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;
using UsaEPay.NET.Models.Enumerations;

namespace UsaEPay.NET.Tests.IntegrationTests;

public sealed class ReturnBinTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<ReturnBinTests>()
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

    [Test, Category("ReturnBin")]
    public async Task Sale_ThenLookupWithReturnBin_ReturnsFundingType()
    {
        var sale = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "John Doe",
            FirstName = "John",
            LastName = "Doe",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123"
        });

        var saleResponse = await _client.SendRequest<UsaEPayResponse>(sale);

        await Assert.That(saleResponse).IsNotNull();
        await Assert.That(saleResponse!.ResultCode).IsEqualTo("A");
        await Assert.That(saleResponse.Key).IsNotNull();
        // return_bin is ignored on a POST, so the sale response itself never carries the bin.
        await Assert.That(saleResponse.Bin).IsNull();

        var lookup = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = saleResponse.Key }, returnBin: true);

        var lookupResponse = await _client.SendRequest<UsaEPayResponse>(lookup);

        await Assert.That(lookupResponse).IsNotNull();
        await Assert.That(lookupResponse!.Bin).IsNotNull();
        await Assert.That(lookupResponse.Bin!.FundingType).IsNotEqualTo(CardFundingType.Unknown);
    }
}
