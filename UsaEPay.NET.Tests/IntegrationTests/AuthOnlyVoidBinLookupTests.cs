using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;
using UsaEPay.NET.Models.Enumerations;

namespace UsaEPay.NET.Tests.IntegrationTests;

/// <summary>
/// Mirrors the consumer portal's card-on-file flow: authorise $0.01 and save the card, release the
/// authorisation (void), then look the transaction up with <c>return_bin=true</c> to read the funding type.
/// </summary>
public sealed class AuthOnlyVoidBinLookupTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<AuthOnlyVoidBinLookupTests>()
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
    public async Task AuthOnlySaveCard_ThenRelease_ThenLookupWithReturnBin_ReturnsFundingType()
    {
        // (a) Authorise $0.01 and save the card.
        var auth = UsaEPayRequestFactory.AuthOnlySaleRequest(new UsaEPayTransactionParams
        {
            Amount = 0.01m,
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
        auth.SaveCard = true;

        var authResponse = await _client.SendRequest<UsaEPayResponse>(auth);

        await Assert.That(authResponse).IsNotNull();
        await Assert.That(authResponse!.ResultCode).IsEqualTo("A");
        await Assert.That(authResponse.Key).IsNotNull().And.IsNotEmpty();
        await Assert.That(authResponse.SavedCard).IsNotNull();
        await Assert.That(authResponse.SavedCard!.Key).IsNotNull().And.IsNotEmpty();

        // (b) Release the authorisation (void and free the funds).
        var release = UsaEPayRequestFactory.ReleaseFundsRequest(
            new UsaEPayTransactionParams { TransactionKey = authResponse.Key });

        var releaseResponse = await _client.SendRequest<UsaEPayResponse>(release);

        await Assert.That(releaseResponse).IsNotNull();
        await Assert.That(releaseResponse!.ResultCode).IsEqualTo("A");

        // (c) Look the same transaction up after the void, asking for the bin.
        var lookup = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = authResponse.Key }, returnBin: true);

        var lookupResponse = await _client.SendRequest<UsaEPayResponse>(lookup);

        await Assert.That(lookupResponse).IsNotNull();
        await Assert.That(lookupResponse!.Bin).IsNotNull();
        await Assert.That(lookupResponse.Bin!.FundingType).IsNotEqualTo(CardFundingType.Unknown);
    }
}
