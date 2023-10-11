using UsaEPay.NET.Factories;

namespace UsaEPay.NET.Tests
{
    public class UsaEPayClientTests
    {
        private UsaEPayClient _client;

        [SetUp]
        public void Setup()
        {
            // TODO: Read these settings in from local configs
            _client = new UsaEPayClient("", "", "", "", true);
        }

        [Test]
        public async Task TestCardSale()
        {
            var request = UsaEPayRequestFactory.CreditCardSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestTokenSale(string token)
        {
            var request = UsaEPayRequestFactory.TokenSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", token, 123);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestPaymentKeySale(string paymentKey)
        {
            var request = UsaEPayRequestFactory.PaymentKeySaleRequest(10, paymentKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestQuickSale(string transactionKey)
        {
            var request = UsaEPayRequestFactory.QuickSaleRequest(10, transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestCheckSale()
        {
            var request = UsaEPayRequestFactory.CheckSaleRequest(10, "John", "Doe", "555 Test Street", "", "Testington", "OK", "33242", "234234", "234234", "checking", "342424");

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestCashSale()
        {
            var request = UsaEPayRequestFactory.CashSaleRequest(10);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestAuthOnlySale()
        {
            var request = UsaEPayRequestFactory.AuthOnlySaleRequest(10, "John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestPostPayment()
        {
            var request = UsaEPayRequestFactory.PostPaymentRequest(10, "AUTHCODE", "John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestCreditCardRefund()
        {
            var request = UsaEPayRequestFactory.CreditCardRefundRequest(10, "John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestCheckRefund()
        {
            var request = UsaEPayRequestFactory.CheckRefundRequest(10, "John Doe", "234234", "234234", "checking", "342424");

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestCreditVoid(string transactionKey)
        {
            var request = UsaEPayRequestFactory.CreditVoidRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestVoidPayment(string transactionKey)
        {
            var request = UsaEPayRequestFactory.VoidPaymentRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestCapturePayment(string transactionKey)
        {
            var request = UsaEPayRequestFactory.CapturePaymentRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestCapturePaymentError(string transactionKey)
        {
            var request = UsaEPayRequestFactory.CapturePaymentErrorRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestCapturePaymentReauth(string transactionKey)
        {
            var request = UsaEPayRequestFactory.CapturePaymentReauthRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestCapturePaymentOverride(string transactionKey)
        {
            var request = UsaEPayRequestFactory.CapturePaymentOverrideRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestReleaseFunds(string transactionKey)
        {
            var request = UsaEPayRequestFactory.ReleaseFundsRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestUnvoid(string transactionKey)
        {
            var request = UsaEPayRequestFactory.UnvoidRequest(transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestCashRefund()
        {
            var request = UsaEPayRequestFactory.CashRefundRequest(10);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        [TestCase("")]
        public async Task TestQuickRefund(string transactionKey)
        {
            var request = UsaEPayRequestFactory.QuickRefundRequest(10, transactionKey);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }

        [Test]
        public async Task TestTokenizeCard()
        {
            var request = UsaEPayRequestFactory.TokenizeCardRequest("John Doe", "4000100011112224", "0924", 123);

            var response = await _client.SendRequest(request);

            Assert.That(response, Is.Not.Null);
        }
    }
}