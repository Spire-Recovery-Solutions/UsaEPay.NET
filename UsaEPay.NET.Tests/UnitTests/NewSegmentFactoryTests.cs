using RestSharp;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class NewSegmentFactoryTests
{
    #region CashSaleRequest

    [Test]
    public async Task CashSaleRequest_SetsCorrectEndpoint()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 25m };
        var request = UsaEPayRequestFactory.CashSaleRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task CashSaleRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 25m };
        var request = UsaEPayRequestFactory.CashSaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CashSale);
    }

    [Test]
    public async Task CashSaleRequest_SetsAmount()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 25m };
        var request = UsaEPayRequestFactory.CashSaleRequest(tranParams);
        await Assert.That(request.Amount).IsEqualTo(25m);
    }

    [Test]
    public async Task CashSaleRequest_SetsPostMethod()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 25m };
        var request = UsaEPayRequestFactory.CashSaleRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region AuthOnlySaleRequest

    [Test]
    public async Task AuthOnlySaleRequest_SetsCorrectEndpoint()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.AuthOnlySaleRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task AuthOnlySaleRequest_SetsCorrectCommand()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.AuthOnlySaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.AuthOnlySale);
    }

    [Test]
    public async Task AuthOnlySaleRequest_SetsCreditCardFields()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.AuthOnlySaleRequest(tranParams);
        await Assert.That(request.CreditCard).IsNotNull();
        await Assert.That(request.CreditCard!.Number).IsEqualTo("4111111111111111");
        await Assert.That(request.CreditCard!.Cvc).IsEqualTo("123");
        await Assert.That(request.CreditCard!.Expiration).IsEqualTo("1225");
    }

    [Test]
    public async Task AuthOnlySaleRequest_SetsPostMethod()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.AuthOnlySaleRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region PostPaymentRequest

    [Test]
    public async Task PostPaymentRequest_SetsCorrectEndpoint()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.PostPaymentRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task PostPaymentRequest_SetsCorrectCommand()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.PostPaymentRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.PostPayment);
    }

    [Test]
    public async Task PostPaymentRequest_SetsCreditCardFields()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.PostPaymentRequest(tranParams);
        await Assert.That(request.CreditCard).IsNotNull();
        await Assert.That(request.CreditCard!.Number).IsEqualTo("4111111111111111");
        await Assert.That(request.CreditCard!.Expiration).IsEqualTo("1225");
        await Assert.That(request.CreditCard!.Cvc).IsEqualTo("123");
    }

    [Test]
    public async Task PostPaymentRequest_SetsPostMethod()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.PostPaymentRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region TokenizeCardRequest

    [Test]
    public async Task TokenizeCardRequest_SetsCorrectEndpoint()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.TokenizeCardRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task TokenizeCardRequest_SetsCorrectCommand()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.TokenizeCardRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.TokenizeCard);
    }

    [Test]
    public async Task TokenizeCardRequest_SetsCreditCardAndAvsFields()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.TokenizeCardRequest(tranParams);
        await Assert.That(request.CreditCard).IsNotNull();
        await Assert.That(request.CreditCard!.Number).IsEqualTo("4111111111111111");
        await Assert.That(request.CreditCard!.Cvc).IsEqualTo("123");
        await Assert.That(request.CreditCard!.Expiration).IsEqualTo("1225");
        await Assert.That(request.CreditCard!.AvsStreet).IsEqualTo("123 Main St");
        await Assert.That(request.CreditCard!.AvsPostalCode).IsEqualTo("90210");
    }

    [Test]
    public async Task TokenizeCardRequest_SetsPostMethod()
    {
        var tranParams = CreateSampleCardParams();
        var request = UsaEPayRequestFactory.TokenizeCardRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region CreateTokenFromTransactionRequest

    [Test]
    public async Task CreateTokenFromTransactionRequest_SetsTokensEndpoint()
    {
        var request = UsaEPayRequestFactory.CreateTokenFromTransactionRequest("tkey_abc");
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Tokens);
    }

    [Test]
    public async Task CreateTokenFromTransactionRequest_SetsPostMethod()
    {
        var request = UsaEPayRequestFactory.CreateTokenFromTransactionRequest("tkey_abc");
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task CreateTokenFromTransactionRequest_SetsTransactionKey()
    {
        var request = UsaEPayRequestFactory.CreateTokenFromTransactionRequest("tkey_abc");
        await Assert.That(request.TransactionKey).IsEqualTo("tkey_abc");
    }

    #endregion

    #region PaymentKeySaleRequest

    [Test]
    public async Task PaymentKeySaleRequest_SetsCorrectEndpoint()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 30m, PaymentKey = "pk_123" };
        var request = UsaEPayRequestFactory.PaymentKeySaleRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task PaymentKeySaleRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 30m, PaymentKey = "pk_123" };
        var request = UsaEPayRequestFactory.PaymentKeySaleRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.TransactionSale);
    }

    [Test]
    public async Task PaymentKeySaleRequest_SetsPaymentKey()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 30m, PaymentKey = "pk_123" };
        var request = UsaEPayRequestFactory.PaymentKeySaleRequest(tranParams);
        await Assert.That(request.PaymentKey).IsEqualTo("pk_123");
    }

    [Test]
    public async Task PaymentKeySaleRequest_SetsPostMethod()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 30m, PaymentKey = "pk_123" };
        var request = UsaEPayRequestFactory.PaymentKeySaleRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region CustomerRefundRequest

    [Test]
    public async Task CustomerRefundRequest_SetsCorrectEndpoint()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 15m, CustomerKey = "cust_1", PaymentMethodKey = "pm_1" };
        var request = UsaEPayRequestFactory.CustomerRefundRequest(tranParams);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Transactions);
    }

    [Test]
    public async Task CustomerRefundRequest_SetsCorrectCommand()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 15m, CustomerKey = "cust_1", PaymentMethodKey = "pm_1" };
        var request = UsaEPayRequestFactory.CustomerRefundRequest(tranParams);
        await Assert.That(request.Command).IsEqualTo(UsaEPayCommandTypes.CustomerRefund);
    }

    [Test]
    public async Task CustomerRefundRequest_SetsCustkeyAndPaymethodKey()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 15m, CustomerKey = "cust_1", PaymentMethodKey = "pm_1" };
        var request = UsaEPayRequestFactory.CustomerRefundRequest(tranParams);
        await Assert.That(request.CustomerKey).IsEqualTo("cust_1");
        await Assert.That(request.PaymentMethodKey).IsEqualTo("pm_1");
    }

    [Test]
    public async Task CustomerRefundRequest_SetsPostMethod()
    {
        var tranParams = new UsaEPayTransactionParams { Amount = 15m, CustomerKey = "cust_1", PaymentMethodKey = "pm_1" };
        var request = UsaEPayRequestFactory.CustomerRefundRequest(tranParams);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region SendReceiptRequest

    [Test]
    public async Task SendReceiptRequest_EndpointContainsSend()
    {
        var request = UsaEPayRequestFactory.SendReceiptRequest("tk_123", "test@example.com");
        await Assert.That(request.Endpoint).Contains("/send");
    }

    [Test]
    public async Task SendReceiptRequest_SetsPostMethod()
    {
        var request = UsaEPayRequestFactory.SendReceiptRequest("tk_123", "test@example.com");
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task SendReceiptRequest_SetsToEmail()
    {
        var request = UsaEPayRequestFactory.SendReceiptRequest("tk_123", "test@example.com");
        await Assert.That(request.ToEmail).IsEqualTo("test@example.com");
    }

    #endregion

    #region RetrieveReceiptRequest

    [Test]
    public async Task RetrieveReceiptRequest_EndpointContainsReceipts()
    {
        var request = UsaEPayRequestFactory.RetrieveReceiptRequest("tk_123", "rcpt_456");
        await Assert.That(request.Endpoint).Contains("/receipts/");
    }

    [Test]
    public async Task RetrieveReceiptRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveReceiptRequest("tk_123", "rcpt_456");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveReceiptRequest_EndpointContainsTransactionKey()
    {
        var request = UsaEPayRequestFactory.RetrieveReceiptRequest("tk_123", "rcpt_456");
        await Assert.That(request.Endpoint).Contains("tk_123");
    }

    #endregion

    #region RetrieveCurrentBatchTransactionsRequest

    [Test]
    public async Task RetrieveCurrentBatchTransactionsRequest_SetsCorrectEndpoint()
    {
        var request = UsaEPayRequestFactory.RetrieveCurrentBatchTransactionsRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("batches/current/transactions");
    }

    [Test]
    public async Task RetrieveCurrentBatchTransactionsRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveCurrentBatchTransactionsRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveCurrentBatchTransactionsRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveCurrentBatchTransactionsRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region CreateCustomerRequest

    [Test]
    public async Task CreateCustomerRequest_SetsCustomersEndpoint()
    {
        var customerRequest = new UsaEPayCustomerRequest();
        var request = UsaEPayRequestFactory.CreateCustomerRequest(customerRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Customers);
    }

    [Test]
    public async Task CreateCustomerRequest_SetsPostMethod()
    {
        var customerRequest = new UsaEPayCustomerRequest();
        var request = UsaEPayRequestFactory.CreateCustomerRequest(customerRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region RetrieveCustomerRequest

    [Test]
    public async Task RetrieveCustomerRequest_EndpointContainsCustkey()
    {
        var request = UsaEPayRequestFactory.RetrieveCustomerRequest("cust_abc");
        await Assert.That(request.Endpoint).Contains("cust_abc");
    }

    [Test]
    public async Task RetrieveCustomerRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveCustomerRequest("cust_abc");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region RetrieveCustomerListRequest

    [Test]
    public async Task RetrieveCustomerListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveCustomerListRequest(15, 3);
        await Assert.That(request.Endpoint).Contains("limit=15");
        await Assert.That(request.Endpoint).Contains("offset=3");
    }

    [Test]
    public async Task RetrieveCustomerListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveCustomerListRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region UpdateCustomerRequest

    [Test]
    public async Task UpdateCustomerRequest_SetsPutMethod()
    {
        var customerRequest = new UsaEPayCustomerRequest();
        var request = UsaEPayRequestFactory.UpdateCustomerRequest("cust_abc", customerRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateCustomerRequest_EndpointContainsCustkey()
    {
        var customerRequest = new UsaEPayCustomerRequest();
        var request = UsaEPayRequestFactory.UpdateCustomerRequest("cust_abc", customerRequest);
        await Assert.That(request.Endpoint).Contains("cust_abc");
    }

    #endregion

    #region DeleteCustomerRequest

    [Test]
    public async Task DeleteCustomerRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeleteCustomerRequest("cust_abc");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeleteCustomerRequest_EndpointContainsCustkey()
    {
        var request = UsaEPayRequestFactory.DeleteCustomerRequest("cust_abc");
        await Assert.That(request.Endpoint).Contains("cust_abc");
    }

    #endregion

    #region BulkDeleteCustomersRequest

    [Test]
    public async Task BulkDeleteCustomersRequest_EndpointContainsBulk()
    {
        var request = UsaEPayRequestFactory.BulkDeleteCustomersRequest(new[] { "k1", "k2" });
        await Assert.That(request.Endpoint).Contains("/bulk");
    }

    [Test]
    public async Task BulkDeleteCustomersRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.BulkDeleteCustomersRequest(new[] { "k1", "k2" });
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    #endregion

    #region AddPaymentMethodRequest

    [Test]
    public async Task AddPaymentMethodRequest_SetsCorrectEndpointPattern()
    {
        var pmRequest = new UsaEPayPaymentMethodRequest();
        var request = UsaEPayRequestFactory.AddPaymentMethodRequest("cust_1", pmRequest);
        await Assert.That(request.Endpoint).Contains("customers/cust_1/payment_methods");
    }

    [Test]
    public async Task AddPaymentMethodRequest_SetsPostMethod()
    {
        var pmRequest = new UsaEPayPaymentMethodRequest();
        var request = UsaEPayRequestFactory.AddPaymentMethodRequest("cust_1", pmRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region RetrievePaymentMethodRequest

    [Test]
    public async Task RetrievePaymentMethodRequest_SetsCorrectEndpointPattern()
    {
        var request = UsaEPayRequestFactory.RetrievePaymentMethodRequest("cust_1", "pm_1");
        await Assert.That(request.Endpoint).Contains("customers/cust_1/payment_methods/pm_1");
    }

    [Test]
    public async Task RetrievePaymentMethodRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrievePaymentMethodRequest("cust_1", "pm_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region RetrievePaymentMethodListRequest

    [Test]
    public async Task RetrievePaymentMethodListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrievePaymentMethodListRequest("cust_1", 10, 2);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=2");
    }

    [Test]
    public async Task RetrievePaymentMethodListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrievePaymentMethodListRequest("cust_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region UpdatePaymentMethodRequest

    [Test]
    public async Task UpdatePaymentMethodRequest_SetsPutMethod()
    {
        var pmRequest = new UsaEPayPaymentMethodRequest();
        var request = UsaEPayRequestFactory.UpdatePaymentMethodRequest("cust_1", "pm_1", pmRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdatePaymentMethodRequest_SetsCorrectEndpointPattern()
    {
        var pmRequest = new UsaEPayPaymentMethodRequest();
        var request = UsaEPayRequestFactory.UpdatePaymentMethodRequest("cust_1", "pm_1", pmRequest);
        await Assert.That(request.Endpoint).Contains("customers/cust_1/payment_methods/pm_1");
    }

    #endregion

    #region DeletePaymentMethodRequest

    [Test]
    public async Task DeletePaymentMethodRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeletePaymentMethodRequest("cust_1", "pm_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeletePaymentMethodRequest_SetsCorrectEndpointPattern()
    {
        var request = UsaEPayRequestFactory.DeletePaymentMethodRequest("cust_1", "pm_1");
        await Assert.That(request.Endpoint).Contains("customers/cust_1/payment_methods/pm_1");
    }

    #endregion

    #region BulkDeletePaymentMethodsRequest

    [Test]
    public async Task BulkDeletePaymentMethodsRequest_EndpointContainsBulk()
    {
        var request = UsaEPayRequestFactory.BulkDeletePaymentMethodsRequest("cust_1", new[] { "pm_1", "pm_2" });
        await Assert.That(request.Endpoint).Contains("/bulk");
    }

    [Test]
    public async Task BulkDeletePaymentMethodsRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.BulkDeletePaymentMethodsRequest("cust_1", new[] { "pm_1", "pm_2" });
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    #endregion

    #region CreateBillingScheduleRequest

    [Test]
    public async Task CreateBillingScheduleRequest_SetsCorrectEndpointPattern()
    {
        var scheduleRequest = new UsaEPayBillingScheduleRequest();
        var request = UsaEPayRequestFactory.CreateBillingScheduleRequest("cust_1", scheduleRequest);
        await Assert.That(request.Endpoint).Contains("customers/cust_1/billing_schedules");
    }

    [Test]
    public async Task CreateBillingScheduleRequest_SetsPostMethod()
    {
        var scheduleRequest = new UsaEPayBillingScheduleRequest();
        var request = UsaEPayRequestFactory.CreateBillingScheduleRequest("cust_1", scheduleRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region RetrieveBillingScheduleRequest

    [Test]
    public async Task RetrieveBillingScheduleRequest_SetsCorrectEndpointPattern()
    {
        var request = UsaEPayRequestFactory.RetrieveBillingScheduleRequest("cust_1", "sched_1");
        await Assert.That(request.Endpoint).Contains("customers/cust_1/billing_schedules/sched_1");
    }

    [Test]
    public async Task RetrieveBillingScheduleRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBillingScheduleRequest("cust_1", "sched_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region RetrieveBillingScheduleListRequest

    [Test]
    public async Task RetrieveBillingScheduleListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveBillingScheduleListRequest("cust_1", 10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveBillingScheduleListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBillingScheduleListRequest("cust_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    #endregion

    #region Products

    [Test]
    public async Task CreateProductRequest_SetsProductsEndpoint()
    {
        var productRequest = new UsaEPayProductRequest();
        var request = UsaEPayRequestFactory.CreateProductRequest(productRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Products);
    }

    [Test]
    public async Task CreateProductRequest_SetsPostMethod()
    {
        var productRequest = new UsaEPayProductRequest();
        var request = UsaEPayRequestFactory.CreateProductRequest(productRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task RetrieveProductRequest_EndpointContainsProductKey()
    {
        var request = UsaEPayRequestFactory.RetrieveProductRequest("prod_1");
        await Assert.That(request.Endpoint).Contains("prod_1");
    }

    [Test]
    public async Task RetrieveProductRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveProductRequest("prod_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveProductListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveProductListRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveProductListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveProductListRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task UpdateProductRequest_SetsPutMethod()
    {
        var productRequest = new UsaEPayProductRequest();
        var request = UsaEPayRequestFactory.UpdateProductRequest("prod_1", productRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateProductRequest_EndpointContainsProductKey()
    {
        var productRequest = new UsaEPayProductRequest();
        var request = UsaEPayRequestFactory.UpdateProductRequest("prod_1", productRequest);
        await Assert.That(request.Endpoint).Contains("prod_1");
    }

    [Test]
    public async Task DeleteProductRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeleteProductRequest("prod_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeleteProductRequest_EndpointContainsProductKey()
    {
        var request = UsaEPayRequestFactory.DeleteProductRequest("prod_1");
        await Assert.That(request.Endpoint).Contains("prod_1");
    }

    #endregion

    #region Product Categories

    [Test]
    public async Task CreateProductCategoryRequest_SetsProductCategoriesEndpoint()
    {
        var categoryRequest = new UsaEPayProductCategoryRequest();
        var request = UsaEPayRequestFactory.CreateProductCategoryRequest(categoryRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.ProductCategories);
    }

    [Test]
    public async Task CreateProductCategoryRequest_SetsPostMethod()
    {
        var categoryRequest = new UsaEPayProductCategoryRequest();
        var request = UsaEPayRequestFactory.CreateProductCategoryRequest(categoryRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task RetrieveProductCategoryRequest_EndpointContainsCategoryKey()
    {
        var request = UsaEPayRequestFactory.RetrieveProductCategoryRequest("cat_1");
        await Assert.That(request.Endpoint).Contains("cat_1");
    }

    [Test]
    public async Task RetrieveProductCategoryRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveProductCategoryRequest("cat_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveProductCategoryListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveProductCategoryListRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveProductCategoryListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveProductCategoryListRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task UpdateProductCategoryRequest_SetsPutMethod()
    {
        var categoryRequest = new UsaEPayProductCategoryRequest();
        var request = UsaEPayRequestFactory.UpdateProductCategoryRequest("cat_1", categoryRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateProductCategoryRequest_EndpointContainsCategoryKey()
    {
        var categoryRequest = new UsaEPayProductCategoryRequest();
        var request = UsaEPayRequestFactory.UpdateProductCategoryRequest("cat_1", categoryRequest);
        await Assert.That(request.Endpoint).Contains("cat_1");
    }

    [Test]
    public async Task DeleteProductCategoryRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeleteProductCategoryRequest("cat_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeleteProductCategoryRequest_EndpointContainsCategoryKey()
    {
        var request = UsaEPayRequestFactory.DeleteProductCategoryRequest("cat_1");
        await Assert.That(request.Endpoint).Contains("cat_1");
    }

    #endregion

    #region Inventory Locations

    [Test]
    public async Task CreateInventoryLocationRequest_SetsInventoryLocationsEndpoint()
    {
        var locationRequest = new UsaEPayInventoryLocationRequest();
        var request = UsaEPayRequestFactory.CreateInventoryLocationRequest(locationRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.InventoryLocations);
    }

    [Test]
    public async Task CreateInventoryLocationRequest_SetsPostMethod()
    {
        var locationRequest = new UsaEPayInventoryLocationRequest();
        var request = UsaEPayRequestFactory.CreateInventoryLocationRequest(locationRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task RetrieveInventoryLocationRequest_EndpointContainsLocationKey()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryLocationRequest("loc_1");
        await Assert.That(request.Endpoint).Contains("loc_1");
    }

    [Test]
    public async Task RetrieveInventoryLocationRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryLocationRequest("loc_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveInventoryLocationListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryLocationListRequest(50, 10);
        await Assert.That(request.Endpoint).Contains("limit=50");
        await Assert.That(request.Endpoint).Contains("offset=10");
    }

    [Test]
    public async Task RetrieveInventoryLocationListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryLocationListRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task UpdateInventoryLocationRequest_SetsPutMethod()
    {
        var locationRequest = new UsaEPayInventoryLocationRequest();
        var request = UsaEPayRequestFactory.UpdateInventoryLocationRequest("loc_1", locationRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateInventoryLocationRequest_EndpointContainsLocationKey()
    {
        var locationRequest = new UsaEPayInventoryLocationRequest();
        var request = UsaEPayRequestFactory.UpdateInventoryLocationRequest("loc_1", locationRequest);
        await Assert.That(request.Endpoint).Contains("loc_1");
    }

    [Test]
    public async Task DeleteInventoryLocationRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeleteInventoryLocationRequest("loc_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeleteInventoryLocationRequest_EndpointContainsLocationKey()
    {
        var request = UsaEPayRequestFactory.DeleteInventoryLocationRequest("loc_1");
        await Assert.That(request.Endpoint).Contains("loc_1");
    }

    #endregion

    #region Inventory

    [Test]
    public async Task CreateInventoryRequest_SetsInventoryEndpoint()
    {
        var inventoryRequest = new UsaEPayInventoryRequest();
        var request = UsaEPayRequestFactory.CreateInventoryRequest(inventoryRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.Inventory);
    }

    [Test]
    public async Task CreateInventoryRequest_SetsPostMethod()
    {
        var inventoryRequest = new UsaEPayInventoryRequest();
        var request = UsaEPayRequestFactory.CreateInventoryRequest(inventoryRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task RetrieveInventoryRequest_EndpointContainsInventoryKey()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryRequest("inv_1");
        await Assert.That(request.Endpoint).Contains("inv_1");
    }

    [Test]
    public async Task RetrieveInventoryRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryRequest("inv_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveInventoryListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryListRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveInventoryListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveInventoryListRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task UpdateInventoryRequest_SetsPutMethod()
    {
        var inventoryRequest = new UsaEPayInventoryRequest();
        var request = UsaEPayRequestFactory.UpdateInventoryRequest("inv_1", inventoryRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateInventoryRequest_EndpointContainsInventoryKey()
    {
        var inventoryRequest = new UsaEPayInventoryRequest();
        var request = UsaEPayRequestFactory.UpdateInventoryRequest("inv_1", inventoryRequest);
        await Assert.That(request.Endpoint).Contains("inv_1");
    }

    [Test]
    public async Task DeleteInventoryRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeleteInventoryRequest("inv_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeleteInventoryRequest_EndpointContainsInventoryKey()
    {
        var request = UsaEPayRequestFactory.DeleteInventoryRequest("inv_1");
        await Assert.That(request.Endpoint).Contains("inv_1");
    }

    #endregion

    #region Bulk Transaction Methods

    [Test]
    public async Task RetrieveBulkTransactionStatusRequest_SetsCorrectEndpoint()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionStatusRequest("bulk_1");
        await Assert.That(request.Endpoint).IsEqualTo($"{UsaEPayEndpoints.BulkTransactions}/bulk_1");
    }

    [Test]
    public async Task RetrieveBulkTransactionStatusRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionStatusRequest("bulk_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveBulkTransactionCurrentRequest_SetsCorrectEndpoint()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionCurrentRequest();
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.BulkTransactionsCurrent);
    }

    [Test]
    public async Task RetrieveBulkTransactionCurrentRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionCurrentRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveBulkTransactionFileTransactionsRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionFileTransactionsRequest("bulk_1", 10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveBulkTransactionFileTransactionsRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionFileTransactionsRequest("bulk_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveBulkTransactionCurrentTransactionsRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionCurrentTransactionsRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveBulkTransactionCurrentTransactionsRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveBulkTransactionCurrentTransactionsRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task PauseBulkTransactionRequest_EndpointContainsPause()
    {
        var request = UsaEPayRequestFactory.PauseBulkTransactionRequest("bulk_1");
        await Assert.That(request.Endpoint).Contains("/pause");
    }

    [Test]
    public async Task PauseBulkTransactionRequest_SetsPostMethod()
    {
        var request = UsaEPayRequestFactory.PauseBulkTransactionRequest("bulk_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task PauseBulkTransactionCurrentRequest_EndpointContainsPause()
    {
        var request = UsaEPayRequestFactory.PauseBulkTransactionCurrentRequest();
        await Assert.That(request.Endpoint).Contains("/pause");
    }

    [Test]
    public async Task PauseBulkTransactionCurrentRequest_SetsPostMethod()
    {
        var request = UsaEPayRequestFactory.PauseBulkTransactionCurrentRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task ResumeBulkTransactionRequest_EndpointContainsResume()
    {
        var request = UsaEPayRequestFactory.ResumeBulkTransactionRequest("bulk_1");
        await Assert.That(request.Endpoint).Contains("/resume");
    }

    [Test]
    public async Task ResumeBulkTransactionRequest_SetsPostMethod()
    {
        var request = UsaEPayRequestFactory.ResumeBulkTransactionRequest("bulk_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    #endregion

    #region Device Methods

    [Test]
    public async Task RegisterDeviceRequest_SetsPaymentEngineDevicesEndpoint()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.RegisterDeviceRequest(deviceRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.PaymentEngineDevices);
    }

    [Test]
    public async Task RegisterDeviceRequest_SetsPostMethod()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.RegisterDeviceRequest(deviceRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task RetrieveDeviceRequest_EndpointContainsDeviceKey()
    {
        var request = UsaEPayRequestFactory.RetrieveDeviceRequest("dev_1");
        await Assert.That(request.Endpoint).Contains("dev_1");
    }

    [Test]
    public async Task RetrieveDeviceRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveDeviceRequest("dev_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task RetrieveDeviceListRequest_SetsPaginationParams()
    {
        var request = UsaEPayRequestFactory.RetrieveDeviceListRequest(10, 5);
        await Assert.That(request.Endpoint).Contains("limit=10");
        await Assert.That(request.Endpoint).Contains("offset=5");
    }

    [Test]
    public async Task RetrieveDeviceListRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrieveDeviceListRequest();
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task UpdateDeviceRequest_SetsPutMethod()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.UpdateDeviceRequest("dev_1", deviceRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateDeviceRequest_EndpointContainsDeviceKey()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.UpdateDeviceRequest("dev_1", deviceRequest);
        await Assert.That(request.Endpoint).Contains("dev_1");
    }

    [Test]
    public async Task UpdateDeviceSettingsRequest_SetsPutMethod()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.UpdateDeviceSettingsRequest("dev_1", deviceRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateDeviceSettingsRequest_EndpointContainsSettings()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.UpdateDeviceSettingsRequest("dev_1", deviceRequest);
        await Assert.That(request.Endpoint).Contains("/settings");
    }

    [Test]
    public async Task UpdateDeviceTerminalConfigRequest_SetsPutMethod()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.UpdateDeviceTerminalConfigRequest("dev_1", deviceRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Put);
    }

    [Test]
    public async Task UpdateDeviceTerminalConfigRequest_EndpointContainsTerminalConfig()
    {
        var deviceRequest = new UsaEPayDeviceRequest();
        var request = UsaEPayRequestFactory.UpdateDeviceTerminalConfigRequest("dev_1", deviceRequest);
        await Assert.That(request.Endpoint).Contains("/terminal-config");
    }

    [Test]
    public async Task DeleteDeviceRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.DeleteDeviceRequest("dev_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task DeleteDeviceRequest_EndpointContainsDeviceKey()
    {
        var request = UsaEPayRequestFactory.DeleteDeviceRequest("dev_1");
        await Assert.That(request.Endpoint).Contains("dev_1");
    }

    #endregion

    #region Pay Request Methods

    [Test]
    public async Task CreatePayRequestRequest_SetsPayRequestsEndpoint()
    {
        var payRequest = new UsaEPayPayRequestRequest();
        var request = UsaEPayRequestFactory.CreatePayRequestRequest(payRequest);
        await Assert.That(request.Endpoint).IsEqualTo(UsaEPayEndpoints.PaymentEnginePayRequests);
    }

    [Test]
    public async Task CreatePayRequestRequest_SetsPostMethod()
    {
        var payRequest = new UsaEPayPayRequestRequest();
        var request = UsaEPayRequestFactory.CreatePayRequestRequest(payRequest);
        await Assert.That(request.RequestType).IsEqualTo(Method.Post);
    }

    [Test]
    public async Task RetrievePayRequestRequest_EndpointContainsRequestKey()
    {
        var request = UsaEPayRequestFactory.RetrievePayRequestRequest("req_1");
        await Assert.That(request.Endpoint).Contains("req_1");
    }

    [Test]
    public async Task RetrievePayRequestRequest_SetsGetMethod()
    {
        var request = UsaEPayRequestFactory.RetrievePayRequestRequest("req_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Get);
    }

    [Test]
    public async Task CancelPayRequestRequest_SetsDeleteMethod()
    {
        var request = UsaEPayRequestFactory.CancelPayRequestRequest("req_1");
        await Assert.That(request.RequestType).IsEqualTo(Method.Delete);
    }

    [Test]
    public async Task CancelPayRequestRequest_EndpointContainsRequestKey()
    {
        var request = UsaEPayRequestFactory.CancelPayRequestRequest("req_1");
        await Assert.That(request.Endpoint).Contains("req_1");
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

    #endregion
}
