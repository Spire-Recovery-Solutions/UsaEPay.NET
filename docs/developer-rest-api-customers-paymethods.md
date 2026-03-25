Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Payment Methods - USAePay Help

# Customer Payment Methods

## Overview

This page will show you how to interact with Customer Payment methods.

It is not recommended to store credit card information on the client side. A payment method is a mechanism provided by USAepay to solve this issue. Once a payment method is created, the token created by this API can be passed into the transaction API via the `UMCardRef` parameter.

The API endpoint is as follows:

```
/api/v2/customers/
```

## Obtaining a Card Reference from a Payment Method

The preferred way of storing sensitive payment information is through a Card reference token which exposes no sensitive data. This card reference token can be obtained from a Payment Method.

### Request

```
curl -X GET
  https://sandbox.usaepay.com/api/v2/customers/<CUST_KEY>/payment_methods/<PM_KEY>/cardref
  -H 'authorization: Basic X3IxeUM2MEJCbkNFMzAxRm9oakw0c2gwRTB0NW51c3U6czIva2VOREF1cDRSQ2xtSk5XTi9iNWM2MmVhYmYwZDQyNGNmOTIyZTlmZWFlOWMxNmI1MzEzMTAzYWUwYzliMTI2MjVkNjk4MWNlYjA5NGI0ODY5'
  -H 'content-type: application/json'
```

> This cURL request is an example of a merchant trying to get a user-safe Payment Method key.

### Response

```
{
    "type": "token",
    "cardref": "jayu-e1rc-32in-m84z",
    "expires": "2019-09-30",
    "ccnumlast4": "2225",
    "cardtype": "03",
    "custkey": "csddgmfjd3rwkt8p",
    "pmkey": "dn02p95bn2f9bj7y8"
}
```

> This is the sample response object sent back from the server.

## Request Parameters

This is a complete overview of the request parameters that are sent. Note that each command type may only accept a subset of the parameters.

| Parameter Name | Type | Description |
| --- | --- | --- |
| **Cust_Key** | string | This is the key of the customer who owns the payment method. However, although a legacy feature, a customer ID can also be passed in. |
| **Pm_Key** | string | This is the key of the customer's payment method. However, although a legacy feature, a payment method ID can also be passed in. |

## Response Parameter

| Parameter | Type | Description |
| --- | --- | --- |
| type | string | This denotes the fact that this response object is a token. |
| cardref | string | This is the value of the token that allows references to the payment method. |
| expires | number | This is the Time To Live (TTL) of the Payment Method token. It is tied to the duration of the Payment Method itself. |
| ccnumlast4 | number | The last 4 digits of the Credit Card Number. |
| cardtype | string | Numerical code that represents the type of card. Card type codes can be found [here][1] |
| custkey | string | This is the key of the customer who owns the payment method. |
| pmkey | string | This is the key of the customer's payment method. |

## Change Log

| Date | Change |
| --- | --- |
| 2018-08-01 | Added page. |

**[Click here for the full REST API change log.][2]**

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/reference/cardtype/
[2]: https://help.usaepay.info/developer/rest-api/changelog/
