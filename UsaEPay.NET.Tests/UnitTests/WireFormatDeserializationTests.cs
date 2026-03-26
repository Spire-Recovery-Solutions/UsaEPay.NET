using System.Text.Json;
using UsaEPay.NET.Models;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.UnitTests;

/// <summary>
/// Validates that real API JSON responses from sandbox probing deserialize correctly
/// through the SDK's System.Text.Json pipeline. Any failure here indicates a
/// JsonPropertyName mismatch or converter bug.
/// </summary>
public sealed class WireFormatDeserializationTests
{
    #region 01 - Credit Card Sale Response

    private const string CcSaleJson = """
        {"type":"transaction","key":"enfy1zj9dh3cdjc","refnum":"3225593736","is_duplicate":"N","result_code":"A","result":"Approved","authcode":"277588","creditcard":{"type":"V","number":"4000xxxxxxxx2224","category_code":"A","entry_mode":"Card Not Present, Manually Keyed"},"invoice":"INV-001","avs":{"result_code":"YYY","result":"Address: Match & 5 Digit Zip: Match"},"cvc":{"result_code":"M","result":"Match"},"batch":{"type":"batch","key":"ct189xsq1m3kdbp","batchrefnum":542814,"sequence":"279"},"auth_amount":"29.99","amount_detail":{"tip":"1.50","tax":"2.50","shipping":"1.00","subtotal":"24.99"},"savedcard":{"type":"Visa","key":"mq96-iisr-f0e8-6w78","cardnumber":"4000xxxxxxxx2224","expiration":"0930"},"trantype":"Credit Card Sale","receipts":{"customer":"Mail Sent Successfully"}}
        """;

    [Test]
    public async Task Deserialize_CcSale_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayResponse>(CcSaleJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("transaction");
        await Assert.That(r.Key).IsEqualTo("enfy1zj9dh3cdjc");
        await Assert.That(r.ReferenceNumber).IsEqualTo("3225593736");
        await Assert.That(r.IsDuplicate).IsEqualTo("N");
        await Assert.That(r.ResultCode).IsEqualTo("A");
        await Assert.That(r.Result).IsEqualTo("Approved");
        await Assert.That(r.AuthCode).IsEqualTo("277588");
        await Assert.That(r.Invoice).IsEqualTo("INV-001");
        await Assert.That(r.AmountAuthorized).IsEqualTo("29.99");
        await Assert.That(r.TransactionType).IsEqualTo("Credit Card Sale");

        // Nested: CreditCard
        await Assert.That(r.CreditCard).IsNotNull();
        await Assert.That(r.CreditCard!.CardType).IsEqualTo("V");
        await Assert.That(r.CreditCard.Number).IsEqualTo("4000xxxxxxxx2224");
        await Assert.That(r.CreditCard.CategoryCode).IsEqualTo("A");
        await Assert.That(r.CreditCard.EntryMode).IsEqualTo("Card Not Present, Manually Keyed");

        // Nested: AVS
        await Assert.That(r.Avs).IsNotNull();
        await Assert.That(r.Avs!.ResultCode).IsEqualTo("YYY");

        // Nested: CVC
        await Assert.That(r.Cvc).IsNotNull();
        await Assert.That(r.Cvc!.ResultCode).IsEqualTo("M");

        // Nested: Batch
        await Assert.That(r.Batch).IsNotNull();
        await Assert.That(r.Batch!.Key).IsEqualTo("ct189xsq1m3kdbp");
        await Assert.That(r.Batch.BatchReferenceNumber).IsEqualTo(542814L);
        await Assert.That(r.Batch.Sequence).IsEqualTo("279");

        // Nested: AmountDetail
        await Assert.That(r.AmountDetail).IsNotNull();
        await Assert.That(r.AmountDetail!.Tip).IsEqualTo(1.50m);

        // Nested: SavedCard
        await Assert.That(r.SavedCard).IsNotNull();
        await Assert.That(r.SavedCard!.Type).IsEqualTo("Visa");
        await Assert.That(r.SavedCard.Key).IsEqualTo("mq96-iisr-f0e8-6w78");
        await Assert.That(r.SavedCard.CardNumber).IsEqualTo("4000xxxxxxxx2224");
        await Assert.That(r.SavedCard.Expiration).IsEqualTo("0930");

        // Nested: Receipts
        await Assert.That(r.Receipts).IsNotNull();
        await Assert.That(r.Receipts!.Customer).IsEqualTo("Mail Sent Successfully");
    }

    #endregion

    #region 04 - Check (ACH) Sale Response

    private const string CheckSaleJson = """
        {"type":"transaction","key":"nnfkp3bkd8vpmsz","refnum":"3225593745","is_duplicate":"N","result_code":"A","result":"Approved","authcode":"TM8930","invoice":"INV-CHECK-001","proc_refnum":"26032674555318","auth_amount":"15.00"}
        """;

    [Test]
    public async Task Deserialize_CheckSale_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayResponse>(CheckSaleJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("transaction");
        await Assert.That(r.Key).IsEqualTo("nnfkp3bkd8vpmsz");
        await Assert.That(r.ReferenceNumber).IsEqualTo("3225593745");
        await Assert.That(r.ResultCode).IsEqualTo("A");
        await Assert.That(r.AuthCode).IsEqualTo("TM8930");
        await Assert.That(r.Invoice).IsEqualTo("INV-CHECK-001");
        await Assert.That(r.ProcessorReferenceNumber).IsEqualTo("26032674555318");
        await Assert.That(r.AmountAuthorized).IsEqualTo("15.00");
    }

    #endregion

    #region 19 - Get Transaction (Full Detail)

    private const string GetTransactionJson = """
        {"type":"transaction","key":"enfy1zj9dh3cdjc","refnum":"3225593736","created":"2026-03-26 13:01:37","trantype_code":"S","trantype":"Credit Card Sale","result_code":"A","result":"Approved","authcode":"277588","status_code":"P","status":"Authorized (Pending Settlement)","creditcard":{"cardholder":"","number":"4000xxxxxxxx2224","avs_street":"123 Main St","avs_postalcode":"90046","category_code":"A","type":"V","entry_mode":"Card Not Present, Manually Keyed"},"avs":{"result_code":"YYY","result":"Address: Match & 5 Digit Zip: Match"},"cvc":{"result_code":"M","result":"Match"},"batch":{"type":"batch","key":"ct189xsq1m3kdbp","batchrefnum":"542814","sequence":"279"},"amount":"35.00","amount_detail":{"tip":"1.50","tax":"2.50","shipping":"1.00","discount":"0.00","subtotal":"24.99"},"ponum":"PO-12345","invoice":"INV-001","orderid":"ORD-99999","description":"Test sale with all fields","comments":"Comprehensive field test","clerk":"Clerk01","customer_email":"john@example.com","merchant_email":"","clientip":"192.168.1.100","source_name":"Sandbox","billing_address":{"company":"Acme Inc","first_name":"John","last_name":"Doe","street":"123 Main St","street2":"Apt 4","city":"Los Angeles","state":"CA","country":"US","postalcode":"90046","phone":"5551234567","fax":"5559876543"},"shipping_address":{"company":"Ship Co","first_name":"Jane","last_name":"Smith","street":"456 Oak Ave","street2":"Suite 100","city":"New York","state":"NY","country":"US","postalcode":"10001","phone":"5555551234"},"lineitems":[{"sku":"WDG-001","name":"Widget","description":"Blue Widget","cost":"12.50","taxable":"Y","qty":"2.0000","tax_amount":"1.25","tax_rate":"5.000","discount_rate":"0.000","commodity_code":"1234","um":"EA"}],"custom_fields":{"1":"Custom1Value","2":"Custom2Value"},"platform":{"name":"Test Bed"},"available_actions":["void","void:release","queue","quicksale","quickrefund","addtip"]}
        """;

    [Test]
    public async Task Deserialize_GetTransaction_FullDetail()
    {
        var r = JsonSerializer.Deserialize<UsaEPayResponse>(GetTransactionJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Key).IsEqualTo("enfy1zj9dh3cdjc");
        await Assert.That(r.CreatedTimestamp).IsNotNull();
        await Assert.That(r.TrantypeCode).IsEqualTo("S");
        await Assert.That(r.TransactionType).IsEqualTo("Credit Card Sale");
        await Assert.That(r.StatusCode).IsEqualTo("P");
        await Assert.That(r.Status).IsEqualTo("Authorized (Pending Settlement)");
        await Assert.That(r.Amount).IsEqualTo(35.00m);
        await Assert.That(r.Ponum).IsEqualTo("PO-12345");
        await Assert.That(r.OrderId).IsEqualTo("ORD-99999");
        await Assert.That(r.Description).IsEqualTo("Test sale with all fields");
        await Assert.That(r.Comments).IsEqualTo("Comprehensive field test");
        await Assert.That(r.Clerk).IsEqualTo("Clerk01");
        await Assert.That(r.CustomerEmail).IsEqualTo("john@example.com");
        await Assert.That(r.ClientIp).IsEqualTo("192.168.1.100");
        await Assert.That(r.SourceName).IsEqualTo("Sandbox");

        // Nested: CreditCard with AVS fields
        await Assert.That(r.CreditCard).IsNotNull();
        await Assert.That(r.CreditCard!.AvsStreet).IsEqualTo("123 Main St");
        await Assert.That(r.CreditCard.AvsPostalCode).IsEqualTo("90046");

        // Nested: BillingAddress
        await Assert.That(r.BillingAddress).IsNotNull();
        await Assert.That(r.BillingAddress!.Company).IsEqualTo("Acme Inc");
        await Assert.That(r.BillingAddress.FirstName).IsEqualTo("John");
        await Assert.That(r.BillingAddress.LastName).IsEqualTo("Doe");
        await Assert.That(r.BillingAddress.Street).IsEqualTo("123 Main St");
        await Assert.That(r.BillingAddress.Street2).IsEqualTo("Apt 4");
        await Assert.That(r.BillingAddress.City).IsEqualTo("Los Angeles");
        await Assert.That(r.BillingAddress.State).IsEqualTo("CA");
        await Assert.That(r.BillingAddress.PostalCode).IsEqualTo("90046");

        // Nested: ShippingAddress
        await Assert.That(r.ShippingAddress).IsNotNull();
        await Assert.That(r.ShippingAddress!.FirstName).IsEqualTo("Jane");
        await Assert.That(r.ShippingAddress.City).IsEqualTo("New York");

        // Nested: LineItems
        await Assert.That(r.LineItems).IsNotNull();
        await Assert.That(r.LineItems!.Count).IsEqualTo(1);
        await Assert.That(r.LineItems[0].StockKeepingUnitNumber).IsEqualTo("WDG-001");
        await Assert.That(r.LineItems[0].Name).IsEqualTo("Widget");

        // Nested: CustomFields
        await Assert.That(r.CustomFields).IsNotNull();
        await Assert.That(r.CustomFields!["1"]).IsEqualTo("Custom1Value");
        await Assert.That(r.CustomFields["2"]).IsEqualTo("Custom2Value");

        // Nested: Platform
        await Assert.That(r.Platform).IsNotNull();
        await Assert.That(r.Platform!.Name).IsEqualTo("Test Bed");

        // AvailableActions
        await Assert.That(r.AvailableActions).IsNotNull();
        await Assert.That(r.AvailableActions!.Length).IsEqualTo(6);
    }

    #endregion

    #region 20 - List Transactions

    private const string ListTransactionsJson = """
        {"type":"list","limit":2,"offset":0,"data":[{"type":"transaction","key":"pnftvbd8nm0jm20","refnum":"3225593799","created":"2026-03-26 13:03:35","trantype_code":"S","trantype":"Credit Card Sale","result_code":"A","result":"Approved","authcode":"123456","status_code":"P","status":"Authorized (Pending Settlement)","creditcard":{"cardholder":"","number":"4000xxxxxxxx2224","category_code":"","type":"V","entry_mode":"Card Not Present, Manually Keyed"},"batch":{"type":"batch","key":"ct189xsq1m3kdbp","batchrefnum":"542814","sequence":"279"},"amount":"25.00","amount_detail":{"tip":"0.00","tax":"0.00","shipping":"0.00","discount":"0.00","subtotal":"25.00"},"invoice":"INV-POSTAUTH-001","merchant_email":"","source_name":"Sandbox","platform":{"name":"Test Bed"},"available_actions":["void","queue","quicksale","quickrefund","addtip"]},{"type":"transaction","key":"jnfw41jqp315rrg","refnum":"3225593793","created":"2026-03-26 13:03:16","trantype_code":"H","trantype":"Reverse ACH","result_code":"A","result":"Approved","authcode":"TM3C00","status_code":"P","status":"Pending","check":{"account_number":"xxxxx3524","routing_number":"xxxxx6789","account_type":"Checking","accountholder":"Jane Smith","checknum":"1002","trackingnum":"26032674555397","effective":"2026-03-27","processed":null,"settled":null,"returned":null,"banknote":null,"format":"WEB"},"amount":"5.00","amount_detail":{"tip":"0.00","tax":"0.00","shipping":"0.00","discount":"0.00","subtotal":"5.00"},"merchant_email":"","source_name":"Sandbox","platform":{"name":"TestBed"}}],"total":"22552"}
        """;

    [Test]
    public async Task Deserialize_ListTransactions_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayListTransactionResponse>(ListTransactionsJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("list");
        await Assert.That(r.Limit).IsEqualTo(2L);
        await Assert.That(r.Offset).IsEqualTo(0L);
        await Assert.That(r.Total).IsEqualTo(22552L);
        await Assert.That(r.Data).IsNotNull();
        await Assert.That(r.Data!.Length).IsEqualTo(2);

        // First item: credit card transaction
        var cc = r.Data[0];
        await Assert.That(cc.Key).IsEqualTo("pnftvbd8nm0jm20");
        await Assert.That(cc.TrantypeCode).IsEqualTo("S");
        await Assert.That(cc.Amount).IsEqualTo(25.00m);
        await Assert.That(cc.CreditCard).IsNotNull();
        await Assert.That(cc.CreditCard!.CardType).IsEqualTo("V");

        // Second item: ACH transaction with check details
        var ach = r.Data[1];
        await Assert.That(ach.Key).IsEqualTo("jnfw41jqp315rrg");
        await Assert.That(ach.TrantypeCode).IsEqualTo("H");
        await Assert.That(ach.Check).IsNotNull();
        await Assert.That(ach.Check!.AccountNumber).IsEqualTo("xxxxx3524");
        await Assert.That(ach.Check.RoutingNumber).IsEqualTo("xxxxx6789");
        await Assert.That(ach.Check.AccountType).IsEqualTo("Checking");
        await Assert.That(ach.Check.AccountHolder).IsEqualTo("Jane Smith");
        await Assert.That(ach.Check.CheckNum).IsEqualTo("1002");
        await Assert.That(ach.Check.TrackingNum).IsEqualTo("26032674555397");
        await Assert.That(ach.Check.Effective).IsEqualTo("2026-03-27");
        await Assert.That(ach.Check.Format).IsEqualTo("WEB");
    }

    #endregion

    #region 23 - Batch List

    private const string BatchListJson = """
        {"type":"list","limit":2,"offset":0,"data":[{"key":"ct189xsq1m3kdbp","type":"batch","batchrefnum":"542814","sequence":"279","parent_sequence":null,"opened":"2026-03-26 13:01:37","closed":null,"resubmitted":null,"locked":false,"lockdate":null,"trancutoff":null,"sales_count":"6","sales":"132.50","credits_count":"2","credits":"4.00"},{"key":"9t1j3xgy5w9467s","type":"batch","batchrefnum":"542811","sequence":"2612","parent_sequence":null,"opened":"2026-03-26 12:49:01","closed":"2026-03-26 12:52:33","resubmitted":null,"locked":false,"lockdate":"2026-03-26 12:52:32","trancutoff":null,"sales_count":"16","sales":"157.25","credits_count":"6","credits":"31.75"}],"total":"283"}
        """;

    [Test]
    public async Task Deserialize_BatchList_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayBatchListResponse>(BatchListJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("list");
        await Assert.That(r.Limit).IsEqualTo(2L);
        await Assert.That(r.Total).IsEqualTo(283L);
        await Assert.That(r.Data).IsNotNull();
        await Assert.That(r.Data!.Length).IsEqualTo(2);

        // First batch (open)
        var b1 = r.Data[0];
        await Assert.That(b1.Key).IsEqualTo("ct189xsq1m3kdbp");
        await Assert.That(b1.Type).IsEqualTo("batch");
        await Assert.That(b1.BatchReferenceNumber).IsEqualTo(542814L);
        await Assert.That(b1.Sequence).IsEqualTo("279");
        await Assert.That(b1.Opened).IsNotNull();
        await Assert.That(b1.Closed).IsNull();
        await Assert.That(b1.Locked).IsEqualTo(false);
        await Assert.That(b1.SalesCount).IsEqualTo(6L);
        await Assert.That(b1.Sales).IsEqualTo("132.50");
        await Assert.That(b1.CreditsCount).IsEqualTo("2");
        await Assert.That(b1.Credits).IsEqualTo("4.00");

        // Second batch (closed)
        var b2 = r.Data[1];
        await Assert.That(b2.Key).IsEqualTo("9t1j3xgy5w9467s");
        await Assert.That(b2.Closed).IsNotNull();
        await Assert.That(b2.LockDate).IsEqualTo("2026-03-26 12:52:32");
    }

    #endregion

    #region 24 - Batch Current (Single Batch)

    private const string BatchCurrentJson = """
        {"type":"batch","key":"ct189xsq1m3kdbp","batchrefnum":"542814","opened":"2026-03-26 13:01:37","status":"open","scheduled":"2026-03-27 01:01:37","total_amount":128.5,"total_count":9,"sales_amount":132.5,"sales_count":6,"voids_amount":75,"voids_count":1,"refunds_amount":4,"refunds_count":2}
        """;

    [Test]
    public async Task Deserialize_BatchCurrent_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<Batch>(BatchCurrentJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("batch");
        await Assert.That(r.Key).IsEqualTo("ct189xsq1m3kdbp");
        await Assert.That(r.BatchReferenceNumber).IsEqualTo(542814L);
        await Assert.That(r.Status).IsEqualTo("open");
        await Assert.That(r.Opened).IsNotNull();
        await Assert.That(r.Scheduled).IsNotNull();
        await Assert.That(r.TotalAmount).IsEqualTo(128.5m);
        await Assert.That(r.TotalCount).IsEqualTo(9L);
        await Assert.That(r.SalesAmount).IsEqualTo(132.5m);
        await Assert.That(r.SalesCount).IsEqualTo(6L);
        await Assert.That(r.VoidsAmount).IsEqualTo(75m);
        await Assert.That(r.VoidsCount).IsEqualTo(1L);
        await Assert.That(r.RefundsAmount).IsEqualTo(4m);
        await Assert.That(r.RefundsCount).IsEqualTo(2L);
    }

    #endregion

    #region 21 - Create Token

    private const string CreateTokenJson = """
        {"token":{"cardref":"tl5e-smxx-s6yh-o8c6","masked_card_number":"XXXXXXXXXXXX2224","card_type":"Visa"}}
        """;

    [Test]
    public async Task Deserialize_CreateToken_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayTokenResponse>(CreateTokenJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Token).IsNotNull();
        await Assert.That(r.Token!.CardRef).IsEqualTo("tl5e-smxx-s6yh-o8c6");
        await Assert.That(r.Token.MaskedCardNumber).IsEqualTo("XXXXXXXXXXXX2224");
        await Assert.That(r.Token.CardType).IsEqualTo("Visa");
    }

    #endregion

    #region Customer Get

    private const string CustomerGetJson = """
        {"key":"dsddpf0skjps3rvh","type":"customer","customerid":"CUST-001","custid":"20625189","company":"Test Corp","first_name":"John","last_name":"Doe","street":"123 Main St","street2":"Suite 100","city":"Los Angeles","state":"CA","postalcode":"90001","country":"US","phone":"5555551234","fax":"5555554321","email":"john@example.com","url":"https://example.com","notes":"Test customer for probing","description":"Probe test customer","custom_fields":{"1":null,"2":null,"3":null,"4":null,"5":null,"6":null,"7":null,"8":null,"9":null,"10":null,"11":null,"12":null,"13":null,"14":null,"15":null,"16":null,"17":null,"18":null,"19":null,"20":null},"billing_schedules":[],"payment_methods":[]}
        """;

    [Test]
    public async Task Deserialize_CustomerGet_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayCustomerResponse>(CustomerGetJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Key).IsEqualTo("dsddpf0skjps3rvh");
        await Assert.That(r.Type).IsEqualTo("customer");
        await Assert.That(r.CustomerId).IsEqualTo("CUST-001");
        await Assert.That(r.CustId).IsEqualTo(20625189L);
        await Assert.That(r.Company).IsEqualTo("Test Corp");
        await Assert.That(r.FirstName).IsEqualTo("John");
        await Assert.That(r.LastName).IsEqualTo("Doe");
        await Assert.That(r.Street).IsEqualTo("123 Main St");
        await Assert.That(r.Street2).IsEqualTo("Suite 100");
        await Assert.That(r.City).IsEqualTo("Los Angeles");
        await Assert.That(r.State).IsEqualTo("CA");
        await Assert.That(r.PostalCode).IsEqualTo("90001");
        await Assert.That(r.Country).IsEqualTo("US");
        await Assert.That(r.Phone).IsEqualTo("5555551234");
        await Assert.That(r.Fax).IsEqualTo("5555554321");
        await Assert.That(r.Email).IsEqualTo("john@example.com");
        await Assert.That(r.Url).IsEqualTo("https://example.com");
        await Assert.That(r.Notes).IsEqualTo("Test customer for probing");
        await Assert.That(r.Description).IsEqualTo("Probe test customer");

        // Custom fields - all null but dictionary should be populated
        await Assert.That(r.CustomFields).IsNotNull();
        await Assert.That(r.CustomFields!.Count).IsEqualTo(20);

        // Empty arrays
        await Assert.That(r.BillingSchedules).IsNotNull();
        await Assert.That(r.BillingSchedules!.Length).IsEqualTo(0);
        await Assert.That(r.PaymentMethods).IsNotNull();
        await Assert.That(r.PaymentMethods!.Length).IsEqualTo(0);
    }

    #endregion

    #region Customer List

    private const string CustomerListJson = """
        {"type":"list","limit":2,"offset":0,"data":[{"key":"dsddpf0skjps3rvh","type":"customer","customerid":"CUST-001","custid":"20625189","company":"Test Corp","first_name":"John","last_name":"Doe","street":"123 Main St","street2":"Suite 100","city":"Los Angeles","state":"CA","postalcode":"90001","country":"US","phone":"5555551234","fax":"5555554321","email":"john@example.com","url":"https://example.com","notes":"Test customer for probing","description":"Probe test customer","custom_fields":{"1":null,"2":null,"3":null,"4":null,"5":null,"6":null,"7":null,"8":null,"9":null,"10":null,"11":null,"12":null,"13":null,"14":null,"15":null,"16":null,"17":null,"18":null,"19":null,"20":null}},{"key":"asddx7vwk4x0sw6s","type":"customer","customerid":"CUST-001","custid":"20625186","company":"Test Corp","first_name":"John","last_name":"Doe","street":"123 Main St","street2":"Suite 100","city":"Los Angeles","state":"CA","postalcode":"90001","country":"US","phone":"5555551234","fax":"5555554321","email":"john@example.com","url":"https://example.com","notes":"Test customer for probing","description":"Probe test customer","custom_fields":{"1":null,"2":null,"3":null,"4":null,"5":null,"6":null,"7":null,"8":null,"9":null,"10":null,"11":null,"12":null,"13":null,"14":null,"15":null,"16":null,"17":null,"18":null,"19":null,"20":null}}],"total":"3"}
        """;

    [Test]
    public async Task Deserialize_CustomerList_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayCustomerListResponse>(CustomerListJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("list");
        await Assert.That(r.Limit).IsEqualTo(2L);
        await Assert.That(r.Offset).IsEqualTo(0L);
        await Assert.That(r.Total).IsEqualTo(3L);
        await Assert.That(r.Data).IsNotNull();
        await Assert.That(r.Data!.Length).IsEqualTo(2);

        await Assert.That(r.Data[0].Key).IsEqualTo("dsddpf0skjps3rvh");
        await Assert.That(r.Data[0].CustId).IsEqualTo(20625189L);
        await Assert.That(r.Data[1].Key).IsEqualTo("asddx7vwk4x0sw6s");
        await Assert.That(r.Data[1].CustId).IsEqualTo(20625186L);
    }

    #endregion

    #region Payment Method List

    private const string PaymentMethodListJson = """
        {"type":"list","limit":20,"offset":0,"data":[{"key":"an02pc6wdjfsr7mv8","type":"customerpaymentmethod","method_name":"Visa","expires":"0929","cardholder":"Test User","card_type":"Visa","ccnum4last":"2224","avs_street":"123 Main","avs_postalcode":"90001","sortord":"1","added":"2026-03-26 13:02:26","updated":"2026-03-26 13:02:26","pay_type":"cc"},{"key":"dn02pfzv609hyy0n8","type":"customerpaymentmethod","method_name":"My Checking","account_number":"3210","routing_number":"6789","sortord":"1","added":"2026-03-26 13:02:32","updated":"2026-03-26 13:02:32","pay_type":"check"}],"total":"2"}
        """;

    [Test]
    public async Task Deserialize_PaymentMethodList_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayPaymentMethodListResponse>(PaymentMethodListJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Type).IsEqualTo("list");
        await Assert.That(r.Limit).IsEqualTo(20L);
        await Assert.That(r.Total).IsEqualTo(2L);
        await Assert.That(r.Data).IsNotNull();
        await Assert.That(r.Data!.Length).IsEqualTo(2);

        // Credit card payment method
        var cc = r.Data[0];
        await Assert.That(cc.Key).IsEqualTo("an02pc6wdjfsr7mv8");
        await Assert.That(cc.Type).IsEqualTo("customerpaymentmethod");
        await Assert.That(cc.MethodName).IsEqualTo("Visa");
        await Assert.That(cc.Expires).IsEqualTo("0929");
        await Assert.That(cc.Cardholder).IsEqualTo("Test User");
        await Assert.That(cc.CardType).IsEqualTo("Visa");
        await Assert.That(cc.Ccnum4Last).IsEqualTo("2224");
        await Assert.That(cc.AvsStreet).IsEqualTo("123 Main");
        await Assert.That(cc.AvsPostalCode).IsEqualTo("90001");
        await Assert.That(cc.SortOrder).IsEqualTo(1L);
        await Assert.That(cc.Added).IsEqualTo("2026-03-26 13:02:26");
        await Assert.That(cc.Updated).IsEqualTo("2026-03-26 13:02:26");
        await Assert.That(cc.PayType).IsEqualTo("cc");

        // ACH payment method
        var ach = r.Data[1];
        await Assert.That(ach.Key).IsEqualTo("dn02pfzv609hyy0n8");
        await Assert.That(ach.MethodName).IsEqualTo("My Checking");
        await Assert.That(ach.AccountNumber).IsEqualTo("3210");
        await Assert.That(ach.RoutingNumber).IsEqualTo("6789");
        await Assert.That(ach.PayType).IsEqualTo("check");
    }

    #endregion

    #region Billing Schedule Get Single

    private const string BillingScheduleJson = """
        {"key":"5n0mt8q5pm0dj82y8","type":"billingschedule","paymethod_key":"an02pc6wdjfsr7mv8","method_name":"Visa","amount":"10.00","currency_code":"0","description":"","enabled":"0","frequency":"monthly","next_date":"2027-01-01","numleft":"12","orderid":"","receipt_note":"","send_receipt":false,"source":"0","start_date":"0000-00-00","tax":"0.00","user":"","username":"","skip_count":"0","rules":[]}
        """;

    [Test]
    public async Task Deserialize_BillingSchedule_AllFieldsPopulated()
    {
        var r = JsonSerializer.Deserialize<UsaEPayBillingScheduleResponse>(BillingScheduleJson, UsaEPaySerializerContext.Default.Options);

        await Assert.That(r).IsNotNull();
        await Assert.That(r!.Key).IsEqualTo("5n0mt8q5pm0dj82y8");
        await Assert.That(r.Type).IsEqualTo("billingschedule");
        await Assert.That(r.PaymentMethodKey).IsEqualTo("an02pc6wdjfsr7mv8");
        await Assert.That(r.MethodName).IsEqualTo("Visa");
        await Assert.That(r.Amount).IsEqualTo("10.00");
        await Assert.That(r.CurrencyCode).IsEqualTo("0");
        await Assert.That(r.Enabled).IsEqualTo("0");
        await Assert.That(r.Frequency).IsEqualTo("monthly");
        await Assert.That(r.NextDate).IsEqualTo("2027-01-01");
        await Assert.That(r.NumLeft).IsEqualTo("12");
        await Assert.That(r.SendReceipt).IsEqualTo(false);
        await Assert.That(r.Source).IsEqualTo("0");
        await Assert.That(r.StartDate).IsEqualTo("0000-00-00");
        await Assert.That(r.Tax).IsEqualTo("0.00");
        await Assert.That(r.SkipCount).IsEqualTo("0");
        await Assert.That(r.Rules).IsNotNull();
        await Assert.That(r.Rules!.Count).IsEqualTo(0);
    }

    #endregion
}
