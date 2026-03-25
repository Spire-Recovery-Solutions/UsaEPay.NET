Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Sale - USAePay Help

# Sale

## Overview

This page will show you how to initiate a sale transaction, whether it be via credit/debit card, check, or cash. The API endpoint below encapsulates all of these use cases related to sales. One specifies what type of sale is being performed via the `command` parameter. Note that while this allows for great flexibility on the user-facing API, this one endpoint may accept/reject certain parameters based on the type of sale being performed. Please refer to the [typical use cases][1] at the bottom for specific implementation details.

The API endpoint is as follows:

```
POST /api/v2/transactions
```

## Quick Start

### Request

```
curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
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
    "invoice": "12356"
    }'
```

> This cURL request is an example of a merchant who is trying to charge a customer via a credit card sale.

### Response

```
{
  "type": "transaction",
  "key": "bnf17whjvqqpj2s",
  "refnum": "124444201",
  "is_duplicate": "N",
  "result_code": "A",
  "result": "Approved",
  "authcode": "147598",
  "creditcard": {
    "number": "4000xxxxxxxx2224",
    "category_code": "A"
  },
  "invoice": "12356",
  "avs": {
    "result_code": "YYY",
    "result": "Address: Match & 5 Digit Zip: Match"
  },
  "cvc": {
    "result_code": "M",
    "result": "Match"
  },
  "batch": {
    "type": "batch",
    "key": "0t1k3yx5xs37cvb",
    "sequence": "1"
  },
  "auth_amount": "5"
}
```

> This is the sample response object sent back from the server.

## Request Parameters

This is a complete overview of the request parameters that are sent. Note that each command type may only accept a subset of the parameters.

| Parameter Name | Type | Description |
| --- | --- | --- |
| **Command** | string | This specified the type of transaction to run. Look [here][2] for the list of supported commands. |
| **Amount** | double | Total transaction amount (Including tax, tip, shipping, etc.) |
| **[creditcard*][3]** | [object][4] | Object which holds all credit card information |
| **[check**][5]** | [object][6] | Object which holds all check information |
| payment_key | string | One time token created when using the [Client JS Library][7]. |
| save_card | bool | Set to true to tokenize card. |
| currency | string | Currency numerical code. [Full list of supported numerical currency codes can be found here.][8] Defaults to 840 (USD). |
| customerid | string | Merchant assigned customer ID |
| invoice | string | Custom Invoice Number to easily retrieve sale details. (11 chars max) |
| ponum | string | Customer's purchase order number (required for level 3 processing) |
| orderid | string | Merchant assigned order ID |
| description | string | Public description of the transaction. |
| comments | string | Private comment details only visible to the merchant. |
| terminal | string | Terminal identifier (i.e. multilane) |
| table | integer | Restaurant table number |
| clerk | string | Clerk/Cashier/Server name |
| email | string | Customer's email address |
| send_receipt | bool | If set, this parameter will send an email to the email passed in the request.3 |
| [terminal_detail][9] | [object][10] | Details on the type of terminal |
| [amount_detail][11] | [object][12] | Object containing a more detailed breakdown of the amount. Not required if Amount is previously set. |
| [billing_address][13] | [object][14] | Object which holds the billing address information. |
| [shipping_address][15] | [object][16] | Object which holds the shipping address information. |
| [traits][17] | [object][18] | Object which holds transaction characteristics. |
| clientip | string | IP address of client. Used in conjunction with the `Block By Host or IP` fraud module. |
| geolocation | string | Latitude and longitude of transaction location. |
| software | string | Software name and version (useful for troubleshooting) |
| ignore_duplicate | bool | Set to `true` to Bypass duplicate detection/folding. |

* Required field for credit card transactions

** Required field for check transactions

## Response Variables

| Variable | Type | Description |
| --- | --- | --- |
| type | string | The type of sale performed. Successful sales return a value of 'transaction'. |
| key | string | Unique key for the transaction record |
| refnum | string | Transaction reference number |
| is_duplicate | bool | If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. |
| result_code | string | Result code ('A' = Approved, 'P' = Partial Approval, 'D' = Declined, 'E' = Error, or 'V' = Verification Required) |
| result | string | Long version of above result_code ('Approved', etc) |
| authcode | string | Authorization code |
| auth_amount | double | Amount authorized |
| [avs][19] | [object][20] | Object which contains the verification status of the AVS response system |
| [cvc][21] | [object][22] | Object which contains the status of the Card Security Code (the 3-4 check digit on a credit card) |

# Sample Code

## List of available sale commands

| Credit Card | Check | Cash |
| --- | --- | --- |
| [cc:sale][23] | [check:sale][24] | [cash:sale][25] |

## Credit Card Sale

To make a credit card sale:

```
curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
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
    "invoice": "12356"
    }'
```

> This cURL request is an example of a merchant who is trying to charge a customer via a credit card sale.

```
{
  "type": "transaction",
  "key": "bnf17whjvqqpj2s",
  "refnum": "124444201",
  "is_duplicate": "N",
  "result_code": "A",
  "result": "Approved",
  "authcode": "147598",
  "creditcard": {
    "number": "4000xxxxxxxx2224",
    "category_code": "A"
  },
  "invoice": "12356",
  "avs": {
    "result_code": "YYY",
    "result": "Address: Match & 5 Digit Zip: Match"
  },
  "cvc": {
    "result_code": "M",
    "result": "Match"
  },
  "batch": {
    "type": "batch",
    "key": "0t1k3yx5xs37cvb",
    "sequence": "1"
  },
  "auth_amount": "5"
}
```

> This is the sample response object sent back from the server.

## Check Sale

To make a check sale:

```
curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "check:sale",
    "amount": "5.00",
    "amount_detail": {
        "tax": "1.00",
        "tip": "0.50"
    },
    "check": {
        "accountholder": "John Doe",
        "account": "324523524",
        "routing": "123456789"
    },
    "invoice": "12356"
}'
```

> This cURL request is an example of a merchant who is trying to charge a customer via a check sale.

```
{
  "type": "transaction",
  "key": "3nf18vbrqpp32ts",
  "refnum": "124444297",
  "is_duplicate": "N",
  "result_code": "A",
  "result": "Approved",
  "authcode": "TM4054",
  "invoice": "12356",
  "proc_refnum": "17062698499437"
}
```

> This is the sample response object sent back from the server.

## Payment Key Sale

If processing using the [Client JS Library][26], you can also process a sale with a `payment_key` in place of a card number. The `payment_key` is a one time use token. To create a reusable token, set the `save_card` flag to true.

```
curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "cc:sale",
        "payment_key": "02zxfg87ty824",
    "amount": "1124.25",
    "amount_detail": {
        "tax": "0.00",
        "subtotal": "1124.25"
    },
    "billing_address": {
        "firstname": "John",
        "lastname": "Jones",
        "street": "1234 Main",
        "street2": "Apt 123",
        "city": "Spring",
                "state": "TX",
                "postalcode": "77379",
                "country": "USA",
                "phone": "5558675309",
        "email": "12345"
    },
    "custemailaddr": "john@tmcode.com"
    }'
```

> This cURL request is an example of a merchant who is trying to charge a customer via a payment key.

```
{
  "type": "transaction",
  "key": "bnf17whjvqqpj2s",
  "refnum": "124444201",
  "is_duplicate": "N",
  "result_code": "A",
  "result": "Approved",
  "authcode": "147598",
  "creditcard": {
    "number": "4000xxxxxxxx2224",
    "category_code": "A"
  },
  "avs": {
    "result_code": "YYY",
    "result": "Address: Match & 5 Digit Zip: Match"
  },
  "cvc": {
    "result_code": "M",
    "result": "Match"
  },
  "batch": {
    "type": "batch",
    "key": "0t1k3yx5xs37cvb",
    "sequence": "1"
  },
  "auth_amount": "1124.25"
}
```

> This is the sample response object sent back from the server.

## Cash Sale

To make a cash sale:

```
curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "cash:sale",
    "amount": "5.00",
    "amount_detail": {
        "tax": "1.00",
        "tip": "0.50"
    }
}'
```

> This cURL request is an example of a merchant who is trying to charge a customer via a cash sale.

```
{
  "type": "transaction",
  "key": "nnf4ktszxfk0f2y",
  "refnum": "124444369",
  "is_duplicate": "N",
  "result_code": "A",
  "result": "Approved",
  "authcode": ""
}
```

> This is the sample response object sent back from the server.

## Change Log

| Date | Change |
| --- | --- |
| 2017-08-01 | Added page. |
| 2017-11-29 | Added `is_final` parameter. |

**[Click here for the full REST API change log.][27]**

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/#sample-code
[2]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/#sample-code
[3]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#creditcard
[4]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#creditcard
[5]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#check
[6]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#check
[7]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/
[8]: https://help.usaepay.info/developer/reference/currencycodes/#currency-codes
[9]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#terminal_detail
[10]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#terminal_detail
[11]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#amount_detail
[12]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#amount_detail
[13]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects/#billing_address
[14]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#billing_address
[15]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#shipping_address
[16]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#shipping_address
[17]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#traits
[18]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#traits
[19]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#shipping_address#avs
[20]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#avs
[21]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#cvc
[22]: https://help.usaepay.info/developer/rest-api/transactions/more/commonobjects#cvc
[23]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/#credit-card-sale
[24]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/#check-sale
[25]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/#make-a-cash-sale
[26]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/
[27]: https://help.usaepay.info/developer/rest-api/changelog/
