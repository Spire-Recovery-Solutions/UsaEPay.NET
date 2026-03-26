using RestSharp;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class FactoryTests
{
    #region CreditCardSaleRequest

    [Test]
    public async Task CreditCardSaleRequest_SetsCorrectEndpoint()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task CreditCardSaleRequest_SetsCorrectCommand()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.TransactionSale);
    }

    [Test]
    public async Task CreditCardSaleRequest_SetsPostMethod()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task CreditCardSaleRequest_MapsBillingAddress()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams);
        await Assert.That(request.BillingAddress).IsNotNull();
        await Assert.That(request.BillingAddress!.FirstName).IsEqualTo("John");
        await Assert.That(request.BillingAddress!.LastName).IsEqualTo("Doe");
        await Assert.That(request.BillingAddress!.Street).IsEqualTo("123 Main St");
        await Assert.That(request.BillingAddress!.City).IsEqualTo("Anytown");
        await Assert.That(request.BillingAddress!.State).IsEqualTo("CA");
        await Assert.That(request.BillingAddress!.PostalCode).IsEqualTo("90210");
        await Assert.That(request.BillingAddress!.Country).IsEqualTo("USA");
        await Assert.That(request.BillingAddress!.Phone).IsEqualTo("5551234567");
    }

    [Test]
    public async Task CreditCardSaleRequest_MapsCreditCard()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams);
        await Assert.That(request.CreditCard).IsNotNull();
        await Assert.That(request.CreditCard!.Number).IsEqualTo("4111111111111111");
        await Assert.That(request.CreditCard!.Cvc).IsEqualTo("123");
        await Assert.That(request.CreditCard!.Expiration).IsEqualTo("1225");
    }

    [Test]
    public async Task CreditCardSaleRequest_WithCustomFields_SetsCustomFields()
    {
        var tranParams = CreateSampleCardParams();
        var customFields = new Dictionary<string, string> { { "custom1", "value1" } };
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams, customFields);
        await Assert.That(request.CustomFields!).IsNotNull();
        await Assert.That(request.CustomFields!["custom1"]).IsEqualTo("value1");
    }

    [Test]
    public async Task CreditCardSaleRequest_WithoutCustomFields_CustomFieldsIsNull()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardSaleRequest(tranParams);
        await Assert.That((object?)request.CustomFields).IsNull();
    }

    #endregion

    #region CheckSaleRequest

    [Test]
    public async Task CheckSaleRequest_SetsCheckFields()
    {
        var tranParams = CreateSampleCheckParams();
        var request = UsaEPayRequestFactory.CheckSaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CheckSale);
        await Assert.That(request.Check).IsNotNull();
        await Assert.That(request.Check!.Number).IsEqualTo("1001");
        await Assert.That(request.Check!.Account).IsEqualTo("123456789");
        await Assert.That(request.Check!.Routing).IsEqualTo("021000021");
        await Assert.That(request.Check!.AccountType).IsEqualTo("checking");
    }

    #endregion

    #region TokenSaleRequest

    [Test]
    public async Task TokenSaleRequest_PassesCustomFields()
    {
        var tranParams = CreateSampleCardParams();
        tranParams.Token = "tok_abc123";
        var customFields = new Dictionary<string, string> { { "field1", "val1" }, { "field2", "val2" } };
        var request = UsaEPayRequestFactory.TokenSaleRequest(tranParams, customFields);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.TransactionSale);
        await Assert.That(request.CreditCard!.Number).IsEqualTo("tok_abc123");
        await Assert.That(request.CustomFields!).IsNotNull();
        await Assert.That(request.CustomFields!.Count).IsEqualTo(2);
    }

    [Test]
    public async Task TokenSaleRequest_WithoutCustomFields_SetsCustomFieldsNull()
    {
        var tranParams = CreateSampleCardParams();
        tranParams.Token = "tok_abc123";
        var request = UsaEPayRequestFactory.TokenSaleRequest(tranParams);
        await Assert.That((object?)request.CustomFields).IsNull();
    }

    #endregion

    #region QuickSaleRequest

    [Test]
    public async Task QuickSaleRequest_WithTransactionKey_SetsTransactionKey()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 10m, TransactionKey = "tkey123" };
        var request = UsaEPayRequestFactory.QuickSaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.QuickSale);
        await Assert.That(request.TransactionKey).IsEqualTo("tkey123");
        await Assert.That(request.ReferenceNumber).IsNull();
    }

    [Test]
    public async Task QuickSaleRequest_WithRefnum_FallsBackToRefnum()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 10m, Refnum = "ref456" };
        var request = UsaEPayRequestFactory.QuickSaleRequest(tranParams);
        await Assert.That(request.ReferenceNumber).IsEqualTo("ref456");
        await Assert.That(request.TransactionKey).IsNull();
    }

    [Test]
    public async Task QuickSaleRequest_WithBoth_PrefersTransactionKey()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 10m, TransactionKey = "tkey123", Refnum = "ref456" };
        var request = UsaEPayRequestFactory.QuickSaleRequest(tranParams);
        await Assert.That(request.TransactionKey).IsEqualTo("tkey123");
        await Assert.That(request.ReferenceNumber).IsNull();
    }

    #endregion

    #region QuickRefundRequest

    [Test]
    public async Task QuickRefundRequest_WithTransactionKey_SetsTransactionKey()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 5m, TransactionKey = "tkey789" };
        var request = UsaEPayRequestFactory.QuickRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.QuickRefund);
        await Assert.That(request.TransactionKey).IsEqualTo("tkey789");
    }

    [Test]
    public async Task QuickRefundRequest_WithRefnum_FallsBackToRefnum()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 5m, Refnum = "ref999" };
        var request = UsaEPayRequestFactory.QuickRefundRequest(tranParams);
        await Assert.That(request.ReferenceNumber).IsEqualTo("ref999");
        await Assert.That(request.TransactionKey).IsNull();
    }

    #endregion

    #region Capture/Void/Refund Commands

    [Test]
    public async Task CapturePaymentRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.CapturePaymentRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CapturePayment);
        await Assert.That(request.TransactionKey).IsEqualTo("tk1");
    }

    [Test]
    public async Task CapturePaymentReauthRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.CapturePaymentReauthRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CapturePaymentReauth);
    }

    [Test]
    public async Task CapturePaymentOverrideRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.CapturePaymentOverrideRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CapturePaymentOverride);
    }

    [Test]
    public async Task CapturePaymentErrorRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.CapturePaymentErrorRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CapturePaymentError);
    }

    [Test]
    public async Task CreditVoidRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.CreditVoidRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CreditVoid);
    }

    [Test]
    public async Task VoidPaymentRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.VoidPaymentRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.VoidPayment);
    }

    [Test]
    public async Task CreditCardRefundRequest_SetsCorrectCommand()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.CreditCardRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CreditCardRefund);
    }

    [Test]
    public async Task CheckRefundRequest_SetsCorrectCommand()
    {
        var tranParams = CreateSampleCheckParams();
        var request = UsaEPayRequestFactory.CheckRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CheckRefund);
    }

    [Test]
    public async Task ConnectedRefundRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 10m, TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.ConnectedRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.Refund);
    }

    [Test]
    public async Task CashRefundRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 10m };
        var request = UsaEPayRequestFactory.CashRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CashRefund);
    }

    [Test]
    public async Task ReleaseFundsRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.ReleaseFundsRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.ReleaseFunds);
    }

    [Test]
    public async Task UnvoidRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.UnvoidRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.Unvoid);
    }

    [Test]
    public async Task AdjustPaymentRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1" };
        var request = UsaEPayRequestFactory.AdjustPaymentRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.AdjustPayment);
    }

    [Test]
    public async Task AdjustPaymentRefundRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk1", Amount = 5m };
        var request = UsaEPayRequestFactory.AdjustPaymentRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.AdjustPaymentRefund);
        await Assert.That(request.Amount).IsEqualTo(5m);
    }

    #endregion

    #region CustomerSaleRequest

    [Test]
    public async Task CustomerSaleRequest_UsesCustkeyAndPaymethodKey()
    {
        var tranParams = new UsaEPayTransactionParams
        {
            Amount = 50m,
            CustomerKey = "cust_abc",
            PaymentMethodKey = "pm_xyz"
        };
        var request = UsaEPayRequestFactory.CustomerSaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CustomerSale);
        await Assert.That(request.CustomerKey).IsEqualTo("cust_abc");
        await Assert.That(request.PaymentMethodKey).IsEqualTo("pm_xyz");
        await Assert.That(request.Amount).IsEqualTo(50m);
    }

    #endregion

    #region GET Request Methods

    [Test]
    public async Task RetrieveTransactionDetailsRequest_UsesGetMethod()
    {
        var tranParams = new UsaEPayTransactionParams { TransactionKey = "tk123" };
        var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("transactions/tk123");
    }

    [Test]
    public async Task RetrieveSpecificBatchRequest_UsesGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveSpecificBatchRequest("batch_key_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("batches/batch_key_1");
    }

    [Test]
    public async Task RetrieveBatchListRequest_UsesGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBatchListRequest(10, 5);
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("batches?limit=10&offset=5");
    }

    [Test]
    public async Task RetrieveCurrentBatchRequest_UsesGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveCurrentBatchRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("batches/current");
    }

    [Test]
    public async Task RetrieveTokenDetailsRequest_UsesGetMethod()
    {
        var tranParams = new UsaEPayTransactionParams { Token = "tok_abc" };
        var request = UsaEPayRequestFactory.RetrieveTokenDetailsRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
        await Assert.That(request.Endpoint).IsEqualTo("tokens/tok_abc");
    }

    #endregion

    #region RetrieveBatchListByDateRequest

    [Test]
    public async Task RetrieveBatchListByDateRequest_BuildsCorrectQueryParams()
    {
        var request = UsaEPayRequestFactory.RetrieveBatchListByDateRequest(
            openedBefore: "2024-01-01",
            openedAfter: "2023-01-01",
            closedBefore: "2024-06-01",
            closedAfter: "2023-06-01",
            limit: 50,
            offset: 10);
        await Assert.That(request.Endpoint).Contains("limit=50");
        await Assert.That(request.Endpoint).Contains("offset=10");
        await Assert.That(request.Endpoint).Contains("openedlt=2024-01-01");
        await Assert.That(request.Endpoint).Contains("openedge=2023-01-01");
        await Assert.That(request.Endpoint).Contains("closedlt=2024-06-01");
        await Assert.That(request.Endpoint).Contains("closedge=2023-06-01");
    }

    [Test]
    public async Task RetrieveBatchListByDateRequest_NullDates_OmitsDateParams()
    {
        var request = UsaEPayRequestFactory.RetrieveBatchListByDateRequest(limit: 20, offset: 0);
        await Assert.That(request.Endpoint).Contains("limit=20");
        await Assert.That(request.Endpoint).Contains("offset=0");
        await Assert.That(request.Endpoint).DoesNotContain("openedlt");
        await Assert.That(request.Endpoint).DoesNotContain("openedge");
        await Assert.That(request.Endpoint).DoesNotContain("closedlt");
        await Assert.That(request.Endpoint).DoesNotContain("closedge");
    }

    #endregion

    #region Helpers

    private static UsaEPayTransactionParams CreateSampleCardParams()
    {
        return new UsaEPayTransactionParams
        {
            Amount = 100.00m,
            CardNumber = "4111111111111111",
            Cvc = "123",
            Expiration = "1225",
            AccountHolder = "John Doe",
            FirstName = "John",
            LastName = "Doe",
            Address = "123 Main St",
            Address2 = "Apt 4",
            City = "Anytown",
            State = "CA",
            Zip = "90210",
            Country = "USA",
            Phone = "5551234567",
            Email = "john@example.com",
            Description = "Test sale",
            Invoice = "INV-001",
            OrderId = "ORD-001",
            ClientIp = "192.168.1.1"
        };
    }

    private static UsaEPayTransactionParams CreateSampleCheckParams()
    {
        return new UsaEPayTransactionParams
        {
            Amount = 75.00m,
            CheckNumber = "1001",
            Account = "123456789",
            Routing = "021000021",
            AccountType = "checking",
            AccountHolder = "Jane Doe",
            FirstName = "Jane",
            LastName = "Doe",
            Address = "456 Oak Ave",
            City = "Othertown",
            State = "NY",
            Zip = "10001",
            Country = "USA",
            Phone = "5559876543",
            Email = "jane@example.com",
            Description = "Check sale test",
            Invoice = "INV-002",
            OrderId = "ORD-002",
            ClientIp = "10.0.0.1"
        };
    }

    #endregion
}
