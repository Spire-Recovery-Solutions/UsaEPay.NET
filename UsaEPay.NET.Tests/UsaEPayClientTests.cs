using Microsoft.Extensions.Configuration;
using UsaEPay.NET.Factories;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests
{
    public class UsaEPayClientTests
    {
        private UsaEPayClient _client;
        private IConfiguration _config;
        string apiUrl = string.Empty;
        string apiKey = string.Empty;
        string apiPin = string.Empty;
        string randomSeed = string.Empty;
        private string _token = string.Empty;
        private string _tranKey = string.Empty;
        private string _tranCheckKey = string.Empty;

        [SetUp]
        public void Setup()
        {
            _config = new ConfigurationBuilder()
                          .AddEnvironmentVariables()
                          .AddUserSecrets<UsaEPayClientTests>()
                          .Build();

            apiUrl = _config["API_URL"];
            apiKey = _config["API_KEY"];
            apiPin = _config["API_PIN"];
            randomSeed = _config["RANDOM_SEED"];

            _client = new UsaEPayClient(apiUrl, apiKey, apiPin, randomSeed, true);
        }

        [Test]
        [Order(1)]
        public async Task TestCardSale()
        {
            var request = UsaEPayRequestFactory.CreditCardSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);
            if (response.ResultCode == "A")
            {
                _tranKey = response.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(2)]
        public async Task TestTokenizeCard()
        {
            var request = UsaEPayRequestFactory.TokenizeCardRequest("John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);
            if (response.ResultCode == "A")
            {
                _token = response.SavedCard.Key?.Replace("-", "");
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(3)]
        public async Task TestTokenSale()
        {
            var request = UsaEPayRequestFactory.TokenSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", _token, 123);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(4)]
        public async Task TestQuickSale()
        {
            var request = UsaEPayRequestFactory.QuickSaleRequest(10, _tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(5)]
        public async Task TestCheckSale()
        {
            var request = UsaEPayRequestFactory.CheckSaleRequest(10, "Remus", "Lupin", "555 Test Street", "", "Testington", "OK", "33242", "123456789", "324523524", "checking", "101");

            var response = await _client.SendRequest(request);
            if (response.ResultCode == "A")
            {
                _tranCheckKey = response.Key;
            }

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(6)]
        public async Task TestAuthOnlySale()
        {
            var request = UsaEPayRequestFactory.AuthOnlySaleRequest(10, "John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        //[Test]
        //[Order(8)]
        //public async Task TestPostPayment() 
        //{
        //    var request = UsaEPayRequestFactory.PostPaymentRequest(10, "AUTH_CODE", "John Doe", "4000100011112224", "0924", 123);

        //    var response = await _client.SendRequest(request);

        //    Assert.That(response.ResultCode, Is.EqualTo("A"));
        //}

        [Test]
        [Order(7)]
        public async Task TestRetrieveTransactionDetails()
        {
            var request = UsaEPayRequestFactory.RetrieveTransactionDetailsRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(8)]
        public async Task TestCreditCardRefund()
        {
            var request = UsaEPayRequestFactory.CreditCardRefundRequest(10, "John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(9)]
        public async Task TestCheckRefund()
        {
            var request = UsaEPayRequestFactory.CheckRefundRequest(10, "John Doe", "234234", "123456789", "checking", "101");

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(10)]
        public async Task TestQuickRefund()
        {
            var request = UsaEPayRequestFactory.QuickRefundRequest(2, _tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(11)]
        public async Task TestAdjustPaymentRefund()
        {
            var request = UsaEPayRequestFactory.AdjustPaymentRefundRequest(_tranCheckKey, 5);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(12)]
        public async Task TestCreditVoid()
        {
            var request = UsaEPayRequestFactory.CreditVoidRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(13)]
        public async Task TestUnvoid()
        {
            var request = UsaEPayRequestFactory.UnvoidRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(14)]
        public async Task TestVoidPayment()
        {
            var request = UsaEPayRequestFactory.VoidPaymentRequest(_tranCheckKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(15)]
        public async Task TestCapturePayment()
        {
            var request = UsaEPayRequestFactory.CapturePaymentRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(16)]
        public async Task TestCapturePaymentError()
        {
            var request = UsaEPayRequestFactory.CapturePaymentErrorRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(17)]
        public async Task TestCapturePaymentReauth()
        {
            var request = UsaEPayRequestFactory.CapturePaymentReauthRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(18)]
        public async Task TestCapturePaymentOverride()
        {
            var request = UsaEPayRequestFactory.CapturePaymentOverrideRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(19)]
        public async Task TestAdjustPayment()
        {
            var request = UsaEPayRequestFactory.AdjustPaymentRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(20)]
        public async Task TestReleaseFunds()
        {
            var request = UsaEPayRequestFactory.ReleaseFundsRequest(_tranKey);

            var response = await _client.SendRequest(request);

            Assert.That(response.ResultCode, Is.EqualTo("A"));
        }

        [Test]
        [Order(21)]
        public async Task TestBatchList()
        {
            var request = UsaEPayRequestFactory.RetrieveBatchListRequest();

            var response = await _client.SendRequest<UsaEPayBatchListResponse>(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase(20171003, 20171004)]
        [Order(22)]
        public async Task TestBatchListByDate(long openedAfter, long openedBefore)
        {
            var request = UsaEPayRequestFactory.RetrieveBatchListByDateRequest(openedAfter, openedBefore);

            var response = await _client.SendRequest<UsaEPayBatchListResponse>(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [Order(23)]
        public async Task TestCloseCurrentBatch()
        {
            var request = UsaEPayRequestFactory.CloseCurrentBatchRequest();

            var response = await _client.SendRequest<Batch>(request);
            
            Assert.That(response.Status, Is.EqualTo("closing"));
        }

        
        //[Test]
        //[Order(29)]
        //public async Task TestCashRefund()
        //{
        //    var request = UsaEPayRequestFactory.CashRefundRequest(10);

        //    var response = await _client.SendRequest(request);

        //    Assert.That(response.ResultCode, Is.EqualTo("A"));
        //}
    }
}