Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Transaction ID's - USAePay Help

# Transactions Key vs. ID

In the legacy [Transaction API][1] and [SOAP API][2] the _Transaction ID_ or _Refnum_ was the gateway's unique transaction identifier. With the implementation of the [REST API][3], the _Transaction Key_ was introduced, and also added the current [SOAP API][4] version, but maintained the _Transaction ID_ for backwards compatibility.

```
"type": "transaction",
"key": "bnf17whjvqqpj2s",
"refnum": "124444201",
....
```

**The _Transaction Key_ is the preferred transaction identifier. It is unique across the whole gateway no matter the transaction scope.**

The [Transaction Scope][5] feature allows for more than one merchant to have a the same _Transaction ID_. The _Transaction ID_ is unique within a merchant's account, but no longer unique across the whole gateway.

# Storing Transaction ID's as Integers

On October 1, 2018 USAePay's sequential transaction identifier (also known as the `Transaction ID` or `refnum`) will surpass the limit for 32 bit integers. While this will not impact the gateway's operation in any way. If you are storing the sequential transaction identifier as a signed 32 bit integer it is likely you will experience issues storing transaction results after October 1, 2018.

We recommend you revise your software to store the Transaction ID as a string or larger numeric variable. If this is not possible, you can update the your transaction scope to _merchant scoped_ using one of the API flags below:

# Transaction Scope

If your integration is only designed to handle integers in the `TransactionID` or `refnum` field, you can update the merchant's Transaction ID's to be _merchant scoped_ rather than _system scoped_.

* * *

**Merchant scoped** transaction ids are unique to the merchant's gateway account.

**System scoped** transaction ids are unique across the entire gateway.

* * *

Merchant scoped transaction ids will start well below the integer limit at 100000 and they will grow sequentially without gaps. The gateway supports system scoped and merchant scoped transactions within the same account. This allows integrations only designed to handle integers in the Transaction ID field to continue to process using merchant scoped transaction ids that are much lower than the system ones. Meanwhile other transactions run by the merchant such as in the virtual terminal could use the system ids. To enable merchant scoped transaction ids you can use the following methods.

## Option 1: Contact Support to Update Merchant Settings

To update the overall merchant setting to **Merchant Scoped**, call into our customer support department at (866) 872-3729 or email support@usaepay.com with a request to change the merchant's setting to **Merchant Scoped**. This will cause all transactions run on the merchant to use a transaction id unique only to the merchant. This will also default the merchants starting transaction id to 100,000.

## Option 2: Set Transaction Scope Using the API

You can set the transaction scope in the REST, SOAP, or legacy transaction API. If scope is not set in the API, the gateway defaults to the merchant's settings. If the scope is set in the API this will override the merchant settings.

### REST

For the REST API, pass through the variable `refnum_scope`. Set to `system` to use system scoped transaction ids. Set to `merchant` to use merchant scoped transaction ids. Examples of each below:

#### Merchant Scoped REST Example

> This request is an example of a merchant who is processing a credit card sale with scope set to merchant.

```
'{
    "command": "cc:sale",
    "amount": "5.00",
    "amount_detail": {
        "tax": "1.00",
        "tip": "0.50"
    },
    "creditcard": {
        "cardholder": "John Doe",
        "number": "4000100011112224",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Main",
        "avs_zip": "12345"
    },
    "refnum_scope": "merchant"
    }'
```

#### System Scoped REST Example

> This request is an example of a merchant who is processing a credit card sale with scope set to system.

```
'{
    "command": "cc:sale",
    "amount": "5.00",
    "amount_detail": {
        "tax": "1.00",
        "tip": "0.50"
    },
    "creditcard": {
        "cardholder": "John Doe",
        "number": "4000100011112224",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Main",
        "avs_zip": "12345"
    },
    "refnum_scope": "system"
    }'
```

### SOAP

If using SOAP, developers would need to add `UMrefnumScope` to the custom field array or use the http header outlined in [Option 3][6]. If using the custom field array, set to `system` to use system scoped transaction ids or set to `merchant` to use merchant scoped transaction ids.

### Legacy Transaction API

To set the transaction scope using TransactionAPI, include the `UMrefnumScope` variable. Set `UMrefnumScope` to `system` to use system scoped transaction ids or set to `merchant` to use merchant scoped transaction ids.

## Option 3: Set Transaction Scope Using HTTP Header

Setting the scope in the HTTP header will override both the merchant config and the setting passed through API request. Set the scope in the HTTP header, using `Refnum-Scope`. Set to `system` to use system scoped transaction ids or set to `merchant` to use merchant scoped transaction ids.

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/transaction-api/
[2]: https://help.usaepay.info/developer/soap-api/
[3]: https://help.usaepay.info/developer/rest-api/
[4]: https://help.usaepay.info/developer/soap-api/
[5]: https://help.usaepay.info/developer/reference/transid/#transaction-scope
[6]: https://help.usaepay.info/developer/reference/transid/#option-3-set-transaction-scope-using-http-header
