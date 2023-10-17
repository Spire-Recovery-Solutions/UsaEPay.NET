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
        public static string _tranKey = string.Empty;
        public static string _tranCheckKey = string.Empty;
        public string _batchKey = string.Empty;
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
            var request = UsaEPayRequestFactory.TokenizeCardRequest("John Doe", "4000100011112224", "0924", 123);

            var response = await Client.SendRequest(request);
            if (response.ResultCode == "A")
            {
                Token = response.SavedCard.Key?.Replace("-", "");
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(2), Category("Token")]
        public async Task TestTokenSale()
        {
            var request = UsaEPayRequestFactory.TokenSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", Token, 123);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }
    }

    [Order(2)]
    public class Transaction : Shared
    {
        [Test, Order(1), Category("Sale")]
        public async Task TestCardSale()
        {
            var request = UsaEPayRequestFactory.CreditCardSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", "4000100011112224", "0924", 123);

            var response = await Client.SendRequest(request);
            if (response.ResultCode == "A")
            {
                _tranKey = response.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(2), Category("Sale")]
        public async Task TestQuickSale()
        {
            var request = UsaEPayRequestFactory.QuickSaleRequest(10, _tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(3), Category("Sale")]
        public async Task TestCheckSale()
        {
            var request = UsaEPayRequestFactory.CheckSaleRequest(10, "Remus", "Lupin", "555 Test Street", "", "Testington", "OK", "33242", "123456789", "324523524", "checking", "101");

            var response = await Client.SendRequest(request);
            if (response.ResultCode == "A")
            {
                _tranCheckKey = response.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(4), Category("Sale")]
        public async Task TestAuthOnlySale()
        {
            var request = UsaEPayRequestFactory.AuthOnlySaleRequest(10, "John Doe", "4000100011112224", "0924", 123);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(5), Category("Refund")]
        public async Task TestCreditCardRefund()
        {
            var request = UsaEPayRequestFactory.CreditCardRefundRequest(10, "John Doe", "4000100011112224", "0924", 123);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(6), Category("Refund")]
        public async Task TestCheckRefund()
        {
            var request = UsaEPayRequestFactory.CheckRefundRequest(10, "John Doe", "234234", "123456789", "checking", "101");

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(7), Category("Refund")]
        public async Task TestQuickRefund()
        {
            var request = UsaEPayRequestFactory.QuickRefundRequest(10, _tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(8), Category("Refund")]
        public async Task TestCashRefund()
        {
            var request = UsaEPayRequestFactory.CashRefundRequest(10);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(9), Category("Refund")]
        public async Task TestAdjustPaymentRefund()
        {
            var request = UsaEPayRequestFactory.AdjustPaymentRefundRequest(_tranKey, 10);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(10), Category("Capture")]
        public async Task TestCapturePayment()
        {
            var request = UsaEPayRequestFactory.CapturePaymentRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(11), Category("Capture")]
        public async Task TestCapturePaymentError()
        {
            var request = UsaEPayRequestFactory.CapturePaymentErrorRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(12), Category("Capture")]
        public async Task TestCapturePaymentReauth()
        {
            var request = UsaEPayRequestFactory.CapturePaymentReauthRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(13), Category("Capture")]
        public async Task TestCapturePaymentOverride()
        {
            var request = UsaEPayRequestFactory.CapturePaymentOverrideRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(14), Category("Adjust")]
        public async Task TestAdjustPayment()
        {
            var request = UsaEPayRequestFactory.AdjustPaymentRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(15), Category("RetrieveDetails")]
        public async Task TestRetrieveTransactionDetails()
        {
            var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(16), Category("Void")]
        public async Task TestCreditVoid()
        {
            var request = UsaEPayRequestFactory.CreditVoidRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(17), Category("Void")]
        public async Task TestVoidPayment()
        {
            var request = UsaEPayRequestFactory.VoidPaymentRequest(_tranCheckKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(18), Category("Void")]
        public async Task TestUnvoid()
        {
            var request = UsaEPayRequestFactory.UnvoidRequest(_tranKey);

            var response = await Client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test, Order(19), Category("Void")]
        public async Task TestReleaseFunds()
        {
            var request = UsaEPayRequestFactory.ReleaseFundsRequest(_tranKey);

            var response = await Client.SendRequest(request);

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

            Assert.That(response, Is.Not.Null);
        }

        [Test, Order(2), Category("BatchListByDate")]
        [TestCase(20171003, 20171004)]
        public async Task TestBatchListByDate(long openedAfter, long openedBefore)
        {
            var request = UsaEPayRequestFactory.RetrieveBatchListByDateRequest(openedAfter, openedBefore);

            var response = await Client.SendRequest<UsaEPayBatchListResponse>(request);

            Assert.That(response.Data, Is.Not.Null);
        }

        [Test, Order(3), Category("RetreiveSpecificBatch")]
        public async Task TestRetrieveSpecificBatch()
        {
            var request = UsaEPayRequestFactory.RetrieveSpecificBatchRequest(_batchKey);

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
    }

    //[Test]
    //[Order(8)]
    //public async Task TestPostPayment() 
    //{
    //    var request = UsaEPayRequestFactory.PostPaymentRequest(10, "AUTH_CODE", "John Doe", "4000100011112224", "0924", 123);

    //    var response = await _client.SendRequest(request);

    //    Assert.That(response.ResultCode, Is.EqualTo("A"));
    //}
    //[Test]
    //[Order(29)]
    //public async Task TestCashRefund()
    //{
    //    var request = UsaEPayRequestFactory.CashRefundRequest(10);

    //    var response = await _client.SendRequest(request);

    //    Assert.That(response.ResultCode, Is.EqualTo("A"));
    //}
}