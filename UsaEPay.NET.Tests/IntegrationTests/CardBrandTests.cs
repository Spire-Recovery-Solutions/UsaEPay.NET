using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.IntegrationTests;

public sealed class CardBrandTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<CardBrandTests>()
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

    [Test, Category("CardBrand")]
    public async Task Sale_Visa_Approved_WithAvsAndCvv()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
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

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Avs).IsNotNull();
        await Assert.That(response.Avs!.ResultCode).IsEqualTo("YYY");
        await Assert.That(response.Cvc).IsNotNull();
        await Assert.That(response.Cvc!.ResultCode).IsEqualTo("M");
    }

    [Test, Category("CardBrand")]
    public async Task Sale_MasterCard_Approved_WithCvvMatch()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "Jane Doe",
            FirstName = "Jane",
            LastName = "Doe",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = "5555444433332226",
            Expiration = "0929",
            Cvc = "123"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Cvc).IsNotNull();
        await Assert.That(response.Cvc!.ResultCode).IsEqualTo("M");
    }

    [Test, Category("CardBrand")]
    public async Task Sale_Amex_Approved_WithCvvMatch()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "Jane Doe",
            FirstName = "Jane",
            LastName = "Doe",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = "371122223332225",
            Expiration = "0929",
            Cvc = "1234"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Cvc).IsNotNull();
        await Assert.That(response.Cvc!.ResultCode).IsEqualTo("M");
    }

    [Test, Category("CardBrand")]
    public async Task Sale_Discover_Approved_WithCvvMatch()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1,
            AccountHolder = "Jane Doe",
            FirstName = "Jane",
            LastName = "Doe",
            Address = "555 Test Street",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            CardNumber = "6011222233332224",
            Expiration = "0929",
            Cvc = "123"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Cvc).IsNotNull();
        await Assert.That(response.Cvc!.ResultCode).IsEqualTo("M");
    }
}
