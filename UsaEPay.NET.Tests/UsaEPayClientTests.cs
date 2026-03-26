using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests;

[NotInParallel]
public sealed class UsaEPayClientTests
{
    private IConfiguration _config = null!;
    private UsaEPayClient _client = null!;

    // Shared state captured across ordered tests
    private static string _sToken = string.Empty;
    private static string _sTransKey = string.Empty;
    private static string _sTransAuthKey = string.Empty;
    private static string _sTranCheckKey = string.Empty;
    private static string _sBatchKey = string.Empty;

    [Before(Test)]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<UsaEPayClientTests>()
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

    // ── Tokenization ────────────────────────────────────────────────

    [Test, Category("Token")]
    public async Task TokenizeCard_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.TokenizeCardRequest(new UsaEPayTransactionParams
        {
            CardHolder = "John Doe",
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");

        if (response.ResultCode == "A")
        {
            _sToken = response.SavedCard!.Key!;
        }
    }

    [Test, Category("Token"), DependsOn(nameof(TokenizeCard_ReturnsApproved))]
    public async Task TokenSale_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.TokenSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            FirstName = "John",
            LastName = "Doe",
            Address = "555 Test Street",
            Address2 = "",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            Cvc = "123",
            Token = _sToken
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Credit Card Sale ────────────────────────────────────────────

    [Test, Category("Sale")]
    public async Task CardSale_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            FirstName = "John",
            LastName = "Doe",
            AccountHolder = "John Doe",
            Address = "555 Test Street",
            Address2 = "",
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

        if (response.ResultCode == "A")
        {
            _sTransKey = response.Key!;
        }
    }

    [Test, Category("Sale"), DependsOn(nameof(CardSale_ReturnsApproved))]
    public async Task QuickSale_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.QuickSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Check Sale ──────────────────────────────────────────────────

    [Test, Category("Sale")]
    public async Task CheckSale_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CheckSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            FirstName = "Remus",
            LastName = "Lupin",
            Address = "555 Test Street",
            Address2 = "",
            City = "Testington",
            State = "OK",
            Zip = "33242",
            Routing = "123456789",
            Account = "324523524",
            AccountType = "checking",
            CheckNumber = "101"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");

        if (response.ResultCode == "A")
        {
            _sTranCheckKey = response.Key!;
        }
    }

    // ── Auth Only ───────────────────────────────────────────────────

    [Test, Category("Sale")]
    public async Task AuthOnlySale_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.AuthOnlySaleRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            AccountHolder = "John Doe",
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123",
            FirstName = "John",
            LastName = "Doe",
            Address = "555 Test Street",
            Address2 = "Street 2",
            City = "Testington",
            State = "OK",
            Zip = "33242"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");

        if (response.ResultCode == "A")
        {
            _sTransAuthKey = response.Key!;
        }
    }

    // ── Refunds ─────────────────────────────────────────────────────

    [Test, Category("Refund")]
    public async Task CreditCardRefund_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CreditCardRefundRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            CardHolder = "John Doe",
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Refund")]
    public async Task CheckRefund_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CheckRefundRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            AccountHolder = "John Doe",
            AccountNumber = "234234",
            Routing = "123456789",
            AccountType = "checking",
            CheckNumber = "101"
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Refund"), DependsOn(nameof(CardSale_ReturnsApproved))]
    public async Task QuickRefund_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.QuickRefundRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Refund"), DependsOn(nameof(AuthOnlySale_ReturnsApproved))]
    public async Task ConnectedRefund_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.ConnectedRefundRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            Email = "test@.com",
            ClientIp = "10.1.0.1",
            TransactionKey = _sTransAuthKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Refund"), DependsOn(nameof(CheckSale_ReturnsApproved))]
    public async Task AdjustPaymentRefund_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.AdjustPaymentRefundRequest(new UsaEPayTransactionParams
        {
            Amount = 10,
            TransactionKey = _sTranCheckKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Capture ─────────────────────────────────────────────────────

    [Test, Category("Capture"), DependsOn(nameof(CardSale_ReturnsApproved))]
    public async Task CapturePayment_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CapturePaymentRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Capture"), DependsOn(nameof(CapturePayment_ReturnsApproved))]
    public async Task CapturePaymentError_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CapturePaymentErrorRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Capture"), DependsOn(nameof(CapturePaymentError_ReturnsApproved))]
    public async Task CapturePaymentReauth_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CapturePaymentReauthRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Capture"), DependsOn(nameof(CapturePaymentReauth_ReturnsApproved))]
    public async Task CapturePaymentOverride_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CapturePaymentOverrideRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Adjust ──────────────────────────────────────────────────────

    [Test, Category("Adjust"), DependsOn(nameof(CapturePaymentOverride_ReturnsApproved))]
    public async Task AdjustPayment_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.AdjustPaymentRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Retrieve Details ────────────────────────────────────────────

    [Test, Category("RetrieveDetails"), DependsOn(nameof(AdjustPayment_ReturnsApproved))]
    public async Task RetrieveTransactionDetails_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Void ────────────────────────────────────────────────────────

    [Test, Category("Void"), DependsOn(nameof(RetrieveTransactionDetails_ReturnsApproved))]
    public async Task CreditVoid_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.CreditVoidRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Void"), DependsOn(nameof(CheckSale_ReturnsApproved))]
    public async Task VoidPayment_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.VoidPaymentRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTranCheckKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Void"), DependsOn(nameof(CreditVoid_ReturnsApproved))]
    public async Task Unvoid_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.UnvoidRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    [Test, Category("Void"), DependsOn(nameof(Unvoid_ReturnsApproved))]
    public async Task ReleaseFunds_ReturnsApproved()
    {
        var request = UsaEPayRequestFactory.ReleaseFundsRequest(new UsaEPayTransactionParams
        {
            TransactionKey = _sTransKey
        });

        var response = await _client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
    }

    // ── Transaction List ────────────────────────────────────────────

    [Test, Category("ListTransactions"), DependsOn(nameof(ReleaseFunds_ReturnsApproved))]
    public async Task TransactionList_ReturnsData()
    {
        var request = UsaEPayRequestFactory.RetrieveTransactionsRequest(5);

        var response = await _client.SendRequest<UsaEPayListTransactionResponse>(request);

        await Assert.That(response).IsNotNull();
    }

    // ── Batch ───────────────────────────────────────────────────────

    [Test, Category("BatchList")]
    public async Task BatchList_ReturnsData()
    {
        var request = UsaEPayRequestFactory.RetrieveBatchListRequest();

        var response = await _client.SendRequest<UsaEPayBatchListResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Data).IsNotNull();
        await Assert.That(response.Data!.Length).IsGreaterThan(0);

        _sBatchKey = response.Data.First().Key!;
    }

    [Test, Category("BatchListByDate")]
    public async Task BatchListByDate_ReturnsData()
    {
        var request = UsaEPayRequestFactory.RetrieveBatchListByDateRequest(
            openedBefore: "20240201",
            openedAfter: "20230101");

        var response = await _client.SendRequest<UsaEPayBatchListResponse>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Data).IsNotNull();
    }

    [Test, Category("RetrieveSpecificBatch"), DependsOn(nameof(BatchList_ReturnsData))]
    public async Task RetrieveSpecificBatch_ReturnsOpen()
    {
        var request = UsaEPayRequestFactory.RetrieveSpecificBatchRequest(_sBatchKey);

        var response = await _client.SendRequest<Batch>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Status).IsEqualTo("open");
    }

    [Test, Category("RetrieveCurrentBatch")]
    public async Task RetrieveCurrentBatch_ReturnsOpen()
    {
        var request = UsaEPayRequestFactory.RetrieveCurrentBatchRequest();

        var response = await _client.SendRequest<Batch>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Status).IsEqualTo("open");
    }

    [Test, Category("CloseCurrentBatch"),
     DependsOn(nameof(RetrieveCurrentBatch_ReturnsOpen)),
     DependsOn(nameof(RetrieveSpecificBatch_ReturnsOpen)),
     DependsOn(nameof(RetrieveBatchTransactionsById_ReturnsData))]
    public async Task CloseCurrentBatch_ReturnsClosing()
    {
        var request = UsaEPayRequestFactory.CloseCurrentBatchRequest();

        var response = await _client.SendRequest<Batch>(request);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Status).IsEqualTo("closing");
    }

    [Test, Category("BatchTransactions"), DependsOn(nameof(BatchList_ReturnsData))]
    public async Task RetrieveBatchTransactionsById_ReturnsData()
    {
        var request = UsaEPayRequestFactory.RetrieveBatchTransactionsByIdRequest(_sBatchKey, 5, 1);

        var response = await _client.SendRequest<UsaEPayBatchTransactionResponse>(request);

        await Assert.That(response).IsNotNull();
    }
}
