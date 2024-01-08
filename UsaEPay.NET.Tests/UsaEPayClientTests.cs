using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests
{
    public class Shared
    {
        public static UsaEPayClient Client;
        public static IConfiguration Config;
        public static string Token = string.Empty;
        public static string TransKey = string.Empty;
        public static string TranCheckKey = string.Empty;
        public string BatchKey = string.Empty;
    }
    [Order(1)]
    public class Tokenization : Shared
    {
        [SetUp]
        public void Setup()
        {
            Config = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddUserSecrets<Tokenization>()
                .Build();
            Client = new UsaEPayClient(
                Config["API_URL"],
                Config["API_KEY"],
                Config["API_PIN"],
                Config["RANDOM_SEED"],
                true
                );
        }

        [Test, Order(1), Category("Token")]
        public async Task TestTokenizeCard()
        {
            var tokenizeCardParams = new UsaEPayTransactionParams
            {
                CardHolder = "John Doe",
                CardNumber = "4000100011112224",
                Expiration = "0924",
                Cvc = 123
            };
            var request = UsaEPayRequestFactory.TokenizeCardRequest(tokenizeCardParams);

            var response = await Client.SendRequest<UsaEPayResponse>(request);
            if (response.ResultCode == "A")
            {
                Token = response.SavedCard.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(2), Category("Token")]
        public async Task TestTokenSale()
        {
            var tokenSaleParams = new UsaEPayTransactionParams
            {
                Amount = 10,
                FirstName = "John",
                LastName = "Doe",
                Address = "555 Test Street",
                Address2 = "",
                City = "Testington",
                State = "OK",
                Zip = "33242",
                Cvc = 123
            };
            var request = UsaEPayRequestFactory.TokenSaleRequest(tokenSaleParams, Token);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }
    }

    [Order(2)]
    public class Transaction : Shared
    {
        [Test, Order(1), Category("Sale")]
        public async Task TestCardSale()
        {
            var creditCardSaleParams = new UsaEPayTransactionParams
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
                Expiration = "0924",
                Cvc = 123
            };
            var request = UsaEPayRequestFactory.CreditCardSaleRequest(creditCardSaleParams);

            var response = await Client.SendRequest<UsaEPayResponse>(request);
            if (response.ResultCode == "A")
            {
                TransKey = response.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(2), Category("Sale")]
        public async Task TestQuickSale()
        {
            var request = UsaEPayRequestFactory.QuickSaleRequest(10, TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(3), Category("Sale")]
        public async Task TestCheckSale()
        {
            var checkSaleParams = new UsaEPayTransactionParams
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
            };
            var request = UsaEPayRequestFactory.CheckSaleRequest(checkSaleParams);

            var response = await Client.SendRequest<UsaEPayResponse>(request);
            if (response.ResultCode == "A")
            {
                TranCheckKey = response.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(4), Category("Sale")]
        public async Task TestAuthOnlySale()
        {
            var authOnlyParams = new UsaEPayTransactionParams
            {
                Amount = 10,
                CardHolder = "John Doe",
                CardNumber = "4000100011112224",
                Expiration = "0924",
                Cvc = 123
            };
            var request = UsaEPayRequestFactory.AuthOnlySaleRequest(authOnlyParams);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(5), Category("Refund")]
        public async Task TestCreditCardRefund()
        {
            var refundParams = new UsaEPayTransactionParams
            {
                Amount = 10,
                CardHolder = "John Doe",
                CardNumber = "4000100011112224",
                Expiration = "0924",
                Cvc = 123
            };
            var request = UsaEPayRequestFactory.CreditCardRefundRequest(refundParams);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(6), Category("Refund")]
        public async Task TestCheckRefund()
        {
            var refundParams = new UsaEPayTransactionParams
            {
                Amount = 10,
                AccountHolder = "John Doe",
                AccountNumber = "234234",
                Routing = "123456789",
                AccountType = "checking",
                CheckNumber = "101"
            };
            var request = UsaEPayRequestFactory.CheckRefundRequest(refundParams);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(7), Category("Refund")]
        public async Task TestQuickRefund()
        {
            var request = UsaEPayRequestFactory.QuickRefundRequest(10, TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(8), Category("Refund")]
        public async Task TestAdjustPaymentRefund()
        {
            var request = UsaEPayRequestFactory.AdjustPaymentRefundRequest(TranCheckKey, 10);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(9), Category("Capture")]
        public async Task TestCapturePayment()
        {
            var request = UsaEPayRequestFactory.CapturePaymentRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(10), Category("Capture")]
        public async Task TestCapturePaymentError()
        {
            var request = UsaEPayRequestFactory.CapturePaymentErrorRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(11), Category("Capture")]
        public async Task TestCapturePaymentReauth()
        {
            var request = UsaEPayRequestFactory.CapturePaymentReauthRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(12), Category("Capture")]
        public async Task TestCapturePaymentOverride()
        {
            var request = UsaEPayRequestFactory.CapturePaymentOverrideRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(13), Category("Adjust")]
        public async Task TestAdjustPayment()
        {
            var request = UsaEPayRequestFactory.AdjustPaymentRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(14), Category("RetrieveDetails")]
        public async Task TestRetrieveTransactionDetails()
        {
            var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(15), Category("Void")]
        public async Task TestCreditVoid()
        {
            var request = UsaEPayRequestFactory.CreditVoidRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(16), Category("Void")]
        public async Task TestVoidPayment()
        {
            var request = UsaEPayRequestFactory.VoidPaymentRequest(TranCheckKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(17), Category("Void")]
        public async Task TestUnvoid()
        {
            var request = UsaEPayRequestFactory.UnvoidRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(18), Category("Void")]
        public async Task TestReleaseFunds()
        {
            var request = UsaEPayRequestFactory.ReleaseFundsRequest(TransKey);

            var response = await Client.SendRequest<UsaEPayResponse>(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }
    }

    [Order(3)]
    public class Batch : Shared
    {
        [Test, Order(1), Category("BatchList")]
        public async Task TestBatchList()
        {
            var request = UsaEPayRequestFactory.RetrieveBatchListRequest();

            var response = await Client.SendRequest<UsaEPayBatchListResponse>(request);
            if (response.Data != null)
            {
                var batchItem = response.Data.First();
                BatchKey = batchItem.Key;
            }

            Assert.That(response, Is.Not.Null);
        }

        [Test, Order(2), Category("BatchListByDate")]
        [TestCase(20230101, 20240201)]
        public async Task TestBatchListByDate(long openedAfter, long openedBefore)
        {
            var request = UsaEPayRequestFactory.RetrieveBatchListByDateRequest(openedAfter, openedBefore);

            var response = await Client.SendRequest<UsaEPayBatchListResponse>(request);

            Assert.That(response.Data, Is.Not.Null);
        }

        [Test, Order(3), Category("RetreiveSpecificBatch")]
        public async Task TestRetrieveSpecificBatch()
        {
            var request = UsaEPayRequestFactory.RetrieveSpecificBatchRequest(BatchKey);

            var response = await Client.SendRequest<Models.Classes.Batch>(request);

            Assert.That(response.Status, Is.EqualTo("open"));
        }

        [Test, Order(4), Category("RetreiveCurrentBatch")]
        public async Task TestRetrieveCurrentBatch()
        {
            var request = UsaEPayRequestFactory.RetrieveCurrentBatchRequest();

            var response = await Client.SendRequest<Models.Classes.Batch>(request);

            Assert.That(response.Status, Is.EqualTo("open"));
        }

        [Test, Order(5), Category("CloseCurrentBatch")]
        public async Task TestCloseCurrentBatch()
        {
            var request = UsaEPayRequestFactory.CloseCurrentBatchRequest();

            var response = await Client.SendRequest<Models.Classes.Batch>(request);

            Assert.That(response.Status, Is.EqualTo("closing"));
        }

        
        [Test, Order(6), Category("TestRetrieveBatchTransactionsByIdRequest")]
        public async Task TestRetrieveBatchTransactionsByIdRequest()
        {
            var request = UsaEPayRequestFactory.RetrieveBatchTransactionsByIdRequest(BatchKey, 5, 1);

            var response = await Client.SendRequest<UsaEPayBatchListResponse>(request);

            Assert.That(response, Is.Not.Null);
        }
    }

    //[Test]
    //[Order(8)]
    //public async Task TestPostPayment() 
    //{
    //    var request = UsaEPayRequestFactory.PostPaymentRequest(10, "AUTH_CODE", "John Doe", "4000100011112224", "0924", 123);

    //    var response = await _Client.SendRequest<UsaEPayResponse>(request);

    //    Assert.That(response.ResultCode, Is.EqualTo("A"));
    //}
    //[Test]
    //[Order(29)]
    //public async Task TestCashRefund()
    //{
    //    var request = UsaEPayRequestFactory.CashRefundRequest(10);

    //    var response = await _Client.SendRequest<UsaEPayResponse>(request);

    //    Assert.That(response.ResultCode, Is.EqualTo("A"));
    //}
}