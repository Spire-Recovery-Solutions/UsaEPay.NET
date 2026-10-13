using System.Net;
using System.Text;
using RestSharp;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;
using UsaEPay.NET.Models.Enumerations;

namespace UsaEPay.NET.Tests.UnitTests;

/// <summary>
/// <c>return_bin</c> only works as a query parameter on a transaction GET (USAePay ignores it on a
/// POST). These tests cover the request the factory builds and, through the real
/// <see cref="UsaEPayClient.SendRequest{T}"/>, the URL that actually reaches the HTTP layer and the
/// deserialization of the sandbox fixtures returned for it. No network is used: a capturing
/// <see cref="HttpMessageHandler"/> stands in for USAePay.
/// </summary>
public sealed class ReturnBinRequestTests
{
    #region Request factory

    [Test]
    public async Task RetrieveTransactionDetailsRequest_ReturnBinTrue_AddsReturnBinQuery()
    {
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = "tk123" }, returnBin: true);

        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("transactions/tk123?return_bin=true");
    }

    [Test]
    public async Task RetrieveTransactionDetailsRequest_ReturnBinFalse_IsUnchanged()
    {
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = "tk123" }, returnBin: false);

        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("transactions/tk123");
    }

    [Test]
    public async Task RetrieveTransactionDetailsRequest_WithoutFlag_IsUnchanged()
    {
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = "tk123" });

        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("transactions/tk123");
    }

    [Test]
    public async Task SaleRequest_DoesNotCarryReturnBin()
    {
        // return_bin is ignored on a POST by USAePay, so the SDK must never add it there.
        var sale = UsaEPayRequestFactory.CreditCardSaleRequest(new UsaEPayTransactionParams
        {
            Amount = 1m,
            CardNumber = "4000100011112224",
            Expiration = "0929",
            Cvc = "123"
        });

        var json = System.Text.Json.JsonSerializer.Serialize(sale, UsaEPay.NET.Models.UsaEPaySerializerContext.Default.Options);

        await Assert.That(sale.Endpoint).DoesNotContain("return_bin");
        await Assert.That(json).DoesNotContain("return_bin");
    }

    #endregion

    #region What reaches the wire (through SendRequest)

    [Test]
    public async Task SendRequest_ReturnBinTrue_PutsQueryStringOnTheWire_AndReadsBin()
    {
        var handler = new CapturingHandler(TestDataFiles.Read(TestDataFiles.ApprovedWithBin));
        using var client = CreateClient(handler);
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = "anf4mkmdjg7bsj4" }, returnBin: true);

        var response = await client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(handler.Method).IsEqualTo(HttpMethod.Get);
        await Assert.That(handler.RequestUri!.AbsolutePath).IsEqualTo("/api/v2/transactions/anf4mkmdjg7bsj4");
        await Assert.That(handler.RequestUri.Query).IsEqualTo("?return_bin=true");
        await Assert.That(handler.RequestUri.Host).IsEqualTo("sandbox.usaepay.com");

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Bin).IsNotNull();
        await Assert.That(response.Bin!.FundingType).IsEqualTo(CardFundingType.Credit);
    }

    [Test]
    public async Task SendRequest_WithoutFlag_SendsNoQueryString_AndHasNoBin()
    {
        var handler = new CapturingHandler(TestDataFiles.Read(TestDataFiles.ApprovedWithoutBin));
        using var client = CreateClient(handler);
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = "anf4mkmdjg7bsj4" });

        var response = await client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(handler.Method).IsEqualTo(HttpMethod.Get);
        await Assert.That(handler.RequestUri!.AbsolutePath).IsEqualTo("/api/v2/transactions/anf4mkmdjg7bsj4");
        await Assert.That(handler.RequestUri.Query).IsEqualTo(string.Empty);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("A");
        await Assert.That(response.Bin).IsNull();
    }

    [Test]
    public async Task SendRequest_DeclinedTransaction_WithBin_ReadsFundingType()
    {
        var handler = new CapturingHandler(TestDataFiles.Read(TestDataFiles.DeclinedWithBin));
        using var client = CreateClient(handler);
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(
            new UsaEPayTransactionParams { TransactionKey = "dnfb2j57n4gqnfk" }, returnBin: true);

        var response = await client.SendRequest<UsaEPayResponse>(request);

        await Assert.That(handler.RequestUri!.Query).IsEqualTo("?return_bin=true");
        await Assert.That(response).IsNotNull();
        await Assert.That(response!.ResultCode).IsEqualTo("D");
        await Assert.That(response.Bin).IsNotNull();
        await Assert.That(response.Bin!.FundingType).IsEqualTo(CardFundingType.Credit);
    }

    #endregion

    private static UsaEPayClient CreateClient(HttpMessageHandler handler) =>
        new("v2", "testkey", "1234", "randomseed", useSandbox: true, maxTimeout: 20000, messageHandler: handler);

    /// <summary>
    /// Records the request that reaches the HTTP layer and answers 200 with a canned JSON body.
    /// </summary>
    private sealed class CapturingHandler(string responseJson) : HttpMessageHandler
    {
        public HttpMethod? Method { get; private set; }
        public Uri? RequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            Method = request.Method;
            RequestUri = request.RequestUri;

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(responseJson, Encoding.UTF8, "application/json")
            });
        }
    }
}
