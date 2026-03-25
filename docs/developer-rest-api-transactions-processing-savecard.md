Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Tokenize - USAePay Help

# Tokenization (Save Card)

## Overview

This page will show you how to tokenize a card number. Tokenization is the process of breaking a stream of text/numbers up into words, phrases, symbols, or other meaningful elements called tokens. This token can then be used in place of a credit card number when processing a transaction. This is useful when a developer does not want to under take the security requirements of storing card data.

This command validates the card data and then returns a card reference token. The card reference token can be used in the card number field in most scenarios. Tokens only store the card number and the expiration date. Tokens can also be used across multiple merchants. They are not related to a specific merchant.

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
    "command": "cc:save",

    "creditcard": {
        "cardholder": "John Doe",
        "number": "4000100011112224",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Main",
        "avs_zip": "12345"
    }

 }
```

> The above cURL request is an example of how to tokenize a card.

### Response

```
{
    "type": "transaction",
    "key": "",
    "refnum": "",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "",
    "creditcard": {
        "number": "4000xxxxxxxx2224"
    },
    "savedcard": {
        "type": "Visa",
        "key": "tqb9-o076-tpfk-lb8l",
        "cardnumber": "4000xxxxxxxx2224"
    }
}
```

> The above is a sample response object sent back from the server.

## Request Parameters

This is a complete overview of the request parameters that are sent. Note that each command type may only accept a subset of the parameters.

| Parameter Name | Type | Description |
| --- | --- | --- |
| **Command** | string | This specifies the type of transaction to run. `cc:save` validates and tokenizes a card |
| **[creditcard][1]** | [object][2] | Object which holds all credit card information |

## Response Variables

| Variable | Type | Description |
| --- | --- | --- |
| type | string | The type of sale performed. Successful tokenizations will return a value of 'transaction'. |
| key | string | Unique key for the transaction record. This will be empty for the `cc:save` command. |
| refnum | string | Unique key for the transaction record. This will be empty for the `cc:save` command. |
| is_duplicate | char(Y/N) | If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. |
| result_code | char | Result code ('A' = Approved or 'E' = Error) |
| result | string | Long version of above result_code ('Approved', etc.) |
| authcode | integer | Authorization code. This will be empty for the `cc:save` command. |
| [creditcard][3] | [object][4] | Object which contains masked credit card information. |
| [savedcard][5] | [object][6] | Object which contains tokenized credit card information. |

# Object Definitions

## creditcard

This request object holds all of the information needed to process a credit card transaction. While only some of the parameters are required by default, additional fraud modules may further put restrictions on these parameters. Parameters in **bold** are required.

| Parameter Name | Type | Description |
| --- | --- | --- |
| **number** | number | Credit card number or token if missing or incorrect. Error codes 11-14 |
| **expiration** | string | Credit card expiration date. All numbers, and needs to be formatted as MMYY. Error codes 15-17 |
| cardholder | string | Name of the Cardholder |
| cvc | integer | Card verification code on back of card. Its format should be ### or #### |
| avs_street | string | Street address for address verification |
| avs_zip | integer | Postal (Zip) code for address verification |

## savedcard

This response object contains the tokenized credit card information.

| Parameter Name | Type | Description |
| --- | --- | --- |
| type | string | The card brand of the tokenized card. `Visa`, `MasterCard`, `AMEX`, etc. |
| key | string | Unique key for the card. This is the token. |
| number | string | Masked credit card information. |

## Change Log

| Date | Change |
| --- | --- |
| 2017-11-29 | Added page |

**[Click here for the full REST API change log.][7]**

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/#object-definitions
[2]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/#creditcard
[3]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/#object-definitions
[4]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/#creditcard
[5]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/#object-definitions
[6]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/#savedcard
[7]: https://help.usaepay.info/developer/rest-api/changelog/
