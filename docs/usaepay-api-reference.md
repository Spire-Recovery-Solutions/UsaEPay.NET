

---

Published Time: Mon, 16 Mar 2026 19:34:25 GMT

INTRODUCTION
Base URL
Authentication
Formatting
SDKS
PHP Guide
.Net Guide
Python Guide
TRANSACTION ENDPOINTS
Transactions
Bulk Transactions
Tokenization
Batches
PAYMENT ENGINE ENDPOINTS
Devices
Payment Request
CUSTOMER ENDPOINTS
Customers
Payment Methods
Recurring Schedules
Customer Transactions
PRODUCT ENDPOINTS
Products
Categories
INVENTORY ENDPOINTS
Inventory Locations
Inventories
WEBHOOK RESPONSES
Event Responses
REFERENCE
Change Log
Developer Portal
Introduction
Base URL

Request This cURL request is an example a credit card sale.

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "User-Agent: uelib v6.8"
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
        "cardholder": "John doe",
        "number": "4000100011112224",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Main",
        "avs_zip": "12345"
    },
    "invoice": "12356"
    }'


Response This is the sample response object sent back from the server.

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


The base url for the production api is:

https://usaepay.com/api/v2/

The base url for the sandbox api is:

https://sandbox.usaepay.com/api/v2/

The hostname usaepay.com can be swapped out for any of the other gateway hostnames (see the High Availability Guide). During development, sandbox.usaepay.com should be used. For more information on the development sandbox see the Sandbox Guide.

The "v2" refers to the api version and can replaced with an endpoint key. The api limits the number of api calls allowed per minute and per day based on this key. Using "v2" is fine for development and smaller merchant use cases, but could result in api rate limit errors for high traffic merchants. Larger merchants and developers should register their own software endpoint key in the Dev Portal. For more details, see the API Rate Limits section below.

Other features available with developer registered endpoints include the ability to restrict the use of the endpoint by IP address and list contact information for support. The endpoint keys are independent of the merchant api keys. An endpoint can be used with any merchant account.

Authentication

Parameters Only Example

var seed = random_value();  
var prehash = apikey + seed + apipin;
var apihash = 's2/'+ seed + '/' + sha256(prehash);
var authKey = base64Encode(apikey + ':' + apihash)


Javascript Example Implementation

var sha256 = require('sha256');

var seed = "abcdefghijklmnop"
var apikey = "_V87Qtb513Cd3vabM7RC0TbtJWeSo8p7"
var apipin = "123456"
var prehash = apikey + seed + apipin;
var apihash = 's2/'+ seed + '/' + sha256(prehash);
var authKey = new Buffer(apikey + ":" + apihash).toString('base64')
console.log("Authorization: Basic " + authKey);


PHP Example Implementation

All api calls requires an apikey (sourcekey) and an api hash. The api hash is built by hashing a random seed, the api key, and the private api pin. The general concept of generating the authorization header is shown here. Please note, this is not a real implementation.

The api key and api hash must be sent in a basic auth http header, and sent as a base64 encoded representation of the following string concatenation: (apiKey:apiHash). An HTTP header is then sent with the following structure:

Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2


Rate Limits

To prevent abuse, the api implements per minute and daily rate limits. These are counted per endpoint key (see above) and IP address. When using the default endpoint url, the limits are 45/minute and 5000/day. When the limit is exceeded, the api will return HTTP Error Code 429:

HTTP/1.1 429 API Rate Limit Exceed

The body of the response will be:

{"error":"API Rate Limit Exceed","errorcode":155}

Developers can utilize the "X-Rate-Limit" HTTP header to manage their api rate limit usage:

X-Rate-Limit: "8 of 45/min; 8 of 5000/day"

Higher limits are immediately available by registering an endpoint in the Dev Portal. Further increases to rate limits are granted on a case by case basis. Contact the integration support team for further information. To test the API Rate Limit feature, use the api endpoint:

https://sandbox.usaepay.com/api/RATELIMT/_

Your second request will be rate limited.

IP Access Restrictions

By default, api endpoints are available to clients on any IP address. Developers can edit their api endpoint to restrict calls from a set list of addresses. To test the IP access restriction, you can use the url:

https://sandbox.usaepay.com/api/IPACCESS/_

When access is blocked HTTP Error Code 401 will be returned:

HTTP/1.1 401 Access Denied

The body of the response will be:

{"error":"Access Denied","errorcode":156}

Formatting
Pagination
{
   "type": "list",
   "limit": 100,
   "offset": 0,
   "data": [
      {...},
      {...},
   ],
   "total": 200
 }


By default, GET operations, which return a list of requested items, return only the first 20 items. To get a different set of items, you can use the offset and limit parameters in the query string of the GET request. All api endpoints that return multi objects use a standard list format. Standard list format includes the following parameters:

Parameter	Description
limit	Number of objects returned per request
offset	The item number you would like the response to begin with
data	The list of objects within the limit and offset criteria response
total	Total number of data objects that match request

To change the number of objects returned in the result set, pass a "limit" variable in the request url. For example:

/api/customers?limit=1000

To retrieve the next group of objects, pass in "offset" with the item # to start with. "0" is the first item. For example, if there are 21 customers you could pull them in three calls:

	Request URL	Result
First Call	/api/customers?limit=10&offset=0	10 customers returned
Second Call	/api/customers?limit=10&offset=10	10 customers returned
Third Call	/api/customers?limit=10&offset=10	1 customer returned
Objects
{
  "key": "a8ai3k7i77tw",
  "type": "customer",
  "firstname": "John",
  "lastname": "Doe",
  ...
 }


Single objects will include a "key" which is the primary public key for the object and a "type" which is the type of object (i.e. customer, transaction, product, etc.).

SDKS
PHP Guide

This guide provides information on using the PHP SDK 2.0 for USAePay's Rest API. The Rest API provides an advanced interface to USAePay that allows merchants and resellers to access a wide range of functionality.

Requirements
php: >=7.1
ext-curl
ext-json
ext-mbstring
Installing PHP SDK 2.0
USING COMPOSER

Either run
composer require usaepay/usaepay-php
or include usaepay/usaepay-php in your composer.json file before running composer install.

MANUAL INSTALLATION

Download or clone repo https://gitlab.com/usaepay/php-sdk. Then download vendor.zip and place the vendor folder within the repo directory.

Using PHP SDK 2.0
INCLUDE

To load the sdk please use the follow line if your php script is running from the same director you installed the sdk in.

require_once(__DIR__ . '/vendor/autoload.php');

If you have installed the sdk in another directory, please modify the above path to the autoload.php files location.

PHP AUTHENTICATION

First generate a API key by logging into the merchant console, going to Settings, then going to API keys, and clicking Add API Key. Adding a PIN to your API Key is also recommend and required for most functionality. Once you have created your api key use the following method to setup the sdks authentication.

USAePay\API::setAuthentication('Enter_API_Key','Enter_API_Pin');

OPTIONAL API METHODS

Timeouts

You can set a customized timeout in seconds for api calls with the following method
USAePay\API::setTimeOut('30');

Endpoint Key

If you have created a developer account and want to specify a endpoint key please use the following(Note only the alphanumeric string at the end of base URL is needed)
USAePay\API::setEndpointKey('v2');

High Availability

If you reviewed our high availability documentation and are looking to implement active or pro-active failover, we have included a couple of methods to assist. Ping without a subdomain will provide a status for all subdomains, Ping with a subdomain will provide that specific subdomains status.

USAePay\API::ping();
USAePay\API::ping('www-01);

setSubdomain will allow you to specify which subdomain you wish to connect to. This is only recommended if you are implementing active failover since it can override the passive failover or our default subdomain.

USAePay\API::setSubdomain('www-02');

Webhook Authentication

To authenticate a message generated by our webhooks you will need to have the message body json, retrieve the signature from the message header, and get the signature key from the webhook settings in the merchant console. Once you have all three, you can send them to the verify function and it will return a boolean indicating if the message hmac signature authenticated.

USAePay\Middleware\Webhook::verify($json_message,$signature,$signature_key)

.Net Guide

This guide provides information on using the .Net SDK 2.0 for USAePay's Rest API. The Rest API provides an advanced interface to USAePay that allows merchants and resellers to access a wide range of functionality.

Requirements
Nuget Package manager
Installing .Net SDK 2.0
USING NUGET
Download from - https://www.nuget.org/packages/USAePAY.SDK/
Using .NET SDK 2.0
INCLUDE

To load the sdk please add the following line to your script.

using USAePay;

.NET AUTHENTICATION

First generate a API key by logging into the merchant console, going to Settings, then going to API keys, and clicking Add API Key. Adding a PIN to your API Key is also recommend and required for most functionality. Once you have created your api key use the following method to setup the sdks authentication.

USAePay.API.SetAuthentication("key", "pin");

WEBHOOK HMAC AUTHENTICATION

To authenticate a message generated by our webhooks you will need to have the message body json, retrieve the signature from the message header, and get the signature key from the webhook settings in the merchant console. Once you have all three, you can send them to the verify function and it will return a boolean indicating if the message hmac signature authenticated.
USAePay.Middleware.Webhook.Verify(string jsonPayload, string signature, string signatureKey);

Python Guide

This guide provides information on using the Python SDK for USAePay's Rest API. The Rest API provides an advanced interface to USAePay that allows merchants and resellers to access a wide range of functionality.

Requirements
Python version 3.6+ - Download from https://www.python.org/downloads/
Pip For installation instructions go to https://pip.pypa.io/en/stable/installation/
Installing Python SDK
USING PYPI
Run
pip install usaepay
DIRECT DOWNLOAD
Our SDK can also be directly downloaded from https://pypi.org/project/usaepay/
Using Python SDK
INCLUDE

To load the sdk please add the following line to your script.

import usaepay

PYTHON AUTHENTICATION

First generate a API key by logging into the merchant console, going to Settings, then going to API keys, and clicking Add API Key. Adding a PIN to your API Key is also recommend and required for most functionality. Once you have created your api key use the following method to setup the sdks authentication.

usaepay.api.set_authentication("key","pin")

WEBHOOK HMAC AUTHENTICATION

To authenticate a message generated by our webhooks you will need to have the message body json, retrieve the signature from the message header, and get the signature key from the webhook settings in the merchant console. Once you have all three, you can send them to the verify function and it will return a boolean indicating if the message hmac signature authenticated.

usaepay.middleware.webhook.verify(json_payload,signature,signature_key)

Transaction Endpoints
Transactions

Endpoint

POST /api/v2/transactions


Use the transactions endpoint to process credit card, debit card, and ACH transactions including:

				
Sale	Refund	Capture	Adjust	Void

Use the command parameter to specify which kind of transaction you would like to process.

Sale

Endpoint

POST /api/v2/transactions


This page will show you how to perform a sale transaction, whether it be via credit/debit card, a tokenized card, check, or cash. Specify what type of sale you are performing via the command parameter. While this allows for great flexibility on the user-facing API, this one endpoint may accept/reject certain parameters based on the type of sale being performed. Please refer to the typical use cases for specific implementation details.

Credit/Debit Card Sale

Processing a credit/debit card sale uses the sale command. An example of this transaction type is shown here.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
 '{
        "command": "sale",
      "invoice": "98454685",
      "ponum": "af416fsd5",
      "description": "Antique Pensieve",
      "comments": "Powerful magical object. Use with caution.",
      "email": "brian@hogwarts.com",
        "send_receipt": 1,
        "ignore_duplicate": 1,
        "merchemailaddr": "receipts@fandb.net",
      "amount": "500.00",
      "amount_detail": {
        "subtotal": "450.00",
        "tax": "45.00",
        "tip": "5.00",
        "discount": "50.00",
        "shipping": "50.00"
      },
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444333322221111",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Portkey Ave",
        "avs_zip": "12345"
      },
        "traits": {
        "is_debt": false,
        "is_bill_pay": false,
        "is_recurring": false,
        "is_healthcare": false,
        "is_cash_advance": false
      },
        "custkey": "ksddgpqgpbs5zkmb",
      "billing_address": {
        "firstname": "Albus",
        "lastname": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "postalcode": "10005",
        "country": "USA",
        "phone": "555-253-3673",
        "fax": "666-253-3673"
      },
      "shipping_address": {
        "firstname": "Aberforth",
        "lastname": "Dumbledore",
        "street": "987 HogsHead St",
        "city": "Hogsmead",
        "state": "WY",
        "postalcode": "30005",
        "country": "USA",
        "phone": "555-253-3673",
      },
      "lineitems": [
        {
          "product_key": "ds4bb5ckg059vdn8",
          "name": "Antique Pensieve",
          "cost": "450.00",
          "qty": "1",
          "tax_amount": "50.00",
          "location_key": "dnyyjc8s2vbz8hb33",
          "list_price": "500.00"
        }
      ],
        "custom_fields": {
            "1": "Gryffindor",
            "2": "Headmaster"
        }
 }'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to sale for a credit or debit card sale (Required)
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
creditcard	object	Object holding credit card information (Required)
save_card	bool	Set to true to tokenize the card used to process the transaction.
traits	object	This object holding transaction characteristics.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
save_customer	bool	Set to true to save the customer information to the customer database
save_customer_paymethod	bool	Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true in the transaction OR pass in the custkey to attach transaction to existing customer.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "dnfwxwhz5kvnbgb",
    "refnum": "100061",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "314407",
    "creditcard": {
            "number": "4444xxxxxxxx1111",
            "cardholder": "Hogwarts School of Witchcraft and Wizardry",
            "category_code": "A",
            "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "98454685",
    "avs": {
            "result_code": "YYY",
            "result": "Address: Match & 5 Digit Zip: Match"
    },
    "cvc": {
            "result_code": "N",
            "result": "No Match"
    },
    "batch": {
            "type": "batch",
            "key": "0t1jyndn769q1vb",
            "batchrefnum": 409384,
            "sequence": "1"
    },
    "auth_amount": "500.00",
    "trantype": "Credit Card Sale",
    "receipts": {
            "customer": "Mail Sent Successfully",
            "merchant": "Mail Sent Successfully"
    }
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
%customer%	object	Customer information when customer is saved to customer database.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
iccdata	string	ICC information fields. This will only be included for EMV transactions.
receipts	object	Receipt information.
Token Sale

Endpoint

POST /api/v2/transactions


You can also process a sale using a token in the place of a credit card number. For more information on how to tokenize credit/debit card numbers, click here. Processing a credit/debit card sale using a token also uses the sale command. An example of this transaction type is shown below.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    `{
        "creditcard": {
            "number": "dx0c3ns6xips28h3",
            "cvc": "123"
            },
        "command": "sale",
        "invoice": "101",
        "ponum": "af416fsd5",
        "description": "Woolen Socks",
        "comments": "Best socks in the world.",
        "amount": "17.99"
    }`

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:sale for a credit or debit card sale (Required)
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
creditcard	object	Object holding token information (Required)
traits	object	This object holding transaction characteristics.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "nnfkg4y0452kztm",
    "refnum": "100071",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "329045",
    "creditcard": {
            "number": "4444xxxxxxxx7779",
            "cardholder": "Hogwarts School of Witchcraft and Wizardry",
            "category_code": "A",
            "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "101",
    "avs": {
            "result_code": "YYY",
            "result": "Address: Match & 5 Digit Zip: Match"
    },
    "cvc": {
            "result_code": "N",
            "result": "No Match"
    },
    "batch": {
            "type": "batch",
            "key": "0t1jyndn769q1vb",
            "batchrefnum": 409384,
            "sequence": "1"
    },
    "auth_amount": "17.99",
    "trantype": "Credit Card Sale"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information for the token used.
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Payment Key Sale

Endpoint

POST /api/v2/transactions


If processing using the Client JS Library, you can also process a sale with a payment_key in place of a card number. The payment_key is a one time use token. To create a reusable token, set the save_card flag to true.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    `{
        "payment_key": "02zxfg87ty824",
        "command": "sale",
        "invoice": "101",
        "ponum": "af416fsd5",
        "description": "Woolen Socks",
        "comments": "Best socks in the world.",
        "amount": "17.99"
    }`

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:sale for a credit or debit card sale (Required)
payment_key	string	One time use token provided by Client JS Library (Required)
traits	object	This object holding transaction characteristics.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
save_card	bool	Set to true to tokenize the card used to process the transaction.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
save_customer	bool	Set to true to save the customer information to the customer database
save_customer_paymethod	bool	Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true to save this as a new customer OR pass in the custkey to attach transaction to existing customer.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "nnfkg4y0452kztm",
    "refnum": "100071",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "329045",
    "creditcard": {
            "number": "4444xxxxxxxx7779",
            "cardholder": "Hogwarts School of Witchcraft and Wizardry",
            "category_code": "A",
            "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "101",
    "avs": {
            "result_code": "YYY",
            "result": "Address: Match & 5 Digit Zip: Match"
    },
    "cvc": {
            "result_code": "N",
            "result": "No Match"
    },
    "batch": {
            "type": "batch",
            "key": "0t1jyndn769q1vb",
            "batchrefnum": 409384,
            "sequence": "1"
    },
    "auth_amount": "17.99",
    "trantype": "Credit Card Sale"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information for the token used.
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Check Sale

Endpoint

POST /api/v2/transactions


To process a sale through a checking or savings account, use the check:sale command. An example of this transaction type is shown here.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
        "command": "check:sale",
    "invoice": "101",
    "ponum": "af416fsd5",
    "description": "Wolfsbane Potion",
    "amount": "75.00",
    "check": {
            "accountholder": "Remus Lupin",
            "routing": "123456789",
            "account": "324523524",
            "account_type": "checking",
            "number": "101",
            "format": "WEB"
    }
    }'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to check:sale for an ACH transaction (Required)
invoice	string	Custom Invoice Number to easily retrieve sale details.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
check	object	Object which holds all check information (Required)
ponum	string	Customer's purchase order number.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
save_customer	bool	Set to true to save the customer information to the customer database
save_customer_paymethod	bool	Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true to save this as a new customer OR pass in the custkey to attach transaction to existing customer.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "1nf0crqxd88f683",
    "refnum": "100075",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "TMD0C7",
    "invoice": "101",
    "proc_refnum": "18120443975126",
    "auth_amount": "75.00"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
proc_refnum	string	Reference number returned from the check processor. This will not be returned by all ACH processors.
receipts	object	Receipt information.
Cash Sale

Endpoint

POST /api/v2/transactions


To log a cash sale, use the cash:sale command. An example of this transaction type is shown here.

Example Request

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

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cash:sale for cash transactions (Required)
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
save_customer	bool	Set to true to save the customer information to the customer database
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "3nft52wn1wy7rx8",
    "refnum": "100077",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "",
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authcode field will be empty for cash transactions.
receipts	object	Receipt information.
QuickSale

Endpoint

POST /api/v2/transactions


You can also process a sale using the payment information used for a previous sale using the quicksale command. This command works for Credit Card, Token, and Check transactions. You simply need to reference the previous transaction using the trankey or refnum of the previous transaction. This is meant for very simple transactions, so some fields/objects are not available for this command including:

lineitems
customfields
comments
shipping
duty
discount
currency
terminal
clerk
software

Some objects are copied from previous transactions and cannot be updated including:

billing_address
shipping_address

All fields that can be updated will be in the Request Parameters section below. An example of this transaction type using the trankey is shown below.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    `{
    "command": "quicksale",
    "trankey": "gnfkd12yhsr7mbs",
    "amount": "5.00"
    }`

Request Parameters

Below are the request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to quicksale for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key, for the transaction you are referencing with this QuickSale (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are referencing with this QuickSale (Required) if trankey field not included.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number (required for level 3 processing).
orderid	string	Merchant assigned order ID.
description	string	Public description of the transaction.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Credit Card or Token Response

{
    "type": "transaction",
    "key": "mnfwvcf1thnvrgb",
    "refnum": "100070",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "328906",
    "avs": {
        "result_code": "YYY",
        "result": "Address: Match & 5 Digit Zip: Match"
    },
    "creditcard": {
        "category_code": "A"
    },
    "cvc": {
        "result_code": "P",
        "result": "Not Processed"
    },
    "batch": {
        "type": "batch",
        "key": "0t1jyndn769q1vb",
                "batchrefnum": 409384,
        "sequence": "1"
    },
    "auth_amount": "5.00",
    "trantype": "Credit Card Sale"
}

Credit Card or Token Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
invoice	string	Custom Invoice Number to easily retrieve sale details. Copied from original transaction if no update was passed in the request.
avs	object	The Address Verification System (AVS) result. AVS information was copied from original transaction.
cvc	object	The Card Security Code (3-4 digit code) verification result. Security code is NOT copied from original transaction.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.

Example Check Response

{

  "type": "transaction",
  "key": "5nfty0s5m91vb3b",
  "refnum": "100079",
  "is_duplicate": "N",
  "result_code": "A",
    "result": "Approved",
    "authcode": "TM9406",
  "proc_refnum": "18120443978215",
  "trantype": "Check Sale"
}

Check Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
proc_refnum	string	Reference number returned from the check processor. This will not be returned by all ACH processors.
receipts	object	Receipt information.
AuthOnly

Processing a credit/debit card authorization uses the authonly command. An example of this transaction type is shown here. It will run an authorization check on a customer's credit card or checking account without actually charging the customer's account. AuthOnly commands can also be run with tokens or payment keys.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
 '{
        "command": "authonly",
      "invoice": "98454685",
      "ponum": "af416fsd5",
      "description": "Antique Pensieve",
      "comments": "Powerful magical object. Use with caution.",
      "email": "brian@hogwarts.com",
        "send_receipt": 1,
        "ignore_duplicate": 1,
        "merchemailaddr": "receipts@fandb.net",
      "amount": "500.00",
      "amount_detail": {
        "subtotal": "450.00",
        "tax": "45.00",
        "tip": "5.00",
        "discount": "50.00",
        "shipping": "50.00"
      },
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444333322221111",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Portkey Ave",
        "avs_zip": "12345"
      },
        "traits": {
        "is_debt": false,
        "is_bill_pay": false,
        "is_recurring": false,
        "is_healthcare": false,
        "is_cash_advance": false
      },
        "custkey": "ksddgpqgpbs5zkmb",
      "billing_address": {
        "firstname": "Albus",
        "lastname": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "postalcode": "10005",
        "country": "USA",
        "phone": "555-253-3673",
        "fax": "666-253-3673"
      },
      "shipping_address": {
        "firstname": "Aberforth",
        "lastname": "Dumbledore",
        "street": "987 HogsHead St",
        "city": "Hogsmead",
        "state": "WY",
        "postalcode": "30005",
        "country": "USA",
        "phone": "555-253-3673",
      },
      "lineitems": [
        {
          "product_key": "ds4bb5ckg059vdn8",
          "name": "Antique Pensieve",
          "cost": "450.00",
          "qty": "1",
          "tax_amount": "50.00",
          "location_key": "dnyyjc8s2vbz8hb33",
          "list_price": "500.00"
        }
      ],
        "custom_fields": {
            "1": "Gryffindor",
            "2": "Headmaster"
        }
 }'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to authonly for this transaction type. (Required)
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
creditcard	object	Object holding credit card information (Required)
save_card	bool	Set to true to tokenize the card used to process the transaction.
traits	object	This object holds transaction characteristics.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
save_customer	bool	Set to true to save the customer information to the customer database
save_customer_paymethod	bool	Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true to save this as a new customer OR pass in the custkey to attach transaction to existing customer.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "dnfwxwhz5kvnbgb",
    "refnum": "100061",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "314407",
    "creditcard": {
            "number": "4444xxxxxxxx1111",
            "cardholder": "Hogwarts School of Witchcraft and Wizardry",
            "category_code": "A",
            "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "98454685",
    "avs": {
            "result_code": "YYY",
            "result": "Address: Match & 5 Digit Zip: Match"
    },
    "cvc": {
            "result_code": "N",
            "result": "No Match"
    },
    "batch": {
            "type": "batch",
            "key": "0t1jyndn769q1vb",
            "batchrefnum": 409384,
            "sequence": "1"
    },
    "auth_amount": "500.00",
    "trantype": "Credit Card Sale",
    "receipts": {
            "customer": "Mail Sent Successfully",
            "merchant": "Mail Sent Successfully"
    }
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
iccdata	string	ICC information fields. This will only be included for EMV transactions.
receipts	object	Receipt information.
Refund

Endpoint

POST /api/v2/transactions


Specify what type of refund you are performing via the command parameter. A refund should be used once the transaction you are refunding has settled. If you are trying to cancel a transaction that is still in the currently open batch, you should use the void command instead.

Credit/Debit Open Refund

Endpoint

POST /api/v2/transactions


The cc:credit command is a command to specifically refund credit cards. You can include the card information in the request one of two ways:

1. Refer to a specific transaction (using the trankey or refnum) to refer to the card information in that transaction. This is similar to the example given in Connected Refund.
2. Include card or token information in the request. When using this method, you must pass the amount in the request.

The example below is using the second method. To see an example of the transaction using the first method, refer to Connected Refund.

Depending on the Credit Policy, a refund can be processed for larger than the original transaction amount.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
        "command": "cc:credit",
        "amount": "100.00",
        "invoice": "3579864532",
         "creditcard": {
        "cardholder": "Remus Lupin",
        "number": "4444333322221111",
        "expiration": "0919"
        }
    }'


Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to refund for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key for the transaction you are refunding.
refnum	string	Gateway generated transaction reference number for the transaction you are refunding.
creditcard	object	Object holding credit card information. (Required) if refnum or trankey is not sent.
amount	double	Amount to refund. (Required) if refnum or trankey is not sent.
amount_detail	object	Object containing a more detailed breakdown of the amount.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "lnftkz3fqjc2682",
    "refnum": "100095",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "110118",
    "creditcard": {
        "number": "4444xxxxxxxx1111",
        "cardholder": "Remus Lupin",
        "category_code": "A",
        "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "3579864532",
    "avs": {
        "result_code": "   ",
        "result": "Unmapped AVS response (   )"
    },
    "batch": {
        "type": "batch",
        "key": "0t1k3yx5xs37cvb",
        "batchrefnum": 409384,
        "sequence": "1"
    },
    "auth_amount": "100.00",
    "trantype": "Credit Card Refund (Credit)"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
avs	object	The Address Verification System (AVS) result.
batch	object	Batch information.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Check Open Refund

Endpoint

POST /api/v2/transactions


The check:credit command is a command to send funds to a checking or savings account. This uses ACH to send funds to a customer, employee, vendor, etc. It can be used to refund a sale or make a payment to someone (i.e. payroll).

This is a stand alone transaction and does not pull the account and routing number from a previous sale. You must provide account information to use this.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
    '{
    "command": "check:credit",
    "check": {
        "accountholder": "Remus Lupin",
        "account": "324523524",
        "routing": "123456789",
                "account_type": "checking",
                "number": "101",
                "format": "WEB"
    },
    "amount" : "5.00"
    }'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to check:credit for this transaction type. (Required)
check	object	Object which holds all check information (Required)
amount	double	This is an optional quantity, which will allow for the partial refund of a transaction. Note: The request will fail if the refund amount exceeds the transaction amount.
amount	double	This is an optional quantity, which will allow for the partial refund of a transaction. Note: The request will fail if the refund amount exceeds the transaction amount.
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{  
   "type":"transaction",
   "key":"6nfj34qh3n8qbhw",
   "refnum":"21726542",
   "is_duplicate":"N",
   "result_code":"A",
   "result":"Approved",
   "authcode":"TM5CA5",
   "proc_refnum":"17092106017626",
   "auth_amount" : "5.00"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
proc_refnum	string	Reference number returned from the check processor. This will not be returned by all ACH processors.
receipts	object	Receipt information.
Cash Open Refund

Endpoint

POST /api/v2/transactions


Performs a cash refund. Requires an amount field. The trankey and refnum fields are not supported.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
    "command": "cash:refund",
    "amount" : "5.00"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cash:refund for a cash refund (Required)
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount	double	This is an optional quantity, which will allow for the partial refund of a transaction. Note: The request will fail if the refund amount exceeds the transaction amount.
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.
{
    "type": "transaction",
    "key": "onfwht4cdb21r58",
    "refnum": "100098",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "",
    "invoice": "98454685",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code. This is always blank for cash refunds.
invoice	string	Custom Invoice Number to easily retrieve sale details.
Connected Refund

To perform a connected refund you will use the refund command. This command can be used with credit/debit card transactions, token transactions, or check transactions. By default, this command will perform a full refund on the transaction you reference (using the trankey or refnum) in the request. Partial refunds are possible by including the optional amount parameter field.

Please Note: This command is a refund connected to one particular transaction. When you are using this command, you will not be able to refund more than the original amount regardless of your Credit Policy Settings.

This is NOT a valid command for cash. To do a cash refund use the cash:refund command.

Example Credit Card, Token, or Check Request for Connected Refunds

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
        "command": "refund",
        "trankey": "hnftkyjj2r93ft2"
    }'

Credit Card, Token, and Check Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to refund for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key, for the transaction you are refunding. (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are refunding. (Required) if trankey field not included.
amount	double	This is an optional quantity, which will allow for the partial refund of a transaction. Note: The request will fail if the refund amount exceeds the transaction amount.
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Credit Card and Token Connected Refund Response

{
  "type": "transaction",
  "key": "2nf57pywqh09zjs",
  "refnum": "124445986",
  "is_duplicate": "N",
  "result_code": "A",
  "result": "Approved",
  "authcode": "445986",
  "avs": {
    "result_code": "   ",
    "result": "Unmapped AVS response (   )"
  },
  "batch": {
    "type": "batch",
    "key": "0t1k3yx5xs37cvb",
    "batchrefnum": 409384,
    "sequence": "1"
  }
}

Credit Card or Token Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
avs	object	The Address Verification System (AVS) result.
batch	object	Batch information.

Example Check Connected Refund Response

{
    "type": "transaction",
    "key": "anf0cybmfqt9c63",
    "refnum": "100084",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "TM3670"
}

Check Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
receipts	object	Receipt information.
Quick Refund

Endpoint

POST /api/v2/transactions


You can also process a refund using the payment information used for a previous sale using the quickrefund command. This command works for Credit Card, Token, and Check transactions. You simply need to reference the previous transaction using the trankey or refnum of the previous transaction and the amount you would like to refund.

Please Note: This command requires you to pass in an amount.

This is meant for very simple transactions, so some fields/objects are not available for this command including:

lineitems
customfields
comments
shipping
duty
discount
currency
terminal
clerk
software

Some objects are copied from previous transactions and cannot be updated including:

billing_address
shipping_address

All fields that can be updated will be in the Request Parameters section below. An example of this transaction type using the trankey is shown below.

This is NOT a valid command for cash. To do a cash refund use the cash:refund command.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
    "command": "quickrefund",
    "trankey": "dnfwxwhz5kvnbgb",
    "amount": "5.00"
}'


The quick refund command requires that you specify an amount to refund.

Request Parameters

Below are the request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to quickrefund for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key, for the transaction you are referencing with this QuickRefund (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are referencing with this QuickRefund (Required) if trankey field not included.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number (required for level 3 processing).
orderid	string	Merchant assigned order ID.
description	string	Public description of the transaction.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Credit Card or Token Response

{
    "type": "transaction",
    "key": "pnftkk568yskw30",
    "refnum": "100099",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "110122",
    "creditcard": {
        "number": "4444xxxxxxxx1111"
    },
    "avs": {
        "result_code": "   ",
        "result": "Unmapped AVS response (   )"
    },
    "batch": {
        "type": "batch",
        "key": "pt1qxpnx0f5303d",
        "batchrefnum": 409384,
        "sequence": "1"
    },
    "trantype": "Credit Card Refund (Credit)",
    "auth_amount": "5.00",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Credit Card or Token Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
avs	object	The Address Verification System (AVS) result.
batch	object	Batch information.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.

Example Check Response

{
    "type": "transaction",
    "key": "0nftkkdy9v0kgbb",
    "refnum": "100100",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "TMF291",
    "proc_refnum": "18120544051570",
    "trantype": "Reverse ACH",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Check Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
proc_refnum	string	Reference number returned from the check processor. This will not be returned by all ACH processors.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Credit Void

Endpoint

POST /api/v2/transactions


This allows you to void or credit back a credit/debit card, token, or check transaction. The command automatically checks the status of the transaction, if the transaction has been settled then a credit (for all or part of the initial transaction) is entered into the current batch. If the transaction has not been settled then it will be voided. The amount field is optional, it must be equal to or less than the original transaction. If no amount is specified, the full amount will be refunded.

This is NOT a valid command for cash. To do a cash refund use the cash:refund command.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
    "command": "creditvoid",
    "trankey": "dnfwxwhz5kvnbgb",
    }'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to creditvoid for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key, for the transaction you are refunding. (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are refunding. (Required) if trankey field not included.
amount	double	This is an optional quantity, which will allow for the partial reversal of a transaction. Note: The request will fail if the refund amount exceeds the transaction amount.

Example Credit Card or Token Response

{
    "type": "transaction",
    "key": "pnftkk568yskw30",
    "refnum": "100099",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "110122",
    "creditcard": {
        "number": "4444xxxxxxxx1111"
    },
    "avs": {
        "result_code": "   ",
        "result": "Unmapped AVS response (   )"
    },
    "batch": {
        "type": "batch",
        "key": "pt1qxpnx0f5303d",
        "batchrefnum": 409384,
        "sequence": "1"
    },
    "trantype": "Credit Card Refund (Credit)",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Credit Card or Token Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique key for the transaction record. This will match trankey in the request if the transaction was voided and it will be different than the request trankey if the transaction was refunded.
refnum	string	Transaction reference number. This will match refnum in the request if the transaction was voided and it will be different than the request refnum if the transaction was refunded.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
avs	object	The Address Verification System (AVS) result.
batch	object	Batch information.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.

Example Check Response

{
    "type": "transaction",
    "key": "0nftkkdy9v0kgbb",
    "refnum": "100100",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "TMF291",
    "proc_refnum": "18120544051570",
    "trantype": "Reverse ACH",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Check Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique key for the transaction record. This will match trankey in the request if the transaction was voided and it will be different than the request trankey if the transaction was refunded.
refnum	string	Transaction reference number. This will match refnum in the request if the transaction was voided and it will be different than the request refnum if the transaction was refunded.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
proc_refnum	string	Reference number returned from the check processor. This will not be returned by all ACH processors.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Capture/Adjust

Endpoint

POST /api/v2/transactions


The capture command moves an authorized transaction into the current batch for settlement. It is possible to capture an amount other than the one originally authorized, however, you must follow the guidelines established by the merchant service bank. Capturing a higher or lower dollar amount could result in additional penalties and fees.

Most banks typically allow no more than 10 days to pass between the authorization/capture and settlement of a transaction.

The adjust command allows you to make changes to an existing (unsettled) sale. The authorization amount can be increased (incremental authorization) or decreased (partial reversal) or not changed. Additional data elements such as tax amount and po number can be added. The tolerances for the settle amount vary depending on the type of merchant account and the merchant service provider. The adjust and capture commands function identically except that the adjust command does not place the transaction in the batch.

Please Note: These commands are only valid for credit card transactions.

Capture

When capturing a transaction you can use one of the following commands:

cc:capture- This command captures transactions that have not yet expired and errs expired transactions according to your settings in API Keys.
cc:capture:error- This command captures transactions that have not yet expired and if the authorization has expired, the command will throw an error.
cc:capture:reauth- This command captures transactions that have not yet expired and if the authorization has expired, it will attempt to reauthorize the same card.
cc:capture:override- This command captures transactions that have not yet expired and if the authorization has expired it will force a capture, even if the authorization has exceeded the maximum 30 days. We would not recommend capturing transactions over 60 days due to the charge back risk.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "cc:capture",
    "trankey": "4nftw5kqd7bjy82"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:capture, cc:capture:error, cc:capture:reauth, or cc:capture:override for this transaction type. (Required) See above for further information about these commands.
trankey	string	Unique gateway generated transaction key, for the transaction you are attempting to capture. (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are attempting to capture. (Required) if trankey field not included.
amount	double	This is an optional quantity, which can alter the transaction amount.
amount_detail	object	Object containing a more detailed breakdown of the amount.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number (required for level 3 processing).
orderid	string	Merchant assigned order ID.
description	string	Public description of the transaction.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.

Example Response

{
    "type": "transaction",
    "key": "4nftw5kqd7bjy82",
    "refnum": "100104",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "326349",
    "invoice": "98454685",
    "batch": {
        "type": "batch",
        "key": "pt1qxpnx0f5303d",
                "batchrefnum": 409384,
        "sequence": "1"
    },
    "auth_amount": "500.00"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	The type of sale performed. Successful sales return a value of 'transaction'.
key	string	Unique key for the transaction record. This will match trankey in the request unless the transaction was reauthorized.
refnum	string	Transaction reference number. This will match refnum in the request unless the transaction was reauthorized.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate.
result_code	string	Result code ('A' = Approved or 'E' = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
batch	object	Batch information.
auth_amount	double	The amount authorized
Adjust

Endpoint

POST /api/v2/transactions


The adjust command allows you to make changes to an existing (unsettled) sale. The authorization amount can be increased (incremental authorization) or decreased (partial reversal) or not changed. Additional data elements such as tax amount and PO number can be added. The tolerances for the settle amount vary depending on the type of merchant account and the merchant service provider. The adjust and capture commands function identically except that the adjust command does not place the transaction in the batch.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "cc:adjust",
    "trankey": "4nftw5kqd7bjy82"
    "amount" : "3.00"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:adjust for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key, for the transaction you are attempting to capture. (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are attempting to capture. (Required) if trankey field not included.
amount	double	This is an optional quantity, which can alter the transaction amount.
amount_detail	object	Object containing a more detailed breakdown of the amount.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number (required for level 3 processing).
orderid	string	Merchant assigned order ID.
description	string	Public description of the transaction.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.

Example Response

{
   "type": "transaction",
   "key": "2nfmnj4fxj13b3q",
   "refnum": "21726798",
   "is_duplicate": "N",
   "result_code": "A",
   "result": "Approved",
   "authcode": "002048",
   "batch": {
      "type": "batch",
      "key": "0t1jyndn769q1vb",
            "batchrefnum": 409384,
      "sequence": "1"
   },
   "auth_amount": "3.00"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	The type of sale performed. Successful sales return a value of 'transaction'.
key	string	Unique key for the transaction record. This will match trankey in the request unless the transaction was reauthorized.
refnum	string	Transaction reference number. This will match refnum in the request unless the transaction was reauthorized.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate.
result_code	string	Result code ('A' = Approved or 'E' = Error
result	string	Description of above result_code ('Approved', etc.)
authcode	string	Authorization code.
batch	object	Batch information.
auth_amount	double	The amount authorized.
Refund Adjust

Endpoint

POST /api/v2/transactions


Interchangable with the command cc:refund:adjust. If the authorization has not settled yet, it will adjust the authorization amount down by the inputted amount (releasing funds back to the card holder in realtime). If the authorization has settled, a credit transaction will be submitted.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
    "command": "refund:adjust",
    "refnum": "21726766",
    "amount" : "3.00"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:refund:adjust for this transaction type. (Required)
refnum	string	The transaction id or transaction key of the sale you wish to refund (Required)
amount	double	The amount of the transaction you would like to refund. (Required)
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number (required for level 3 processing).
orderid	string	Merchant assigned order ID.
description	string	Public description of the transaction.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.

Example Response

{
   "type": "transaction",
   "key": "mnfky8s1mz0gmbd",
   "refnum": "21726766",
   "is_duplicate": "N",
   "result_code": "A",
   "result": "Approved",
   "authcode": "002035",
   "batch": {
      "type": "batch",
      "key": "0t1jyndn769q1vb",
            "batchrefnum": 409384,
      "sequence": "1"
   },
   "auth_amount": "97.00"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	The type of sale performed. Successful sales return a value of 'transaction'.
key	string	Unique key for the transaction record. This will match trankey in the request unless the transaction was reauthorized.
refnum	string	Transaction reference number. This will match refnum in the request unless the transaction was reauthorized.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate.
result_code	string	Result code ('A' = Approved or 'E' = Error
result	string	Description of above result_code ('Approved', etc.)
authcode	string	Authorization code.
batch	object	Batch information.
auth_amount	double	The authorized amount after the partial reversal.
Post

Endpoint

POST /api/v2/transactions


A post authorization transaction is most often used when a merchant attempts to process a transaction and receives an error code indicating the transaction requires voice authorization. When processing a post authorization, the merchant must contact the customer's issuing bank for an authorization code. Once you have received the authorization code, pass it in through the API to add the transaction to the batch.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZDNkOTQwMDM1MzA0ZDZmMmE1MGUxNWQ2ZmNmYWEyYjMvMGNkMDVjMGEwMzRiNDVlYzlkNDM0MTRmZGFlOWIxMGRkNGI5YTM1ZWM2ODBiYWVjMTNiMjY3ODI2N2IyYzU5OA=="
  -H "Content-Type: application/json"
  -d'{
  "creditcard": {
    "number": "4000100611112228",
    "expiration": "0919",
    "cvc": "123",
    "avs_street": "1234 Portkey Ave",
    "avs_zip": "12345"
  },
  "command": "cc:postauth",
  "authcode": "684538",
  "invoice": "101",
  "ponum": "af416fsd5",
  "description": "Woolen Socks",
  "comments": "Best socks in the world.",
  "send_receipt": 1,
  "amount": "17.99"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:postauth for a postauth transaction. (Required)
authcode	string	Authorization code received from the processor. Required
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
creditcard	object	Object holding credit card information (Required)
save_card	bool	Set to true to tokenize the card used to process the transaction.
traits	object	This object holding transaction characteristics.
custkey	string	Customer key for a previously saved customer. Unique gateway generated key.
save_customer	bool	Set to true to save the customer information to the customer database
save_customer_paymethod	bool	Set to true to save the customer payment method to customer profile. You must either have the save_customer flag set to true in the transaction OR pass in the custkey to attach transaction to existing customer.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.

Example Response

{
    "type": "transaction",
    "key": "inftypr7p2fwxr2",
    "refnum": "100170",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "684538",
    "creditcard": {
        "number": "4000xxxxxxxx2228",
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "category_code": "A",
        "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "101",
    "batch": {
        "type": "batch",
        "key": "ft1m9m5p9wgd9mb",
        "sequence": "2902"
    },
    "auth_amount": "17.99"
}


Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
iccdata	string	ICC information fields. This will only be included for EMV transactions.
receipts	object	Receipt information.
Void

Endpoint

POST /api/v2/transactions


Once a transaction has been voided, it will not show up on the customer's credit card statement. Customers who have online banking that allows them to see "Pending" transactions may see the voided transaction for a few days before it disappears.

You can only void a transaction that hasn't been settled yet. A transaction is settled when the batch that it is in has been closed. If the transaction has been settled, you must run a credit instead using a refund transaction. If you run a credit, both the credit and the initial charge will show up on the customer's credit card statement.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "void",
    "trankey": "bnfkqy54w4qwtmq"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to void for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key for the transaction you are voiding. (Required) if or trankey is not sent.
refnum	string	Gateway generated transaction reference number for the transaction you are voiding. (Required) if trankey is not sent.

Example Response

{  
   "type":"transaction",
   "key":"bnfkqy54w4qwtmq",
   "refnum":"21725793",
   "is_duplicate":"N",
   "result_code":"A",
   "result":"Approved",
   "authcode":"000973"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
Release Funds

Endpoint

POST /api/v2/transactions


This command will void a credit card transaction and release the funds immediately.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
    "command": "cc:void:release",
    "trankey" : "dnft1yqgm9rh6b0"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to cc:void:release for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key for the transaction you are voiding. (Required) if or trankey is not sent.
refnum	string	Gateway generated transaction reference number for the transaction you are voiding. (Required) if trankey is not sent.

Example Response

{  
   "type":"transaction",
   "key":"dnft1yqgm9rh6b0",
   "refnum":"21726107",
   "is_duplicate":"N",
   "result_code":"A",
   "result":"Approved",
   "authcode":"001788"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
Unvoid

Endpoint

POST /api/v2/transactions


This command unvoids a previously voided transaction. This only unvoids transactions in a currently open batch and cannot unvoid transactions if the funds have been released.

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    '{
    "command": "unvoid",
    "trankey": "bnfkqy54w4qwtmq"
}'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type		Description
command	string	This must be set to unvoid for this transaction type. (Required)	
refnum	number	The transaction id or transaction key of the void you wish to reverse (Required)	

Example Response

{
   "type":"transaction",
   "key":"bnfkqy54w4qwtmq",
   "refnum":"21725793",
   "is_duplicate":"N",
   "result_code":"A",
   "result":"Approved",
   "authcode":"000973"
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
Credit Void

Endpoint

POST /api/v2/transactions


This allows you to void or credit back a credit/debit card, token, or check transaction. The command automatically checks the status of the transaction, if the transaction has been settled then a credit (for all or part of the initial transaction) is entered into the current batch. If the transaction has not been settled then it will be voided. The amount field is optional, it must be equal to or less than the original transaction. If no amount is specified, the full amount will be refunded.

This is NOT a valid command for cash. To do a cash refund use the cash:refund command.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
  -d
  '{
    "command": "creditvoid",
    "trankey": "dnfwxwhz5kvnbgb",
    }'

Request Parameters

Below are request parameter fields. Parameters marked Required are required to process this type of transaction. Some listed parameters may not be shown in the example, but are still valid for this transaction type.

Parameter Name	Type	Description
command	string	This must be set to creditvoid for this transaction type. (Required)
trankey	string	Unique gateway generated transaction key, for the transaction you are refunding. (Required) if refnum field not included. This is the preferred transaction identifier.
refnum	string	Gateway generated transaction reference number, for the transaction you are refunding. (Required) if trankey field not included.
amount	double	This is an optional quantity, which will allow for the partial reversal of a transaction. Note: The request will fail if the refund amount exceeds the transaction amount.

Example Credit Card or Token Response

{
    "type": "transaction",
    "key": "pnftkk568yskw30",
    "refnum": "100099",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "110122",
    "creditcard": {
        "number": "4444xxxxxxxx1111",
                "cardholder": "John Doe"
    },
    "avs": {
        "result_code": "   ",
        "result": "Unmapped AVS response (   )"
    },
    "batch": {
        "type": "batch",
        "key": "0t1jyndn769q1vb",
                "batchrefnum": 409384,
        "sequence": "1"
    },
    "trantype": "Credit Card Refund (Credit)",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Credit Card or Token Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique key for the transaction record. This will match trankey in the request if the transaction was voided and it will be different than the request trankey if the transaction was refunded.
refnum	string	Transaction reference number. This will match refnum in the request if the transaction was voided and it will be different than the request refnum if the transaction was refunded.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
avs	object	The Address Verification System (AVS) result.
batch	object	Batch information.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.

Example Check Response

{
    "type": "transaction",
    "key": "0nftkkdy9v0kgbb",
    "refnum": "100100",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "TMF291",
    "proc_refnum": "18120544051570",
    "trantype": "Reverse ACH",
    "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
    }
}

Check Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique key for the transaction record. This will match trankey in the request if the transaction was voided and it will be different than the request trankey if the transaction was refunded.
refnum	string	Transaction reference number. This will match refnum in the request if the transaction was voided and it will be different than the request refnum if the transaction was refunded.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
invoice	string	Custom Invoice Number to easily retrieve sale details.
proc_refnum	string	Reference number returned from the check processor. This will not be returned by all ACH processors.
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Retrieve Transaction Detail

Endpoint

GET /api/v2/transactions/:trankey:


Retrieves the details of a previously processed transaction. You will need the unique identifier key that returned after the original transaction request.

Example Request

curl -X GET https://secure.usaepay.com/api/v2/transactions/hnftkyjj2r93ft2 \
-H "Authorization: Basic dENvbk81VWQ5ekRjTGhzajBEcDNsZTduUVhrOUhmSk86czIvZmRiYjY1ZjU5MGM5N2E1YzYzZjY5Y2NkYWQzNDJiNTAvZTZiMDk3NDY2YmU5MTMwNzgwZmUzMDk4MGUzMGYwZGRjMGE4Mjk1MTFmOWQ0OWU0MDg0ZGYyYmNlMGMwMzU0ZA==" \

Request Parameters

Parameters marked Required are required to process the request.

Parameter Name	Type	Description
transaction_key	string	Unique gateway generated key. (Required)
return_origin	bool	Set as true to include origin detail in the response
return_bin	bool	Set as true to include card bin detail in the response
return_fraud	bool	Set as true to include fraud detail in the response
Credit/Debit Card Response

Example Credit/Debit Card Response

{
    "type": "transaction",
    "key": "hnftkyjj2r93ft2",
    "refnum": "100065",
    "created": "2018-12-04 10:48:44",
    "trantype_code": "S",
    "trantype": "Credit Card Sale",
    "result_code": "A",
    "result": "Approved",
    "authcode": "326075",
    "status_code": "P",
    "status": "Authorized (Pending Settlement)",
    "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444xxxxxxxx1111",
        "avs_street": "1234 Portkey Ave",
        "avs_postalcode": "12345",
        "category_code": "A",
        "type": "V",
        "aid": "A000000025010801",
        "entry_mode": "Contactless"
    },
    "avs": {
        "result_code": "YYY",
        "result": "Address: Match & 5 Digit Zip: Match"
    },
    "cvc": {
        "result_code": "N",
        "result": "No Match"
    },
    "batch": {
        "type": "batch",
        "key": "et1m9h57b44h16g",
        "batchrefnum": "408318",
        "sequence": "2515"
    },
    "bin": {
        "bin": "444433",
        "number": "4444xxxxxxxx1111",
        "type": "CREDIT",
        "issuer": "VISA",
        "bank": "",
        "country": "US",
        "country_name": "UNITED STATES",
        "country_iso": "840",
        "location": "",
        "phone": "",
        "category": "",
        "data_source": "binbase"
    },
    "fraud": {
        "card_blocked": "none"
    },
    "amount": "157.50",
    "amount_detail": {
        "tip": "0.00",
        "tax": "11.00",
        "shipping": "20.00",
        "duty": "10.00",
        "discount": "103.50",
        "subtotal": "230.00"
    },
    "ponum": "af416fsd5",
    "invoice": "98454685",
    "orderid": "8521adsf5312",
    "description": "Received First Year Discount",
    "comments": "Paid for by Headmaster",
    "terminal": "multilane",
    "clerk": "Madam Malkin",
    "drawer_shift": {
        "key": "3nmz17h2f4489t7y8",
        "clerk_key": "4nmmfrpn7szx8kzt8"
    },
    "cust_key": "lsddpx5f6ynkwf1s",
    "custid": "10245373",
    "customerid": "8645384653",
    "customer_email": "test@test.com",
    "merchant_email": "test@testmerchant.com",
    "clientip": "111.11.11.111",
    "source_name": "REST API INTEGRATION",
    "billing_address": {
        "company": "Hogwarts School of Witchcraft and Wizardry",
        "first_name": "Albus",
        "last_name": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "country": "USA",
        "postalcode": "10005",
        "phone": "5552533673"
    },
    "shipping_address": {
        "company": "Hogwarts School of Witchcraft and Wizardry",
        "first_name": "Albus",
        "last_name": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "country": "USA",
        "postalcode": "10005",
        "phone": "555-253-3673"
    },
    "lineitems": [
        {
            "name": "The Standard Book of Spells (Grade 1)",
            "description": "by Miranda Goshawk An indispensable guide to your basic magical needs.",
            "cost": "10.00",
            "taxable": "N",
            "qty": "1.0000",
            "tax_rate": "5.000",
            "commodity_code": "715-86",
            "product_key": "4s4b4qrn3k4pmsg8",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "name": "A History of Magic by Bathilda Bagshot",
            "description": "Bathilda Bagshot embarked on the journey of magical knowledge decades ago. She has always been fascinated by the mysteries and curiosities of the wizarding world. A History of Magic examines significant moments and facts from the beginning of time to the ",
            "cost": "20.00",
            "taxable": "Y",
            "qty": "1.0000",
            "tax_amount": "1.00",
            "tax_rate": "5.000",
            "commodity_code": "715-86",
            "product_key": "as4b4s9sfbwzd1v8",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "sku": "378675689",
            "name": "Dress Robes",
            "description": "Very pretty\/handsome",
            "cost": "100.00",
            "taxable": "Y",
            "qty": "2.0000",
            "tax_amount": "10.00",
            "tax_rate": "5.000",
            "commodity_code": "S2502",
            "product_key": "bs4b03tc5m4ccs82",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        }
    ],
    "custom_fields": {
        "1": "Gryffindor",
        "2": "Headmaster"
    },
    "origin": {
        "source_name": "REST TEST",
        "source_key": "9n4xhp4j80yd4jw80",
        "api_type": "Rest API",
        "clientip": "209.37.25.121",
        "clientip_country": "",
        "serverip": "104.35.239.146",
        "serverip_country": "",
        "software": "Floo Network",
        "user": "(auto)"
    },
    "platform": {
        "name": "Test Bed"
    },
    "available_actions": [
        "void",
        "queue",
        "quicksale",
        "quickrefund",
        "addtip"
    ]
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
trantype_code	string	Transaction type code. See possible values here
trantype	string	Transaction Type code description. See possible values here
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
status_code	string	Transaction status code see possible values here
status	string	Transaction status code description see possible values here
creditcard	object	Object holding credit card information
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
bin	object	Credit card bin detail. This will only be included if the return_bin flag is passed through.
fraud	object	Credit card fraud detail. This will only be included if the return_fraud flag is passed through.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount.
ponum	string	Customer's purchase order number (required for level 3 processing)
invoice	string	Custom Invoice Number to easily retrieve sale details.
orderid	string	Merchant assigned order ID
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
terminal	string	Transaction terminal description.
clerk	string	Clerk name.
drawer_shift	object	Object with shift drawer information.
cust_key	string	Gateway generated unique identifier for customer associated with the customer database. This is the preferred identifier for the REST API.
custid	string	Gateway generated unique identifier for customer associated with the customer database. This is the identifier most closely associated with the SOAP API.
customerid	string	Merchant generated identifier for customer record.
customer_email	string	Email customer reciept was sent to.
merchant_email	string	Email merchant receipt was sent to.
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
source_name	string	API key name of key
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	array	Array of line item objects
custom_fields	object	Comma delimited list of custom fields.
origin	object	Transaction origin detail. This will only be included if the return_origin flag is passed through.
platform	object	Processor information.
available_actions	array	Comma delimited list of valid actions on this trasaction. Possible actions are:
- void
- releasefunds
- refund
- unvoid
- queue
- capture
- tip
- quicksale
- quickcredit
Check Response

Example Check Response

{
    "type": "transaction",
    "key": "7nfkgg1pbvtrkj2",
    "refnum": "100107",
    "created": "2018-12-04 15:06:38",
    "trantype_code": "K",
    "trantype": "Check Sale",
    "result_code": "A",
    "result": "Approved",
    "authcode": "TM57DD",
    "status_code": "P",
    "status": "Pending",
    "check": {
        "accountholder": "Remus Lupin",
        "checknum": "101",
        "trackingnum": "19020549391376",
        "effective": "2018-12-06",
        "processed": "2018-12-05",
        "settled": "2018-12-06",
        "returned": null,
        "banknote": null
    },
    "amount": "157.50",
    "amount_detail": {
        "tip": "0.00",
        "tax": "11.00",
        "shipping": "20.00",
        "duty": "10.00",
        "discount": "103.50",
        "subtotal": "230.00"
    },
    "ponum": "af416fsd5",
    "invoice": "98454685",
    "orderid": "8521adsf5312",
    "description": "Received First Year Discount",
    "comments": "Paid for by Headmaster",
    "clerk": "Madam Malkin",
    "cust_key": "lsddpx5f6ynkwf1s",
    "custid": "10245373",
    "customerid": "8645384653",
    "customer_email": "test@test.com",
    "merchant_email": "test@testmerchant.com",
    "clientip": "111.11.11.111",
    "source_name": "REST API INTEGRATION",
    "billing_address": {
        "company": "Hogwarts School of Witchcraft and Wizardry",
        "first_name": "Albus",
        "last_name": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "country": "USA",
        "postalcode": "10005",
        "phone": "5552533673"
    },
    "shipping_address": {
        "company": "Hogwarts School of Witchcraft and Wizardry",
        "first_name": "Albus",
        "last_name": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "country": "USA",
        "postalcode": "10005",
        "phone": "555-253-3673"
    },
    "lineitems": [
        {
            "name": "The Standard Book of Spells (Grade 1)",
            "description": "by Miranda Goshawk An indispensable guide to your basic magical needs.",
            "cost": "10.00",
            "taxable": "N",
            "qty": "1.0000",
            "tax_rate": "5.000",
            "commodity_code": "715-86",
            "product_key": "4s4b4qrn3k4pmsg8",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "name": "A History of Magic by Bathilda Bagshot",
            "description": "Bathilda Bagshot embarked on the journey of magical knowledge decades ago. She has always been fascinated by the mysteries and curiosities of the wizarding world. A History of Magic examines significant moments and facts from the beginning of time to the ",
            "cost": "20.00",
            "taxable": "Y",
            "qty": "1.0000",
            "tax_amount": "1.00",
            "tax_rate": "5.000",
            "commodity_code": "715-86",
            "product_key": "as4b4s9sfbwzd1v8",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "sku": "378675689",
            "name": "Dress Robes",
            "description": "Very pretty\/handsome",
            "cost": "100.00",
            "taxable": "Y",
            "qty": "2.0000",
            "tax_amount": "10.00",
            "tax_rate": "5.000",
            "commodity_code": "S2502",
            "product_key": "bs4b03tc5m4ccs82",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        }
    ],
    "custom_fields": {
        "1": "Gryffindor",
        "2": "Headmaster"
    },
    "origin": {
        "source_name": "REST TEST",
        "source_key": "9n4xhp4j80yd4jw80",
        "api_type": "Rest API",
        "clientip": "209.37.25.121",
        "clientip_country": "",
        "serverip": "104.35.239.146",
        "serverip_country": "",
        "software": "Floo Network",
        "user": "(auto)"
    },
    "platform": {
        "name": "Test Bed"
    },
    "available_actions": [
        "void",
        "queue",
        "quicksale",
        "quickrefund",
        "addtip"
    ]
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
trantype_code	string	Transaction type code. See possible values here
trantype	string	Transaction Type code description. See possible values here
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
status_code	string	Transaction status code see possible values here
status	string	Transaction status code description see possible values here
check	object	Object holding bank account information
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount.
ponum	string	Customer's purchase order number (required for level 3 processing)
invoice	string	Custom Invoice Number to easily retrieve sale details. (25 chars max)
orderid	string	Merchant assigned order ID
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
clerk	string	Clerk name.
custid	string	Gateway generated unique identifier for customer associated with the customer database. This is the identifier most closely associated with the SOAP API.
customerid	string	Merchant generated identifier for customer record.
customer_email	string	Email customer reciept was sent to.
merchant_email	string	Email merchant receipt was sent to.
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
source_name	string	API key name of key
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	array	Array of line item objects
custom_fields	object	Comma delimited list of custom fields.
origin	object	Transaction origin detail. This will only be included if the return_origin flag is passed through.
platform	object	Processor information.
available_actions	array	Comma delimited list of valid actions on this trasaction. Possible actions are:
- void
- releasefunds
- refund
- unvoid
- queue
- capture
- tip
- quicksale
- quickcredit
Cash Response

Example Check Response

{
    "type": "transaction",
    "key": "6nfty5v2dc764cs",
    "refnum": "100106",
    "created": "2018-12-06 15:05:29",
    "trantype_code": "G",
    "trantype": "Cash Sale",
    "result_code": "A",
    "result": "Approved",
    "authcode": null,
    "status_code": "P",
    "status": "Posted",
    "amount": "157.50",
    "amount_detail": {
        "tip": "0.00",
        "tax": "11.00",
        "shipping": "20.00",
        "discount": "103.50"
    },
    "ponum": "af416fsd5",
    "invoice": "98454685",
    "orderid": "8521adsf5312",
    "description": "Received First Year Discount",
    "comments": "Paid for by Headmaster",
    "terminal": "multilane",
    "clerk": "Madam Malkin",
    "drawer_shift": {
        "key": "3nmz17h2f4489t7y8",
        "clerk_key": "4nmmfrpn7szx8kzt8"
    },
    "cust_key": "lsddpx5f6ynkwf1s",
    "custid": "10245373",
    "customerid": "8645384653",
    "customer_email": "test@test.com",
    "merchant_email": "test@testmerchant.com",
    "clientip": "111.11.11.111",
    "source_name": "REST API INTEGRATION",
    "billing_address": {
        "company": "Hogwarts School of Witchcraft and Wizardry",
        "first_name": "Albus",
        "last_name": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "country": "USA",
        "postalcode": "10005",
        "phone": "5552533673"
    },
    "shipping_address": {
        "company": "Hogwarts School of Witchcraft and Wizardry",
        "first_name": "Albus",
        "last_name": "Dumbledore",
        "street": "123 Astronomy Tower",
        "street2": "Suite 1",
        "city": "Phoenix",
        "state": "CA",
        "country": "USA",
        "postalcode": "10005",
        "phone": "555-253-3673"
    },
    "lineitems": [
        {
            "name": "The Standard Book of Spells (Grade 1)",
            "description": "by Miranda Goshawk An indispensable guide to your basic magical needs.",
            "cost": "10.00",
            "taxable": "N",
            "qty": "1.0000",
            "tax_rate": "5.000",
            "commodity_code": "715-86",
            "product_key": "4s4b4qrn3k4pmsg8",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "name": "A History of Magic by Bathilda Bagshot",
            "description": "Bathilda Bagshot embarked on the journey of magical knowledge decades ago. She has always been fascinated by the mysteries and curiosities of the wizarding world. A History of Magic examines significant moments and facts from the beginning of time to the ",
            "cost": "20.00",
            "taxable": "Y",
            "qty": "1.0000",
            "tax_amount": "1.00",
            "tax_rate": "5.000",
            "commodity_code": "715-86",
            "product_key": "as4b4s9sfbwzd1v8",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "sku": "378675689",
            "name": "Dress Robes",
            "description": "Very pretty\/handsome",
            "cost": "100.00",
            "taxable": "Y",
            "qty": "2.0000",
            "tax_amount": "10.00",
            "tax_rate": "5.000",
            "commodity_code": "S2502",
            "product_key": "bs4b03tc5m4ccs82",
            "um": "EAC",
            "locationid": "13",
            "location_key": "dnyyjc8s2vbz8hb33"
        }
    ],
    "custom_fields": {
        "1": "Gryffindor",
        "2": "Headmaster"
    },
    "origin": {
        "source_name": "REST TEST",
        "source_key": "9n4xhp4j80yd4jw80",
        "api_type": "Rest API",
        "clientip": "209.37.25.121",
        "clientip_country": "",
        "serverip": "104.35.239.146",
        "serverip_country": "",
        "software": "Floo Network",
        "user": "(auto)"
    },
    "platform": {
        "name": "Test Bed"
    },
    "available_actions": [
        "void",
        "queue",
        "quicksale",
        "quickrefund",
        "addtip"
    ]
}

Response Parameters

Below are the response parameter fields. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique gateway generated key.
refnum	string	Unique transaction reference number.
trantype_code	string	Transaction type code. See possible values here
trantype	string	Transaction Type code description. See possible values here
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code. This will be empty for cash transactions.
status_code	string	Transaction status code see possible values here
status	string	Transaction status code description see possible values here
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount.
ponum	string	Customer's purchase order number (required for level 3 processing)
invoice	string	Custom Invoice Number to easily retrieve sale details.
orderid	string	Merchant assigned order ID
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
terminal	string	Transaction terminal description.
clerk	string	Clerk name.
drawer_shift	object	Object with shift drawer information.
cust_key	string	Gateway generated unique identifier for customer associated with the customer database. This is the preferred identifier for the REST API.
custid	string	Gateway generated unique identifier for customer associated with the customer database. This is the identifier most closely associated with the SOAP API.
customerid	string	Merchant generated identifier for customer record.
customer_email	string	Email customer reciept was sent to.
merchant_email	string	Email merchant receipt was sent to.
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
source_name	string	API key name of key
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	array	Array of line item objects
custom_fields	object	Comma delimited list of custom fields.
origin	object	Transaction origin detail. This will only be included if the return_origin flag is passed through.
available_actions	array	Comma delimited list of valid actions on this trasaction. Possible actions are:
- void
- releasefunds
- refund
- unvoid
- queue
- capture
- tip
- quicksale
- quickcredit
Transaction Webhook Events

Transaction change events are triggered when a transaction record is updated. The overall transaction event is fired for all transaction types (creditcard,check,etc). If you only need ACH updates rather than all tenders then use the ACH Status events outlined in the next section. The following webhook events are available for transaction processing:

Event Name	Description
transaction.sale.success	A transaction sale approved
transaction.sale.failure	A transaction sale failed - includes declines and errors
transaction.sale.voided	A transaction sale was voided
transaction.sale.captured	A transaction sale was captured
transaction.sale.adjusted	A transaction sale was adjusted
transaction.sale.queued	A transaction sale was queued
transaction.sale.unvoided	A voided transaction sale was unvoided
transaction.refund.success	A transaction refund approved
transaction.refund.voided	A transaction refund was voided.
ACH Status Webhook Events

ACH Status events fire when updates from the ACH processor are received. The following webhook events are available for ACH Status Updates:

Event Name	Description
ach.submitted	ACH status updated to submitted.
ach.settled	ACH status updated to settled.
ach.returned	ACH status updated to returned.
ach.voided	ACH status updated to voided.
ach.failed	ACH status updated to failed.
ach.note_added	Note added to ACH transaction.
Transaction Receipts

With the transaction receipt method, you can preview a receipt for a specific transaction or send a receipt after a transaction has already been processed.

Send Transaction Receipt

Endpoint

POST /api/v2/transactions/:transaction_key:/send


This method allows you to send a transaction receipt after the transaction has already been processed.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions/3nfsck05vt9xgqb/receipts/email
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3MwQUYyVmtHN3dRZHg3elM3TTkwQlU5MTBQcTU0bG46czIvZjQwNTg0YTU1MDk4MzA0Mjg5YzA4ZDBjNDkwODE4ZjUvOGFhOTcyZGVjOTViNDExNjc2YTAwODRlNjc1ZGRhZTNjMGE0ZDJjYzVhMDg5ZjEyYmQwNzhkODY5MDYwYTA3OQ=="
    -d
    '{
    "toemail": "example@example.com"
    }'

Request Parameters

Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
toemail	string	This is the email address of the desired recipient of the receipt. (Required)

Example Response

{
  "status": "success"
}

Response Parameters
Parameter Name	Type	Description
status	string	This shows the status of the receipt delivery.
Retrieve Transaction Receipt
GET /api/v2/transactions/:transaction_key:/receipts/:receiptid:


For viewing, an optional receiptid can be set to specify a custom receipt template formatting. If no receiptid is set, then the standard vterm receipt template is used.

curl -X GET
  https://sandbox.usaepay.com/api/v2/transactions/124444201/receipts/
  -H 'content-type: application/json'
  -H 'authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2'


Request Parameters
Parameter Name	Type	Description
transaction_key	string	The identifier key for the transaction. (Required)
receiptid	string	The identifier key for the receipt.

Example Response

"PHRhYmxlIHdpZHRoPSIyMDAiIHN0eWxlPSIgZm9udC1zaXplOiAxMHB4OyBmb250LWZhbWlseTogc2Fucy1zZXJpZjsiPgo8dHI+PHRkPgo8YnI+CjxjZW50ZXI+PGI+CgpVU0FlUGF5IFNhbmRib3ggMjxicj4KPGJyPgosIDxicj4KVGVsOiA8YnI+Ck1JRDogNTE2ODQxNTM0Njg0NTYzMTxicj4KPGJyPgo8L2I+Cgo8Yj5EYXRlOiA8L2I+IDA2LzI2LzE3IDAyOjEyIHBtPGJyPgo8L2NlbnRlcj4KPGJyPgoKQ2FyZCBOdW1iZXI6IDIyMjQgPGJyPgpDYXJkIEhvbGRlcjogSm9obiBkb2U8YnI+CkNhcmQgVHlwZTogPGJyPgpBVlMgU3RyZWV0OiAxMjM0IE1haW48YnI+CkFWUyBaaXA6IDEyMzQ1PGJyPgoKClJlZiAjOiAxMjQ0NDc2NTE8YnI+CkF1dGggQ29kZTogMTQ4NTY5PGJyPgpJbnZvaWNlICM6IDEyMzU2PGJyPgpQTyAjOiA8YnI+Ck9yZGVyICM6IDxicj4KVHlwZSBvZiBTYWxlOiBDcmVkaXQgQ2FyZCBTYWxlPGJyPgpMaW5lIEl0ZW06IDxicj4KQ3VzdG9tZXIgSUQ6IDxicj4KPGJyPgoKPGJyPgo8cCBhbGlnbj1yaWdodD4KPHRhYmxlIHN0eWxlPSIgZm9udC1zaXplOiAxMHB4OyBmb250LWZhbWlseTogc2Fucy1zZXJpZjsiPgo8dHI+PHRkIGFsaWduPXJpZ2h0PjxiPkFNT1VOVDo8L2I+PC90ZD4KPHRkIGFsaWduPXJpZ2h0PjxiPjMuNTA8L2I+PC90ZD48L3RyPgo8dHI+PHRkIGFsaWduPXJpZ2h0PjxiPlRBWDo8L2I+PC90ZD4KPHRkIGFsaWduPXJpZ2h0PjxiPjEuMDA8L2I+PC90ZD48L3RyPgoKPHRyPjx0ZCBhbGlnbj1yaWdodD48Yj5USVA6PC9iPjwvdGQ+Cjx0ZCBhbGlnbj1yaWdodD48Yj4wLjUwPC9iPjwvdGQ+PC90cj4KCgo8dHI+PHRkIGFsaWduPXJpZ2h0IGNvbHNwYW49Mj49PT09PT09PT09PT09PT08L3RkPjwvdHI+Cjx0cj48dGQgYWxpZ249cmlnaHQ+PGI+VE9UQUw6PC9iPjwvdGQ+Cjx0ZCBhbGlnbj1yaWdodD48Yj4KNS4wMDwvdGQ+PC90cj4KPC90YWJsZT4KPC9wPgoKPGJyPjxicj48YnI+PGJyPgoKPGNlbnRlcj4KWF9fX19fX19fX19fX19fX19fX19fX19fX19fX19fX19fX19fXzxicj4KSm9obiBkb2U8YnI+PGJyPgoKICBJIEFHUkVFIFRPIFBBWSBBQk9WRSBUT1RBTCBBTU9VTlQKICBBQ0NPUkRJTkcgVE8gQ0FSRCBJU1NVRVIgQUdSRUVNRU5UCgo8L2NlbnRlcj4KCjxicj4KX19fX19fX19fX19fX19fX19fX19fX19fX19fX19fX19fX19fCjxicj4KCjwvdGFibGU+Cg=="

Response

This is a base-64 encoded representation of the HTML-formatted receipt. To retrieve the HTML, perform a base-64 decode.
If using an SDK, our SDK returns the base-64 decoded result.

Bulk Transactions

Endpoint

api/v2/bulk_transactions


This method allows you to upload and manage a .csv file of transactions. Files can contain credit card, ACH, or token transactions. You will also be able to retrieve the status of the upload and detail for the transactions contained in the file.

Upload Transaction File

Endpoint

POST api/v2/bulk_transactions


This is the endpoint to upload a .csv file of transactions. Files can contain credit card, ACH, or token transactions.

Example Request

curl -F 'data=@/Users/adminuser/Desktop/uploads/upload1.csv' https://secure.usaepay.com/api/v2/bulk_transactions
-H "Authorization: Basic aGswTmVoV1lYbzdWbXd4MFM3OWJmMTdrMmJvVzJqVDc6czIvNmQyZjY1NmM4ZGEwMDgzMTBjMmJhZWVkNDg5MzBhYzUvYmUzZDZlNDQ4ODE4ZmFkOGNhNjc4N2ZlNDljNjFkNDYxYmMyM2Q4MjFmZjBmYzI4OThlNmQ0ZTBkMWRjMjNjNA=="

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
data	string	This the reference to the file you would like to upload. The reference to the file upload in this example is /Users/adminuser/Desktop/uploads/upload1.csv (Required)

Example Response

{
    "lines": 3,
    "transactions": 3,
    "errors": [],
    "dupes": 3,
    "bulk_key": "intn1h9ksrg8c4yrr"
}

Response Parameters
Parameter Name	Type	Description
lines	integer	Total possible transactions found in .csv file.
transactions	integer	Number of lines detected with in file which will attempt to process. transactions + errors = lines
errors	integer	Number of lines that could not be accepted for processing due to incorrect formatting. transactions + errors = lines
dupes	integer	Number of duplicate files detected in past bulk transaction uploads.
bulk_key	string	Unique identifier for group of transactions generated by gateway.
Retrieve File Status

Endpoint

GET /api/v2/bulk_transactions/:bulk_key:


OR

GET /api/v2/bulk_transactions/current


This method retrieves status information for a specific group of bulk transactions. You can also use current to reference the group of transactions that is currently processing.

Example Request

curl -X GET https://secure.usaepay.com/api/v2/bulk_transactions/antn1r9j6bss668h1
-H "Authorization: Basic OWZuVVQ3Zkk2Tjc2YjU1M0pEUkk1MTQwOVlVYnA4OTg6czIvZDUxYjkxMTUxMThkNTRkZGUzYzYxOTA3ODVjNDJhZGUvMGMyZDdkMDVlNGViMjNkMDAzZDdhZTc5YmZiM2M1NWNjYzM2NmMzYTQzZjgzOGYxYzBiZDNkMWNlZWYwYjQ3Yw=="

Request Parameters

Parameters marked Required are required to process this type of transaction.

Parameter Name	Type	Description
bulk_key	string	Unique identifier for group of transactions generated by gateway. You can also use current reference the group of transactions that is currently processing. (Required)

Example Response

{
    "bulk_key": "antn1r9j6bss668h1",
    "uploaded": "2018-08-15 14:49:57",
    "filename": "upload1.csv",
    "status": "Completed",
    "transactions": "3",
    "remaining": "0",
    "approved": "3",
    "declined": "0",
    "errors": "0"
}

Response Parameters
Parameter Name	Type	Description
bulk_key	string	Unique identifier for group of transactions generated by gateway.
uploaded	string	Date and time the file was uploaded.
filename	string	Name of the file containing the transactions.
status	string	Current status of the bulk transaction upload.
transactions	integer	Number of total transactions uploaded.
remaining	integer	Number of transactions left to process.
approved	integer	Number of approved transactions currently processed within the bulk transaction file.
declined	integer	Number of declined transactions currently processed within the bulk transaction file.
errors	integer	Number of err'd transactions currently processed within the bulk transaction file.
Retrieve File Transactions

Endpoint

GET /api/v2/bulk_transactions/:bulk_key:/transactions


OR

GET /api/v2/bulk_transactions/current/transactions


Use this method to see the details of each transaction in bulk transaction file. You can also use current to reference the group of transactions that is currently processing.

Example Request

curl -X GET https://secure.usaepay.com/api/v2/bulk_transactions/antn1r9j6bss668h1/transactions
-H "Authorization: Basic OWZuVVQ3Zkk2Tjc2YjU1M0pEUkk1MTQwOVlVYnA4OTg6czIvZDUxYjkxMTUxMThkNTRkZGUzYzYxOTA3ODVjNDJhZGUvMGMyZDdkMDVlNGViMjNkMDAzZDdhZTc5YmZiM2M1NWNjYzM2NmMzYTQzZjgzOGYxYzBiZDNkMWNlZWYwYjQ3Yw=="

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
bulk_key	string	Unique identifier for group of transactions generated by gateway. You can also use current reference the group of transactions that is currently processing. (Required)

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "type": "transaction",
            "key": "gdb5vq0641y9vbpn",
            "refnum": "2071671514",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "544324",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "Cary Grant",
                "number": "4000xxxxxxxx2222",
                "avs_street": "789 North Way",
                "avs_postalcode": "90005",
                "category_code": "A"
            },
            "avs": {
                "result_code": "NYZ",
                "result": "Address: No Match & 5 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "M",
                "result": "Match"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1"
            },
            "amount": "20.00",
            "invoice": "103"
        },
        {
            "type": "transaction",
            "key": "fdb5vq1hf1vg9q3m",
            "refnum": "2071671513",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "544325",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "James Dean",
                "number": "4000xxxxxxxx2223",
                "avs_street": "456 Griffith Park Dr.",
                "avs_postalcode": "90005",
                "category_code": "A"
            },
            "avs": {
                "result_code": "YYX",
                "result": "Address: Match & 9 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "M",
                "result": "Match"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1"
            },
            "amount": "20.00",
            "invoice": "102"
        },
        {
            "type": "transaction",
            "key": "ddb57bdmpybmjm2c",
            "refnum": "2071671511",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "544326",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "Audrey Hepburn",
                "number": "4000xxxxxxxx2224",
                "avs_street": "123 Tiffany St.",
                "avs_postalcode": "90005",
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
                "batchrefnum": 409384,
                "sequence": "1"
            },
            "amount": "20.00",
            "invoice": "101"
        }
    ],
    "total": "3"
}

Response Parameters
Parameter Name	Type	Description
type	string	This is will always be list for this method.
limit	integer	Limit setting for this response. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
data	array	Array of transactions uploaded.
total	integer	Total number of transactions in the file you uploaded.
Pause File Processing

Endpoint

POST api/v2/bulk_transactions/:bulk_key:/pause


OR

POST api/v2/bulk_transactions/current/pause


Once you have uploaded a file of transactions, you can pause the upload to continue later. You can also use current to reference the group of transactions that is currently processing.

Example Request

curl -X POST https://secure.usaepay.com/api/v2/bulk_transactions/intn1h9ksrg8c4yrr/pause
-H "Authorization: Basic aGswTmVoV1lYbzdWbXd4MFM3OWJmMTdrMmJvVzJqVDc6czIvYTI2OTkzMjNhNGZlOWRhZjZkNWUzNTY2ODA5NDUyNTkvZjkxOGU3MTI0NmQyMjgyNmQ4ZTYwYjg1ZDdjMjM0NGY1ZGMwNWI1ODU5YWQ2NjEwOWM4Mzg0ZDE5ZjFiNWU4Yg=="

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
bulk_key	string	Unique identifier for group of transactions generated by gateway. You can also use current reference the group of transactions that is currently processing. (Required)

Example Response

{
    "status": "success"
}

Response Parameters
Parameter Name	Type	Description
status	string	If the bulk transaction has been paused successfully then status will be returned as success. If not successful, an error will be returned.
Resume File Processing

Endpoint

POST api/v2/bulk_transactions/:bulk_key:/resume


This page will show you how to how to resume processing a paused group of bulk transactions.

Example Request

curl -X POST https://secure.usaepay.com/api/v2/bulk_transactions/intn1h9ksrg8c4yrr/resume
-H "Authorization: Basic aGswTmVoV1lYbzdWbXd4MFM3OWJmMTdrMmJvVzJqVDc6czIvYTI2OTkzMjNhNGZlOWRhZjZkNWUzNTY2ODA5NDUyNTkvZjkxOGU3MTI0NmQyMjgyNmQ4ZTYwYjg1ZDdjMjM0NGY1ZGMwNWI1ODU5YWQ2NjEwOWM4Mzg0ZDE5ZjFiNWU4Yg=="

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
bulk_key	string	Unique identifier for group of transactions generated by gateway. (Required)

Example Response

{
    "status": "success"
}

Response Parameters
Parameter Name	Type	Description
status	string	If the bulk transaction have resumed successfully then status will be returned as success. If not successful, an error will be returned.
Tokenization

Tokenization is the process of breaking a stream of text/numbers up into words, phrases, symbols, or other meaningful elements called tokens. This token can then be used in place of a credit card number when processing a transaction. This is useful when a developer does not want to under take the security requirements of storing card data.

Create Token

Endpoint

POST /api/v2/transactions


This command validates the card data and then returns a card reference token. The card reference token can be used in the card number field in most scenarios. Tokens only store the card number and the expiration date. Tokens are not related to a specific merchant and can be used across multiple merchants.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
    `{
    "command": "cc:save",
    "creditcard": {
        "cardholder": "John Doe",
        "number": "4000100011112224",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Main",
        "avs_postalcode": "12345"
    }
 }`

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
command	string	This must be set to cc:save to tokenize a card. (Required)
creditcard	object	Object holding credit card information

Example Response

{
    "type": "transaction",
    "key": "",
    "refnum": "",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "",
    "creditcard": {
        "number": "4000xxxxxxxx2224",
        "cardholder": "John Doe"
    },
    "savedcard": {
        "type": "Visa",
        "key": "tqb9-o076-tpfk-lb8l",
        "cardnumber": "4000xxxxxxxx2224"
    }
}

Response Parameters
Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Unique key for the transaction record. This will be empty for the cc:save command.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved or E = Error
result	string	Long version of above result_code (Approved, etc)
authcode	string	Authorization code. This will be empty for the cc:save command.
creditcard	object	Object holding credit card information
savedcard	object	Object holding credit card information
Create Token from Transaction

Endpoint

POST /api/v2/tokens


This method allows you to tokenize a card used in a previous transaction.

Example Request

  curl -X POST https://sandbox.usaepay.com/api/v2/tokens
      -H "Content-Type: application/json"
      -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
      -d
      `{
  "trankey":"onfdx69s5cjb3x8"
        }
`

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
trankey	string	Unique key for the transaction record.

Example Response

{
    "cardref": "w4a4-ge1p-36rk-j4jy",
    "masked_card_number": "XXXXXXXXXXXX1111",
    "card_type": "Visa"
}

Response Parameters
Parameter Name	Type	Description
cardref	string	Unique key for the card. This is the token.
masked_card_number	string	Masked credit card data.
card_type	string	The card brand of the tokenized card. (Visa, MasterCard, AMEX, etc.)
Create Multiple Tokens

Endpoint

POST /api/v2/tokens/:token:


This method allows you to tokenize multiple cards in one call.

Example Request

  curl -X POST https://sandbox.usaepay.com/api/v2/tokens
      -H "Content-Type: application/json"
      -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
      -d
      `[
        {
          "creditcard": {
              "cardholder": "John Doe",
              "number": "4000100011112224",
              "expiration": "0919",
              "cvc": "123",
              "avs_street": "1234 Main",
              "avs_postalcode": "12345"
          }
        },
        {
          "creditcard": {
              "cardholder": "Jane Smith",
              "number": "44444555566667779",
              "expiration": "0919",
              "cvc": "123",
              "avs_street": "4321 Center St",
              "avs_postalcode": "12345"
          }
        },
        {
          "creditcard": {
              "cardholder": "Bad Card",
              "number": "44444555566667778",
              "expiration": "0919",
              "cvc": "123",
              "avs_street": "4321 Center St",
              "avs_postalcode": "12345"
          }
        }
      ]`

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
creditcard	array	Array of credit card objects to tokenize. Object fields shown below.

Example Response

[
  {
    "type":"token",
    "key":"34g3sdg34vesfdvsb"
  },
  {
    "type":"token",
    "key":"wefuh29738cg38gsdf"
  },
  {
    "type":"token",
    "error":"Invalid Card Number"
  }
]

Response Parameters
Parameter Name	Type	Description
type	string	Type of object being returned. This will always be "token" for this method
key	string	The token for the credit card.
error	string	If the gateway isn't able to tokenize the information, an error message will be sent back in this field.
Retrieve Specific Token

Endpoint

GET /api/v2/tokens


This method allows you to retrieve token detail for a specific token.

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/tokens/w4a4ge1p36rkj4jy
  -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvYWY3YzMyNzZlYTk1ZjQwMzBkYmNhZmYyYTM0ODFiYzkvMWUzNTk4NzUyMGFjNWM2NWRiYWRjZDE4MzVhZjYyYWI0MWExYjZhNTg1ZGNhMjc4MDM3OTgwZmM4MjJhZmM1Yg=="
  -H "Content-Type: application/json"

Request Parameters

Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
token	string	Unique key for the card. This is the token. Required

Example Response

{
    "cardref": "w4a4-ge1p-36rk-j4jy",
    "masked_card_number": "XXXXXXXXXXXX1111",
    "card_type": "Visa"
}

Response Parameters
Parameter Name	Type	Description
cardref	string	Unique key for the card. This is the token.
masked_card_number	string	Masked credit card data.
card_type	string	The card brand of the tokenized card. (Visa, MasterCard, AMEX, etc.)
Batches
Retrieve Batch List

Endpoint

GET /api/v2/batches


The GET batches endpoint retrieves a list of batches in descending order starting with the newest. You can set the offset and limits for results as well as filter results by date.

Example Request With NO Filters

curl -X GET https://sandbox.usaepay.com/api/v2/batches
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"

Request Parameters

These are a list of optional parameters which can be sent with the GET request to manipulate results.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
openedlt	string	Format of date should be YYYYmmdd. Will include the results that are opened before this date.
openedgt	string	Format of date should be YYYYmmdd. Will include the results that are opened after this date.
closedlt	string	Format of date should be YYYYmmdd. Will include the results that are closed before this date.
closedgt	string	Format of date should be YYYYmmdd. Will include the results that are closed after this date.
openedle	string	Format of date should be YYYYmmdd. Will include the results that are opened before this date. (Including this date)
openedge	string	Format of date should be YYYYmmdd. Will include the results that are opened after this date. (Including this date)
closedle	string	Format of date should be YYYYmmdd. Will include the results that are closed before this date. (Including this date)
closedge	string	Format of date should be YYYYmmdd. Will include the results that are closed after this date. (Including this date)

Example Response

{  
   "type":"list",
   "limit":20,
   "offset":0,
   "data":[  
      {
          "type": "batch",
          "key": "et1m9h57b44h16g",
          "batchrefnum": "408318",
          "sequence": "1519",
          "opened": "2019-02-15 12:32:21"
      },
      {
          "type": "batch",
          "key": "dt18n0g3b4q4sm8",
          "batchrefnum": "408317",
          "sequence": "1518",
          "opened": "2019-02-15 09:56:35",
          "closed": "2019-02-15 10:00:23"
      },
      {
          "type": "batch",
          "key": "ft1m9m5p9wgd9mb",
          "batchrefnum": "407929",
          "sequence": "1423",
          "opened": "2019-01-28 17:30:37",
          "closed": "2019-02-14 15:20:43"
      }
   ],
   "total":"3"
}

Response Parameters
Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of batches that will be included.
offset	integer	The number of batches skipped from the results.
data	array	An array of batches matching the search.
total	integer	The total amount of batches, including filtering results.
Example with Date Filter

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/batches?openedge=20171003&openedlt=20171004
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"


This request is an example of a GET request with a Date Filter implemented.

Example Response

{  
   "type":"list",
   "limit":20,
   "offset":0,
   "data":[  
      {  
         "type":"batch",
         "key":"lt1s38k8zyqd9t0",
         "batchrefnum": "408317",
         "sequence": "1518",
         "opened":"2017-10-03 10:46:49",
         "closed":"2017-10-04 10:31:01"
      },
      {  
         "type":"batch",
         "key":"kt1swm641hbwm0x",
         "batchrefnum": "407929",
         "sequence": "1423",
         "opened":"2017-10-03 10:08:45",
         "closed":"2017-10-03 10:31:02"
      }
   ],
   "total":"2"
}

Retrieve Specific Batch

Endpoint

GET /api/v2/batches/:batch_key:


OR

GET /api/v2/batches/current


These batch endpoints can either get the merchant's current batch or a batch by its key. The response will show details about the batches status and relevant dates.

By Batch Key

Example Request

curl -X GET https://secure.usaepay.com/api/v2/batches/bt1vswm59pb2488
-H "Authorization: Basic dENvbk81VWQ5ekRjTGhzajBEcDNsZTduUVhrOUhmSk86czIvMjk1ZGJkNTA3ZjgxYWJiMDBiZGNkNWQ3NDkxZDdmMjMvZTBmNTgxODUwNTQzOWEzMjg4MzI3MTZkYWIxOTg2ZWZkMWJjYWI2OWJiMDdhNTUxOGIyZDNlY2Y5YWNiYWFiNA=="


This request is an example of a GET request using a batch_key.

Request Parameters

Parameters marked Required are required to process this type of transaction.

Parameter Name	Type	Description
batch_key	string	Unique identifier for the batch generated by gateway. You can also use current reference the currently open batch. (Required)

Example Response

{
    "type": "batch",
    "key": "bt1vswm59pb2488",
    "batchrefnum": "404494",
    "opened": "2018-10-15 05:04:37",
    "status": "open",
    "scheduled": "2018-10-16 17:04:37",
    "total_amount": 73.78,
    "total_count": 9,
    "sales_amount": 126,
    "sales_count": 7,
    "voids_amount": 5,
    "voids_count": 1,
    "refunds_amount": 52.22,
    "refunds_count": 1
}

Response Parameters
Parameter Name	Type	Description
type	string	Type of object. Will always be batch for this endpoint.
key	string	This is the unique batch identifier.
batchrefnum	integer	This is the unique batch identifier. This was originally used in the SOAP API.
opened	string	Date and time the batch was opened. Format will be, YYYY-MM-DD HH:MM:SS.
status	string	Batch status. Options are: open, closed, and closing when the batch is in the process of closing.
scheduled	string	Date and time the batch is scheduled to be closed. Format will be, YYYY-MM-DD HH:MM:SS. Only shows for open batches.
closed	string	Date and time the batch was closed. Format will be, YYYY-MM-DD HH:MM:SS. Only shows for closed batches.
total_amount	double	Total amount in dollars in the specific batch. This includes sales, voids, and refunds.
total_count	integer	Total transaction count. This includes sales, voids, and refunds.
sales_amount	double	Total amount for sales in dollars in the specific batch. Only shows when batch contains this transaction type.
sales_count	integer	Total transaction count. This includes sales only. Only shows when batch contains this transaction type.
voids_amount	double	Total amount for voids in dollars in the specific batch. Only shows when batch contains this transaction type.
voids_count	integer	Total transaction count. This includes voids only. Only shows when batch contains this transaction type.
refunds_amount	double	Total amount for refunds in dollars in the specific batch. Only shows when batch contains this transaction type.
refunds_count	integer	Total transaction count. This includes refunds only. Only shows when batch contains this transaction type.
Current Batch

Example Request

curl -X GET https://secure.usaepay.com/api/v2/batches/current
-H "Authorization: Basic dENvbk81VWQ5ekRjTGhzajBEcDNsZTduUVhrOUhmSk86czIvNDhlOTg0NTg5ZTI2ZDJlOTA1OGY1OWUzNjhjOGU1YjMvYTc5MDViODMwYjNlMDQxZGM1OTdlOTZiZDBhM2Y4ZTk2Zjg2MmFiZWIxNGY1NjQ0ZGVhOTRkZTM0YzNkYjE0MA=="


This request is an example of a GET request for the currently open batch. Request and response parameters are the same as theby batch key example.

Example Response

{
    "type": "batch",
    "key": "bt1vswm59pb2488",
    "batchrefnum": "403685",
    "opened": "2018-10-15 05:04:37",
    "status": "open",
    "scheduled": "2018-10-16 17:04:37",
    "total_amount": 131,
    "total_count": 8,
    "sales_amount": 131,
    "sales_count": 8
}

Retrieve Batch Transactions

Endpoint

GET /api/v2/batches/:batch_key:/transactions


OR

GET /api/v2/batches/current/transactions


These batch endpoints can either get the merchant's current batch or a batch by its key. The response will show transaction details for the transactions contained in the specified batch. You can set the offset and limits for results.

By Batch Key

Example Request

curl -X GET https://secure.usaepay.com/api/v2/batches/pt1qxpnx0f5303d/transactions
-H "Authorization: Basic dENvbk81VWQ5ekRjTGhzajBEcDNsZTduUVhrOUhmSk86czIvM2Y1NWIxMTYzYzY4MTAwZTZmYjg2NzhhNWY3MDY1ZGYvMGZjNDFjZTA3ZWUzNGQ0ZjYxMWQxYjBmOWMxZTRmMTYyOGUzYjE5MTBmZDQyOWUzZTc3YzExNmM5NzIxOTBjOA=="

Request Parameters
Parameter Name	Type	Description
batch_key	string	This is the unique batch identifier.
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "type": "transaction",
            "key": "cnfyy402xz086z8",
            "refnum": "2144606814",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "703449",
            "status_code": "S",
            "status": "Settled",
            "creditcard": {
                "cardholder": "non token customer Customer",
                "number": "4444xxxxxxxx7779",
                "avs_street": "1234 main st",
                "avs_postalcode": "12345",
                "category_code": "A",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "avs": {
                "result_code": "YYY",
                "result": "Address: Match & 5 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "P",
                "result": "Not Processed"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1417"
            },
            "amount": "50.00",
            "amount_detail": {
                "tip": "0.00",
                "tax": "2.50",
                "shipping": "0.00",
                "discount": "0.00"
            },
            "orderid": "1655102995",
            "description": "Welcome to Costco, I love you",
            "comments": "addCustomer test Created Charge",
            "billing_address": {
                "company": "PHP",
                "first_name": "non token customer",
                "last_name": "Customer",
                "street": "1234 main st",
                "street2": "Suite #123",
                "city": "Los Angeles",
                "state": "CA",
                "country": "US",
                "postalcode": "12345",
                "phone": "333-333-3333"
            }
        },
        {
            "type": "transaction",
            "key": "8nfk1vtz58ckfc4",
            "refnum": "2143977090",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "701199",
            "status_code": "S",
            "status": "Settled",
            "creditcard": {
                "cardholder": "non token customer Customer",
                "number": "4444xxxxxxxx7779",
                "avs_street": "1234 main st",
                "avs_postalcode": "12345",
                "category_code": "A",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "avs": {
                "result_code": "YYY",
                "result": "Address: Match & 5 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "P",
                "result": "Not Processed"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1417"
            },
            "amount": "50.00",
            "amount_detail": {
                "tip": "0.00",
                "tax": "2.50",
                "shipping": "0.00",
                "discount": "0.00"
            },
            "orderid": "1655102995",
            "description": "Welcome to Costco, I love you",
            "comments": "addCustomer test Created Charge",
            "billing_address": {
                "company": "PHP",
                "first_name": "non token customer",
                "last_name": "Customer",
                "street": "1234 main st",
                "street2": "Suite #123",
                "city": "Los Angeles",
                "state": "CA",
                "country": "US",
                "postalcode": "12345",
                "phone": "333-333-3333"
            }
        }
    ],
    "total": "2"
}

Response Parameters
Parameter Name	Type	Description
type	string	This is will always be list for this method.
limit	integer	Limit setting for this response. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
data	array	Array of transactions uploaded.
total	integer	Total number of transactions in the batch.
Current Batch

Example Request

curl -X GET https://secure.usaepay.com/api/v2/batches/current/transactions
-H "Authorization: Basic dENvbk81VWQ5ekRjTGhzajBEcDNsZTduUVhrOUhmSk86czIvMDU4MzczZjVkYTAzMzM3Zjk5YWFkODBhMzU4ODdiOTUvYzgxMzVhNWNmZGI4OWY5ZDYzNzE4YTY4YTMzMzFjMmNhMWRjZmYxYTNlODRlZTU5Zjc4ZGUyOTc1NjE3OWY4Ng=="


This request is an example of a GET request for the currently open batch. Request and response parameters are the same as the by batch key example.

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "type": "transaction",
            "key": "enfy5w3bnxqq433",
            "refnum": "2146101010",
            "trantype_code": "C",
            "trantype": "Credit Card Refund (Credit)",
            "result_code": "A",
            "result": "Approved",
            "authcode": "214611",
            "status_code": "P",
            "status": "Pending Settlement",
            "creditcard": {
                "cardholder": "Thelma Rogers",
                "number": "5555xxxxxxxx2226",
                "category_code": "",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1418"
            },
            "amount": "52.22",
            "amount_detail": {
                "tax": "2.22"
            },
            "invoice": "52381283",
            "orderid": "75665798",
            "description": "Recurring Bill"
        },
        {
            "type": "transaction",
            "key": "jnfychx0fs524t0",
            "refnum": "2146100989",
            "trantype_code": "Z",
            "trantype": "Voided Credit Card Refund (Credit)",
            "result_code": "A",
            "result": "Approved",
            "authcode": "214611",
            "status_code": "P",
            "status": "Voided",
            "creditcard": {
                "cardholder": "non token customer Customer",
                "number": "4444xxxxxxxx7779",
                "category_code": "",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1418"
            },
            "amount": "50.00",
            "amount_detail": {
                "tax": "2.50"
            },
            "orderid": "1655102995",
            "description": "Welcome to Costco, I love you"
        },
        {
            "type": "transaction",
            "key": "2nfj5cpgsf4syhb",
            "refnum": "2145985220",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "707113",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "Thelma Rogers",
                "number": "5555xxxxxxxx2226",
                "avs_street": "123 Sesame St",
                "avs_postalcode": "12345",
                "category_code": "M",
                "entry_mode": "Card Not Present, Manually Keyed"
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
                "batchrefnum": 409384,
                "sequence": "1418"
            },
            "amount": "5.00",
            "amount_detail": {
                "tip": "0.50",
                "tax": "1.00",
                "shipping": "0.00",
                "discount": "0.00"
            },
            "billing_address": {
                "company": "PBS",
                "first_name": "Fred",
                "last_name": "Rogers",
                "street": "143 Neighborhood Way",
                "street2": "Apt 143",
                "city": "Pittsburgh",
                "state": "PA",
                "country": "USA",
                "postalcode": "15106",
                "phone": "555-5623"
            }
        },
        {
            "type": "transaction",
            "key": "fnfkwdsxmbs7f4k",
            "refnum": "2145983751",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "707110",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "Thelma Rogers",
                "number": "5555xxxxxxxx2226",
                "avs_street": "123 Sesame St",
                "avs_postalcode": "12345",
                "category_code": "M",
                "entry_mode": "Card Not Present, Manually Keyed"
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
                "batchrefnum": 409384,
                "sequence": "1418"
            },
            "amount": "5.00",
            "amount_detail": {
                "tip": "0.50",
                "tax": "1.00",
                "shipping": "0.00",
                "discount": "0.00"
            },
            "billing_address": {
                "company": "PBS",
                "first_name": "Fred",
                "last_name": "Rogers",
                "street": "143 Neighborhood Way",
                "street2": "Apt 143",
                "city": "Pittsburgh",
                "state": "PA",
                "country": "USA",
                "postalcode": "15106",
                "phone": "555-5623"
            }
        },
        {
            "type": "transaction",
            "key": "5nf0c9thbmmydfh",
            "refnum": "2145970637",
            "trantype_code": "V",
            "trantype": "Voided Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "707069",
            "status_code": "R",
            "status": "Voided (Funds Released)",
            "creditcard": {
                "cardholder": "Cornelius Fudge",
                "number": "4444xxxxxxxx7779",
                "avs_street": "1234 Main",
                "avs_postalcode": "34545",
                "category_code": "A",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "avs": {
                "result_code": "YYY",
                "result": "Address: Match & 5 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "P",
                "result": "Not Processed"
            },
            "batch": {
                "type": "batch",
                "key": "0t1k3yx5xs37cvb",
                "batchrefnum": 409384,
                "sequence": "1418"
            },
            "amount": "5.00",
            "amount_detail": {
                "tip": "0.00",
                "tax": "0.00",
                "shipping": "0.00",
                "discount": "0.00"
            },
            "billing_address": {
                "company": "Ministry of Magic",
                "first_name": "Cornelius",
                "last_name": "Fudge",
                "street": "123 Ministers Way",
                "street2": "Suite 505",
                "city": "London",
                "state": "CA",
                "country": "UK",
                "postalcode": "WC2N5DU",
                "phone": "555-867-5309"
            }
        }
    ],
    "total": "5"
}

Common Errors
{  
   "error":"Invalid record locator key",
   "errorcode":47
}

Invalid record locator key.

If the batch key does not exist for your merchant, you will get the following response.

Close Batch

Endpoint

POST /api/v2/batches/current/close


Use this API endpoint to close the currently open batch.

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/batches/current/close
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"

Request Parameters

Parameters marked Required are required to process this type of transaction.

Parameter Name	Type	Description
batch_key	string	Unique identifier for the batch generated by gateway. You can also use current reference the currently open batch. (Required)

Example Response

{
    "type": "batch",
    "key": "bt1vswm59pb2488",
    "batchrefnum": "403881",
    "sequence": "1418",
    "opened": "2018-10-15 05:04:37",
    "status": "closing"
}

Response Variables
Parameter Name	Type	Description
type	string	This is a batch.
key	string	Unique identifier for the batch generated by gateway.
batchrefnum	integer	This is the unique batch identifier. This was originally used in the SOAP API.
sequence	integer	The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc.
opened	string	Date and time the batch was opened. Format will be, YYYY-MM-DD HH:MM:SS.
status	string	Batch status. Options are: open, closed, and closing when the batch is in the process of closing.
Batch Webhook Events

The following webhook events are available for settlement:

Event Name	Description
settlement.batch.success	A batch settles successfully
settlement.batch.failure	A batch receives an error when attempting to settle
Payment Engine Endpoints
api/v2/paymentengine


The Payment Engine is a cloud based EMV middleware that point of sale developers can use to simplify their integration with various EMV capable payment terminals.

Devices
api/v2/paymentengine/devices


Use the devices endpoint to register, remove, and manage payment terminals.

Register Device

Endpoint

POST /api/v2/paymentengine/devices


Registers a payment terminal device. The device registration process associates a terminal with a merchant. Depending on the terminal_type, the process may require two steps.

For terminals running the payment engine standalone middleware such as the Castles MP200 a device pairing code is used. When a new terminal is first turned on, it will display a screen prompting for a pairing code. The point of sale software should use API resource to obtain a code. The pairing code is then entered into the terminal. If successful, the terminal should display a welcome idle screen and the API will show a status of "online" in GET /api/v2/paymentengine/devices/:devicekey:.

For security reasons, pairing codes are only valid for a few minutes. If the time limit has expired without the pairing code being entered on the terminal, the pairing code and associated device key are automatically removed.

Once a terminal is successfully paired, it will remain registered until deleted: there is no need to re-register a terminal when it reboots.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
  -H "Content-Type: application/json" -X POST \ https://usaepay.com/api/v2/paymentengine/devices
    -d '{"name":"Register 1","terminal_type":"standalone","settings":{"timeout":30,"share_device":false,"enable_standalone":false,"sleep_battery_device":5,"sleep_battery_display":1,"sleep_powered_device":0,"sleep_powered_display":0},"terminal_config":{"enable_emv":true,"enable_debit_msr":true,"enable_tip_adjust":true,"enable_contactless":true}}'

Parameter Name	Type	Description
terminal_type	string	Type of terminal being registered, currently the only option is 'standalone'.
name	string	A name associated with the terminal. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.
settings		Device settings
terminal_config		Terminal configuration
Response

Example Response

{
    "type": "device",
    "key": "sa_WKwzyQawBG0RMy0XpDGFXb6pXA23r",
    "apikeyid": "tvYTIxIhm83SlTO4zU",
    "terminal_type": "standalone",
    "status": "waiting for device pairing",
    "name": "Register 1",
    "settings": {
    "timeout": 30,
    "share_device": false,
    "enable_standalone": false,
    "sleep_battery_device": 5,
    "sleep_battery_display": 1,
    "sleep_powered_device": 0,
    "sleep_powered_display": 0
    },
    "pairing_code": 628215,
    "expiration": "2016-10-29 13:20:47",
    "terminal_config": {
        "enable_emv": false,
        "enable_debit_msr": false,
        "enable_tip_adjust": false,
        "enable_contactless": true
    }
}

Parameter Name	Type	Description
type	string	Object type. This will always be device.
key	string	Unique device identifier
apikeyid	string	The id of API key (source key) associated with the device.
terminal_type	string	Terminal type: "standalone" for payment engine cloud based terminal.
status	string	Current device status
name	string	Developer assigned device name. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.
settings		Device settings
terminal_info		Details of terminal
terminal_config		Terminal configuration
pairing_code	string	If terminal type is 'standalone', this is the pairing code required to pair the payment device with the payment engine.
expiration	string	If terminal type is 'standalone', the expiration is the date/time that the pairing code is no longer valid.
Get Specific Device

Endpoint

GET /api/v2/paymentengine/devices/:devicekey:


Retrieve information about a device by its device key.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
https://usaepay.com/api/v2/paymentengine/devices/sa_CCpRfLbkYXNV9rVLdRGvPjwf6ytgN

Parameter Name	Type	Description
key	string	Unique device identifier
Response

Example Response

{
  "type": "device",
  "key": "sa_WKwzyQawBG0RMy0XpDGFXb6pXA23r",
  "apikeyid": "ntC8nP31Moh0wtvYT",
  "terminal_type": "standalone",
  "status": "connected",
  "name": "Register 1",
  "settings": {
    "timeout": 30,
    "share_device": false,
    "enable_standalone": false,
    "sleep_battery_device": 5,
    "sleep_battery_display": 1,
    "sleep_powered_device": 0,
    "sleep_powered_display": 0
  },
  "terminal_info": {
    "make": "Castles",
    "model": "VEGA3000",
    "revision": "18043001-0055.00",
    "serial": "011118170300198",
    "key_pin": "FFFF5B04",
    "key_pan": "FF908A"
  },
  "terminal_config": {
    "enable_emv": true,
    "enable_debit_msr": true,
    "enable_tip_adjust": true,
    "enable_contactless": true
  }
}

Parameter Name	Type	Description
type	string	Object type. This will always be device.
key	string	Unique device identifier
apikeyid	string	The id of API key (source key) associated with the device.
terminal_type	string	Terminal type: "standalone" for payment engine cloud based terminal.
status	string	Current device status
name	Developer assigned device name. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.	
settings		Device settings
terminal_info		Details of terminal
terminal_config		Terminal configuration
pairing_code	string	If terminal type is 'standalone', this is the pairing code required to pair the payment device with the payment engine.
expiration	string	If terminal type is 'standalone', the expiration is the date/time that the pairing code is no longer valid.
Get Device List

Endpoint

GET /api/v2/paymentengine/devices


Retrieves a list of devices registered to merchant.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
https://usaepay.com/api/v2/paymentengine/devices

Response

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "type": "device",
            "key": "sa_WKwzyQawBG0RMy0XpDGFXb6pXA23r",
            "apikeyid": "ntC8nP31Moh0wtvYT",
            "terminal_type": "standalone",
            "status": "connected",
            "name": "Example",
            "settings": {
                "timeout": 30,
                "enable_standalone": false,
                "share_device": false
            },
            "terminal_info": {
                "make": "Castles",
                "model": "MP200",
                "revision": null,
                "serial": "000313162200473"
            },
            "terminal_config": {
                "enable_emv": false,
                "enable_debit_msr": false,
                "enable_tip_adjust": false,
                "enable_contactless": true
            }
        }
    ],
    "total": 1
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The max number of items returned in each result set (see limit request param).
offset	integer	The number of product categories skipped from the results.
data	array	An array of product categories matching the request.
Update Device

Endpoint

PUT /api/v2/paymentengine/devices/:devicekey:


Updates an existing device. Parameters are only updated if they are present in the body.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
 -X PUT \
 -H "Content-Type: application/json" \
 -d '{"name":"Testing","settings":{"share_device":true}}' \
 https://usaepay.com/api/v2/paymentengine/devices/sa_1BTTI5Yys0G3gVQa6beYxM4K0hhjC

Parameter Name	Type	Description
terminal_type	string	Type of terminal being registered, currently the only option is 'standalone'.
name	string	A name associated with the terminal. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.
settings		Device settings
terminal_config		Terminal configuration
Response

Example Response

{
  "type": "device",
  "key": "sa_WKwzyQawBG0RMy0XpDGFXb6pXA23r",
  "apikeyid": "ntC8nP31Moh0wtvYT",
  "terminal_type": "standalone",
  "status": "connected",
  "name": "Register 1",
  "settings": {
    "timeout": 30,
    "share_device": true,
    "enable_standalone": false,
    "sleep_battery_device": 5,
    "sleep_battery_display": 1,
    "sleep_powered_device": 0,
    "sleep_powered_display": 0
  },
  "terminal_info": {
    "make": "Castles",
    "model": "VEGA3000",
    "revision": "18043001-0055.00",
    "serial": "011118170300198",
    "key_pin": "FFFF5B04",
    "key_pan": "FF908A"
  },
  "capabilities": {
    "emv": true,
    "swipe": true,
    "contactless": true,
    "signature_capture": true,
    "printer": true,
    "pin": true,
    "sleep_battery": true,
    "sleep_power": true
  },
  "terminal_config": {
    "enable_emv": true,
    "enable_debit_msr": true,
    "enable_tip_adjust": true,
    "enable_contactless": true
  }
}

Parameter Name	Type	Description
type	string	Object type. This will always be device.
key	string	Unique device identifier
apikeyid	string	The id of API key (source key) associated with the device.
terminal_type	string	Terminal type: "standalone" for payment engine cloud based terminal.
status	string	Current device status
name	Developer assigned device name. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.	
settings		Device settings
terminal_info		Details of terminal
terminal_config		Terminal configuration
pairing_code	string	If terminal type is 'standalone', this is the pairing code required to pair the payment device with the payment engine.
expiration	string	If terminal type is 'standalone', the expiration is the date/time that the pairing code is no longer valid.
Update Device Settings

Endpoint

PUT /api/v2/paymentengine/devices/:devicekey:/settings


Updates the device settings. These settings change the behavior of the device. If a setting is not included in the request, it will not be updated.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
 -X POST \
 -H "Content-Type: application/json" \
 -d '{"share_device":true}' \
 https://usaepay.com/api/v2/paymentengine/devices/sa_p7wyCLbGgGtPyDE12U9zHJy3pg6WO/settings

Parameter Name	Type	Description
timeout		Transaction timeout, how long to wait for transaction authorization to complete.
enable_standalone		Allows transactions to be initiated from terminal (if supported).
share_device		If true, this allows the payment device to be used by other merchants. If false, only the merchant associated with the apikeyid may send transactions to device.
notify_update		If true, device will be notified on all future updates
notify_update_next		If true, device will be notified only on the next update. After notification, this is automatically set back to false.
sleep_battery_device		This is the amount of inactive time (in minutes) before the device enters full sleep if it is running on battery. If in full sleep, you will need to turn the device on again before processing. Wifi and Bluetooth pairing should persist, even after full sleep. Set to '0' to never sleep.
sleep_battery_display		This is the amount of inactive time (in minutes) before the device enters display sleep if it is running on battery. If in display sleep, just send a transaction to the device or tap any button to wake the device. Set to '0' to never sleep.
sleep_powered_device		This is the amount of inactive time (in minutes) before the device enters full sleep if it is plugged into power. If in full sleep, you will need to turn the device on again before processing. Wifi and Bluetooth pairing should persist, even after full sleep. Set to '0' to never sleep.
sleep_powered_display		This is the amount of inactive time (in minutes) before the device enters display sleep if it is plugged into power. If in display sleep, just send a transaction to the device or tap any button to wake the device. Set to '0' to never sleep.
Response

Example Response

{
  "type": "device",
  "key": "sa_WKwzyQawBG0RMy0XpDGFXb6pXA23r",
  "apikeyid": "ntC8nP31Moh0wtvYT",
  "terminal_type": "standalone",
  "status": "connected",
  "name": "Register 1",
  "settings": {
    "timeout": 30,
    "share_device": true,
    "enable_standalone": false,
    "sleep_battery_device": 5,
    "sleep_battery_display": 1,
    "sleep_powered_device": 0,
    "sleep_powered_display": 0
  },
  "terminal_info": {
    "make": "Castles",
    "model": "VEGA3000",
    "revision": "18043001-0055.00",
    "serial": "011118170300198",
    "key_pin": "FFFF5B04",
    "key_pan": "FF908A"
  },
  "capabilities": {
    "emv": true,
    "swipe": true,
    "contactless": true,
    "signature_capture": true,
    "printer": true,
    "pin": true,
    "sleep_battery": true,
    "sleep_power": true
  },
  "terminal_config": {
    "enable_emv": true,
    "enable_debit_msr": true,
    "enable_tip_adjust": true,
    "enable_contactless": true
  }
}

Parameter Name	Type	Description
type	string	Object type. This will always be device.
key	string	Unique device identifier
apikeyid	string	The id of API key (source key) associated with the device.
terminal_type	string	Terminal type: "standalone" for payment engine cloud based terminal.
status	string	Current device status
name	Developer assigned device name. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.	
settings		Device settings
terminal_info		Details of terminal
terminal_config		Terminal configuration
pairing_code	string	If terminal type is 'standalone', this is the pairing code required to pair the payment device with the payment engine.
expiration	string	If terminal type is 'standalone', the expiration is the date/time that the pairing code is no longer valid.
Update Device Terminal Config

Endpoint

PUT /api/v2/paymentengine/devices/:devicekey:/terminal-config


Updates the device settings. These settings change the behavior of the device. If a setting is not included in the request, it will not be updated.

Request

Example Request

curl --user APIKEY:PINHASH \
-X PUT \
-H "Content-Type: application/json" \
-d '{"enable_emv":false}' \
https://usaepay.com/api/v2/paymentengine/devices/sa_p7wyCLbGgGtPyDE12U9zHJy3pg6WO/terminal-config

Parameter Name	Type	Description
enable_emv		Enables EMV processing.
enable_debit_msr		Enables PIN debit for swiped transactions.
enable_tip_adjust		Allows EMV transaction amounts to be adjusted after authorization (to add tip). Disables PIN authentication.
enable_contactless		Enables NFC reader.
Response

Example Response

{
  "type": "device",
  "key": "sa_WKwzyQawBG0RMy0XpDGFXb6pXA23r",
  "apikeyid": "ntC8nP31Moh0wtvYT",
  "terminal_type": "standalone",
  "status": "connected",
  "name": "Register 1",
  "settings": {
    "timeout": 30,
    "share_device": true,
    "enable_standalone": false,
    "sleep_battery_device": 5,
    "sleep_battery_display": 1,
    "sleep_powered_device": 0,
    "sleep_powered_display": 0
  },
  "terminal_info": {
    "make": "Castles",
    "model": "VEGA3000",
    "revision": "18043001-0055.00",
    "serial": "011118170300198",
    "key_pin": "FFFF5B04",
    "key_pan": "FF908A"
  },
  "capabilities": {
    "emv": true,
    "swipe": true,
    "contactless": true,
    "signature_capture": true,
    "printer": true,
    "pin": true,
    "sleep_battery": true,
    "sleep_power": true
  },
  "terminal_config": {
    "enable_emv": true,
    "enable_debit_msr": true,
    "enable_tip_adjust": true,
    "enable_contactless": true
  }
}

Parameter Name	Type	Description
type	string	Object type. This will always be device.
key	string	Unique device identifier
apikeyid	string	The id of API key (source key) associated with the device.
terminal_type	string	Terminal type: "standalone" for payment engine cloud based terminal.
status	string	Current device status
name	Developer assigned device name. Device name can contain letters, numbers, spaces, and dashes. All other characters will be filtered out.	
settings		Device settings
terminal_info		Details of terminal
terminal_config		Terminal configuration
pairing_code	string	If terminal type is 'standalone', this is the pairing code required to pair the payment device with the payment engine.
expiration	string	If terminal type is 'standalone', the expiration is the date/time that the pairing code is no longer valid.
Delete Device

Endpoint

DELETE /api/v2/paymentengine/devices/:devicekey:


Removes terminal registration and un-registers the device referenced by devicekey. This will delete the device key and place the terminal back on the pairing code prompt screen.

Request

Example Request

curl --user APIKEY:PINHASH \
-X DELETE \
https://usaepay.com/api/v2/paymentengine/devices/sa_p7wyCLbGgGtPyDE12U9zHJy3pg6WO

Response

Example Response

{
    "status": "success"
}

Payment Request
api/v2/paymentengine/devices


Use the devices endpoint to register, remove, and manage payment terminals.

Start Request

Endpoint

POST /api/v2/paymentengine/payrequests


Starts a new payment request (transaction). This will cause the terminal to walk the customer through the payment processing screens:

Swipe/dip/tap card.
Choose credit/debit (if supported).
Enter PIN (if necessary).
Prompt for tip (if enabled).
Approve amount.
Capture signature (if enabled/supported).

A payment requestkey will be returned which can be used to track the customers progress on the terminal via GET /api/v2/paymentengine/payrequests/:requestkey:. When the process is complete, transaction details will be available.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
 -X POST \
 -H "Content-Type: application/json" \
 -d '{"devicekey":"sa_1BTTI5Yys0G3gVQa6beYxM4K0hhjC","command":"cc:sale","amount":"8.88"}' \
 https://usaepay.com/api/v2/paymentengine/payrequests



Parameters marked Required are required to process this type of transaction.

Parameter Name	Type	Description
devicekey	string	Device key. If device key is not specified, the device associated with current source key is used.
command	string	This must be set to sale for a credit or debit card sale (Required)
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
timeout		Time in seconds to wait for payment. Default is 180 seconds. See timeout handling below.
block_offline		By default, the payment engine will wait for an offline payment terminal to connect and then start the transaction, up to the timeout time limit. If "block_offline" is set to true, the payment request will return an error right away.
ignore_duplicate		Bypass duplicate detection/folding if it has been configured on the api key
save_card		Save card and return token
manual_key		If true, an option will be displayed to manually key the transaction.
prompt_tip		Customer will be prompted to leave a tip.
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
email	string	Customer's email address
custom_fields	array	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
lineitems	array	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_flow	comma delimited string	Allows you to customize the order in which each payment screen is displayed on the device. Below are the parameters that can be included in the comma delimited string of the custom_flow parameter. Each of these parameters represents a screen which appears on the payment device. The payment and result screens are required. All others are optional.
custom_options	array	A parameter for custom_flow: options. options must be listed FIRST in the "custom_flow" comma delimited string. See below for detail.
save_card	bool	Set to true to tokenize the card used to process the transaction.
Response

Example Response

{
  "type": "request",
  "key": "pr_3mW7rstrdA0Sp32LW9MN3djCITAhx",
  "expiration": "2016-07-08 16:40:48",
  "status": "sent to device"
}

Parameter Name	Type	Description
type	string	Object type. This will always be request.
key	string	Request key
expiration	string	The expiration is the date/time the request will expire and no longer be valid.
status	string	The status of the request.
Common Errors
Errorcode	Message	Troubleshooting
21121	Unknown devicekey xxxxxxx	Double check that the device key is valid. If the device status was still "waiting for device pairing," the request may have expired.
21143	Request failed (Payment terminal battery too low)	Charge the terminal before processing any more transactions. The terminal does not have the power to complete the transaction.
21144	Transaction request has timed out.	Retry transaction, and complete before time out allotted (default is 180 seconds). Lengthen
timeout
parameter if necessary.
21145	Device is currently offline.	Verify terminal is connected to payment engine and try again.
21146	Device is currently processing another transaction.	Two requests were sent simultaneously. Wait until the first request is complete and try again.

Example AddOn Request

{
  "amount": "52.10",
  "custom_options": [
    {
      "display": "Donate to Charity?",
      "denominations": [
        {
          "amount": "1.00",
          "display": "$1.00"
        },
        {
          "amount": "5.00",
          "display": "$5.00"
        },
        {
          "amount": "10.00",
          "display": "$10.00"
        },
        {
          "display": "Other",
          "amount_other": true
        }
      ],
      "type": "AddOn",
      "sku": "DONATION-12345"
    }
  ],
  "custom_flow": "options,amount,payment,result"
}


Example AddOn Response

{
  "type": "request",
  "status": "transaction complete",
  "transaction": {
    "type": "transaction",
    "auth_amount": "57.10",
    "lineitems": [
      {
        "sku": "subtotal",
        "cost": "52.10",
        "qty": 1
      },
      {
        "sku": "DONATION-12345",
        "cost": "5.00",
        "qty": 1
      }
    ],
    "complete": true
  }
}


Example Tip Request

{
  "amount": "15",
  "custom_options": [
    {
      "display": "Add a Tip?",
      "denominations": [
        {
          "amount": "2.25",
          "display": "$2.25"
        },
        {
          "amount": "2.70",
          "display": "$2.70"
        },
        {
          "amount": "3.00",
          "display": "$3.00"
        },
        {
          "display": "Other",
          "amount_other": true
        }
      ],
      "type": "Tip"
    }
  ],
  "custom_flow": "options,amount,payment,result"
}


Example Tip Response

{
  "type": "request",
  "status": "transaction complete",
  "transaction": {
    "type": "transaction",
    "auth_amount": "17.25",
    "amount_detail": {
      "tip": "2.25",
      "subtotal": "15.00"
    },
    "complete": true
  }
}

Custom Options

A parameter for custom_flow: options. options must be listed FIRST in the custom_flow comma delimited string. Custom options can be AddOn or Tip:

If AddOn option is selected it will be added to transaction as a lineitem and original amount will be a subtotal line item.

If Tip is selected amount will be added in amount_detail with the original amount as the subtotal.

Timeout Handling

Once a payment request is initiated, the payment engine does not require any further input from the point of sale software to complete the payment process. This can lead to confusion and potentially duplicate transactions if the point of sale software does not correctly poll the payment request. Consider the follow sequence of events:

User starts the transaction on the point of sale software.
The point of sale software starts polling the status of the request, but is configured to give up after 1 minute.
User takes longer than 1 minute to swipe card.
Payment engine successfully processes transaction, approval is shown on terminal.
User returns to point of sale and sees an error indicating that the request timed out.
User repeats the process and this time swipes the card in under a minute.

Duplicate transaction is run and two charges are now in the batch, despite the point of sale only acknowledging one.

By default the payment engine will wait for 3 minutes (180 seconds) for the user to swipe, dip or tap their card. This can be configured by passing the "timeout" parameter. To prevent the above, the point of sale software should continue to poll the payment request for the full amount of time set in the timeout. When timeout is exceeded, the payment request status will change to "timeout".

Retrieve Specific Request

Endpoint

GET /api/paymentengine/payrequests/:requestkey:


Gets the status of a pay requestkey.

Request

Example Request

curl --basic --user APIKEY:PINHASH \
 https://www-stage.usaepay.com/api/v2/paymentengine/payrequests/pr_TJtcst4SfmEbztWk6V4RmJL5HKMLO

Response

Example Response

{
  "type": "request",
  "key": "pr_3mW7rstrdA0Sp32LW9MN3djCITAhx",
  "expiration": "2016-07-08 16:40:48",
  "status": "sent to device"
}

Parameter Name	Type	Description
key	string	Unique request identifier
expiration	string	The expiration is the date/time the request will expire and no longer be valid.
status	string	The status of the request. Possible Values are:
sending to device
sent to device
waiting for card dip
changing interfaces
customer see phone and tap again
processing payment
completing payment
capturing signature
signature capture error
transaction complete
canceled
transaction canceled
transaction failed
timeout
error

transaction	object	Once the transaction has been processed, a transaction object will be added to the result.
complete	string	Confirms if transaction is complete. Value will be: true or false. Please Note: A transaction object could be included in the response (signaling the transaction has been processed), and also receive complete:false. For example, if a custom_flow) is set and signature_required is included, the flag will show as
complete:false
until the signature has been captured.
Retrieve Specific Request

Endpoint

DELETE /api/paymentengine/payrequests/:requestkey:


Deletes/cancels a payment request.

Request

Example Request

curl --user APIKEY:PINHASH \
-X DELETE \
https://usaepay.com/api/v2/paymentengine/payrequests/pr_3mW7rstrdA0Sp32LW9MN3djCITAhx

Response

Example Response

{
    "status": "success"
}

Customer Endpoints
Customers

Customer Endpoint

/api/v2/customers


Use the customers endpoint to create, delete, and manage customer information.

Create Customer

Endpoint

POST /api/v2/customers


This method allows you to create a customer. This is a basic customer record, the other sections below will show how to create Customer Payment Methods and Recurring Billing Schedules.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
     '{
  "company": "Company Men Inc",
  "first_name": "Robert",
  "last_name": "Durst",
  "customerid": "234545",
  "street": "1222 Verdugo Cir",
  "street2": "#303",
  "city": "Los Angeles",
  "state": "CA",
  "postalcode": 90038,
  "country": "USA",
  "phone": "8888888888",
  "fax": "7777777777",
  "email": "johndoe@anon.com",
  "url": "www.google.com",
  "notes": "Signed up during January 2018 Promotion",
  "description": "Gold Level Customer"
}'


Parameters in marked Required are required to process this type of transaction.

Parameter	Type	Description
company	string	Company or Organization Name (Required Only required if first_name AND last_name are not provided.)
first_name	string	First name associated with the customer (Required- Only required if company is not provided.)
last_name	string	Last name associated with the customer (Required- Only required if company is not provided.)
customerid	string	Merchant assigned customer identifier.
street	string	Primary street number/address information. (i.e. 1234 Main Street)
street2	string	Additional address information such as apartment number, building number, suite information, etc.
city	string	Billing City
state	string	Two-letter State abbreviation or full state name.
postalcode	integer	postalcode code
country	string	Three-letter country code. See full list here
phone	string	The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
fax	string	The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
email	string	Email contact for customer.
url	string	Customer's website.
notes	string	Merchant notes about this customer.
description	string	Customer description.
Response Parameters

Example Response

{
  "key": "6sddgs2m179ykp7b",
  "type": "customer",
  "customerid": "234545",
  "custid": "10450020",
  "company": "Company Men Inc",
  "first_name": "Robert",
  "last_name": "Durst",
  "street": "1222 Verdugo Cir",
  "street2": "#303",
  "city": "Los Angeles",
  "state": "CA",
  "postalcode": "90038",
  "country": "USA",
  "phone": "8888888888",
  "fax": "7777777777",
  "email": "johndoe@anon.com",
  "url": "www.google.com",
  "notes": "Signed up during January 2018 Promotion",
  "description": "Gold Level Customer",
  "payment_methods": null,
  "billing_schedules": null
}

Parameter Name	Type	Description
key	string	Gateway generated customer identifier.
type	string	The object type. Successful calls will always return "customer".
customerid	string	Merchant assigned customer identifier.
custid	string	Gateway assigned customer identifier. This was originally used in SOAP API
company	string	Company or Organization Name
first_name	string	First name associated with the customer
last_name	string	Last name associated with the customer
street	string	Primary street number/address information. (i.e. 1234 Main Street)
street2	string	Additional address information such as apartment number, building number, suite information, etc.
city	string	Billing City
state	string	Two-letter State abbreviation or full state name.
postalcode	integer	postalcode code
country	string	Three-letter country code. See full list here
phone	string	The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
fax	string	The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
email	string	Email contact for customer.
url	string	Customer's website.
notes	string	Merchant notes about this customer.
description	string	Customer description.
billing_schedules	array	Array of customer billing schedules.
payment_methods	array	Array of customer payment methods.
Create Customer From Transaction

This method allows you to create a customer using an existing transaction record.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
     '{
  "transaction_key": "5nf0c9trt2gckmh"
}'


Parameters in marked Required are required to process this type of transaction.

Parameter	Type	Description
transaction_key	string	The transaction key you would like to reference to create the customer record. (Required)
Retrieve Specific Customer

Endpoint

GET api/v2/customers/:custkey:


This endpoint returns the customer details for a specific customer.

Request Parameters

Example Request

curl -X GET https://secure.usaepay.com/api/v2/customers/fsddgkr534kt7pvc
  -H "Content-Type: application/json"
  -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"


Parameters in marked Required are required to process this type of transaction.

Parameter	Required	Type	Description
custkey	string	Customer identifier generated by gateway. This is passed in through the endpoint. (Required)	
Response Parameters

Example Response

{
  "key": "fsddgkr534kt7pvc",
  "type": "customer",
  "customerid": "234545",
  "custid": "74183319",
  "company": "Company 1",
  "first_name": "Mark",
  "last_name": "Hoppus",
  "street": "123 Blink St",
  "street2": "Apt 182",
  "city": "Anytown",
  "state": "CA",
  "postalcode": "12345",
  "country": "USA",
  "phone": "555-555-5551",
  "fax": "777-777-7771",
  "email": "email@email.com",
  "url": "www.website.com",
  "notes": "I miss you",
  "description": "this is for your schedule",
  "billing_schedules": [
    {
      "key": "an0mtryxp2d6mw9g9",
      "type": "billingschedule",
      "paymethod_key": "4n02pccv6tcmd8ky8",
      "method_name": "Travis Barker-1111",
      "amount": "10.00",
      "currency_code": "0",
      "description": "this is for your schedule",
      "enabled": "1",
      "frequency": "weekly",
      "next_date": "2018-10-22",
      "numleft": "-1",
      "orderid": "12",
      "receipt_note": "Thank you for your business! ",
      "send_receipt": "1",
      "source": "0",
      "start_date": "2017-10-12",
      "tax": "2.00",
      "user": "",
      "username": "",
      "skip_count": "1",
      "rules": [
        {
          "key": "hn0vr74x82nj7tntb",
          "type": "billingschedulerule",
          "day_offset": "0",
          "month_offset": "0",
          "subject": "mon"
        }
      ]
    }
  ],
  "payment_methods": [
    {
      "key": "4n02pccv6tcmd8ky8",
      "type": "customerpaymentmethod",
      "method_name": "Travis Barker-1111",
      "expires": "2019-09-01",
      "card_type": "V",
      "ccnum4last": "1111",
      "avs_street": "123 Main St",
      "avs_postalcode": "90005",
      "added": "2018-08-29 16:40:24",
      "updated": "2018-08-29 16:40:24"
    },
    {
      "key": "9n02pcf95294xrgtk",
      "type": "customerpaymentmethod",
      "method_name": "Mastercard",
      "expires": "2022-09-30",
      "card_type": "M",
      "ccnum4last": "2275",
      "avs_street": "1236 Main",
      "avs_postalcode": "12345",
      "added": "2018-07-12 12:03:01",
      "updated": "2018-07-12 12:03:01"
    }
  ]
}

Parameter Name	Type	Description
key	string	Gateway generated customer identifier.
type	string	The object type. Successful calls will always return "customer".
customerid	string	Merchant assigned customer identifier.
custid	string	Gateway assigned customer identifier. This was originally used in SOAP API
company	string	Company or Organization Name
first_name	string	First name associated with the customer
last_name	string	Last name associated with the customer
street	string	Primary street number/address information. (i.e. 1234 Main Street)
street2	string	Additional address information such as apartment number, building number, suite information, etc.
city	string	Billing City
state	string	Two-letter State abbreviation or full state name.
postalcode	integer	postalcode code
country	string	Three-letter country code. See full list here
phone	string	The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
fax	string	The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
email	string	Email contact for customer.
url	string	Customer's website.
notes	string	Merchant notes about this customer.
description	string	Customer description.
billing_schedules	array	Array of customer billing schedules.
payment_methods	array	Array of customer payment methods.
Retrieve Customer List

Endpoint

GET api/v2/customers


This endpoint retrieves a list of customers on the merchants account. You can set the offset and limits for results.

Request Parameters

Example Request

curl -X GET https://secure.usaepay.com/api/v2/customers?limit=2&offset=5
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
Response Parameters

Example Response

{
  "type": "list",
  "limit": 2,
  "offset": 5,
  "data": [
    {
      "key": "ebrnm62q29gjdm7k",
      "type": "customer",
      "customerid": "10202212",
      "custid": "78625496",
      "company": "Company Men Inc",
      "first_name": "Robert",
      "last_name": "Durst",
      "street": "1222 Verdugo Cir",
      "street2": "#303",
      "city": "Los Angeles",
      "state": "CA",
      "postalcode": "90038",
      "country": "USA",
      "phone": "8888888888",
      "fax": "7777777777",
      "email": "johndoe@anon.com",
      "url": "www.google.com",
      "notes": "Signed up during January 2018 Promotion",
      "description": "Gold Level Customer"
    },
    {
      "key": "isddgz0505vcjv20",
      "type": "customer",
      "custid": "74183322",
      "customerid": "",
      "company": "Company 2",
      "first_name": "Travis",
      "last_name": "Barker",
      "street": "789 Rock Blvd",
      "street2": "",
      "city": "Anywhere",
      "state": "NY",
      "postalcode": "12345",
      "country": "USA",
      "phone": "555-555-5553",
      "fax": "777-777-7773",
      "email": "email@email.com",
      "url": "www.website.com",
      "notes": "all the small things",
      "description": "this is for your schedule"
    }
  ],
  "total": "7"
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of customers that will be included in response.
offset	integer	The number of customers skipped from the results.
data	array	An array of customers matching the request.
total	integer	The total amount of customers, including filtered results.
Update Customer

Endpoint

PUT api/v2/customers/:custkey:


This endpoint updates already existing customers. Only fields that are passed through will be updated. If you do not wish to update a field, do not pass it in the request. If you pass in a field, but leave the content blank, content will be removed.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers/gbrnkzymbn2g9y3p
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
     '{
  "company": "Company Men Inc",
  "first_name": "Robert",
  "last_name": "",
  "customerid": "234545",
  "street": "1222 Verdugo Cir",
  "street2": "#303"
}'


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. This is the only required field. (Required)
company	string	Company or Organization Name
first_name	string	First name associated with the customer
last_name	string	Last name associated with the customer
custid	string	Gateway assigned customer identifier. This was originally used in SOAP API
customerid	string	Merchant assigned customer identifier.
street	string	Primary street number/address information. (i.e. 1234 Main Street)
street2	string	Additional address information such as apartment number, building number, suite information, etc.
city	string	Billing City
state	string	Two-letter State abbreviation or full state name.
postalcode	integer	postalcode code
country	string	Three-letter country code. See full list here
phone	string	The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
fax	string	The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
email	string	Email contact for customer.
url	string	Customer's website.
notes	string	Merchant notes about this customer.
description	string	Customer description.
billing_schedules	array	Array of customer billing schedules.
payment_methods	array	Array of customer payment methods.
Response Parameters

Example Response

{
  "key": "gbrnkzymbn2g9y3p",
  "type": "customer",
  "custid": "75440446",
  "customerid": "234545",
  "company": "Company Men Inc",
  "first_name": "Robert",
  "last_name": "",
  "street": "1222 Verdugo Cir",
  "street2": "#303",
  "city": "London",
  "state": "CA",
  "postalcode": "WC2N5DU",
  "country": "UK",
  "phone": "555-867-5309",
  "fax": "555-329-6363",
  "email": "minister@mom.spell",
  "url": "http://harrypotter.wikia.com/wiki/Cornelius_Fudge",
  "notes": "Signed up during January 2018 Promotion",
  "description": "Gold Level Customer",
  "payment_methods": null,
  "billing_schedules": null
}

Parameter Name	Type	Description
key	string	Gateway generated customer identifier.
type	string	The object type. Successful calls will always return "customer".
customerid	string	Merchant assigned customer identifier.
custid	string	Gateway assigned customer identifier. This was originally used in SOAP API
company	string	Company or Organization Name
first_name	string	First name associated with the customer
last_name	string	Last name associated with the customer
street	string	Primary street number/address information. (i.e. 1234 Main Street)
street2	string	Additional address information such as apartment number, building number, suite information, etc.
city	string	Billing City
state	string	Two-letter State abbreviation or full state name.
postalcode	integer	postalcode code
country	string	Three-letter country code. See full list here
phone	string	The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
fax	string	The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted.
email	string	Email contact for customer.
url	string	Customer's website.
notes	string	Merchant notes about this customer.
description	string	Customer description.
billing_schedules	array	Array of customer billing schedules.
payment_methods	array	Array of customer payment methods.
Delete Specific Customer

Endpoint

DELETE /api/v2/customers/:custkey:


Use this endpoint to delete a specific customer record.

Request Parameters

Example Request

curl -X DELETE https://secure.usaepay.com/api/v2/customers/1sddg1fyqctgwktj
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvNGVlNDI5YWViMDMxNjc5N2ZkY2FhOGY0ZDU1ODRjNDgvYTkzNTU5MDhjMzg0NmUyNmMzMDVhOTcyOWY2NmI1NmM0YjJiNmI0ZmVlY2FjOTAzODgxYTY5YTNjYjYwOWI2Yg=="


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
custkey	string	Unique customer identifier generated by gateway. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If customers have been deleted then status will be returned as success. If customer is NOT deleted, an error will be returned instead.
Common Errors
Code	Message	Description
99999	Customer Not Found	The customer key is incorrect.
21003	Access Denied	You do not have the permission to perform this action.
Bulk Delete Customers

Endpoint

DELETE /api/v2/customers/bulk


Use this endpoint to delete multiple customers at once.

Request Parameters

Example Request

curl -X DELETE https://secure.usaepay.com/api/v2/customers/bulk
    -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNGM5MDUyODVmMmMzMjc1ZWM4NDUxYTc3MzI1M2VkNGIvZmVhZDE4Yzk0ZWNlZjQzMjc1ZDFmOGMxYTQ1NGY2YTIwODBmYzI3ZTdjM2I2NTAxNDAxNWI4YmFmODVlYmY5ZQ=="
    -H "Content-Type: application/json"
    -d
    '{"keys":
        ["csddp28yy83q4w80",
        "bsddx6sf00dp5r82",
        "asddnfjdh8qc80th",
        "9sddgdhgw0d133br"
        ]
    }'


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
keys	array	List of unique customer identifiers you wish to delete. (Required)
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If customers have been deleted then status will be returned as success. If customer is NOT deleted, an error will be returned instead.
Common Errors
Code	Message	Description
99999	Customer Not Found	The customer key is incorrect.
21003	Access Denied	You do not have the permission to perform this action.
Payment Methods
api/v2/customers/:custkey:/payment_methods


Use the customer payment methods endpoint to create, delete, and manage customer payment method information.

Create Method

Endpoint

POST api/v2/customers/:custkey:/payment_methods


This is the endpoint to create payment methods for existing customers. Below you will find these create payment method examples:

Credit Card Method
Check Method
GiftCard Method
Save Method Used in Transaction
Multiple Methods - This example shows how to add multiple payment methods at once.
Credit Card Method
Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
  '[
    {
      "method_name": "Minerva McGonagall",
      "cardholder": "Hogwarts Card",
      "number": "4000100011112224",
      "expiration": "0922",
      "avs_street": "123 Grindy St",
      "avs_postalcode": "86577",
      "pay_type": "cc",
      "default": true,
      "sortord": 1
    }
  ]'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
method_name	string	Payment nickname.
cardholder	string	Name on card for credit cards. Will default to customer name if not included in request.
number	string	Credit/debit card number. Required
expiration	string	Credit/debit card expiration date. Peferred format is MMYY. Required
avs_street	string	Street address for address verification.
avs_postalcode	string	Postal (Zip) code for address verification.
pay_type	string	Payment method type. cc for this request type. All possible values:
cc= credit/debit card
check = bank account
giftcard = merchant closed loop giftcards
transaction = Reference a transaction key to save the check or credit card method to pay the transaction
Required
default	bool	Set to true to set as default payment. Defaults to false if not included in request.
sortord	integer	
Response Parameters

Example Response

{
    "key": "ln02pc6vxy98c79cs",
    "type": "customerpaymentmethod",
    "method_name": "Hogwarts Card",
    "expires": "2022-09-30",
    "card_type": "V",
    "ccnum4last": "2224",
    "avs_street": "123 Grindy St",
    "avs_postalcode": "86577",
    "sortord": 1
    "added": "2019-01-04 14:54:22",
    "updated": "2019-01-04 14:54:22"
}

    {
    "key": "x8KccrxeydHJ4MmT",
    "type": "customerpaymentmethod",
    "method_name": "Example method",
    "cardholder": "Testor Jones",
    "expiration": "0426",
    "ccnum4last": "xxxxxxxxxxxxxx7779",
    "card_type": "Visa"
    }
]

Parameter Name	Type	Description
key	string	Gateway generated customer payment method identifier.
type	string	The object type. Successful calls will always return "customerpaymentmethod".
cardholder	string	Name on card for credit cards or bank account for ACH payment methods.
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
expires	string	Expiration date for credit/debit card.
ccnum4last	string	Last 4 digits of credit/debit card.
card_type	string	Card brand type (Visa, MasterCard, Discover, etc.)
avs_street	string	Street address for address verification
avs_postalcode	string	Postal (Zip) code for address verification
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
Check Method
Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
  '[
    {
      "method_name": "Gringotts Checking",
      "cardholder": "Albus Dumbledore",
      "account": "46153252224",
      "routing": "026009593",
      "account_type": "checking",
      "account_format": "CCD",
      "pay_type": "check",
      "dl": "8465468",
      "dl_state": "CA",
      "default": false,
      "sortord": 2
    }
  ]'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
cardholder	string	Bank account name for this request type.
routing	string	ACH routing number. Required
account	string	Bank account number. Required
account_type	string	Bank account type. Possible values are: checking or savings. Required
account_format	string	Account file format. Click here to see possible formats and descriptions.
pay_type	string	Payment method type. check for this request type. All possible values:
cc= credit/debit card
check = bank account
giftcard = merchant closed loop giftcards
transaction = Reference a transaction key to save the check or credit card method to pay the transaction

dl	string	Driver's license number for bank account holder.
dl_state	string	State for driver's license number for bank account holder.
default	bool	Set to true to set as default payment. Defaults to false if not included in request.
sortord	integer	
Response Parameters

Example Response

{
    "key": "on02x3rbwshz23nhb",
    "type": "customerpaymentmethod",
    "method_name": "Gringotts Checking",
    "expires": "0000-00-00",
    "sortord": "2",
    "added": "2019-01-04 14:54:22",
    "updated": "2019-01-04 14:54:22"
}

Parameter Name	Type	Description
key	string	Gateway generated customer payment method identifier.
type	string	The object type. Successful calls will always return customerpaymentmethod.
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
expires	string	Check payment method types will always return 0000-00-00.
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
GiftCard Method

This request allows you to add closed loop giftcards. Click here for more general information about giftcards.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
  '[
    {
      "method_name": "BlottCash",
      "number": "986451461346146",
      "pay_type": "giftcard",
      "default": false,
      "sortord": 3
    }
  ]'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
method_name	string	Payment nickname.
number	string	Giftcard number.
pay_type	string	Payment method type. giftcard for this request type. All possible values:
cc= credit/debit card
check = bank account
giftcard = merchant closed loop giftcards
transaction = Reference a transaction key to save the check or credit card method to pay the transaction

default	bool	Set to true to set as default payment. Defaults to false if not included in request.
sortord	integer	
Response Parameters

Example Response

{
  "key": "1n02pc6j72j3hncvb",
  "type": "customerpaymentmethod",
  "method_name": "BlottCash",
  "expires": "0000-00-00",
  "ccnum4last": "8586",
  "sortord": "2",
  "added": "2019-01-04 14:54:22",
  "updated": "2019-01-04 14:54:22"
}

Parameter Name	Type	Description
key	string	Gateway generated customer payment method identifier.
type	string	Object type. This will always be customerpaymentmethod.
method_name	string	Payment nickname.
expires	string	Giftcard payment method types will always return 0000-00-00.
ccnum4last	string	Last 4 digits of credit/debit card or giftcard.
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
Transaction Method

This method allows you to save a payment method from a certain transaction to a customer's account.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
  '[
    {
        "method_name": "Hogwarts Card",
        "default": true,
        "sortord": 5,
        "pay_type": "transaction",
        "transaction_key": "bnftj5zrs68x3cb"
    }
    ]'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
method_name	string	Payment nickname.
pay_type	string	Payment method type. transaction for this request type. All possible values:
cc= credit/debit card
check = bank account
giftcard = merchant closed loop giftcards
transaction = Reference a transaction key to save the check or credit card method to pay the transaction
Required
default	bool	Set to true to set as default payment. Defaults to false if not included in request.
sortord	integer	
Response Parameters

Example Response

[
    {
        "key": "5n02pc67qvzzkmv68",
        "type": "customerpaymentmethod",
        "method_name": "Hogwarts Card",
        "expires": "2019-09-01",
        "card_type": "V",
        "ccnum4last": "2222",
        "sortord": "5",
        "added": "2019-07-03 16:17:55",
        "updated": "2019-07-03 16:17:55"
    }
]

Parameter Name	Type	Description
key	string	Gateway generated customer payment method identifier.
type	string	The object type. Successful calls will always return "customerpaymentmethod".
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
expires	string	Expiration date for credit/debit card.
ccnum4last	string	Last 4 digits of credit/debit card.
card_type	string	Card brand type (Visa, MasterCard, Discover, etc.)
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
Multiple Methods

You can also add multiple payment methods to a customer all at once, by including the multiple payment payment methods in an array.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
  '[
    {
      "method_name": "Minerva McGonagall",
      "cardholder": "Hogwarts Card",
      "number": "4000100011112224",
      "expiration": "0922",
      "avs_street": "1234 Main",
      "avs_postalcode": "12345",
      "pay_type": "cc",
      "default": true,
      "sortord": 1
    },
    {
      "method_name": "Gringotts Checking",
      "cardholder": "Hogwarts Card",
      "account": "46153252224",
      "routing": "026009593",
      "account_type": "checking",
      "account_format": "CCD",
      "pay_type": "check",
      "dl": "8465468",
      "dl_state": "CA",
      "default": false,
      "sortord": 2
    },
    {
      "method_name": "BlottCash",
      "number": "986451461346146",
      "pay_type": "giftcard",
      "default": false,
      "sortord": 3
    }
  ]'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
method_name	string	Payment nickname.
cardholder	string	Name on card for credit cards. Name on bank account for ACH bank accounts. Will default to customer name if not included in request.
number	string	Credit/debit card number or giftcard number for giftcard method. Required for cc and giftcard methods.
expires	string	Credit/debit card expiration date. Preferred format is MMYY. Required for cc method.
avs_street	string	Street address for address verification.
avs_postalcode	string	Postal (Zip) code for address verification.
routing	string	ACH routing number. Required for check method.
account	string	Bank account number. Required for check method.
account_type	string	Bank account type. Possible values are: checking or savings. Required for check method.
account_format	string	Account file format. Click here to see possible formats and descriptions.
pay_type	string	Payment method type. cc for this request type. All possible values:
cc= credit/debit card
check = bank account
giftcard = merchant closed loop giftcards
transaction = Reference a transaction key to save the check or credit card method to pay the transaction
Required
dl	string	Driver's license number for bank account holder.
dl_state	string	State for driver's license number for bank account holder.
default	bool	Set to true to set as default payment. Defaults to false if not included in request.
sortord	integer	
Response Parameters

Example Response

[
  {
      "key": "1n02pc6j72j3hncvb",
      "type": "customerpaymentmethod",
      "method_name": "BlottCash",
      "expires": "0000-00-00",
      "ccnum4last": "8586",
      "sortord": "2",
      "added": "2019-01-04 14:54:22",
      "updated": "2019-01-04 14:54:22"
  },
  {
      "key": "on02x3rbwshz23nhb",
      "type": "customerpaymentmethod",
      "method_name": "Gringotts Checking",
      "expires": "0000-00-00",
      "sortord": "1",
      "added": "2019-01-04 14:54:22",
      "updated": "2019-01-04 14:54:22"
  },
  {
      "key": "ln02pc6vxy98c79cs",
      "type": "customerpaymentmethod",
      "method_name": "Hogwarts Card",
      "expires": "2022-09-30",
      "card_type": "V",
      "ccnum4last": "1111",
      "avs_street": "123 Grindy St",
      "avs_postalcode": "86577",
      "added": "2019-01-04 14:54:22",
      "updated": "2019-01-04 14:54:22"
  }
]


This response will return an array of payment method objects that were added to the customer's profile.

Parameter Name	Type	Description
key	string	Gateway generated customer payment method identifier.
type	string	The object type. Successful calls will always return customerpaymentmethod.
cardholder	string	Name on card for credit cards or name on bank account for ACH payment methods.
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
expires	string	Expiration date for credit/debit card.
ccnum4last	string	Last 4 digits of credit/debit card.
card_type	string	Card brand type (Visa, MasterCard, Discover, etc.)
avs_street	string	Street address for address verification
avs_postalcode	string	Postal (Zip) code for address verification
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
Retrieve Specific Method

Endpoint

GET api/v2/customers/:custkey:/payment_methods/:methodkey:


This endpoint retrieves a specific customer payment method. You can set the offset and limits for results.

Request Parameters

Example Request

curl -X GET https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods/ln02pc6vxy98c79cs
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
methodkey	string	Gateway generated customer payment method identifier. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "key": "ln02pc6vxy98c79cs",
    "type": "customerpaymentmethod",
    "method_name": "Hogwarts Card",
    "expires": "2022-09-30",
    "card_type": "V",
    "ccnum4last": "1111",
    "avs_street": "123 Grindy St",
    "avs_postalcode": "86577",
    "added": "2019-01-04 14:54:22",
    "updated": "2019-01-04 14:54:22"
}

Parameter Name	Type	Description
key	string	Gateway generated customer payment method identifier.
type	string	The object type. Successful calls will always return "customerpaymentmethod".
cardholder	string	Name on card for credit cards or bank account for ACH payment methods.
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
expires	string	Expiration date for credit/debit card.
ccnum4last	string	Last 4 digits of credit/debit card.
card_type	string	Card brand type (Visa, MasterCard, Discover, etc.)
avs_street	string	Street address for address verification
avs_postalcode	string	Postal (Zip) code for address verification
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
Retrieve Method List

Endpoint

GET api/v2/customers/:custkey:/payment_methods


This endpoint retrieves a list of payment methods associated with a specific customer. You can set the offset and limits for results.

Request Parameters

Example Request

curl -X GET https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
Response Parameters

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "key": "1n02pc6j72j3hncvb",
            "type": "customerpaymentmethod",
            "method_name": "BlottCash",
            "expires": "0000-00-00",
            "ccnum4last": "8586",
            "sortord": "2",
            "added": "2019-01-04 14:54:22",
            "updated": "2019-01-04 14:54:22"
        },
        {
            "key": "on02x3rbwshz23nhb",
            "type": "customerpaymentmethod",
            "method_name": "Gringotts Checking",
            "expires": "0000-00-00",
            "sortord": "1",
            "added": "2019-01-04 14:54:22",
            "updated": "2019-01-04 14:54:22"
        },
        {
            "key": "ln02pc6vxy98c79cs",
            "type": "customerpaymentmethod",
            "method_name": "Hogwarts Card",
            "expires": "2022-09-30",
            "card_type": "V",
            "ccnum4last": "1111",
            "avs_street": "123 Grindy St",
            "avs_postalcode": "86577",
            "added": "2019-01-04 14:54:22",
            "updated": "2019-01-04 14:54:22"
        }
    ],
    "total": "3"
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of payment methods that will be included in response.
offset	integer	The number of payment methods skipped from the results.
data	array	An array of methods matching the search.
total	integer	The total amount of methods, including filtering results.
Update Method

Endpoint

PUT api/v2/customers/:custkey:/payment_methods/:methodkey:


This endpoint updates already existing customer payment methods. Only fields that are passed through will be updated. If you do not wish to update a field, do not pass it in the request. If you pass in a field, but leave the content blank, content will be removed. The fields listed in the Request Parameters section of this method are the only fields available for update. To change other fields, delete this payment method and add a new one.

Request Parameters

Example Request

curl -X PUT https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods/ln02pc6vxy98c79cs
  -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvM2MwYzk1MmI2ZmI3OTk4ODYxZTJmY2U1NDAzNjI1MzgvOGY1MzFkOTIxYmNmZWNjZDBlYmEwYzA4NWU1ODFiNDEyMWNmZjFiNzAwMGZiYjFhMGIzNWY3YWZkMThiYThiNA=="
  -H "Content-Type: application/json"
  -d
  '{  
    "method_name": "Hogsmeade Card",
    "avs_street": "789 Low Dr",
    "avs_postalcode": "90005"
  }'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
methodkey	string	Gateway generated customer payment method identifier. This is passed in through the endpoint. (Required)
method_name	string	Payment nickname.
expires	string	Expiration date for credit/debit card.
avs_street	string	Street address for address verification
avs_postalcode	string	Postal (Zip) code for address verification
Response Parameters

Example Response

{
    "key": "ln02pc6vxy98c79cs",
    "type": "customerpaymentmethod",
    "method_name": "Hogsmeade Card",
    "expires": "2022-09-30",
    "card_type": "V",
    "ccnum4last": "1111",
    "avs_street": "789 Low Dr",
    "avs_postalcode": "90005",
    "added": "2019-01-04 14:54:22",
    "updated": "2019-01-04 16:25:52"
}

Parameter Name	Type	Description
Parameter Name	Type	Description
---	---	---
key	string	Gateway generated customer payment method identifier.
type	string	The object type. Successful calls will always return "customerpaymentmethod".
cardholder	string	Name on card for credit cards or bank account for ACH payment methods.
method_name	string	Payment nickname. Will not be sent to processor unless cardholder field is left blank.
expires	string	Expiration date for credit/debit card.
ccnum4last	string	Last 4 digits of credit/debit card.
card_type	string	Card brand type (Visa, MasterCard, Discover, etc.)
avs_street	string	Street address for address verification
avs_postalcode	string	Postal (Zip) code for address verification
sortord	integer	
added	string	Date and time the payment method was added to the customer. Format is YYY-MM-DD HH:MM:SS.
updated	string	Date and time the payment method was last updated. Format is YYY-MM-DD HH:MM:SS.
Delete Specific Method

Endpoint

DELETE api/v2/customers/:custkey:/payment_methods/:methodkey:


Use this endpoint to delete a specific customer payment method.

Request Parameters

Example Request

curl -X DELETE https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods/fn02pc6jpfwzdnc7b
  -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNDAxMmFlZTI0MDdhYzMwMjQ1NGE2MzI1Y2ExODRlMjMvMmU3ODM4MzZkM2EwNTI2ZDExYmI1MGNjZDMxZmQwMTc1M2RlYmUxZTMzY2JmNTkyMmQ3MDM4Mzg5ZGMzMGMyZA=="
  -H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
methodkey	string	Gateway generated customer payment method identifier. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If customer payment method has been deleted then status will be returned as success. If payment method is NOT deleted, an error will be returned instead.
Bulk Delete Methods

Endpoint

DELETE /api/v2/customers/:custkey:/payment_methods/bulk


Use this endpoint to delete multiple customer payment methods at once.

Example Request

curl -X DELETE https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/payment_methods/bulk
  -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNDAxMmFlZTI0MDdhYzMwMjQ1NGE2MzI1Y2ExODRlMjMvMmU3ODM4MzZkM2EwNTI2ZDExYmI1MGNjZDMxZmQwMTc1M2RlYmUxZTMzY2JmNTkyMmQ3MDM4Mzg5ZGMzMGMyZA=="
  -H "Content-Type: application/json"
  -d
  '{"keys":
    ["fn02pc6jpfwzdnc7b",
    "in02pc6td1rj7824s",
    "cn02p9gn12k652wgb"
    ]
  }'

Request Parameters

Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
keys	array	Unique payment method identifiers generated by gateway. (Required)
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If customer payment methods have been deleted then status will be returned as success. If payment method is NOT deleted, an error will be returned instead.
Recurring Schedules
api/v2/customers/:custkey:/billing_schedules


Use this endpoint to create, delete, and update customer recurring billing schedules. You will also be able to enable and disable existing schedules.

Create Schedules

Endpoint

POST api/v2/customers/:custkey:/billing_schedules


Use this endpoint to create billing schedules for existing customers. There are examples for adding

Example Request - Single Schedule

curl -X POST https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/billing_schedules
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZDBkYmRkMTE5MjU5OGE3MmY4Mjg5NDVlZTFjMjJlNTMvNTRlNjgzOWQ0MGVmOTc0NDk2YjY2MmJkMWJkNzMwOWFkZTdhMDU1M2EzZjY3YjI0ZDMyMDE4NTNkNDQxNjc3NQ=="
-H "Content-Type: application/json"
-d
'[
  {
    "amount": "5.00",
    "currency_code": "0",
    "paymethod_key": "8n02pc6tg9whk5j67",
    "description": "Cockroach Clusters",
    "enabled": true,
    "frequency": "monthly",
    "next_date": "2019-01-31",
    "numleft": "12",
    "orderid": "76567898",
    "receipt_note": "So happy we could provide you with these gross gross candies!",
    "send_receipt": true,
    "source": "",
    "start_date": "2019-02-01",
    "tax": "1.00",
    "user": "33956",
    "username": "hgranger",
    "skip_count": "2",
    "rules": [
      {
        "day_offset": "1",
        "month_offset": "0",
        "subject": "Day"
      }
    ]
  }
]'


Example Request - Multiple Schedules

curl -X POST https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/billing_schedules
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZDBkYmRkMTE5MjU5OGE3MmY4Mjg5NDVlZTFjMjJlNTMvNTRlNjgzOWQ0MGVmOTc0NDk2YjY2MmJkMWJkNzMwOWFkZTdhMDU1M2EzZjY3YjI0ZDMyMDE4NTNkNDQxNjc3NQ=="
-H "Content-Type: application/json"
-d
'[
  {
    "paymethod_key": "8n02pc6tg9whk5j67",
    "amount": "15.00",
    "currency_code": "0",
    "description": "Lemon Drops",
    "enabled": true,
    "frequency": "weekly",
    "next_date": "2019-01-09",
    "numleft": "-1",
    "orderid": "12356",
    "receipt_note": "So happy we could provide you with these weird weird muggle candies!",
    "send_receipt": true,
    "source": "",
    "start_date": "2019-01-09",
    "tax": "2.00",
    "user": "",
    "username": "",
    "skip_count": "1",
    "rules": [
      {
        "day_offset": "0",
        "month_offset": "0",
        "subject": "wed"
      }
    ]
  },
  {
    "paymethod_key": "6n02p9w1sswpgwxyh",
    "amount": "5.00",
    "currency_code": "0",
    "description": "Cockroach Clusters",
    "enabled": true,
    "frequency": "monthly",
    "next_date": "2019-02-01",
    "numleft": "12",
    "orderid": "76567898",
    "receipt_note": "So happy we could provide you with these gross gross candies!",
    "send_receipt": true,
    "source": "",
    "start_date": "2019-01-31",
    "tax": "1.00",
    "user": "33956",
    "username": "hgranger",
    "skip_count": "1",
    "rules": [
      {
        "day_offset": "1",
        "month_offset": "0",
        "subject": "Day"
      }
    ]
  },
  {
    "paymethod_key": "8n02pc6tg9whk5j67",
    "amount": "25.00",
    "currency_code": "0",
    "description": "Chocolate Frog Card Value Pack",
    "enabled": false,
    "frequency": "yearly",
    "next_date": "2019-03-15",
    "numleft": "5",
    "orderid": "877564567",
    "receipt_note": "Good Luck on getting your fave wizard cards.",
    "send_receipt": true,
    "source": "",
    "start_date": "2019-03-15",
    "tax": "",
    "user": "",
    "username": "",
    "skip_count": "1",
    "rules": [
      {
        "day_offset": "15",
        "month_offset": "03",
        "subject": "Day"
      }
    ]
  }
]'

Request Parameters

Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
paymethod_key	string	Key of the customer payment method that should be charged for this billing schedule.
amount	double	Total amount charged for recurring billing. Required
currency_code	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
description	string	Describes recurring payment.
enabled	bool	Set to true to enable the recurring schedules. defaults to false.
frequency	string	How often the recurring billing schedule should charge. Possible values are: weekly, monthly, or yearly. Required
next_date	string	The next time you would like the recurring billing schedule to run. Date should be the current date or in the future. Format is: YYYY-MM-DD. Required
numleft	string	The number of times you would like the recurring billing schedule to run. Set to -1 if you want the schedule to run indefinitely. Required
orderid	string	Merchant assigned order ID
receipt_note	string	Message you to include on the customer's receipt.
send_receipt	bool	Set to true to send a receipt when the transaction runs. Defaults to false.
source	string	Set the source for the recurring billing schedule. Defaults to the Recurring source.
start_date	string	The start date for the recurring schedule. Can be set prior to the next date above. Required
tax	double	The portion of the amount, that is tax for the transaction.
user	string	Gateway generated User ID for the user who set up the recurring schedule.
username	string	Username for the user who set up the recurring schedule.
skip_count	string	Frequency interval. If frequency is monthly: set to 1 to charge every month, set to 2 to charge every other month, set to 3 to charge every 3 months, etc. Required
rules	array	Array of billing schedule rules Required

Example Response - Single Schedule

[
    {
        "key": "on0mth840rx9tn6r3",
        "type": "billingschedule",
        "paymethod_key": "8n02pc6tg9whk5j67",
        "method_name": "Smaug Savings",
        "amount": "5.00",
        "currency_code": "0",
        "description": "Cockroach Clusters",
        "enabled": "1",
        "frequency": "monthly",
        "next_date": "2019-01-31",
        "numleft": "12",
        "orderid": "76567898",
        "receipt_note": "So happy we could provide you with these gross gross candies!",
        "send_receipt": "1",
        "source": "",
        "start_date": "2019-02-01",
        "tax": "1.00",
        "user": "33956",
        "username": "hgranger",
        "skip_count": "2",
        "rules": [
            {
                "key": "kn0vr744n0v02c0h3",
                "type": "billingschedulerule",
                "day_offset": "1",
                "month_offset": "0",
                "subject": "Day"
            }
        ]
    }
]


Example Response - Multiple Schedules

[
    {
        "key": "pn0mt8000db4yvhvh",
        "type": "billingschedule",
        "paymethod_key": "8n02pc6tg9whk5j67",
        "method_name": "Smaug Savings",
        "amount": "15.00",
        "currency_code": "0",
        "description": "Lemon Drops",
        "enabled": "1",
        "frequency": "weekly",
        "next_date": "2019-01-09",
        "numleft": "-1",
        "orderid": "12356",
        "receipt_note": "So happy we could provide you with these weird weird muggle candies!",
        "send_receipt": "1",
        "source": "",
        "start_date": "2019-01-09",
        "tax": "2.00",
        "user": "",
        "username": "",
        "skip_count": "1",
        "rules": [
            {
                "key": "ln0vrz6r3x2w48z1s",
                "type": "billingschedulerule",
                "day_offset": "0",
                "month_offset": "0",
                "subject": "wed"
            }
        ]
    },
    {
        "key": "0n0mt805n00txppn8",
        "type": "billingschedule",
        "paymethod_key": "6n02p9w1sswpgwxyh",
        "method_name": "Minerva M-1111",
        "amount": "5.00",
        "currency_code": "0",
        "description": "Cockroach Clusters",
        "enabled": "1",
        "frequency": "monthly",
        "next_date": "2019-02-01",
        "numleft": "12",
        "orderid": "76567898",
        "receipt_note": "So happy we could provide you with these gross gross candies!",
        "send_receipt": "1",
        "source": "",
        "start_date": "2019-01-31",
        "tax": "1.00",
        "user": "33956",
        "username": "hgranger",
        "skip_count": "1",
        "rules": [
            {
                "key": "mn0vrchq2zd6mq9y8",
                "type": "billingschedulerule",
                "day_offset": "1",
                "month_offset": "0",
                "subject": "Day"
            }
        ]
    },
    {
        "key": "1n0mtq7yvs3sqky8n",
        "type": "billingschedule",
        "paymethod_key": "8n02pc6tg9whk5j67",
        "method_name": "Smaug Savings",
        "amount": "25.00",
        "currency_code": "0",
        "description": "Chocolate Frog Card Value Pack",
        "enabled": "",
        "frequency": "yearly",
        "next_date": "2019-03-15",
        "numleft": "5",
        "orderid": "877564567",
        "receipt_note": "Good Luck on getting your fave wizard cards.",
        "send_receipt": "1",
        "source": "",
        "start_date": "2019-03-15",
        "tax": "",
        "user": "",
        "username": "",
        "skip_count": "1",
        "rules": [
            {
                "key": "nn0vr6sz9dp6yp7nz",
                "type": "billingschedulerule",
                "day_offset": "15",
                "month_offset": "03",
                "subject": "Day"
            }
        ]
    }
]

    {
    "type": "billingschedule",
    "rules": [
            {
            "type": "billingschedulerule"
            },
            {
            "type": "billingschedulerule"
            }
        ]
    }
]

Response Parameters
Parameter Name	Type	Description
key	string	Gateway generated customer billing schedule identifier.
type	string	The object type. Successful calls will always return billingschedule.
paymethod_key	string	Key of the customer payment method that will be charged for this billing schedule.
method_name	string	Merchant designated name for the payment method that will be charged for this billing schedule.
amount	double	Total amount charged for recurring billing.
currency_code	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
description	string	Describes recurring payment.
enabled	bool	If 1 is returned, schedule is enabled. If 0 is returned, schedule is not enabled.
frequency	string	How often the recurring billing schedule should charge. Possible values are: weekly, monthly, or yearly.
next_date	string	The next time you would like the recurring billing schedule to run. Date should be the current date or in the future. Format is: YYYY-MM-DD.
numleft	string	The number of times you would like the recurring billing schedule to run. If -1 is returned the schedule will run indefinitely.
orderid	string	Merchant assigned order ID
receipt_note	string	Message to include on the customer's receipt.
send_receipt	bool	If 1 is returned, receipt will be sent. If 0 is returned, receipt will not be sent.
source	string	Source associated with the recurring billing schedule.
start_date	string	The start date for the recurring schedule. Schedule will not be run on this date unless it follows the rules outlined.
tax	double	The portion of the amount, that is tax for the transaction.
user	string	Gateway generated User ID for the user who set up the recurring schedule.
username	string	Username for the user who set up the recurring schedule.
skip_count	string	Frequency interval. If frequency is monthly: set to 1 to charge every month, set to 2 to charge every other month, set to 3 to charge every 3 months, etc.
rules	array	Array of billing schedule rules
Retrieve Specific Schedule

Endpoint

GET api/v2/customers/:custkey:/billing_schedules/:billingschedule_key:


This endpoint retrieves a specific customer billing schedule. You can set the offset and limits for results.

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/billing_schedules/0n0mt805n00txppn8
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvYTY1M2U0NjNhODA2NWU1ZDg2NDQ4OTQ4NTc1YzdkOTUvODE3Y2Q4ZTE0NjM3NzM0ZTllMDRiNTE0ZmJjMjAxMzNmYzZkMDNkZjVhYzAzYzU1NGI2NjZjNGY4MDI5OWZiNg=="
-H "Content-Type: application/json"

Request Parameters

Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
billingschedule_key	string	Gateway generated customer billing schedule identifier. This is passed in through the endpoint. (Required)

Example Response

{
    "key": "0n0mt805n00txppn8",
    "type": "billingschedule",
    "paymethod_key": "8n02pc6tg9whk5j67",
    "method_name": "Smaug Savings",
    "amount": "5.00",
    "currency_code": "0",
    "description": "Cockroach Clusters",
    "enabled": "1",
    "frequency": "monthly",
    "next_date": "2019-02-01",
    "numleft": "12",
    "orderid": "76567898",
    "receipt_note": "So happy we could provide you with these gross gross candies!",
    "send_receipt": "1",
    "source": "0",
    "start_date": "2019-01-31",
    "tax": "1.00",
    "user": "33956",
    "username": "hgranger",
    "skip_count": "1"
}

Response Parameters
Parameter Name	Type	Description
key	string	Gateway generated customer billing schedule identifier.
type	string	The object type. Successful calls will always return billingschedule.
paymethod_key	string	Key of the customer payment method that will be charged for this billing schedule.
method_name	string	Merchant designated name for the payment method that will be charged for this billing schedule.
amount	double	Total amount charged for recurring billing.
currency_code	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
description	string	Describes recurring payment.
enabled	bool	If 1 is returned, schedule is enabled. If 0 is returned, schedule is not enabled.
frequency	string	How often the recurring billing schedule should charge. Possible values are: weekly, monthly, or yearly.
next_date	string	The next time you would like the recurring billing schedule to run. Date should be the current date or in the future. Format is: YYYY-MM-DD.
numleft	string	The number of times you would like the recurring billing schedule to run. If -1 is returned the schedule will run indefinitely.
orderid	string	Merchant assigned order ID
receipt_note	string	Message to include on the customer's receipt.
send_receipt	bool	If 1 is returned, receipt will be sent. If 0 is returned, receipt will not be sent.
source	string	Source associated with the recurring billing schedule.
start_date	string	The start date for the recurring schedule. Schedule will not be run on this date unless it follows the rules outlined.
tax	double	The portion of the amount, that is tax for the transaction.
user	string	Gateway generated User ID for the user who set up the recurring schedule.
username	string	Username for the user who set up the recurring schedule.
skip_count	string	Frequency interval. If frequency is monthly: set to 1 to charge every month, set to 2 to charge every other month, set to 3 to charge every 3 months, etc.
Retrieve Schedule List

Endpoint

GET api/v2/customers/:custkey:/billing_schedules


This endpoint retrieves a specific customer payment method. You can set the offset and limits for results.

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/billing_schedules?limit=3&offset=5
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZDQzOTc5ZTU4YmRkMzk5MzkzMzcxY2UxZTk1YjVhYzcvMDhhZDIxMDhlOGU2MTBkOTNlMTRkMGNhYzM0YjlkZWRiOWE5ZWY2OTFiNGZlZDE4NzhkM2VmNWE0OTI4MjhmYQ=="
-H "Content-Type: application/json"

Request Parameters

Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
custkey	string	Gateway generated customer identifier. This is passed in through the endpoint. (Required)
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".

Example Response

{
    "type": "list",
    "limit": 3,
    "offset": 5,
    "data": [
        {
            "key": "1n0mtq7yvs3sqky8n",
            "type": "billingschedule",
            "paymethod_key": "8n02pc6tg9whk5j67",
            "method_name": "Smaug Savings",
            "amount": "25.00",
            "currency_code": "0",
            "description": "Chocolate Frog Card Value Pack",
            "enabled": "0",
            "frequency": "yearly",
            "next_date": "2019-03-15",
            "numleft": "5",
            "orderid": "877564567",
            "receipt_note": "Good Luck on getting your fave wizard cards.",
            "send_receipt": "1",
            "source": "0",
            "start_date": "2019-03-15",
            "tax": "0.00",
            "user": "",
            "username": "",
            "skip_count": "1"
        },
        {
            "key": "ln0mtq7zpq71qsytr",
            "type": "billingschedule",
            "paymethod_key": "bn02x3r42bwy833ms",
            "method_name": "Gringotts Credit",
            "amount": "5.00",
            "currency_code": "0",
            "description": "Cockroach Clusters",
            "enabled": "1",
            "frequency": "monthly",
            "next_date": "2019-03-01",
            "numleft": "12",
            "orderid": "76567898",
            "receipt_note": "So happy we could provide you with these gross gross candies!",
            "send_receipt": "1",
            "source": "0",
            "start_date": "2019-01-31",
            "tax": "1.00",
            "user": "33956",
            "username": "hgranger",
            "skip_count": "2"
        },
        {
            "key": "fn0mtq7ydjc74sd93",
            "type": "billingschedule",
            "paymethod_key": "6n02p9w1sswpgwxyh",
            "method_name": "Minerva M-1111",
            "amount": "52.00",
            "currency_code": "0",
            "description": "Lemon Drops",
            "enabled": "1",
            "frequency": "monthly",
            "next_date": "2019-02-01",
            "numleft": "-1",
            "orderid": "1881",
            "receipt_note": "Thank you for your order!",
            "send_receipt": "1",
            "source": "0",
            "start_date": "2019-01-01",
            "tax": "2.00",
            "user": "33956",
            "username": "hgranger",
            "skip_count": "1"
        }
    ],
    "total": 18
}

Response Parameters
Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of schedules that will be included in response.
offset	integer	The number of schedules skipped from the results.
data	array	An array of billing schedules matching the request.
total	integer	The total amount of schedules, including filtered results.
Customer Transactions
Retrieve Customer History

Endpoint

GET api/v2/customers/:custkey:/transactions


This endpoint retrieves a list of transactions associated with the customer. You can set the offset and limits for results.

Request Parameters

Example Request

curl -X GET https://secure.usaepay.com/api/v2/customers/ksddgpqgpbs5zkmb/transactions?limit=2
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
custkey	string	Customer identifier generated by gateway. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "type": "list",
    "limit": 2,
    "offset": 0,
    "data": [
        {
            "type": "transaction",
            "key": "bnfwhrb3cjzxftr",
            "refnum": "100345",
            "created": "2019-07-03 11:02:01",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "261144",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "Minerva McGonnegall",
                "number": "4444xxxxxxxx1111",
                "avs_street": "789 Low Dr",
                "avs_postalcode": "90005",
                "category_code": "A",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "avs": {
                "result_code": "YYY",
                "result": "Address: Match & 5 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "P",
                "result": "Not Processed"
            },
            "batch": {
                "type": "batch",
                "key": "et1m9h57b4m26rt",
                "batchrefnum": "409384",
                "sequence": "2516"
            },
            "amount": "5.00",
            "amount_detail": {
                "tip": "0.00",
                "tax": "0.00",
                "shipping": "0.00",
                "discount": "0.00"
            },
            "invoice": "2841373",
            "description": "Recurring Bill",
            "customer_email": "danielle.dartnell@usaepay.com",
            "source_name": "recurring",
            "billing_address": {
                "company": "Hogwarts School of Witchcraft and Wizardry",
                "first_name": "Albus",
                "last_name": "Dumbledore",
                "street": "123 Astronomy Tower",
                "street2": "Suite 1",
                "city": "Phoenix",
                "state": "CA",
                "country": "USA",
                "postalcode": "10005",
                "phone": "555-253-3673"
            },
            "custom_fields": {
                "1": "Gryffindor",
                "2": "Headmaster of Hogwarts"
            }
        },
        {
            "type": "transaction",
            "key": "6nfty5ftf6c0n8t",
            "refnum": "100340",
            "created": "2019-07-02 16:12:37",
            "trantype_code": "S",
            "trantype": "Credit Card Sale",
            "result_code": "A",
            "result": "Approved",
            "authcode": "251951",
            "status_code": "P",
            "status": "Authorized (Pending Settlement)",
            "creditcard": {
                "cardholder": "Hogwarts School of Witchcraft and Wizardry",
                "number": "4444xxxxxxxx1111",
                "avs_street": "1234 Portkey Ave",
                "avs_postalcode": "12345",
                "category_code": "A",
                "entry_mode": "Card Not Present, Manually Keyed"
            },
            "avs": {
                "result_code": "YYY",
                "result": "Address: Match & 5 Digit Zip: Match"
            },
            "cvc": {
                "result_code": "N",
                "result": "No Match"
            },
            "batch": {
                "type": "batch",
                "key": "et1m9h57b4m26rt",
                "batchrefnum": "409384",
                "sequence": "2516"
            },
            "amount": "500.00",
            "amount_detail": {
                "tip": "5.00",
                "tax": "45.00",
                "shipping": "50.00",
                "discount": "50.00"
            },
            "ponum": "af416fsd5",
            "invoice": "98454685",
            "description": "Antique Pensieve",
            "comments": "Powerful magical object. Use with caution.",
            "customer_email": "brian@hogwarts.com",
            "source_name": "REST TEST",
            "billing_address": {
                "first_name": "Albus",
                "last_name": "Dumbledore",
                "street": "123 Astronomy Tower",
                "street2": "Suite 1",
                "city": "Phoenix",
                "state": "CA",
                "country": "USA",
                "postalcode": "10005",
                "phone": "555-253-3673",
                "fax": "666-253-3673"
            },
            "shipping_address": {
                "first_name": "Aberforth",
                "last_name": "Dumbledore",
                "street": "987 HogsHead St",
                "city": "Hogsmead",
                "state": "WY",
                "country": "USA",
                "postalcode": "30005",
                "phone": "555-253-3673"
            },
            "lineitems": [
                {
                    "name": "Antique Pensieve",
                    "cost": "450.00",
                    "taxable": "N",
                    "qty": "1.0000",
                    "tax_amount": "50.00",
                    "product_key": "ds4bb5ckg059vdn8",
                    "um": "EA",
                    "locationid": "13",
                    "location_key": "dnyyjc8s2vbz8hb33"
                }
            ],
            "custom_fields": {
                "1": "Gryffindor",
                "2": "Headmaster"
            }
        }
    ],
    "total": "156"
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of transactions that will be included in response.
offset	integer	The number of transactions skipped from the results.
data	array	An array of transactions matching the request.
total	integer	The total amount of transactions associated with the customer.
Process Customer Transaction

Endpoint

POST api/v2/transactions


This endpoint processed transactions using an existing customer payment method.

Customer Sale

Processing a customer sale uses the customer:sale command.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
    '{
      "command": "customer:sale",
        "custkey": "lsddntp1p7hdthms",
      "paymethod_key": "in02p9swwbnqmcs4s",
      "invoice": "101",
      "ponum": "af416fsd5",
      "description": "Wolfsbane Potion",
      "amount": "75.00",
    }'


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
custkey	string	Customer identifier generated by gateway. (Required)
command	string	This must be set to customer:sale for a this type of transaction. (Required)
paymethod_key	string	Gateway generated customer payment method identifier.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
save_card	bool	Set to true to tokenize the card used to process the transaction.
traits	object	This object holding transaction characteristics.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.
Response Parameters

Example Response

{
    "type": "transaction",
    "key": "fnf0c8975sr76rb",
    "refnum": "100349",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "351293",
    "creditcard": {
        "number": "4444xxxxxxxx7779",
        "category_code": "A",
        "entry_mode": "Card Not Present, Manually Keyed"
    },
    "invoice": "101",
    "avs": {
        "result_code": "YYY",
        "result": "Address: Match & 5 Digit Zip: Match"
    },
    "cvc": {
        "result_code": "P",
        "result": "Not Processed"
    },
    "batch": {
        "type": "batch",
        "key": "et1m9h57b4m26rt",
        "batchrefnum": "409384",
        "sequence": "2516"
    },
    "auth_amount": "75.00",
    "trantype": "Credit Card Sale"
}

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
%customer%	object	Customer information when customer is saved to customer database.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Customer Refund

Processing a customer refund uses the customer:refund command.

Request Parameters

Example Request

curl -X POST https://secure.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X3JMRnlVczMya0pjN2VkNzlJaFBNMGgwdzM4MUxSaEQ6czIvMGM1MjZiYjYzZGI0MmVhZTRhOTY2NGM4NjFjMzljNTgvNGYxNDI3MTI0Y2Y1ZjliMzEwNjkyYzI2ZmU5YmVlZWNmMjQwMTI0YTFjYmRjNzI2MWM0NTg1ZTA2N2RkMDIyNA=="
    -d
    '{
      "command": "customer:refund",
      "invoice": "101",
      "ponum": "af416fsd5",
      "description": "Wolfsbane Potion",
      "amount": "5.00",
      "custkey": "lsddntp1p7hdthms",
      "paymethod_key": "in02p9swwbnqmcs4s"
    }'


Parameters in marked Required are required to process this type of transaction.

Parameter Name	Type	Description
command	string	This must be set to customer:refund for a this type of transaction. (Required)
custkey	string	Customer identifier generated by gateway. (Required)
paymethod_key	string	Gateway generated customer payment method identifier.
invoice	string	Custom Invoice Number to easily retrieve sale details.
ponum	string	Customer's purchase order number. Required for level 3 processing.
orderid	string	Merchant assigned order identifier.
description	string	Public description of the transaction.
comments	string	Private comment details only visible to the merchant.
email	string	Customer's email address
send_receipt	bool	If set, this parameter will send an email receipt to the customer's email.
merchemailaddr	string	Email where merchant receipt should be sent.
amount	double	Total transaction amount (Including tax, tip, shipping, etc.) (Required)
amount_detail	object	Object containing a more detailed breakdown of the amount. Not required if amount is previously set.
save_card	bool	Set to true to tokenize the card used to process the transaction.
traits	object	This object holding transaction characteristics.
billing_address	object	Object which holds the customer's billing address information.
shipping_address	object	Object which holds the customer's shipping address information.
lineitems	object	Array of line items attached to the transaction. Possible fields for each line item listed below.
custom_fields	object	Array custom fields attached to the transaction. You may have up to 20 custom fields. You should set up custom fields in the API or through the merchant console prior to using them.
currency	string	Currency numerical code. Full list of supported numerical currency codes can be found here. Defaults to 840 (USD).
terminal	string	Terminal identifier (i.e. multilane)
clerk	string	Clerk/Cashier/Server name
clientip	string	IP address of client. Used in conjunction with the Block By Host or IP fraud module.
software	string	Software name and version (useful for troubleshooting)
receipt-custemail	string	The name of the receipt template that should be used when sending a customer receipt.
receipt-merchemail	string	The name of the receipt template that should be used when sending a merchant receipt.
ignore_duplicate	bool	Set to true to bypass duplicate detection/folding.
Response Parameters

Example Response

{
    "type": "transaction",
    "key": "gnfkd12kcxh7ng2",
    "refnum": "100350",
    "is_duplicate": "N",
    "result_code": "A",
    "result": "Approved",
    "authcode": "110373",
    "creditcard": {
        "number": "4444xxxxxxxx7779"
    },
    "invoice": "101",
    "avs": {
        "result_code": "   ",
        "result": "Unmapped AVS response (   )"
    },
    "batch": {
        "type": "batch",
        "key": "et1m9h57b4m26rt",
        "batchrefnum": "409384",
        "sequence": "2516"
    },
    "trantype": "Credit Card Refund (Credit)"
}

Parameter Name	Type	Description
type	string	Object type. This will always be transaction.
key	string	Transaction Key. Unique gateway generated key.
refnum	string	Unique transaction reference number.
is_duplicate	bool	If Y, a duplicate transaction was detected. The system is showing the response from the original transaction, rather than running a duplicate. If N, no duplicate was detected.
result_code	string	Result code. Possible options are: A = Approved, P = Partial Approval, D = Declined, E = Error, or V = Verification Required
result	string	Description of above result_code (Approved, etc)
authcode	string	Authorization code
creditcard	object	Object holding credit card information
invoice	string	Custom Invoice Number to easily retrieve sale details.
avs	object	The Address Verification System (AVS) result.
cvc	object	The Card Security Code (3-4 digit code) verification result.
batch	object	Batch information.
%customer%	object	Customer information when customer is saved to customer database.
auth_amount	double	Amount authorized
trantype	string	The transaction type. Possible transaction types can be found here.
receipts	object	Receipt information.
Product Endpoints
Products
api/v2/products


Use the products endpoint to create, delete, and manage product information.

Create Product

Endpoint

POST /api/v2/products


This method allows you to create a product.

Request Parameters

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/products
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvYWQwYTYxZjY1NzE3YzRlZDQzY2FiODFlMDRhZDZkM2EvMDJjMjU5ZTRlOTk0YTc4NzlkNGFjZDAwYjQyYjY3ZGY4ODA5OTFkZDJkMWUzNWEzZjY1YmQ5NjhiNjMwOGVlZA=="
-H "Content-Type: application/json"
-d
 '{
  "name": "Advanced Potion-Making",
  "price": "125.00",
  "list_price": "130.00",
  "enabled": true,
  "taxable": true,
  "available_all": true,
  "available_all_date": "2019-01-10",
  "date_available": "2019-01-15",
  "categoryid": "37",
  "commodity_code": "715-86",
  "description": "by Libatius Borage",
  "manufacturer": "Merge Books",
  "merch_productid": "978675",
  "min_quantity": "20",
  "model": "4th Edition",
  "physicalgood": true,
  "ship_weight": "1.25",
  "weight": "1.0",
  "sku": "9876457687",
  "taxclass": "NA",
  "upc": "876457687",
  "um": "EA",
  "url": "http://harrypotter.wikia.com/wiki/Advanced_Potion-Making",
  "wholesale_price": "100.00",
  "allow_overrides": false
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
name	string	Product name. (Required)
price	double	Product cost.
enabled	bool	If set to true if product is enabled. Defaults to false if not passed.
taxable	bool	If set to true if product is taxable. Defaults to false if not passed.
available_all	bool	If set to true if product is available.
available_all_date	string	Date product will become available. Format is YYYY-MM-DD.
categoryid	integer	Gateway generated category identifier
commodity_code	string	Commodity code for product. Click here for a list of commodity_codes.
date_available	string	Date product will become available.
description	string	Product description.
list_price	double	Recommended listing price.
wholesale_price	double	Wholesale price.
manufacturer	string	Manufacturer of product.
merch_productid	string	Merchant assigned product identifier.
min_quantity	integer	When the inventory reaches this quantity, you will see a low inventory flag on this product.
model	string	Product model.
physicalgood	bool	If set to true the product is Physical (eg. a Hard cover book). If set to false the product is Virtual (eg. an eBook). Defaults to false.
weight	integer	This is the product’s weight.
ship_weight	integer	This is the product’s weight adjusted for packing and shipping purposes.
sku	string	This is the product’s Stock Keeping Unit number.
taxclass	string	Product tax class.
um	string	Unit of measure. For a list of commonly used unit of measure codes click here.
upc	string	This is the product’s Universal Product Code.
url	string	Product URL.
allow_override	bool	Set to true to allow users to change product price in console.
inventory	array	Array of inventory objects associated with the product
modifiers	array	Array of modifier objects associated with the product
Response Parameters

Example Response

{
    "type": "product",
    "key": "ms4bb95qgg0y8dn8",
    "product_refnum": 22,
    "name": "Advanced Potion-Making",
    "price": "125.00",
    "enabled": "Y",
    "taxable": "Y",
    "available_all": "Y",
    "available_all_date": "2019-01-10",
    "categoryid": 37,
    "commodity_code": "715-86",
    "date_available": "2019-01-15",
    "description": "by Libatius Borage",
    "image_url": "",
    "list_price": "130.00",
    "manufacturer": "Merge Books",
    "merch_productid": "978675",
    "min_quantity": 20,
    "model": "4th Edition",
    "physicalgood": "Y",
    "ship_weight": 1.25,
    "sku": "9876457687",
    "taxclass": "NA",
    "um": "EA",
    "upc": "876457687",
    "url": "http:\/\/harrypotter.wikia.com\/wiki\/Advanced_Potion-Making",
    "weight": 1,
    "wholesale_price": "100.00",
    "allow_override": false,
    "inventory": [],
    "modifiers": [],
    "created": "2019-01-06 23:57:34",
    "modified": "2019-01-06 23:57:34"
}

Parameter Name	Type	Description
type	string	The object type. Successful calls will always return product.
key	string	Gateway generated product identifier.
product_refnum	string	Gateway generated product identifier.
name	string	Product name.
price	double	Product cost.
enabled	bool	If set to Y, product is enabled.
taxable	bool	If set to Y, product is taxable.
available_all	bool	If set to Y product is available.
available_all_date	string	Date product will become available. Format is YYYY-MM-DD.
categoryid	integer	Gateway generated category identifier
commodity_code	string	Commodity code for product. Click here for a list of commodity_codes.
date_available	string	Date product will become available.
description	string	Product description.
img_url	string	URL where product image is hosted.
list_price	double	Recommended listing price.
manufacturer	string	Manufacturer of product.
merch_productid	string	Merchant assigned product identifier.
min_quantity	integer	When the inventory reaches this quantity, you will see a low inventory flag on this product.
model	string	Product model.
physicalgood	bool	If set to Y the product is Physical (eg. a Hard cover book). If set to N the product is Virtual (eg. an eBook).
ship_weight	integer	This is the product’s weight adjusted for packing and shipping purposes.
sku	string	This is the product’s Stock Keeping Unit number.
taxclass	string	Product tax class.
um	string	Unit of measure. For a list of commonly used unit of measure codes click here.
upc	string	This is the product’s Universal Product Code.
url	string	Product URL.
weight	integer	This is the product’s weight.
wholesale_price	double	Wholesale price.
allow_override	bool	If set to Y, users can change product price in console.
inventory	array	Array of inventory objects associated with the product
modifiers	array	Array of modifier objects associated with the product
created	string	Date and time the product was created. Format is YYYY-MM-DD HH:MM:SS.
modified	string	Date and time the product was last modified. Format is YYYY-MM-DD HH:MM:SS.
Retrieve Specific Product

Endpoint

GET /api/v2/products/:product_key :


This endpoint returns the product details for a specific product.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/products/4s4b4qrn3k4pmsg8
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZTRiZTVhZjU3YTZkOWUwZDQ0ZDM2ZGQ0N2I1ZGU4NzYvYjNiZTJhOGMxZDk2NGZmZGQzNjYyMjg2NDE4MmQwMzA4MTg3OGYyZDc2MDdiODIyMTQzOTYyN2U4Y2Y5ZmQyNA=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
product_key	string	Product identifier generated by gateway. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "type": "product",
    "key": "4s4b4qrn3k4pmsg8",
    "product_refnum": "4",
    "name": "The Standard Book of Spells (Grade 1)",
    "price": "10.00",
    "enabled": "Y",
    "taxable": "N",
    "available_all": "",
    "available_all_date": "0000-00-00",
    "categoryid": "37",
    "commodity_code": "715-86",
    "date_available": "2018-11-30",
    "description": "by Miranda Goshawk An indispensable guide to your basic magical needs.",
    "image_url": "",
    "list_price": "10.00",
    "manufacturer": "Flourish and Blotts",
    "merch_productid": "",
    "min_quantity": "20",
    "model": "1.0",
    "physicalgood": "Y",
    "ship_weight": "2.1500",
    "sku": "",
    "taxclass": "",
    "um": "EAC",
    "upc": "119104110946",
    "url": "",
    "weight": "2.0000",
    "wholesale_price": "7.00",
    "allow_override": false,
    "inventory": [
        {
            "locationid": "13",
            "merch_locationid": "London",
            "name": "London",
            "description": "",
            "inventoryid": "4",
            "productid": "4",
            "qtyonhand": "150",
            "qtyonorder": "0",
            "date_available": "2019-01-06",
            "key": "4",
            "location_key": "dnyyjc8s2vbz8hb33"
        },
        {
            "locationid": "16",
            "merch_locationid": "New York",
            "name": "New York",
            "description": "",
            "inventoryid": "19",
            "productid": "4",
            "qtyonhand": "25",
            "qtyonorder": "55",
            "date_available": "2019-01-06",
            "key": "19",
            "location_key": "gnyyj6yv3jf05pdb3"
        }
    ],
    "modifiers": [],
    "created": "2018-11-30",
    "modified": "2019-01-06"
}

Parameter Name	Type	Description
type	string	The object type. Successful calls will always return product.
key	string	Gateway generated product identifier.
product_refnum	string	Gateway generated product identifier.
name	string	Product name.
price	double	Product cost.
enabled	bool	If set to Y, product is enabled.
taxable	bool	If set to Y, product is taxable.
available_all	bool	If set to Y product is available.
available_all_date	string	Date product will become available. Format is YYYY-MM-DD.
categoryid	integer	Gateway generated category identifier
commodity_code	string	Commodity code for product. Click here for a list of commodity_codes.
date_available	string	Date product will become available.
description	string	Product description.
img_url	string	URL where product image is hosted.
list_price	double	Recommended listing price.
manufacturer	string	Manufacturer of product.
merch_productid	string	Merchant assigned product identifier.
min_quantity	integer	When the inventory reaches this quantity, you will see a low inventory flag on this product.
model	string	Product model.
physicalgood	bool	If set to Y the product is Physical (eg. a Hard cover book). If set to N the product is Virtual (eg. an eBook).
ship_weight	integer	This is the product’s weight adjusted for packing and shipping purposes.
sku	string	This is the product’s Stock Keeping Unit number.
taxclass	string	Product tax class.
um	string	Unit of measure. For a list of commonly used unit of measure codes click here.
upc	string	This is the product’s Universal Product Code.
url	string	Product URL.
weight	integer	This is the product’s weight.
wholesale_price	double	Wholesale price.
allow_override	bool	If set to Y, users can change product price in console.
inventory	array	Array of inventory objects associated with the product
modifiers	array	Array of modifier objects associated with the product
created	string	Date and time the product was created. Format is YYYY-MM-DD.
modified	string	Date and time the product was last modified. Format is YYYY-MM-DD.
Retrieve Product List

Endpoint

GET /api/v2/products


This endpoint retrieves a list of products on the merchants account. You can set the offset and limits for results.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/products
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZWFhZTg0MWFkZGJhZWM3NTIxMmVjZjBiNTFhYWIxOGUvZjM1NTQ3ZjE0ZWRkNzg3MzFjNmZiOGI4YjkyMGZhYTljNmQyYTgzODgxNDY0Nzk4MzVhZjZlMmYxZDNiMTc0ZQ=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
Response Parameters

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "type": "product",
            "key": "ms4bb95qgg0y8dn8",
            "product_refnum": "22",
            "name": "Advanced Potion-Making",
            "price": "125.00",
            "enabled": null,
            "taxable": null,
            "has_inventory": false
        },
        {
            "type": "product",
            "key": "ds4bb5ckg059vdn8",
            "product_refnum": "13",
            "name": "Antique Pensieve",
            "price": "450.00",
            "enabled": null,
            "taxable": null,
            "has_inventory": true
        },
        {
            "type": "product",
            "key": "as4b4s9sfbwzd1v8",
            "product_refnum": "10",
            "name": "A History of Magic by Bathilda Bagshot",
            "price": "20.00",
            "enabled": null,
            "taxable": null,
            "has_inventory": true
        },
        {
            "type": "product",
            "key": "4s4b4qrn3k4pmsg8",
            "product_refnum": "4",
            "name": "The Standard Book of Spells (Grade 1)",
            "price": "10.00",
            "enabled": null,
            "taxable": null,
            "has_inventory": true
        }
    ],
    "total": "4"
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of products that will be included in response.
offset	integer	The number of products skipped from the results.
data	array	An array of products matching the request.
total	integer	The total amount of products, including filtered results.
Update Product

Endpoint

PUT /api/v2/products/:product_key :


This endpoint updates already existing products. Only fields that are passed through will be updated. If you do not wish to update a field, do not pass it in the request. If you pass in a field, but leave the content blank, content will be removed.

Request Parameters

Example Request

curl -X PUT https://sandbox.usaepay.com/api/v2/products/ps4b4j77prx2rc1d
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNjk0MDk1YmFjMjg4ZGYyNTA3MmFjMTM2MTE5N2E0ZjQvOWFmZjZkYWE5MGUwOTQyOTY2MGUyMmQ2NmQxZDMwZjYyY2RiNjNkZWYwMTAwOTliMDRmMDJlM2E5N2RiNGM4MA=="
-H "Content-Type: application/json"
-d
'{
  "name": "Unfogging the Future",
  "price": "200.00",
  "list_price": "180.00",
  "enabled": false,
  "taxable": false,
  "available_all": false,
  "available_all_date": "2019-02-10",
  "date_available": "2019-01-31",
  "categoryid": "43",
  "commodity_code": "052-02",
  "description": "by Cassandra Vablatsky",
  "manufacturer": "Rumihart Books",
  "merch_productid": "98656768",
  "min_quantity": "10",
  "model": "2nd Edition",
  "physicalgood": false,
  "ship_weight": "1",
  "weight": ".75",
  "sku": "457689867",
  "taxclass": "Education",
  "upc": "09876789234",
  "um": "PK",
  "url": "http://harrypotter.wikia.com/wiki/Unfogging_the_Future",
  "wholesale_price": "170.00",
  "allow_overrides": true
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
product_key	string	Product identifier generated by gateway. This is passed in through the endpoint. (Required)
name	string	Product name.
price	double	Product cost.
enabled	bool	If set to true if product is enabled. Defaults to false if not passed.
taxable	bool	If set to true if product is taxable. Defaults to false if not passed.
available_all	bool	If set to true if product is available.
available_all_date	string	Date product will become available. Format is YYYY-MM-DD.
categoryid	integer	Gateway generated category identifier
commodity_code	string	Commodity code for product. Click here for a list of commodity_codes.
date_available	string	Date product will become available.
description	string	Product description.
list_price	double	Recommended listing price.
wholesale_price	double	Wholesale price.
manufacturer	string	Manufacturer of product.
merch_productid	string	Merchant assigned product identifier.
min_quantity	integer	When the inventory reaches this quantity, you will see a low inventory flag on this product.
model	string	Product model.
physicalgood	bool	If set to true the product is Physical (eg. a Hard cover book). If set to false the product is Virtual (eg. an eBook). Defaults to false.
weight	integer	This is the product’s weight.
ship_weight	integer	This is the product’s weight adjusted for packing and shipping purposes.
sku	string	This is the product’s Stock Keeping Unit number.
taxclass	string	Product tax class.
um	string	Unit of measure. For a list of commonly used unit of measure codes click here.
upc	string	This is the product’s Universal Product Code.
url	string	Product URL.
allow_override	bool	Set to true to allow users to change product price in console.
Response Parameters

Example Response

{
    "type": "product",
    "key": "ps4b4j77prx2rc1d",
    "product_refnum": "25",
    "name": "Unfogging the Future",
    "price": "200.00",
    "enabled": "N",
    "taxable": "N",
    "available_all": "N",
    "available_all_date": "2019-02-10",
    "categoryid": 43,
    "commodity_code": "052-02",
    "date_available": "2019-01-31",
    "description": "by Cassandra Vablatsky",
    "image_url": "",
    "list_price": "180.00",
    "manufacturer": "Rumihart Books",
    "merch_productid": "98656768",
    "min_quantity": 10,
    "model": "2nd Edition",
    "physicalgood": "N",
    "ship_weight": 1,
    "sku": "457689867",
    "taxclass": "Education",
    "um": "PK",
    "upc": "09876789234",
    "url": "http:\/\/harrypotter.wikia.com\/wiki\/Unfogging_the_Future",
    "weight": 0.75,
    "wholesale_price": "170.00",
    "allow_override": false,
    "inventory": [],
    "modifiers": [],
    "created": "2019-01-07",
    "modified": "2019-01-07 00:57:43"
}

Parameter Name	Type	Description
type	string	The object type. Successful calls will always return product.
key	string	Gateway generated product identifier.
product_refnum	string	Gateway generated product identifier.
name	string	Product name.
price	double	Product cost.
enabled	bool	If set to Y, product is enabled.
taxable	bool	If set to Y, product is taxable.
available_all	bool	If set to Y product is available.
available_all_date	string	Date product will become available. Format is YYYY-MM-DD.
categoryid	integer	Gateway generated category identifier
commodity_code	string	Commodity code for product. Click here for a list of commodity_codes.
date_available	string	Date product will become available.
description	string	Product description.
img_url	string	URL where product image is hosted.
list_price	double	Recommended listing price.
manufacturer	string	Manufacturer of product.
merch_productid	string	Merchant assigned product identifier.
min_quantity	integer	When the inventory reaches this quantity, you will see a low inventory flag on this product.
model	string	Product model.
physicalgood	bool	If set to Y the product is Physical (eg. a Hard cover book). If set to N the product is Virtual (eg. an eBook).
ship_weight	integer	This is the product’s weight adjusted for packing and shipping purposes.
sku	string	This is the product’s Stock Keeping Unit number.
taxclass	string	Product tax class.
um	string	Unit of measure. For a list of commonly used unit of measure codes click here.
upc	string	This is the product’s Universal Product Code.
url	string	Product URL.
weight	integer	This is the product’s weight.
wholesale_price	double	Wholesale price.
allow_override	bool	If set to Y, users can change product price in console.
inventory	array	Array of inventory objects associated with the product
modifiers	array	Array of modifier objects associated with the product
created	string	Date and time the product was created. Format is YYYY-MM-DD.
modified	string	Date and time the product was last modified. Format is YYYY-MM-DD HH:MM:SS.
Delete Product

Endpoint

DELETE /api/v2/products/:product_key :


Use this endpoint to delete a specific product record.

Request Parameters

Example Request

curl -X DELETE https://sandbox.usaepay.com/api/v2/products/ps4b4j77prx2rc1d
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvM2Y2OTlhZjk0YmY5ZTA2MTRhMzM0ZDI4MzY1MzQyNzkvN2E0ZWJiNGQxYzJlOTg4MWU4MWRhYWQxMjIwODNhYjU0NTY0YWQ3ODAwYjdiY2RmNWJiOTkzM2ZkMDA3ZjliNQ=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
product_key	string	Unique product identifier generated by gateway. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If product has been deleted then status will be returned as success. If product is NOT deleted, an error will be returned instead.
Categories
api/v2/products/categories


Use the products endpoint to create, delete, and manage product categories.

Create Category

Endpoint

POST /api/v2/products/categories


This method allows you to create a product category.

Request Parameters

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/products/categories
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvY2QwN2QyODkxYjA3OWExNzQ2M2VlODNhNDRhOTZhZWYvMWE1NzcwMjY0ZWQyZGY5MzhiNjkwYzU1NTIwMTA5MTgxNmVlNDY2MDBiZmY4MDI2OTRjMTY5ZTI2OWZhNThmYQ=="
-H "Content-Type: application/json"
-d
'{
  "name":"Used Books"
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
name	string	Product category name. (Required)
modifiers	array	Array of modifiers associated with the category.
Response Parameters

Example Response

{
    "key": "nnyfxnq2vm8m8f1vk",
    "type": "product_category",
    "name": "Used Books",
    "modifiers": []
}

Parameter Name	Type	Description
key	string	Gateway generated product category identifier.
type	string	The object type. Successful calls will always return product category.
name	string	Product category name.
modifiers	array	Array of modifiers associated with the category.
Retrieve Specific Category

Endpoint

GET /api/v2/products/categories/:categorykey:


This method returns the details for a specific category.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/products/categories/bnyfx50bwydbvtr82
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZDRhMmUwZGY5Y2ZkNWJhNTg0OTE1YjczZmU1OTdjMjIvZWJiZTU2YmNjMDllOGE2YTFkZjg5MDkyM2FmNjhjZDAzODQ0ZTc0ODU3MDE5OTY4YzA4YjA5ZmU3ZGUwYjM3Zg=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
categorykey	string	Category identifier generated by gateway. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "key": "bnyfx50bwydbvtr82",
    "type": "product_category",
    "name": "Books",
    "modifiers": []
}

Parameter Name	Type	Description
key	string	Gateway generated category identifier.
type	string	The type of object returned. Successful calls will always return product category.
name	string	Product category name.
modifiers	array	Array of modifier objects associated with the category.
Retrieve Category List

Endpoint

GET /api/v2/products/categories


This method returns a list of the product categories associated with the account.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/products/categories
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZDRhMmUwZGY5Y2ZkNWJhNTg0OTE1YjczZmU1OTdjMjIvZWJiZTU2YmNjMDllOGE2YTFkZjg5MDkyM2FmNjhjZDAzODQ0ZTc0ODU3MDE5OTY4YzA4YjA5ZmU3ZGUwYjM3Zg=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
Response Parameters

Example Response

{
    "type": "list",
    "limit": 20,
    "offset": 0,
    "data": [
        {
            "key": "bnyfx50bwydbvtr82",
            "name": "Books"
        },
        {
            "key": "nnyfxnq2vm8m8f1vk",
            "name": "Used Books"
        },
        {
            "key": "enyfxxkp5p90780n8",
            "name": "Clothing"
        },
        {
            "key": "hnyfxxt76p01mpft2",
            "name": "Equipment"
        }
    ],
    "total": "4"
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of product categories that will be included in response.
offset	integer	The number of product categories skipped from the results.
data	array	An array of product categories matching the request.
total	integer	The total amount of product categories, including filtered results.
Update Category

Endpoint

PUT /api/v2/products/categories/:categorykey:


This endpoint updates already existing product categories. Only fields that are passed through will be updated. If you do not wish to update a field, do not pass it in the request. If you pass in a field, but leave the content blank, content will be removed.

Request Parameters

Example Request

curl -X PUT https://sandbox.usaepay.com/api/v2/products/categories/enyfxxkp5p90780n8
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvOWE1ZjhhNjAyZTc1ZmM4YjFlYzhkMDkzOTZiZGExODgvNGYxOTA1ZTA5NWMwOTE1MzgwYTNhNTdkZGRlN2M3MGRhNDJiZTVhOGNhN2NjZjkwYWY3NDVhY2RmZWIzNWI2MA=="
-H "Content-Type: application/json"
-d
'{
  "name":"Apparel"
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
categorykey	string	Category identifier generated by gateway. This is passed in through the endpoint. (Required)
name	string	Product category name.
modifiers	array	Array of modifiers associated with the category.
Response Parameters

Example Response

{
    "key": "enyfxxkp5p90780n8",
    "type": "product_category",
    "name": "Apparel",
    "modifiers": []
}

Parameter Name	Type	Description
key	string	Gateway generated product category identifier.
type	string	The object type. Successful calls will always return product category.
name	string	Product category name.
modifiers	array	Array of modifiers associated with the category.
Delete Category

Endpoint

DELETE /api/v2/products/categories/:categorykey:


Use this endpoint to delete a specific product category record.

Request Parameters

Example Request

curl -X DELETE https://sandbox.usaepay.com/api/v2/products/categories/0nyfxn3h01g7qpgy8
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNmE0OGUyM2QwMzY1OGVjMTgwZGU4MDNkYzBkMGI4YTgvOGVlNTdiMDAzYmI5MTk1YWVjOTBkM2E2M2Y0NTA2YzA2MjJiN2Q1ODY4Y2EyYjIxZDY4ZWNiOWVlNTAxYjM2NA=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
categorykey	string	Category identifier generated by gateway. This is passed in through the endpoint. (Required)
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If category has been deleted then status will be returned as success. If the product category is NOT deleted, an error will be returned instead.
Inventory Endpoints
Inventory Locations
api/v2/inventory/location


Use the products endpoint to create, delete, and manage inventory locations (product warehouses).

Create Location

Endpoint

POST /api/v2/inventory/location


This method allows you to create an inventory location.

Request Parameters

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/inventory/locations
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZTA3N2U5MjFhMmQ1NmUxNDVmMTU4MWE1YTU1NjkwNjMvNzY1NzEyZDg4OWZhMDhlZDVlN2U4NDM3NTViYzI4ZGQ1ODE4M2E4NzExNjFlNDlmMDE3Yjg1ZTFmMGJjYzQzZQ=="
-H "Content-Type: application/json"
-d
'{
  "name": "Los Angeles",
  "merch_locationid": "Los Angeles",
  "description": "West coast hub"
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
merch_locationid	string	The inventory location name specified by the merchant.
name	string	The inventory location name specified by the merchant.
description	string	The inventory location description.
Response Parameters

Example Response

{
    "key": "mnyyj94tyvsc4qb33",
    "type": "inventorylocation",
    "merch_locationid": "Los Angeles",
    "name": "Los Angeles",
    "description": "West coast hub"
}

Parameter Name	Type	Description
key	string	Gateway generated inventory location (warehouse) identifier.
type	string	The object type. Successful calls will always return inventorylocation.
merch_locationid	string	The inventory location name specified by the merchant.
name	string	The inventory location name specified by the merchant.
description	string	The inventory location description.
Retrieve Specific Location

Endpoint

GET /api/v2/inventory/location/:location_key:


This method returns the details for a specific inventory location.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/inventory/locations/dnyyjc8s2vbz8hb33
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNmYyMmQ1NWM5MjRlOTRkZGRkNmY3OWYzMGMwOGIyYzUvYjlkNzUzYjBjYjkwZTEyYmE2ZTU1ZjhkNzdlNDQwMWRlMDMwZWMwNWNjMzE1M2ExYzMwZDFlMjEyY2U5NWJkOA=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Response Parameters

Example Response

{
    "key": "dnyyjc8s2vbz8hb33",
    "type": "inventorylocation",
    "merch_locationid": "London",
    "name": "London",
    "description": ""
}

Parameter Name	Type	Description
key	string	Gateway generated inventory location (warehouse) identifier.
type	string	The object type. Successful calls will always return inventorylocation.
merch_locationid	string	The inventory location name specified by the merchant.
name	string	The inventory location name specified by the merchant.
description	string	The inventory location description.
Retrieve Location List

Endpoint

GET /api/v2/inventory/location


This method returns a list of the product inventory locations associated with the account.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/inventory/locations -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvOWM3NGFjNzE4ZDg1NzY3ODQ0MWUxYzdlZmM5NjVhNzMvMzcyNTAxMGQ4NDdjMTdhNmE5YWY2ZTdjN2VmN2FiOTE5N2QwZmRiNjE2NTEyYTc0ZDlhMTM4NzMwZTNkNWExNA==" -H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "100".
offset	integer	Offset setting for this response. This will default to "0".
Response Parameters

Example Response

{
    "type": "list",
    "limit": 100,
    "offset": 0,
    "data": [
        {
            "key": "dnyyjc8s2vbz8hb33",
            "merch_locationid": "London",
            "name": "London",
            "description": ""
        },
        {
            "key": "gnyyj6yv3jf05pdb3",
            "merch_locationid": "New York",
            "name": "New York",
            "description": ""
        }
    ],
    "total": "2"
}

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of product categories that will be included in response.
offset	integer	The number of product categories skipped from the results.
data	array	An array of inventory locations matching the request.
total	integer	The total amount of product categories, including filtered results.
Update Location

Endpoint

PUT /api/v2/inventory/location/:location_key:

Request Parameters

Example Request

curl -X PUT https://sandbox.usaepay.com/api/v2/inventory/locations/mnyyj94tyvsc4qb33
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvYjRjMDEwOGU4ZmJkMmNkZjllMGYyZWZhNDg0YWQxMDYvNzEwZjNlNjY2NjYyZmM1OWUwMzEzNzhiNmIxNmE4OWZhODQ4NDRkYWU1NmJjYWI2YmJjOWEzMDYyOTUwMWRiYg=="
-H "Content-Type: application/json"
-d
'{
  "name": "City of Angels",
  "merch_locationid": "LosAngeles",
  "description": "LA"
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
location_key	string	Gateway generated inventory location (warehouse) identifier. This will be passed in the endpoint.
merch_locationid	string	The inventory location name specified by the merchant.
name	string	The inventory location name specified by the merchant.
description	string	The inventory location description.
Response Parameters

Example Response

{
    "key": "mnyyj94tyvsc4qb33",
    "type": "inventorylocation",
    "merch_locationid": "LosAngeles",
    "name": "City of Angels",
    "description": "LA"
}

Parameter Name	Type	Description
key	string	Gateway generated inventory location (warehouse) identifier.
type	string	The object type. Successful calls will always return inventorylocation.
merch_locationid	string	The inventory location name specified by the merchant.
name	string	The inventory location name specified by the merchant.
description	string	The inventory location description.
Delete Specific Location

Endpoint

DELETE /api/v2/inventory/location/:location_key:

Request Parameters

Example Request

curl -X DELETE https://sandbox.usaepay.com/api/v2/inventory/locations/mnyyj94tyvsc4qb33
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNjgzY2RjNzQyMmU0NmQ2NzQ4NGEwMDdmZDU0ZjJiMzYvYzVlNjFmM2RlNDY0MzVlODU4NDdiNDliYzlhYWI5ZDk0OGE2Y2Q5ZDkyYTAyMTk1M2M3MmRlMTQwMmQyZTIwZg=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
location_key	string	Gateway generated inventory location (warehouse) identifier. This will be passed in the endpoint.
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If location has been deleted then status will be returned as success. If the inventory location is NOT deleted, an error will be returned instead.
Inventories
api/v2/inventory


Use the inventory endpoint to create, delete, and manage inventories.

Create Inventory

Endpoint

POST /api/v2/inventory


This method allows you to create an inventory.

Request Parameters

Example Request

curl -X POST https://sandbox.usaepay.com/api/v2/inventory
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvZWJjMWI0YWIxYmU4MDI3ODUwNzg5OGZmYzBhMDg0ZWUvNzNkNmE0Mjc2YTIwYWNkMjYzZDM0YTYwZjY5YjdjYWI2NjRlY2Y4YjVjNmYzMjJhYWNlZWI0ZTBhY2UyMDE4NQ=="
-H "Content-Type: application/json"
-d
'{
  "qtyonhand": "500",
  "qtyonorder": "50",
  "product_key": "as4b4s9sfbwzd1v8",
  "location_key": "pnyyj79fhzq8grx30"
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
qtyonhand	integer	Quantity on hand for this product/location combination.
qtyonorder	integer	Quantity on order for this product/location combination.
product_key	string	Gateway generated product identifier.
location_key	string	Unique identifier for warehouse location.
Response Parameters

Example Response

{
    "key": "2nmxtbkvdxy1fw16r",
    "type": "product_inventory",
    "qtyonhand": 500,
    "qtyonorder": 50,
    "date_available": "",
    "product": {
        "type": "product",
        "key": "as4b4s9sfbwzd1v8",
        "product_refnum": "10",
        "name": "A History of Magic by Bathilda Bagshot",
        "price": "20.00",
        "enabled": "Y",
        "taxable": "Y",
        "sku": ""
    },
    "location": {
        "key": "pnyyj79fhzq8grx30",
        "type": "inventorylocation",
        "merch_locationid": "Los Angeles",
        "name": "Los Angeles",
        "description": "West coast hub"
    }
}

Parameter Name	Type	Description
key	string	Gateway generated inventory identifier.
type	string	The object type. Successful calls will always return product_inventory.
qtyonhand	integer	Quantity on hand for this product/location combination.
qtyonorder	integer	Quantity on order for this product/location combination.
date_available	string	Date product will become available.
product	object	Product object.
location	object	Location (warehouse) object.
Retrieve Specific Inventory

Endpoint

GET /api/v2/inventory/:inventory_key:


This method allows you to retrieve the totals for a specific product and location.

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/inventory/2nmxtbkvdxy1fw16r
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNmQ5OGUwZTdjNWQzYjA3Mjc5NWM2NGY4MGFjMzVkZDgvM2IxNGQ1ZjE3N2UxYWY1ZmY5M2U0YjU3YmE0MzM5NDViNmM3ZGM2M2VmYjAxYWJlNGM3MDRhN2RiZDlhMGM4ZQ=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
inventory_key	string	Gateway generated inventory identifier. This is passed through the endpoint.
Response Parameters

Example Response

{
    "key": "2nmxtbkvdxy1fw16r",
    "type": "product_inventory",
    "qtyonhand": "500",
    "qtyonorder": "50",
    "date_available": "0000-00-00",
    "product": {
        "type": "product",
        "key": "as4b4s9sfbwzd1v8",
        "product_refnum": "10",
        "name": "A History of Magic by Bathilda Bagshot",
        "price": "20.00",
        "enabled": "Y",
        "taxable": "Y",
        "sku": ""
    },
    "location": {
        "key": "pnyyj79fhzq8grx30",
        "type": "inventorylocation",
        "merch_locationid": "Los Angeles",
        "name": "Los Angeles",
        "description": "West coast hub"
    }
}

Parameter Name	Type	Description
key	string	Gateway generated inventory identifier.
type	string	The object type. Successful calls will always return product inventory.
qtyonhand	integer	Quantity on hand for this product/location combination.
qtyonorder	integer	Quantity on order for this product/location combination.
date_available	string	Date product will become available.
product	object	Product object.
location	object	Location (warehouse) object.
Retrieve Inventory List

Endpoint

GET /api/v2/inventory

Request Parameters

Example Request

curl -X GET https://sandbox.usaepay.com/api/v2/inventory?limit=2&offset=0
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvN2YwNWQ1NjVmMGY3NmNiYzA0ZTIzZjQzMTU0MDBjMTkvMmI3N2M2YTM2MmI1NWY4ZWQxYzcwNTYyOTU3NGJmMWZjYzAxYmJjZmQ5YjU4MDY0NGNiZDc2N2U0ZWY3MDU5YQ=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
limit	integer	Limit setting for this response. Used for pagination. This will default to "20".
offset	integer	Offset setting for this response. This will default to "0".
Response Parameters

Example Response

{
    "type": "list",
    "limit": 2,
    "offset": 0,
    "data": [
        {
            "key": "anmxtbkr3kg6n0jc3",
            "type": "product_inventory",
            "qtyonhand": "100",
            "qtyonorder": "0",
            "product": {
                "type": "product",
                "key": "as4b4s9sfbwzd1v8",
                "name": "A History of Magic by Bathilda Bagshot",
                "price": "20.00",
                "enabled": "Y",
                "taxable": "Y",
                "sku": ""
            },
            "location": {
                "key": "dnyyjc8s2vbz8hb33",
                "type": "inventorylocation",
                "merch_locationid": "London",
                "name": "London",
                "description": ""
            }
        },
        {
            "key": "new",
            "type": "product_inventory",
            "qtyonhand": "0",
            "qtyonorder": "0",
            "product": {
                "type": "product",
                "key": "as4b4s9sfbwzd1v8",
                "name": "A History of Magic by Bathilda Bagshot",
                "price": "20.00",
                "enabled": "Y",
                "taxable": "Y",
                "sku": ""
            },
            "location": {
                "key": "gnyyj6yv3jf05pdb3",
                "type": "inventorylocation",
                "merch_locationid": "New York",
                "name": "New York",
                "description": ""
            }
        }
      ],
      "total": "10"
  }

Parameter Name	Type	Description
type	string	The type of object returned. Returns a list.
limit	integer	The maximum amount of product categories that will be included in response.
offset	integer	The number of product categories skipped from the results.
data	array	An array of inventories matching the request.
total	integer	The total amount of product categories, including filtered results.
Update Inventory

Endpoint

PUT /api/v2/inventory/:inventory_key:


This endpoint updates already existing product inventories. Only fields that are passed through will be updated. Fields must be passed through as 0 to remove the inventory.

Request Parameters

Example Request

curl -X PUT https://sandbox.usaepay.com/api/v2/inventory/5nmxt2gyrxhrgrgb3 -H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvM2U1NDU0OTViNmIxZjhlZTBlMmU3YjE5OWI4ZjkzYjMvODQ2MmZmZDczMGRmNWI2MjAzN2QxYmY4NzQzZGM4M2M4NjI4NzY2MjczNTM4ZmM3MDU3YmViMWM4OTlkZmYwNg=="
-H "Content-Type: application/json"
-d
'{
  "qtyonhand": "25",
  "qtyonorder": "25",
  "date_available": "2018-08-25"
}'


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
inventory_key	string	Gateway generated inventory identifier. This is passed through the endpoint. (Required)
qtyonhand	integer	Quantity on hand for this product/location combination.
qtyonorder	integer	Quantity on order for this product/location combination.
date_available	string	Date product will become available.
Response Parameters

Example Response

{
    "key": "5nmxt2gyrxhrgrgb3",
    "type": "product_inventory",
    "qtyonhand": 25,
    "qtyonorder": 25,
    "date_available": "2018-08-25",
    "product": {
        "type": "product",
        "key": "as4b4s9sfbwzd1v8",
        "product_refnum": "10",
        "name": "A History of Magic by Bathilda Bagshot",
        "price": "20.00",
        "enabled": "Y",
        "taxable": "Y",
        "sku": ""
    },
    "location": {
        "key": "pnyyj79fhzq8grx30",
        "type": "inventorylocation",
        "merch_locationid": "Los Angeles",
        "name": "Los Angeles",
        "description": "West coast hub"
    }
}

Parameter Name	Type	Description
key	string	Gateway generated inventory identifier.
type	string	The object type. Successful calls will always return product_inventory.
qtyonhand	integer	Quantity on hand for this product/location combination.
qtyonorder	integer	Quantity on order for this product/location combination.
date_available	string	Date product will become available.
product	object	Product object.
location	object	Location (warehouse) object.
Delete Specific Inventory

Endpoint

DELETE /api/v2/inventory/:inventory_key:

Request Parameters

Example Request

curl -X DELETE https://sandbox.usaepay.com/api/v2/inventory/2nmxtbkvdxy1fw16r
-H "Authorization: Basic XzduMjhub0YwOTVoNTh5bUM2UjhIODcyRlp2MnI5Z1o6czIvNDk0MWNjMGRmY2JjYTZhOWY5YmY0NmY3MGE1MDgyOTgvNTMxM2Q2NDk5NzEyNDIzNWRkNjVkMzBkN2ZiYWQxMTZjNWI0ZjkzOGZjMzA3Nzg0YzNhYWI0MDYyNTUzNjBhMw=="
-H "Content-Type: application/json"


Parameters in marked Required are required to process this request.

Parameter Name	Type	Description
inventory_key	string	Gateway generated inventory identifier. This is passed through the endpoint.
Response Parameters

Example Response

{
    "status": "success"
}

Parameter Name	Type	Description
status	string	If inventory has been deleted then status will be returned as success. If the inventory is NOT deleted, an error will be returned instead.
Inventory Webhook Events

Product inventory events are triggered when there are are changes to the product inventory.

Event Name	Description
product.inventory.ordered	The inventory has been adjusted after a sale.
product.inventory.adjusted	The inventory has been adjusted in the Products Database.
Webhook Responses
Overview

Webhooks provide developers with automatic notifications when certain events occur in the gateway. This is a more efficient, realtime solution than repeatedly polling the api for updated information. Webhook events are delivered via HTTPS POST in a JSON format. The developer is responsible for providing a URL endpoint to receive the notification. Multiple event types can be sent to the same URL endpoint and multiple URL endpoints can be configured.

Available Events
Event Name	Description
ach.submitted	ACH status updated to submitted.
ach.settled	ACH status updated to settled.
ach.returned	ACH status updated to returned.
ach.voided	ACH status updated to voided.
ach.failed	ACH status updated to failed.
ach.note_added	Note added to ACH transaction.
cau.created	Card has been queued for update.
cau.submitted	Card has been submitted to processor for update
cau.updated_expiration	An updated expiration data has been received
cau.updated_card	An updated card number has been received
cau.contact_customer	Received message from issuer to contact customer for a new card number
cau.account_closed	Received notification of account closure
product.inventory.ordered	The inventory has been adjusted after a sale.
product.inventory.adjusted	The inventory has been adjusted in the Products Database.
transaction.sale.success	A transaction sale approved
transaction.sale.failure	A transaction sale failed - includes declines and errors
transaction.sale.voided	A transaction sale was voided
transaction.sale.captured	A transaction sale was captured
transaction.sale.adjusted	A transaction sale was adjusted
transaction.sale.queued	A transaction sale was queued
transaction.sale.unvoided	A voided transaction sale was unvoided
transaction.refund.success	A transaction refund approved
transaction.refund.voided	A transaction refund was voided.
Configuration

The webhooks are configured on a per merchant account basis. Click here for information on how to configure webhooks. For legacy accounts, please contact integration support to configure webhooks for an account.

Event Object

The payload object for all events will follow a common schema. Common fields are described below.

Field	Description
type	event
event_triggered	Timestamp for when the event was triggered.
event_type	The type of event that was triggerd. This will be one of the events in the available events list.
event_body	Object containing:
- merchant- The merchant key for related to the event.
- object- Subset of data from object involved in the event.
- changes- old- For "changed" events, this will contain key/values for data prior to change. new- For "changed" events, this will contain key/values for data after the change.
event_id	Unique identifier for triggered event.
Throughput

Delivery of events is multi-threaded and non-sequential. It is possible that multiple events will be delivered at the same time and depending on system and network latency, events may not be delivered in the exact order they were triggered. The event payload will contain the event timestamp which can be used to validate the order in which events occurred.

Delivery is limited to 20 simultaneous connections per endpoint to prevent swamping the developers server. This may lead to delivery lag if a large number events are triggered at the same time and/or the developers server is slow to respond.

Error Handling

Webhook delivery is configured with a 5 second socket timeout and a 45 second read timeout. The successful HTTP response is required. If the connection times out or an error is received (HTTP 4xx or 5xx) the event will be retried:

every 5 minutes for first hour
every hour for the next 11 hours
every 3 hours for the next 12 hours
every 6 hours for the next 48 hours

Note: The url endpoint must have a valid SSL certificate and support TLS 1.2. Self-signed certificates are not supported.

Event Responses
ACH Status Events

ACH Status events fire when updates from the ACH processor are received. If you would like hooks to fire for all tender types, you will want to use the Transaction Webhooks instead. The following webhook events are available for ACH Status Updates:

Event Name	Description
ach.submitted	ACH status updated to submitted.
ach.settled	ACH status updated to settled.
ach.returned	ACH status updated to returned.
ach.voided	ACH status updated to voided.
ach.failed	ACH status updated to failed.
ach.note_added	Note added to ACH transaction.
ACH Submitted

Example ACH Submitted Response

{
  "type": "event",
  "event_triggered": "2021-05-19 08:42:19",
  "event_type": "ach.submitted",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "0nftkkdy9v21dn8",
      "refnum": "100769",
      "orderid": "",
      "check": {
        "trackingcode": "21051921438530"
      },
      "uri": "transactions/0nftkkdy9v21dn8"
    },
    "changes": {
      "old": {
        "status": "P",
        "processed": null
      },
      "new": {
        "status": "B",
        "processed": "2021-05-20"
      }
    }
  },
  "event_id": "319f09a1d3a7f75e0b962d471254c18e87450abfc375b28f3aa2aa046e0c57e5"
}


This event will trigger when an ACH status updated to submitted. Transaction has been submitted to the bank for processing. The transaction can no longer be voided and refund must be issued instead.

ACH events will all contain a transaction object in the object parameter and what has changed will be in the changes object. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
changes	object	Logs what fields changed during the update and displays the old and new values.
ACH Settled

Example ACH Settled Response

{
  "type": "event",
  "event_triggered": "2021-05-19 09:03:04",
  "event_type": "ach.settled",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "0nftkkdy9v21dn8",
      "refnum": "100769",
      "orderid": "",
      "check": {
        "trackingcode": "21051921438530"
      },
      "uri": "transactions/0nftkkdy9v21dn8"
    },
    "changes": {
      "old": {
        "status": "B",
        "settled": null
      },
      "new": {
        "status": "S",
        "settled": "2021-05-20"
      }
    }
  },
  "event_id": "bb247c915f3eeaecee5c353e9fced863c433fc7a536f3957a5d5eb5c76fb6a6d"
}


This event will trigger when an ACH status updated to settled.

ACH events will all contain a transaction object in the object parameter and what has changed will be in the changes object. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
changes	object	Logs what fields changed during the update and displays the old and new values.
ACH Returned

Example ACH Returned Response

{
  "type": "event",
  "event_triggered": "2021-05-19 09:14:15",
  "event_type": "ach.returned",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "0nftkkdy9v21dn8",
      "refnum": "100769",
      "orderid": "",
      "check": {
        "trackingcode": "21051921438530"
      },
      "uri": "transactions/0nftkkdy9v21dn8"
    },
    "changes": {
      "old": {
        "status": "S",
        "returned": null,
        "reason": ""
      },
      "new": {
        "status": "R",
        "returned": "2021-05-20",
        "reason": "last update"
      }
    }
  },
  "event_id": "c2c8f33ed40a29c53cc15191f257a08f6d33e66948c613ae2955661b4e225b75"
}


This event will trigger when an ACH status updated to returned. Transaction has been returned by either the customers bank or by the check processor. reason will contain details on why the transaction was returned. A transaction may be returned after it has settled in which case the funds will be withdrawn from the merchants account. If it is returned before settlement then the funds will not be deposited at all.

ACH events will all contain a transaction object in the object parameter and what has changed will be in the changes object. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
changes	object	Logs what fields changed during the update and displays the old and new values.
ACH Voided

Example ACH Voided Response

{
  "type": "event",
  "event_triggered": "2021-05-19 09:21:58",
  "event_type": "ach.voided",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "0nftkkdy9v21dn8",
      "refnum": "100771",
      "orderid": "8521adsf5312",
      "check": {
        "trackingcode": "21051921441240"
      },
      "uri": "transactions/0nftkkdy9v21dn8"
    },
    "changes": {
      "old": {
        "status": "P"
      },
      "new": {
        "status": "V"
      }
    }
  },
  "event_id": "c67eb8b4d2c6eeae7c8effae3c3e486c0131593ddbd743c98dbb1ac746bd9130"
}


This event will trigger when an ACH status updated to voided. Check transaction has been voided (canceled) prior to being submitted to bank.

ACH events will all contain a transaction object in the object parameter and what has changed will be in the changes object. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
changes	object	Logs what fields changed during the update and displays the old and new values.
ACH Failed

Example ACH Failed Response

{
  "type": "event",
  "event_triggered": "2021-05-19 09:21:58",
  "event_type": "ach.failed",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "0nftkkdy9v21dn8",
      "refnum": "100771",
      "orderid": "8521adsf5312",
      "check": {
        "trackingcode": "21051921441240"
      },
      "uri": "transactions/0nftkkdy9v21dn8"
    },
    "changes": {
      "old": {
        "status": "B",
                "reason": ""
      },
      "new": {
        "status": "E",
                "reason": "Merchant processing disabled."
      }
    }
  },
  "event_id": "c67eb8b4d2c6eeae7c8effae3c3e486c0131593ddbd743c98dbb1ac746bd9130"
}


This event will trigger when an ACH status updated to failed. Check processor has marked transaction as an error. This is only supported on some processors and usually is a result of merchant config or boarding issue. This event is triggered only when the transaction errors out after it was initially accepted (approved) for processing.

ACH events will all contain a transaction object in the object parameter and what has changed will be in the changes object. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
changes	object	Logs what fields changed during the update and displays the old and new values.
ACH Note Added

Example ACH Note Added Response

{
  "type": "event",
  "event_triggered": "2021-05-19 09:21:58",
  "event_type": "ach.failed",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "0nftkkdy9v21dn8",
      "refnum": "100771",
      "orderid": "8521adsf5312",
      "check": {
        "trackingcode": "21051921441240"
      },
      "uri": "transactions/0nftkkdy9v21dn8"
    },
    "changes": {
      "old": {
                "banknote": ""
      },
      "new": {
        "status": "S",
                "banknote": "C02:Corrected Routing:123456789"
      }
    }
  },
  "event_id": "c67eb8b4d2c6eeae7c8effae3c3e486c0131593ddbd743c98dbb1ac746bd9130"
}


This event will trigger when a note is added to the ACH transaction by the bank. A change notification was received from the bank. This is typically used to advise of account or routing number changes.

ACH events will all contain a transaction object in the object parameter and what has changed will be in the changes object. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
changes	object	Logs what fields changed during the update and displays the old and new values.
Card Update Events

Card updater events are triggered when there are new updates from the card updater service or when cards are submitted for updates.

Event Name	Description
cau.created	Card has been queued for update.
cau.submitted	Card has been submitted to processor for update
cau.updated_expiration	An updated expiration data has been received
cau.updated_card	An updated card number has been received
cau.contact_customer	Received message from issuer to contact customer for a new card number
cau.account_closed	Received notification of account closure
Card Update Created

Example Card Update Submitted

{
  "type": "event",
  "event_triggered": "2021-04-19 09:28:15",
  "event_type": "cardupdate.created",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjy5d7ccsx2"
    },
    "object": {
      "key": "enq8rb72rg4bf8h45",
      "type": "cardupdate",
      "original_card": {
        "number": "4444xxxxxxxxx7779",
        "expiration": "0000",
        "type": "Visa"
      },
      "added": "2021-04-19 09:28:15",
      "status": "pending",
      "status_description": "No Response",
      "source": {
        "object": "transaction",
        "type": "api",
        "key": "dnft34x401ckg2b"
      }
    }
  },
  "event_id": "92e494ae2ab56c3ec47754c72bf4b09cc09c8d44a6b12d63f65ccfe11fd14a03"
}


This event will trigger when a card has been submitted to processor for update.

Card Update events will all contain a cardline object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Card Update object related to the event.
Card Update Submitted

Example Card Update Submitted

{
  "type": "event",
  "event_triggered": "2021-04-19 09:28:15",
  "event_type": "cardupdate.submitted",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjy5d7ccsx2"
    },
    "object": {
      "key": "enq8rb72rg4bf8h45",
      "type": "cardupdate",
      "original_card": {
        "number": "4444xxxxxxxxx7779",
        "expiration": "0000",
        "type": "Visa"
      },
      "added": "2021-04-19 09:28:15",
      "status": "submitted",
      "source": {
        "object": "transaction",
        "type": "api",
        "key": "dnft34x401ckg2b"
      }
    },
    "changes": {
      "old": {
                "status": "Pending"
      },
      "new": {
        "status": "Submitted"
      }
    }
  },
  "event_id": "92e494ae2ab56c3ec47754c72bf4b09cc09c8d44a6b12d63f65ccfe11fd14a03"
}


This event will trigger when a card has been submitted to processor for update.

Card Update events will all contain a cardline object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Card Update object related to the event.
changes	object	Logs what fields changed during the update and displays the old and new values.
Card Updater Updated Expiration

Example Card Updater Updated Expiration

{
  "type": "event",
  "event_triggered": "2021-04-19 09:28:15",
  "event_type": "cardupdate.submitted",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjy5d7ccsx2"
    },
    "object":   {
      "key": "lnq8rtgb3thyx6bb2",
        "type": "cardupdate",
        "original_card": {
            "number": "4000xxxxxxxxx2220",
            "expiration": "0000",
            "type": "Visa"
        },
        "added": "2021-05-19 12:06:16",
        "status": "matched",
        "updated_card": {
            "number": "4000xxxxxxxxx2220",
            "expiration": "0525",
            "type": "Visa"
        },
        "source": {
            "object": "transaction",
            "type": "api",
            "key": "anf0cybmfg3pg01"
        }
        },
    "changes": {
      "old": {
                "status": "submitted",
                "original_card": {
                "expiration": "0000"
                    },

      },
      "new": {
        "status": "matched",
                "updated_card": {
                "expiration": "0525"
                    }
                }
            }
        },
  "event_id": "92e494ae2ab56c3ec47754c72bf4b09cc09c8d44a6b12d63f65ccfe11fd14a03"
}


This event will trigger when an updated expiration data has been received.

Card Update events will all contain a cardline object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Card Update object related to the event.
changes	object	Logs what fields changed during the update and displays the old and new values.
Card Updater Updated Card

Example Card Updater Updated Card

{
  "type": "event",
  "event_triggered": "2021-04-19 09:28:15",
  "event_type": "cardupdate.submitted",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjy5d7ccsx2"
    },
    "object":   {
      "key": "lnq8rtgb3thyx6bb2",
        "type": "cardupdate",
        "original_card": {
            "number": "4000xxxxxxxxx2220",
            "expiration": "0000",
            "type": "Visa"
        },
        "added": "2021-05-19 12:06:16",
        "status": "matched",
        "updated_card": {
            "number": "4000xxxxxxxxx9562",
            "expiration": "0525",
            "type": "Visa"
        },
        "source": {
            "object": "transaction",
            "type": "api",
            "key": "anf0cybmfg3pg01"
        }
        },
    "changes": {
      "old": {
                "status": "submitted",
                "original_card": {
                        "number": "4000xxxxxxxxx2220",
                        "expiration": "0000"
                    },

      },
      "new": {
        "status": "matched",
                "updated_card": {
                      "number": "4000xxxxxxxxx9562",
                "expiration": "0525"
                    }
                }
            }
        },
  "event_id": "92e494ae2ab56c3ec47754c72bf4b09cc09c8d44a6b12d63f65ccfe11fd14a03"
}


This event will trigger when an updated card number has been received.

Card Update events will all contain a cardline object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Card Update object related to the event.
changes	object	Logs what fields changed during the update and displays the old and new values.
Card Updater Contact Customer

Example Card Updater Updated Card

{
  "type": "event",
  "event_triggered": "2021-04-19 09:28:15",
  "event_type": "cardupdate.contact_customer",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjy5d7ccsx2"
    },
    "object": {
      "key": "enq8rb72rg4bf8h45",
      "type": "cardupdate",
      "original_card": {
        "number": "4444xxxxxxxxx7779",
        "expiration": "0000",
        "type": "Visa"
      },
      "status": "pending",
      "source": {
        "object": "transaction",
        "type": "api",
        "key": "dnft34x401ckg2b"
      }
    },
    "changes": {
      "old": {
                "contact_cusomer": ""
      },
      "new": {
        "contact_cusomer": true
      }
    }
  },
  "event_id": "92e494ae2ab56c3ec47754c72bf4b09cc09c8d44a6b12d63f65ccfe11fd14a03"
}



This event will trigger when the card updater prompts the merchant to reach out to the customer for an updated card.

Card Update events will all contain a cardline object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Card Update object related to the event.
changes	object	Logs what fields changed during the update and displays the old and new values.
Card Updater Customer Account Closed

Example Card Updater Updated Card

{
  "type": "event",
  "event_triggered": "2021-04-19 09:28:15",
  "event_type": "cardupdate.account_closed",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjy5d7ccsx2"
    },
    "object": {
      "key": "enq8rb72rg4bf8h45",
      "type": "cardupdate",
      "original_card": {
        "number": "4444xxxxxxxxx7779",
        "expiration": "0000",
        "type": "Visa"
      },
      "status": "pending",
      "source": {
        "object": "transaction",
        "type": "api",
        "key": "dnft34x401ckg2b"
      }
    },
    "changes": {
      "old": {
                "cardaccount_closed": ""
      },
      "new": {
        "cardaccount_closed": true
      }
    }
  },
  "event_id": "92e494ae2ab56c3ec47754c72bf4b09cc09c8d44a6b12d63f65ccfe11fd14a03"
}


This event will trigger when the card updater gives notification of closure of the customers account.

Card Update events will all contain a cardline object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Card Update object related to the event.
changes	object	Logs what fields changed during the update and displays the old and new values.
Product Inventory Events

Product inventory events are triggered when there are changes to the product inventory.

Event Name	Description
product.inventory.ordered	The inventory has been adjusted after a sale.
product.inventory.adjusted	The inventory has been adjusted in the Products Database.
Product Inventory Ordered

Example Product Inventory Ordered

{
  "type": "event",
  "event_triggered": "2021-05-19 10:06:14",
  "event_type": "product.inventory.ordered",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "product",
      "key": "4s4b4qrn3k4pmsg8",
      "locationid": "13",
      "qtyonhand": 127,
      "qtyonhand_change": "-6",
      "uri": "products/4s4b4qrn3k4pmsg8"
    }
  },
  "event_id": "c1a50d9162281c7a3f24a6df7fc95798e294f880051a007c72d88104daaa3690"
}


This event will trigger when The inventory has been adjusted in through a sale.

Product inventory events will all contain a product object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Product object related to the event.
Product Inventory Adjusted

Example Product Inventory Adjusted

{
  "type": "event",
  "event_triggered": "2021-05-19 10:05:18",
  "event_type": "product.inventory.adjusted",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "product",
      "key": "4s4b4qrn3k4pmsg8",
      "locationid": "16",
      "qtyonhand": 325,
      "qtyonhand_change": "+300",
      "uri": "products/4s4b4qrn3k4pmsg8"
    }
  },
  "event_id": "6512f25e53a467ae75ac4b2fcfcda71de7441e6dfc9b169f0e1b13a5df644061"
}


This event will trigger when the inventory has been adjusted in the Products Database.

Product inventory events will all contain a product object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Product object related to the event.
Transaction Events

Transaction change events are triggered when a transaction record is updated. The overall transaction event is fired for all transaction types (creditcard,check,etc). The following webhook events are available for transaction processing:

Event Name	Description
transaction.sale.success	A transaction sale approved
transaction.sale.failure	A transaction sale failed - includes declines and errors
transaction.sale.voided	A transaction sale was voided
transaction.sale.captured	A transaction sale was captured
transaction.sale.adjusted	A transaction sale was adjusted
transaction.sale.queued	A transaction sale was queued
transaction.sale.unvoided	A voided transaction sale was unvoided
transaction.refund.success	A transaction refund approved
transaction.refund.voided	A transaction refund was voided.
Transaction Sale Success

Example Transaction Sale Success

{
  "type": "event",
  "event_triggered": "2021-05-19 05:20:34",
  "event_type": "transaction.sale.success",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "8nf0cyn9tfp5q3x",
      "refnum": 100758,
      "auth_amount": "500.00",
      "amount": "500.00",
      "amount_detail": {
        "tax": "5.00",
        "discount": "10.00",
        "subtotal": "450.00",
        "duty": "10.00"
      },
      "authcode": "902366",
      "result_code": "A",
      "invoice": "98454685",
      "avs": {
        "result_code": "YYY",
        "result": "Address: Match & 5 Digit Zip: Match"
      },
      "cvc": {
        "result_code": "P",
        "result": "Not Processed"
      },
      "trantype": "Credit Card Sale",
      "creditcard": {
        "cardholder": "Minerva M",
        "entry_mode": "Card Not Present, Manually Keyed",
        "type": "V",
        "number": "4444xxxxxxxx1111"
      },
      "batch": {
        "batchrefnum": "430411",
        "sequence": "1015",
        "type": "batch",
        "key": "7t1q51d5s28jcxs"
      },
      "receipts": {
        "customer": "Mail Sent Successfully",
        "merchant": "Mail Sent Successfully"
      },
      "result": "Approved"
    }
  },
  "event_id": "8d0bba34f7179dc5e0074ffc06d2ec57831da981cbb6230266f017f2bb9de3f0"
}


This event will trigger when a successful sale transaction is processed.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Sale Failure

Example Transaction Sale Failure

{
  "type": "event",
  "event_triggered": "2021-05-19 06:41:31",
  "event_type": "transaction.sale.failure",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "anf0cybmfg3pg01",
      "refnum": 100760,
      "auth_amount": "0.00",
      "amount": "500.00",
      "amount_detail": {
        "tax": "45.00",
        "discount": "50.00",
        "subtotal": "450.00"
      },
      "authcode": "",
      "result_code": "D",
      "invoice": "98454685",
      "avs": {
        "result_code": ""
      },
      "cvc": {
        "result_code": ""
      },
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "type": "V",
        "number": "4000xxxxxxxx2220"
      },
      "batch": {
        "batchrefnum": "430411",
        "sequence": "1015",
        "type": "batch",
        "key": "7t1q51d5s28jcxs"
      },
      "result": "Declined"
    }
  },
  "event_id": "5d1c0544e9288f87a22d31c7bf1627b3904b303d5b094a556c8b625f38ee1bca"
}


This event will trigger when a failed sale tranaction is processed.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Sale Voided

Example Transaction Sale Voided

{
  "type": "event",
  "event_triggered": "2021-05-19 06:58:30",
  "event_type": "transaction.sale.voided",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "9nf0fnh01msg23w",
      "refnum": "100759",
      "amount_detail": {
        "original_amount": "500.00"
      },
      "authcode": "902762",
      "result_code": "A",
      "result": "Approved",
      "creditcard": {
        "type": "V"
      },
      "original_date": "2021-05-19 06:40:43"
    }
  },
  "event_id": "b94bb4aca0860bf955d9738b257c79846872b138b0926ee7e47e11763c37e570"
}


This event will trigger when a sale transaction is voided or funds are released.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Sale Unvoided

Example Transaction Sale Queued

{
  "type": "event",
  "event_triggered": "2021-05-19 07:02:44",
  "event_type": "transaction.sale.unvoided",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "9nf0fnh01msg23w",
      "refnum": "100759",
      "amount_detail": {
        "original_amount": "500.00"
      },
      "authcode": "902762",
      "result_code": "A",
      "result": "Approved",
      "creditcard": {
        "type": "V"
      },
      "original_date": "2021-05-19 06:40:43"
    }
  },
  "event_id": "cffcdb0616d5dad5a4364d617bc97e43d8e3d3050f706b12f0af66e8284206e0"
}


This event will trigger when a transaction void is reversed.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Sale Captured

Example Transaction Sale Captured

{
  "type": "event",
  "event_triggered": "2021-05-19 07:18:01",
  "event_type": "transaction.sale.captured",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "bnftwqt20rkt282",
      "refnum": "100761",
      "auth_amount": "50.00",
      "amount": "50.00",
      "amount_detail": {
        "original_amount": "500.00"
      },
      "authcode": "903034",
      "result_code": "A",
      "avs": {
        "result_code": ""
      },
      "cvc": {
        "result_code": ""
      },
      "batch": {
        "batchrefnum": "430411",
        "sequence": "1015",
        "type": "batch",
        "key": "7t1q51d5s28jcxs"
      },
      "result": "Approved",
      "creditcard": {
        "type": "V"
      },
      "original_date": "2021-05-19 07:14:45"
    }
  },
  "event_id": "8828773bf51380234036ae005f2e9a4f1542b0e29fc67351c182e2502169348a"
}


This event will trigger when transaction is captured.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Sale Adjusted

Example Transaction Sale Adjust

{
  "type": "event",
  "event_triggered": "2021-05-19 07:21:16",
  "event_type": "transaction.sale.adjusted",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "bnftwqt20rkt282",
      "refnum": "100761",
      "auth_amount": "5.00",
      "amount": "5.00",
      "amount_detail": {
        "original_amount": "50.00"
      },
      "authcode": "903058",
      "result_code": "A",
      "avs": {
        "result_code": ""
      },
      "cvc": {
        "result_code": ""
      },
      "batch": {
        "batchrefnum": "430411",
        "sequence": "1015",
        "type": "batch",
        "key": "7t1q51d5s28jcxs"
      },
      "result": "Approved",
      "creditcard": {
        "type": "V"
      },
      "original_date": "2021-05-19 07:14:45"
    }
  },
  "event_id": "85814705435e14c64256d8a45d9a31689965c78777a8429d4110a467b0bd9506"
}


This event will trigger when transaction is adjusted. This includes changing the original auth amount, adding a signature, or adding Level 3 information after the original authorization.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Sale Queued

Example Transaction Sale Queued

{
  "type": "event",
  "event_triggered": "2021-05-19 07:15:22",
  "event_type": "transaction.sale.queued",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "bnftwqt20rkt282",
      "refnum": "100761",
      "amount": "5.00",
      "amount_detail": {
        "original_amount": "500.00"
      },
      "authcode": "902990",
      "result_code": "A",
      "result": "Approved",
      "creditcard": {
        "type": "V"
      },
      "original_date": "2021-05-19 07:14:45"
    }
  },
  "event_id": "df7ec3fff5a0176482e81a1c61d263c3fe7fa857d0ce9fd03833aff523ee2d29"
}


This event will trigger when a transaction is queued.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Refund Success

Example Transaction Refund Success

{
  "type": "event",
  "event_triggered": "2021-05-19 08:00:25",
  "event_type": "transaction.refund.success",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "enf0c7fms98f8sm",
      "refnum": 100764,
      "auth_amount": "5.00",
      "amount_detail": {
        "original_amount": "5.00"
      },
      "authcode": "100764",
      "result_code": "A",
      "avs": {
        "result_code": "YYY",
        "result": "Address: Match & 5 Digit Zip: Match"
      },
      "cvc": {
        "result_code": "P",
        "result": "Not Processed"
      },
      "batch": {
        "batchrefnum": "430867",
        "sequence": "1016",
        "type": "batch",
        "key": "lt18nky3x82zp1s"
      },
      "result": "Approved",
      "creditcard": {
        "type": "V"
      }
    }
  },
  "event_id": "449f519a0b0933838a1e2717adef0a2b27fd03168864314f1eada47d5a583c34"
}


This event will trigger when a transaction is refunded.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Transaction Refund Voided

Example Transaction Refund Void

{
  "type": "event",
  "event_triggered": "2021-05-19 08:14:32",
  "event_type": "transaction.refund.voided",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "transaction",
      "key": "inft65j3pk333pr",
      "refnum": "100768",
      "amount_detail": {
        "original_amount": "100.00"
      },
      "authcode": "",
      "result_code": "A",
      "result": "Approved",
      "creditcard": {
        "type": "V"
      },
      "original_date": "2021-05-19 08:10:35"
    }
  },
  "event_id": "0eb1780ba07d77dbf5a96660ec052f5b1a1af8ac577b5cdd157ec1c61065fb59"
}


This event will trigger when a transaction refund is voided.

Transaction events will all contain a transaction object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Settlement Events

Settlement change events are triggered when there is an attempt to settle a credit card batch. The following webhook events are available for settlement:

Event Name	Description
settlement.batch.success	A batch settles successfully
settlement.batch.failure	A batch receives an error when attempting to settle
Settlement Batch Success

Example Settlement Batch Success

{
  "type": "event",
  "event_triggered": "2021-05-19 07:37:09",
  "event_type": "settlement.batch.success",
  "event_body": {
    "merchant": {
      "merch_key": "5qwnn5b51gnkd1b"
    },
    "object": {
      "type": "batch",
      "key": "7t1d2xkccqb1v7s",
      "batchnum": "1015",
      "batchrefnum": "430411",
      "response": "A",
      "totalsales": "617799",
      "numsales": 41,
      "totalcredits": "000",
      "uri": "batches/7t1d2xkccqb1v7s"
    }
  },
  "event_id": "46820ed61cdd70e8ba8a8a6d007f641139bd1db3df216aa45b493bc18147cd3a"
}


This event will trigger when a batch is successfully settled.

Settlement events will all contain a batch object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Settlement Batch Failure

Example Settlement Batch Failure

{
  "type": "event",
  "event_triggered": "2021-04-14 10:22:20",
  "event_type": "settlement.batch.failure",
  "event_body": {
    "merchant": {
      "merch_key": "eqwnpjydnzhsg2j"
    },
    "object": {
      "type": "batch",
      "key": "nt1smy3tkdqdfzp",
      "batchnum": "39",
      "batchrefnum": "52128047",
      "response": "E",
      "reason": "Unexpected error closing batch",
      "uri": "batches/nt1smy3tkdqdfzp"
    }
  },
  "event_id": "6877c3685a6a37360391434114ef6d424cfbce3f0c5a852e8e34fb52e6daf147"
}


This event will trigger when a batch fails to settle.

Settlement events will all contain a batch object in the object parameter. Below are the response parameter fields for the event_body object. Some fields will only be returned in the response if they apply.

Parameter Name	Type	Description
merchant	object	Merchant information which triggered the event.
object	object	Transaction object related to the event. Will be similar to the response when the transaction is processed through the REST API.
Reference
Stored Credential

Recurring Stored Credential Example

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
 '{
        "command": "sale",
      "amount": "20.00",
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444333322221111",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Portkey Ave",
        "avs_zip": "12345"
      },
        "traits": {
            "stored_credential": "recurring"
      }'


Installment Stored Credential Example

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
 '{
        "command": "sale",
      "amount": "20.00",
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444333322221111",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Portkey Ave",
        "avs_zip": "12345"
      },
        "traits": {
            "stored_credential": "installment"
      }'


Unscheduled Stored Credential Example

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
 '{
        "command": "sale",
      "amount": "20.00",
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444333322221111",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Portkey Ave",
        "avs_zip": "12345"
      },
        "traits": {
            "stored_credential": "ucof"
      }'


Customer Initiated Stored Credential Example

curl -X POST https://sandbox.usaepay.com/api/v2/transactions
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
    -d
 '{
        "command": "sale",
      "amount": "20.00",
      "creditcard": {
        "cardholder": "Hogwarts School of Witchcraft and Wizardry",
        "number": "4444333322221111",
        "expiration": "0919",
        "cvc": "123",
        "avs_street": "1234 Portkey Ave",
        "avs_zip": "12345"
      },
        "traits": {
            "stored_credential": "customer_intitiated"
      }'


This flag indicates either that merchant is about to store the card data for future use or that the current transaction is being run using data from a card stored in the merchant’s system. When the card is being stored this flag indicates what the intended future use will be.

Value	Mode	Description
recurring	Merchant Initiated Transaction	A transaction in a series of transactions that use a stored credential and that are processed at fixed, regular intervals (not to exceed one year between transactions), representing cardholder agreement for the merchant to initiate future transactions for the purchase of goods or services provided at regular intervals.
installment	Merchant Initiated Transaction	A transaction in a series of transactions that use a stored credential and that represent cardholder agreement for the merchant to initiate one or more future transactions over a period for a single purchase of goods or services
ucof	Merchant Initiated Transaction	(Unscheduled Credential on File ) A transaction using a stored credential for a fixed or variable amount that does not occur on a scheduled or regularly occurring transaction date, where the cardholder has provided consent for the merchant to initiate one or more future transactions. An example of such transaction is an account auto-top up transaction.
customer_intitiated	Custoemr Initiated Transaction	A cardholder-initiated transaction is any transaction where the cardholder is actively participating in the transaction. This can be either at a terminal in-store or through a checkout experience online, or with a stored credential.
Change Log
2021-05-19
Added Webhooks Responses Overview and Webhook response examples for the following event types:
ACH Status Updates
Card Accounr Updater
Product Inventory
Settlement
Transactions
2021-05-13
Updated GET transaction response
Added type to creditcard object
Added return_origin flag to request and bin object to response
Added return_fraud flag to request and fraud object to response
Added subtotal to amount_detail object
tranterm field renamed to terminal
Added drawer_shift object.
Added cust_key, custid, and customerid fields
Added customer_email and merchant_email fields
Added return_origin flag to request and origin object to response
Added platform object.
Added available_actions array.
2021-02-26
Added custom_options parameter to payrequests.
2021-01-12
Added auth_amount parameter to credit card and ACH transaction responses
2020-01-05
Corrected documentation for parameter names expiration (previously documented as expires) for creating customer payment methods.
2018-04-09
Corrected parameter names firstname (previously documented as first_name), lastname(previously documented as last_name), and postalcode (previously documented as zip) in the billing_address and shipping address common objects on Common Objects Page. Also updated examples on:
Invoice Overview
Get Invoices
Create Invoices
2018-03-15
Added company to billing_address and shipping_address objects on Common Objects Page.
Corrected all references of transid to refnum on processing pages including:
Capture/Adjust Page
Refund Page
Void Page
2018-02-28
Added FDMS North platform to Boarding API
2018-02-27
Added Invoice pages including:
Overview
Create
Get
Manage
Delete
Added invoice transactions, customdata, and lineitems to Common Objects page.
2018-01-18
Added Boarding Pages with examples including:
Merchant Application Page
Merchant Page
2017-12-08
Added Common Objects Page. Objects include:
creditcard
check
terminal_detail
amount_detail
billing_address
shipping_address
avs
cvc
batch
2017-11-29
Added Tokenize Card Page
Added examples for Batches pages
Batch Details Page
Batch List Page
Close Batch Page
Added is_final parameter to sale page.
Added Void Page
2017-10-11
Added description for templateId for receipts.
2017-09-27
Added examples to Webhooks Page
2017-09-22
Added Webhooks Page
Added check flags prenote and sameday to check object
2017-08-01
Added REST API Main Page
Added Transaction Processing pages including:
Sale Page
Refund Page
Capture/Adjust Page
Added Receipts Page
Added Batches
Batch Details Page
Batch List Page
Close Batch Page
Added Customer Payment Methods Page
Added Fraud Module API Page

Start of time

cURL
PHP
C#
Java
Python


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

Warning: This is a cached snapshot of the original page, consider retry with caching opt-out.

# Rest api - USAePay Help

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

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


---

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


---

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


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Common Objects - USAePay Help

# Common Objects

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
| magstripe | string | Stores the magstripe data from the swiped card |

## check

This request object holds all of the information needed to process a check transaction. Parameters in **bold** are required.

| Parameter Name | Type | Description |
| --- | --- | --- |
| **accountholder** | string | Account holder name |
| **routing** | integer | Bank Routing Number |
| **account** | integer | Bank Account Number |
| account_type | string | Checking or Savings |
| dl_num | integer | Drivers license number/id |
| dl_state | string | Drivers license state (two letter abbrev). See full list [here][1]. |
| number | integer | Check number |
| format | string | SEC Record type. See list of types [here][2]. |
| flags | string | Comma delimited list of special check process flags. Not need for most scenarios. Available flags: prenote, sameday |

## terminal_detail

This request object holds the information associated with a terminal device (if applicable).

| Parameter Name | Type | Description |
| --- | --- | --- |
| type | string | Type of payment terminal |
| entrymode | string | How payment was presented (i.e. swipe, contactless, manually key, etc.) |
| contactless | bool | Contactless payment flag (Y/N) |
| lane_id | string | A numeric field that identifies a merchant's terminal. The MasterCard lane identifier. |

## amount_detail

This request object holds optional granular details for the transaction amount. This allows a merchant to track the breakdown of the total amount. For example, a merchant can enter the exact tip amounts being collected from a customer.

| Parameter Name | Type | Description |
| --- | --- | --- |
| subtotal | double | This field is optional, but if it is sent, it must be consistent with the following equation: `amount = subtotal - discount + shipping + duty + tax`. |
| tax | double | The amount of tax collected. |
| nontaxable | char(Y/N) | Transaction is non taxable (Y/N) |
| tip | double | Amount of tip collected. |
| discount | double | Amount of discount applied to total transaction. |
| shipping | double | Amount of shipping fees collected. |
| duty | double | Amount of duty collected. |
| enable_partialauth | bool | Enable partial amount authorization. If the available card balance is less than the amount request, the balance will be authorized and the POS must prompt the customer for another payment to cover the remainder. The result_code will be "P" and auth_amount will contain the partial amount that was approved. |
| show_tip_line | bool | Set to true, to add line to add a tip to printed receipts. |

## billing_address

This request object holds all information associated with billing. By default, this object is optional, but becomes required when certain Fraud Modules are enabled.

| Parameter Name | Type | Description |
| --- | --- | --- |
| company | string | Company or Organization Name |
| firstname | string | First name associated with billing address |
| lastname | string | Last name |
| street | string | Primary street number/address information. (i.e. 1234 Main Street) |
| street2 | string | Additional address information such as apartment number, building number, suite information, etc. |
| city | string | Billing City |
| state | string | Two-letter State abbreviation or full state name. |
| postalcode | integer | Zip code |
| country | string | Three-letter country code. [See full list here][3] |
| phone | string | The phone number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted. |
| fax | string | The fax number associated with a billing address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted. |

## shipping_address

This request object holds all information associated with shipping. By default, this object is optional, but becomes required when certain Fraud Modules are enabled.

| Parameter Name | Type | Description |
| --- | --- | --- |
| company | string | Company or Organization Name |
| firstname | string | First name |
| lastname | string | Last name |
| street | string | Primary street number/address information. (i.e. 1234 Main Street) |
| street2 | string | Additional address information such as apartment number, building number, suite information, etc... |
| city | string | Shipping City |
| state | string | Two-letter State abbreviation or full state name. |
| postalcode | integer | Zip code |
| country | string | Three-letter country code. [See full list here][4] |
| phone | string | The phone number associated with a shipping address. The preferred format is to eliminate all non-numeric characters, but any standard formatting is accepted. |

## avs

This response object holds information associated with the verification status of the Address Verification System (AVS). The AVS system checks the billing address supplied with the address on file at the credit card company.

| Parameter Name | Type | Description |
| --- | --- | --- |
| result_code | string | Address verification result code. ([See codes here.][5]) |
| result | string | Long version of above |

## cvc

This response object holds information associated with the verification status of the Card Verification Code (CVC). The cvc system checks to see if the supplied cvc code matches the code on file at the credit card company.

| Parameter Name | Type | Description |
| --- | --- | --- |
| result_code | char | CVC result code ([See codes here.][6]) |
| result | string | Long version of above |

## batch

This response object holds information associated with the batch that the transaction belongs to.

| Parameter Name | Type | Description |
| --- | --- | --- |
| type | string | Denotes this object is a batch. |
| key | string | This is the id of the batch. |
| sequence | string | The batch sequence number. The first batch the merchant closes is 1, the second is 2, etc. |

## traits

This object holds card flags that represents transaction characteristics.

| Parameter Name | Type | Description |
| --- | --- | --- |
| is_debt | bool | Set to true if this transaction is to pay an existing debt. Click [here][7] for more information. |
| is_bill_pay | bool | Set to true if this transaction is a bill pay transaction. |
| is_recurring | bool | Set to true if this transaction is a recurring transaction. |
| is_healthcare | bool | Set to true if this transaction is a healthcare transaction. |
| is_cash_advance | bool | Set to true if this transaction contains a cash advance. |

## custom_fields

This response object holds custom fields created the merchant has created.

| Parameter Name | Type | Description |
| --- | --- | --- |
| 1 | string | Optional fields for storing custom data. _Character Limit: 255_ |
| 2 | string | Optional fields for storing custom data. _Character Limit: 255_ |
| 3 | string | Optional fields for storing custom data. _Character Limit: 255_ |
| ... |  |  |
| 19 | string | Optional fields for storing custom data. _Character Limit: 255_ |
| 20 | string | Optional fields for storing custom data. _Character Limit: 255_ |

## lineitems

This response object holds line items associated with a transaction or an invoice.

| Parameter Name | Required | Type | Description |
| --- | --- | --- | --- |
| lineid |  | integer | Gateway generated unique line identifier. _Will only be included in responses._ |
| product_refnum |  | integer | Gateway generated unique product identifier. _Will only be included if the line item is a product from the database._ |
| product_key |  | string | Gateway generated unique product identifier. _Will only be included if the line item is a product from the database._ |
| sku |  | string | This is the product’s Stock Keeping Unit number. |
| name | **Required** | string | Product name. |
| description | **Required** | string | Line item description. |
| cost | **Required** | double | Cost of line item. |
| qty | **Required** | number | Quantity of products. |
| taxable |  | boolean | Denotes if invoice total is taxable. Possible Values: `Y` or `N`. _Defaults to `N`._ |
| tax_rate |  | number | Tax percentage that should be applied to line item amount. |
| discount_rate |  | number | Discount percentage that should be applied to line item amount. |
| discount_amount |  | double | Discount amount that should be applied to line item amount. |
| locationid |  | number | In response only. Location of warehouse identifier for warehouse selected in [settings][8] |
| commodity_code |  | string | Commodity code for line item. |
| manufacturer |  | string | Manufacturer of line item. |
| category |  | string | Category of line item. _Will only be included if the line item is a product from the database._ |
| size |  | string | Line item size. |
| color |  | string | Line item color. |

## transactions (invoice response)

| Parameter Name | Type | Description |
| --- | --- | --- |
| trans_refnum | number |  |
| trans_key | string |  |
| response | string |  |
| amount | double |  |
| authcode | string |  |
| created | datetime |  |
| type | string |  |
| status | string |  |
| ccnum4 | number | Last 4 digits of credit card number used to pay transaction |
| cardtype | number |  |
| cardtype_name | number |  |
| checknum | number |  |

## Change Log

| Date | Change |
| --- | --- |
| 2018-04-09 | Corrected parameter names `firstname` (previously documented as `first_name`), `lastname` (previously documented as `last_name`), and `postalcode` (previously documented as `zip`) in the `billing_address` and `shipping address` objects. |
| 2018-03-15 | Added `company` to [billing_address][9] and [shipping_address][10]. |
| 2017-09-22 | Added check flags `prenote` and `sameday` to [check][11] object. |
| 2017-12-08 | Added page. Includes sections: [creditcard][12], [check][13], [terminal_detail][14], [amount_detail][15], [billing_address][16], [shipping_address][17], [avs][18], [cvc][19], and [batch][20]. |
| 2017-12-08 | Added [customdata][21], [lineitems][22], and [transactions (invoice response)][23] objects. |

**[Click here for the full REST API change log.][24]**

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: http://www.50states.com/abbreviations.htm
[2]: https://help.usaepay.info/developer/reference/checkformat/
[3]: https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
[4]: https://en.wikipedia.org/wiki/ISO_3166-1_alpha-3
[5]: https://help.usaepay.info/developer/reference/avs/
[6]: https://help.usaepay.info/developer/reference/cvv2/
[7]: https://help.usaepay.info/developer/reference/existingdebt/
[8]: https://help.usaepay.info/merchant/guide/invoices/#inventory-location
[9]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#billing-address
[10]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#shipping-address
[11]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#check
[12]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#creditcard
[13]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#check
[14]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#terminal_detail
[15]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#amount_detail
[16]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#billing_address
[17]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#shipping_address
[18]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#avs
[19]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#cvc
[20]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#batch
[21]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#customdata
[22]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#lineitems
[23]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#transactions-invoice-response
[24]: https://help.usaepay.info/developer/rest-api/changelog/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Public API Key - USAePay Help

# Public API Key

## Retrieve Public API Key

This endpoint will generate a new public API key associated with the API key. If a Public API Key already exists for this API Key, the gateway will return the existing key instead. Public API Keys are used in the [Client JS Library][1] to create `payment_key`s

The API endpoint is as follows:

```
GET /api/v2/publickey
```

### Example Request

```
curl -X GET https://sandbox.usaepay.com/api/v2/publickey
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
```

### Example Response

```
"_P120LOb42a1SF4nN54z1Hp0256K7bl3ki0x8iuJP9"
```

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/client-js


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Change Log - USAePay Help

# REST API Change Log

### 2018-10-09

*   Added `pin_debit_support` field to credit card processing object on [Merchant Application Page][1].

### 2018-09-04

*   Added Bulk Transactions pages including:
    *   [Upload Transactions][2]
    *   [Manage Bulk Upload][3]
    *   [Retrieve Bulk Transactions][4]

### 2018-04-09

*   Corrected parameter names `firstname` (previously documented as `first_name`), `lastname`(previously documented as `last_name`), and `postalcode` (previously documented as `zip`) in the `billing_address` and `shipping address` common objects on [Common Objects Page][5]. Also updated examples on:
    *   [Invoice Overview][6]
    *   [Get Invoices][7]
    *   [Create Invoices][8]

### 2018-03-15

*   Added `company` to `billing_address` and `shipping_address` objects on [Common Objects Page][9].
*   Corrected all references of `transid` to `refnum` on processing pages including:
    *   [Capture/Adjust Page][10]
    *   [Refund Page][11]
    *   [Void Page][12]

### 2018-02-28

*   Added [FDMS North][13] platform to Boarding API

### 2018-02-27

*   Added Invoice pages including:
    *   [Overview][14]
    *   [Create][15]
    *   [Get][16]
    *   [Manage][17]
    *   [Delete][18]

*   Added [invoice transactions][19], [customdata][20], and [lineitems][21] to Common Objects page.

### 2018-01-18

*   Added Boarding Pages with examples including:
    *   [Merchant Application Page][22]
    *   [Merchant Page][23]

### 2017-12-08

*   Added [Common Objects Page][24]. Objects include:
    *   `creditcard`
    *   `check`
    *   `terminal_detail`
    *   `amount_detail`
    *   `billing_address`
    *   `shipping_address`
    *   `avs`
    *   `cvc`
    *   `batch`

### 2017-11-29

*   Added [Tokenize Card Page][25]
*   Added examples for Batches pages
    *   [Batch Details Page][26]
    *   [Batch List Page][27]
    *   [Close Batch Page][28]

*   Added `is_final` parameter to [sale page][29].
*   Added [Void Page][30]

### 2017-10-11

*   Added description for `templateId` for [receipts][31].

### 2017-09-27

*   Added examples to [Webhooks Page][32]

### 2017-09-22

*   Added [Webhooks Page][33]
*   Added check flags `prenote` and `sameday` to [check object][34]

### 2017-08-01

*   Added [REST API Main Page][35]
*   Added Transaction Processing pages including:
    *   [Sale Page][36]
    *   [Refund Page][37]
    *   [Capture/Adjust Page][38]

*   Added [Receipts Page][39]
*   Added Batches
    *   [Batch Details Page][40]
    *   [Batch List Page][41]
    *   [Close Batch Page][42]

*   Added [Customer Payment Methods Page][43]
*   Added [Fraud Module API Page][44]

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/rest-api/more/boarding/merchantapplication/
[2]: https://help.usaepay.info/developer/rest-api/transactions/bulk/post-bulk
[3]: https://help.usaepay.info/developer/rest-api/transactions/bulk/manage-bulk
[4]: https://help.usaepay.info/developer/rest-api/transactions/bulk/get-bulk
[5]: https://help.usaepay.info/developer/rest-api/more/commonobjects/
[6]: https://help.usaepay.info/developer/rest-api/invoices/invoices/
[7]: https://help.usaepay.info/developer/rest-api/invoices/get-invoices/
[8]: https://help.usaepay.info/developer/rest-api/invoices/post-invoices/
[9]: https://help.usaepay.info/developer/rest-api/more/commonobjects/
[10]: https://help.usaepay.info/developer/rest-api/transactions/processing/capture/
[11]: https://help.usaepay.info/developer/rest-api/transactions/processing/refund/
[12]: https://help.usaepay.info/developer/rest-api/transactions/processing/void/
[13]: https://help.usaepay.info/developer/rest-api/more/boarding/merchantapplication/
[14]: https://help.usaepay.info/developer/rest-api/invoices/invoices/
[15]: https://help.usaepay.info/developer/rest-api/invoices/post-invoices/
[16]: https://help.usaepay.info/developer/rest-api/invoices/get-invoices/
[17]: https://help.usaepay.info/developer/rest-api/invoices/send/
[18]: https://help.usaepay.info/developer/rest-api/invoices/delete-invoices/
[19]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#transactions-invoice-response
[20]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#customdata
[21]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#lineitems
[22]: https://help.usaepay.info/developer/rest-api/more/boarding/merchantapplication/
[23]: https://help.usaepay.info/developer/rest-api/more/boarding/merchant/
[24]: https://help.usaepay.info/developer/rest-api/more/commonobjects/
[25]: https://help.usaepay.info/developer/rest-api/transactions/processing/savecard/
[26]: https://help.usaepay.info/developer/rest-api/transactions/batch/details/
[27]: https://help.usaepay.info/developer/rest-api/transactions/batch/list/
[28]: https://help.usaepay.info/developer/rest-api/transactions/batch/close/
[29]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/
[30]: https://help.usaepay.info/developer/rest-api/transactions/processing/void/
[31]: https://help.usaepay.info/developer/rest-api/transactions/reporting/receipts/
[32]: https://help.usaepay.info/developer/rest-api/webhooks/
[33]: https://help.usaepay.info/developer/rest-api/webhooks/
[34]: https://help.usaepay.info/developer/rest-api/more/commonobjects/#check
[35]: https://help.usaepay.info/developer/rest-api/
[36]: https://help.usaepay.info/developer/rest-api/transactions/processing/sale/
[37]: https://help.usaepay.info/developer/rest-api/transactions/processing/refund/
[38]: https://help.usaepay.info/developer/rest-api/transactions/processing/capture/
[39]: https://help.usaepay.info/developer/rest-api/transactions/reporting/receipts/
[40]: https://help.usaepay.info/developer/rest-api/transactions/batch/details/
[41]: https://help.usaepay.info/developer/rest-api/transactions/batch/list/
[42]: https://help.usaepay.info/developer/rest-api/transactions/batch/close/
[43]: https://help.usaepay.info/developer/rest-api/customers/paymethods/
[44]: https://help.usaepay.info/developer/rest-api/more/fraudmodules/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# AVS Result Codes - USAePay Help

The following is a list of result codes for the [Address Verification System AVS and what each one indicates. The codes listed in the Code column are the most common responses, but depending on the platform being used, codes listed in the Alternates column may be received.

| Code | Alternates | Meaning |
| --- | --- | --- |
| YYY | Y, YYA, YYD | Address: Match & 5 Digit Zip: Match |
| NYZ | Z | Address: No Match & 5 Digit Zip: Match |
| YNA | A, YNY | Address: Match & 5 Digit Zip: No Match |
| NNN | N, NN | Address: No Match & 5 Digit Zip: No Match |
| YYX | X | Address: Match & 9 Digit Zip: Match |
| NYW | W | Address: No Match & 9 Digit Zip: Match |
| XXW |  | Card Number Not On File |
| XXU |  | Address Information not verified for domestic transaction |
| XXR | R, U, E | Retry / System Unavailable |
| XXS | S | Service Not Supported |
| XXE |  | Address Verification Not Allowed For Card Type |
| XXG | G,C,I | Global Non-AVS participant |
| YYG | B, M | International Address: Match & Zip: Not Compatible |
| GGG | D | International Address: Match & Zip: Match |
| YGG | P | International Address: No Compatible & Zip: Match |

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Card Level Codes - USAePay Help

# Card Level Codes

| Code | Description |
| --- | --- |
| A | Visa Traditional |
| B | Visa Traditional Rewards |
| C | Visa Signature |
| D | Visa Signature Preferred |
| F | Visa Classic |
| G | Visa Business |
| H | Visa Consumer Check Card |
| I | Visa Commerce |
| K | Visa Corporate |
| M | MasterCard/EuroCard and Diners |
| S | Visa Purchasing |
| U | Visa TravelMoney |
| G1 | Visa Signature Business |
| G2 | Visa Business Check Card |
| J1 | Visa General Prepaid |
| J2 | Visa Prepaid Gift Card |
| J3 | Visa Prepaid Healthcare |
| J4 | Visa Prepaid Commercial |
| K1 | Visa GSA Corporate T&E |
| S1 | Visa Purchasing with Fleet |
| S2 | Visa GSA Purchasing |
| S3 | Visa GSA Purchasing with Fleet |
| DI | Discover |
| AX | American Express |

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# ACH Formats - USAePay Help

# ACH Check File Formats

Depending on which ACH processor they are using, merchants have the ability to select the file format that the ACH processor will use for a transaction. If the merchant's processor supports this feature they will have a drop down available in their virtual terminal listing the file formats that are available to them. Descriptions of these formats are available on this page.

# Formats

## ARC

Accounts Receivable Conversion. Used for payments being made against a bill such as a utility. Signed Authorization by Customer required.

## BOC

Back Office Conversion. Paper checks that are collected in a retail environment and then scanned and batched electronically. Customer notification “Checks are Electronically Processed” required at Point of Sale.

## CCD

Cash Concentration or Disbursement. Used for the transfer of funds between entities. Signed Authorization by Customer required.

## POP

Point-of-Purchase. Paper checks that are converted (scanned) into electronic format at the point of sale. The original paper check is voided and returned to the customer. Point of Sale is authorization.

## PPD

Prearranged Payment or Deposit. Credits or Debits that have been pre-arranged by consumers to draft from their personal bank account. Typically used for recurring payments. Signed Authorization by Customer required, or contract language along with voided check.

## RCK

Re-Presentation Check. After a check has been return for insufficient funds it may be attempted again using this format. RCK may only be retried once. Customer notification “Returned Checks are Electronically Re-Processed” required at Point of Sale.

## TEL

Telephone Intiated Entry. Transaction information was received via the telephone. Transactions Authorization script must be recorded at the time the transaction is processed, or written acknowledgment sent to the customer.

## WEB

Internet Initiated Entry. Transaction was received from consumer via the Internet (ie shopping cart). Web transactions require a drop-down “I agree” or a standard form of acknowledgment initiated by the customer and MUST be HTTPS secured web-page.

## ICL

[Check 21 Transaction][1]

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://en.wikipedia.org/wiki/Check_21_Act


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Currency Codes - USAePay Help

## Currency Codes

When processing an international transaction, the second non-domestic source key will be used. An extra field 'currency' is required. This field indicates currency of the transaction. The currency field must be set to the **numerical code** of the currency. If you are processing on a domestic (USD) account, a currency code is not required. The full ISO 4217 list is available [here][1]

| Currency | Numeric Code | Text Code |
| --- | --- | --- |
| Afghan Afghani | 971 | AFA |
| Aruban Florin | 533 | AWG |
| Australian Dollars | 036 | AUD |
| Argentine Peso | 032 | ARS |
| Azerbaijanian Manat | 944 | AZN |
| Bahamian Dollar | 044 | BSD |
| Bangladeshi Taka | 050 | BDT |
| Barbados Dollar | 052 | BBD |
| Belarussian Rouble | 974 | BYR |
| Bolivian Boliviano | 068 | BOB |
| Brazilian Real | 986 | BRL |
| British Pounds Sterling | 826 | GBP |
| Bulgarian Lev | 975 | BGN |
| Cambodia Riel | 116 | KHR |
| Canadian Dollars | 124 | CAD |
| Cayman Islands Dollar | 136 | KYD |
| Chilean Peso | 152 | CLP |
| Chinese Renminbi Yuan | 156 | CNY |
| Colombian Peso | 170 | COP |
| Costa Rican Colon | 188 | CRC |
| Croatia Kuna | 191 | HRK |
| Cypriot Pounds | 196 | CPY |
| Czech Koruna | 203 | CZK |
| Danish Krone | 208 | DKK |
| Dominican Republic Peso | 214 | DOP |
| East Caribbean Dollar | 951 | XCD |
| Egyptian Pound | 818 | EGP |
| Eritrean Nakfa | 232 | ERN |
| Estonia Kroon | 233 | EEK |
| Euro | 978 | EUR |
| Georgian Lari | 981 | GEL |
| Ghana Cedi | 288 | GHC |
| Gibraltar Pound | 292 | GIP |
| Guatemala Quetzal | 320 | GTQ |
| Honduras Lempira | 340 | HNL |
| Hong Kong Dollars | 344 | HKD |
| Hungary Forint | 348 | HUF |
| Icelandic Krona | 352 | ISK |
| Indian Rupee | 356 | INR |
| Indonesia Rupiah | 360 | IDR |
| Israel Shekel | 376 | ILS |
| Jamaican Dollar | 388 | JMD |
| Japanese yen | 392 | JPY |
| Kazakhstan Tenge | 368 | KZT |
| Kenyan Shilling | 404 | KES |
| Kuwaiti Dinar | 414 | KWD |
| Latvia Lat | 428 | LVL |
| Lebanese Pound | 422 | LBP |
| Lithuania Litas | 440 | LTL |
| Macau Pataca | 446 | MOP |
| Macedonian Denar | 807 | MKD |
| Malagascy Ariary | 969 | MGA |
| Malaysian Ringgit | 458 | MYR |
| Maltese Lira | 470 | MTL |
| Marka | 977 | BAM |
| Mauritius Rupee | 480 | MUR |
| Mexican Pesos | 484 | MXN |
| Mozambique Metical | 508 | MZM |
| Nepalese Rupee | 524 | NPR |
| Netherlands Antilles Guilder | 532 | ANG |
| New Taiwanese Dollars | 901 | TWD |
| New Zealand Dollars | 554 | NZD |
| Nicaragua Cordoba | 558 | NIO |
| Nigeria Naira | 566 | NGN |
| North Korean Won | 408 | KPW |
| Norwegian Krone | 578 | NOK |
| Omani Riyal | 512 | OMR |
| Pakistani Rupee | 586 | PKR |
| Paraguay Guarani | 600 | PYG |
| Peru New Sol | 604 | PEN |
| Philippine Pesos | 608 | PHP |
| Qatari Riyal | 634 | QAR |
| Romanian New Leu | 946 | RON |
| Russian Federation Ruble | 643 | RUB |
| Saudi Riyal | 682 | SAR |
| Serbian Dinar | 891 | CSD |
| Seychelles Rupee | 690 | SCR |
| Singapore Dollars | 702 | SGD |
| Slovak Koruna | 703 | SKK |
| Slovenia Tolar | 705 | SIT |
| South African Rand | 710 | ZAR |
| South Korean Won | 410 | KRW |
| Sri Lankan Rupee | 144 | LKR |
| Surinam Dollar | 968 | SRD |
| Swedish Krona | 752 | SEK |
| Swiss Francs | 756 | CHF |
| Tanzanian Shilling | 834 | TZS |
| Thai Baht | 764 | THB |
| Trinidad and Tobago Dollar | 780 | TTD |
| Turkish New Lira | 949 | TRY |
| UAE Dirham | 784 | AED |
| US Dollars | 840 | USD |
| Ugandian Shilling | 800 | UGX |
| Ukraine Hryvna | 980 | UAH |
| Uruguayan Peso | 858 | UYU |
| Uzbekistani Som | 860 | UZS |
| Venezuela Bolivar | 862 | VEB |
| Vietnam Dong | 704 | VND |
| Zambian Kwacha | 894 | AMK |
| Zimbabwe Dollar | 716 | ZWD |

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: http://en.wikipedia.org/wiki/ISO_4217


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# CVV Result Codes - USAePay Help

# Card Security Codes

## Introduction

Card Verification Value (CVV) provides an additional level of online fraud protection. The card security codes are 3 or 4 digit codes printed or embossed on Visa, Mastercard, American Express and Discover cards. These codes are also referred to as CVV2, CVC, CSC or CCID. Their purpose is to provide additional protection against fraudulent card use.

## Storage and Security Considerations

The card security codes are considered highly sensitive data and should never be stored, even in an encrypted format. Storage of this value will place the merchant in jeopardy with PCI and CISP compliance.

## Reference

### Where To Find the Code

#### American Express

A four digit non-embossed number on the face of the card.

#### Discover

A three digit non-embossed number on the back of the card printed within the signature panel after the account number.

#### MasterCard

A three digit non-embossed number on the back of the card printed within the signature panel after the account number.

#### Visa

A three digit non-embossed number on the back of the card printed within the signature panel after the account number.

### Response Codes

The following is a list of result codes for the CVV2/CVC2/CID verification system and what each one indicates.

The card security codes are 3 or 4 digit codes printed or embossed on Visa, Mastercard, American Express and Discover cards. These codes are also referred to as CVV2, CVC, CSC or CCID. Their purpose is to provide additional protection against fraudulent card use. Below is a list of possible result codes.

| Code | Meaning |
| --- | --- |
| M | Match |
| N | No Match |
| P | Not Processed |
| S | Should be on card but not so indicated |
| U | Issuer Not Certified |
| X | No response from association |
| (blank) | No CVV2/CVC data available for transaction. |

### Additional Information

For addition information on card security codes please visit the following links:

*   [Wikipedia Card Security code page][1]
*   [Visa Information on CVV2][2]

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: http://en.wikipedia.org/wiki/Card_Security_Code
[2]: http://usa.visa.com/personal/security/visa_security_program/3_digit_security_code.html


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# End to End Encryption - USAePay Help

# End To End Encryption

End to end encryption provides an enhanced layer of security for swiped and keyed transactions by encrypting the card data at point of entry. For swiped transactions this typically happens in the mag reader head, for keyed transactions, this happens in a standalone, tamper resistant keypad. The data remains encrypted while it passes through the device, software and communication channels to the gateway. Once in the secure environment of the gateway it is decrypted and used for processing.

Encrypted card data may be passed in the same fields as the clear text card and swipe data is passed. The data must be proceeded by "enc://" and if binary, base 64 encoded. For example, if the following data is read from the device:

```
%B4444*********7779^EXAMPLE TEST CARD^2512*********?;4444*******7779=2512*********?|0600|411785952BA27844F49434FFC261A5CE6E6F3F46BE836D8612B56A53DB480167FD63DA9892B0F471626CDC0B75376AF6759403CA58A4C263|350518BC1F8D63CBD2C47D19FC3C1824D3AFB5CC54AC878595902B927DE850D3||61400200|19CFF0CF6F24A9FAFAFF80EF8258F1C1A81A9D90DB474413E127206B3C32DF4885223C20777CB9FAB21D38864B92BDA43D6699610EDC7D62|C516F135E93DFEB|25776C75DC32EEFD|FFFF87BABCDEF000001|1316||0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

You would submit the following in the UMmagstripe variable (transaction api), CreditCard.MagStripe (soap api) or creditcard.magstripe (rest api):

```
UMmagstripe = "enc://JUI0NDQ0KioqKioqKioqNzc3OV5FWEFNUExFIFRFU1QgQ0FSRF4yNTEyKioqKioqKioqPzs0NDQ0KioqKioqKjc3Nzk9MjUxMioqKioqKioqKj98MDYwMHw0MTE3ODU5NTJCQTI3ODQ0RjQ5NDM0RkZDMjYxQTVDRTZFNkYzRjQ2QkU4MzZEODYxMkI1NkE1M0RCNDgwMTY3RkQ2M0RBOTg5MkIwRjQ3MTYyNkNEQzBCNzUzNzZBRjY3NTk0MDNDQTU4QTRDMjYzfDM1MDUxOEJDMUY4RDYzQ0JEMkM0N0QxOUZDM0MxODI0RDNBRkI1Q0M1NEFDODc4NTk1OTAyQjkyN0RFODUwRDN8fDYxNDAwMjAwfDE5Q0ZGMENGNkYyNEE5RkFGQUZGODBFRjgyNThGMUMxQTgxQTlEOTBEQjQ3NDQxM0UxMjcyMDZCM0MzMkRGNDg4NTIyM0MyMDc3N0NCOUZBQjIxRDM4ODY0QjkyQkRBNDNENjY5OTYxMEVEQzdENjJ8QzUxNkYxMzVFOTNERkVCfDI1Nzc2Qzc1REMzMkVFRkR8RkZGRjg3QkFCQ0RFRjAwMDAwMXwxMzE2fHwwMDAweHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4"
```

# Data Format

The encrypted card data can be passed in several different formats. If using a supported manufacturer format, the developer can pass the entire raw block of data as it was read from the reader and the gateway will handle parsing it. It is important that the gateway receive all data components to be able to decrypt successfully. The most common mistake made by developers is to send only the encrypted data and omit the KSN block which contains the key info necessary for decryption.

 For developers implementing their own encryption or using an unsupported raw format, the data can be sent in json format with the fields below. When using this format it is not necessary to base64 encode the entire block, just the encrypted data element.

## Generic format

| Field | Required | Description |
| --- | --- | --- |
| k | yes | Key ID. For DUKPT based encryption this should be the full KSN block in binhex format including the KSID, device serial and encryption counter |
| t | yes* | All tracks encrypted in one data block. This should be either base64 or binhex encoded. |
| t1 | yes* | Encrypted track 1 data. This should be either base64 or binhex encoded. |
| t2 | yes* | Encrypted track 2 data. This should be either base64 or binhex encoded. |
| t3 | yes* | Encrypted track 3 data. This should be either base64 or binhex encoded. |
| m | no | Masked track data, all tracks in single string |
| c | no | Encrypted, manually keyed card number and expiration. |
| s | no | Some encryption backends will require a device serial number. This is different than the device serial that is present in the KSN. |
| p | no | Specify the encryption format parser. Supported formats: "i" == Ingenico On Guard. Currently only required if using the OnGuard format. |
| b | no | Specify the encryption backend provider. If omitted, requests will be routed automatically based on ksn |

*   Device will either concatenate all tracks together before encryption or encrypt each track individually. Use t if encrypted all together in single block and use t1, t2, t3 if encrypted separately.

```json
enc://{"k":"FFFF9019F8E999000009","t1":"411785952BA27844F49434FFC261A5CE6E6F3F46BE836D8612B56A53DB480167FD63DA9892B0F471626CDC0B75376AF6759403CA58A4C263","t2":"350518BC1F8D63CBD2C47D19FC3C1824D3AFB5CC54AC878595902B927DE850D3","m":"%B4444*********7779^EXAMPLE TEST CARD^2512*********?;4444*******7779=2512*********?"}
```

## IDTech Format

Devices from IDTech or that use IDTech swiper heads share a common data format. The data block should start with \x02 followed by two bytes indicating length, followed by \x80 or \x85. Some devices may output raw binary while others may return a binhex encoding. You can pass the entire raw block of data as it was read from the reader and the gateway will handle parsing it. If in binary format, base64 encode. If in binhex the data can be left as is.

## Magtek Format

Supported Magtek devices output a pipe "|" delimited format. The entire block of data is needed for decryption and should be passed into the gateway as is.

```
%B4444*********7779^EXAMPLE TEST CARD^2512*********?;4444*******7779=2512*********?|0600|411785952BA27844F49434FFC261A5CE6E6F3F46BE836D8612B56A53DB480167FD63DA9892B0F471626CDC0B75376AF6759403CA58A4C263|350518BC1F8D63CBD2C47D19FC3C1824D3AFB5CC54AC878595902B927DE850D3||61400200|19CFF0CF6F24A9FAFAFF80EF8258F1C1A81A9D90DB474413E127206B3C32DF4885223C20777CB9FAB21D38864B92BDA43D6699610EDC7D62|C516F135E93DFEB|25776C75DC32EEFD|FFFF87BABCDEF000001|1316||0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

# Devices

The following is only a partial list of supported devices. For assistance with a device not listed please see the dcontact integration support.

## IDTech SecureKey

Supported. Data can be passed through without modification, but the data must be proceeded by "enc://" and if binary, base 64 encoded.

## Magtek iDynamo

If you are using the idynamo "mtSCRALib" library, you can use the following example:

```
NSString *responseString = [mtSCRALib getResponseData];
NSData *responseData = [responseString dataUsingEncoding:NSASCIIStringEncoding];
NSString *encodedString = [NSString stringWithFormat:@"enc://%@", [^] [responseData base64Encoding]];
```

and then send encodedString to the gateway as MagStripe.

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

Warning: This is a cached snapshot of the original page, consider retry with caching opt-out.

# Error Codes - USAePay Help

# Error Codes

[Error Code List in PDF Format][1]

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/downloads/errorcodelist.pdf


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Existing Debt - USAePay Help

# Existing Debt

'Debt Pay', officially the "Visa US Debt Repayment Incentive Interchange Program", is a Visa only program that allows merchants who issue loans or mortgages to receive a reduced or waived interchange fee when customers use a Visa branded debit card to repay their debt. The basic requirements are as follows:

*   The merchant must be registered with Visa
*   The customer must use a debit card, _not_ a credit card, and the debit card must be Visa branded
*   The merchant's category code (MCC) must be 6012 (Financial Institution) or 6051 (non-Financial Institution, Currency related)
*   The merchant cannot be a debt collection agency

A merchant can run debt pay transactions in one of two ways: 1. Set the merchant industry to "Existing Debt" and then all transactions will be attempted as debt pay 2. In the transaction api set `UMisDebt`=`true`, regardless of merchant industry, to send that particular transaction as debt pay

If a merchant attempts a debt pay transaction but the correct conditions are not met, the transaction will still be processed but will be sent as a regular debit or credit transaction.

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Gift Card Processing - USAePay Help

# Giftcard Processing

## Open Loop vs Closed Loop Gift Cards

Open loop gift cards can be redeemed almost anywhere and run on the multiple credit card systems. These include Visa/MasterCard/Amex gift cards which can be redeemed in stores, online, or via telephone, wherever Visa/MasterCard/Amex is accepted. Open loop gift cards can be processed via USAePay as easily as a similar credit cards with no further requirements.

Closed loop gift cards can only be processed, activated, redeemed, and supported via the issuing company. This type of gift card may also be extended to parent or child companies of the participating merchant (at their discretion). Gift cards of this type require integration/setup with USAePay.

## Gift Card Support

The USAePay gateway provides developers with access to a variety of closed loop gift card processors. Merchants who are interested in signing up for giftcard processing should contact their merchant service provider. The list of currently supported processors is listed below.

Once a merchant's account has been enabled with gift card processing, they can process gift card transactions via the Virtual Terminal in the merchant console or any third party application that has integrated gift card support. Gift card reports are also available in the merchant console.

### Supported Processors

Paya (formerly Sage/Geti)

### Processing Fees

Each processor and/or merchant service provider has their own unique pricing structure. Typically processors will charge for a fee for many of the different gift card activities including card printing, activation, running sales and add funds. Merchants should contact their merchant service provider for pricing details.

### Ordering Gift Cards

Private branded giftcards are typically ordered from the gift card processor at the time the merchant signs up. Please contact your merchant service provider for assistance in ordering cards.

## Gift Card Commands

Giftcards support the expected sale and refund functions much like credit cards. In addition, they also include additional activities such as card activation that the developer may want to account for.

| Command | Description |
| --- | --- |
| giftcard:sale | Like a credit card sale, this command reduces the available balance on the associated account. If funds are not available, the transaction is either partial approved or declined depending on the partial approval flag. On successful sales, the remaining balance on the account is returned as part of the transaction response. |
| giftcard:refund | Refund a sale. Increases the balance of the associated account, equal funds as were deducted by the sale. |
| giftcard:activate | All gift card accounts must be activated prior to usage. A starting balance may be passed in with the call to 'giftcard:activate' but it is not necessary. If an amount is not sent as part of the command, the account is viable to run commands against but will have a balance of 0. You can use the 'giftcard:addvalue' command to add funds to the account. |
| giftcard:addvalue | This command increases the available balance on the associated account. It is possible that processor maximums apply. On a successful call to addvalue, the new balance on the card is returned as part of the transaction response. |
| giftcard:transfer | Transfers the remaining balance from one account onto another, then closes (disables) the first account. The 'card' field of a transaction using this command accepts two card numbers separated by comma. The balance will be transferred from the first card given onto the second. Only the second card continues to be active. |
| giftcard:balance | Returns the current available balance on the associated account. |

## Partial Approval

USAePay supports partial approvals for gift cards. This allows a developer to indicate that they want to authorize as much of the requested amount as they can. For example, if the sale is for $50 but the customers card only has $45 left on it, the transaction would normally decline. With partial approvals enabled, the transaction would approve with a “P” response and include the authorized amount of “45.00”.

Developers using the [transactionapi][1] can enable partial approvals by setting the UMallowPartialAuth field to “true”. You will also need to modify your code to accept a “UMresult” value of “P”. This indicates that only a portion of the “UMamount” you specified was available and processed. The actual amount that was processed is returned in the “UMauthAmount” response field.

## Remaining Balance

Gift card transactions will automatically return the remain card balance in the “UMremainingBalance” response field. This is the amount of funds left on the card after the transaction processed. For example, if you process a $10 sale against a card with a $50 balance, the “UMremainingBalance” will contain $40.

## Transaction API Examples

Gift Card Activation

```
UMkey=EXAMPLEPUTYOURKEYHERE&UMcommand=giftcard:activate&UMcard=78986298658376922&UMamount=100.00
```

Process Sale

```
UMkey=EXAMPLEPUTYOURKEYHERE&UMcommand=giftcard:sale&UMcard=78986298658376922&UMamount=10.00
```

Add Funds to Card

```
UMkey=EXAMPLEPUTYOURKEYHERE&UMcommand=giftcard:addvalue&UMcard=78986298658376922&UMamount=20.00
```

Refund a Sale

```
UMkey=EXAMPLEPUTYOURKEYHERE&UMcommand=giftcard:refund&UMrefNum=123456780
```

Transfer Remaining Balance to Another Card

```
UMkey=EXAMPLEPUTYOURKEYHERE&UMcommand=giftcard:transfer&UMcard=78986298658376922,789862962059728373
```

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/transaction-api/#giftcardsale


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# High Availability - USAePay Help

# High Availability Developers Guide

## Introduction

USAePay provides a variety of tools and strategies to provide merchants with as close to 100% continuous processing of payments as possible. This guide provides developers with the background information and examples necessary to leverage these tools and maximize the availability of payment processing.

## System Architecture

USAePay operates multiple data centers across the country. Each data center is equipped with redundant power backup and generation, cooling, fire suppression, and network connections. In the event of a data center losing grid power, UPS and on site generators ensure continuous power. The data centers are geographically dispersed to prevent downtime due to natural disaster. The data centers are connected to multiple Internet backbone providers to mitigate peering issues between providers. They house all resources necessary to process payments, independent of any other location. During normal operation, all data is replicated between locations and all locations can utilize resources such as platform connections out of other locations. In the event of a major outage such as complete power loss in one location, the other locations can operate completely independently.

Within each data center, servers are operated in high-availability (HA) clusters. During normal operation, traffic is load balanced across all servers in the cluster to ensure highest possible performance. The cluster continuously monitors each server for availability. In the event a server goes off line for any reason (planned maintenance, hardware failure, software misconfiguration) the cluster automatically stops routing traffic to the server. Whenever possible, servers are engineered with redundant components such as multiple power supplies and hard drives in raid configurations.

Server and data center maintenance is always done in a non-invasive manner that does not affect transaction processing.

## URLS

### Default: www.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www.usaepay.com/login][1], [https://secure.usaepay.com/login][2] |
| Transaction API | [https://www.usaepay.com/gate][3], [https://secure.usaepay.com/gate][4] |
| SOAP API | [https://www.usaepay.com/soap/gate][5], [https://secure.usaepay.com/soap/gate][6] |
| Ping | [https://www.usaepay.com/ping][7], [https://secure.usaepay.com/ping][8] |

Currently the default url is setup to direct traffic the primary processing location '03'. In the event of an outage at '03', DNS is updated to route these urls to primary location '01' or '02'. The difference between www.usaepay.com and secure.usaepay.com is the type of SSL certificate used. www.usaepay.com uses an "unchained" 2-year Verisign certificate. This certificate should work with the widest range of ssl libraries including those that do not support chained certificates. Secure.usaepay.com uses an [extended validation (EV) ssl certificate][9]. This provides the green bar in modern web browsers but causes some issues with certificate validation in older SSL libaries.

### Primary Processing Location: www-01.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www-01.usaepay.com/login][10] |
| Transaction API | [https://www-01.usaepay.com/gate][11] |
| SOAP API | [https://www-01.usaepay.com/soap/gate][12] |
| Ping | [https://www-01.usaepay.com/ping][13] |

This url sends traffic directly to the primary processing location '01'. It is recommended that this is the first backup url that developers try. This location has all resources necessary to operate independently of the other primary location.

### Primary Processing Location: www-02.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www-02.usaepay.com/login][14] |
| Transaction API | [https://www-02.usaepay.com/gate][15] |
| SOAP API | [https://www-02.usaepay.com/soap/gate][16] |
| Ping | [https://www-02.usaepay.com/ping][17] |

This url sends traffic directly to the primary processing location '02'. It is recommended that this is the second backup url that developers try. This location has all resources necessary to operate independently of the other primary location.

### Primary Processing Location: www-03.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www-03.usaepay.com/login][18] |
| Transaction API | [https://www-03.usaepay.com/gate][19] |
| SOAP API | [https://www-03.usaepay.com/soap/gate][20] |
| Ping | [https://www-03.usaepay.com/ping][21] |

This url sends traffic directly to the primary processing location '03'. During normal operation, this url is identical to the default www.usaepay.com url.

## Testing Connectivity

USAePay does not allowing 'pinging' its servers. ICMP Echo requests (Ping) are dropped at the edge firewalls to prevent network probing and simplistic DDOS attacks. Ping requests sent to any of our servers will result in time out messages. Ping timeouts do not mean that the server is not available. The following example is the normal expected output:

```
PING www.usaepay.com (209.239.233.7): 56 data bytes
```

1.   -- www.usaepay.com ping statistics ---

10 packets transmitted, 0 packets received, 100% packet loss

To test your ability to connect to a given url, access the ping urls listed above. They will respond with a state and cluster id. As long as the response you receive starts with the string "UP" then the url is available for use:

```
# curl https://www.usaepay.com/ping
    UP:ca403
```

The string "DOWN" will appear if the datacenter is not recommended for use. The majority of the time the url will still be able to accept transactions. The DOWN flag is used to indicate planned maintenance where there is the potential for a disruption of service.

The second part of the string indicates which cluster in which datacenter you are connecting to. For example "ca403" means that you were routed to cluster 3 in datacenter ca4.

## Firewall Rules

If your network is utilizing outbound firewall rules to restrict which IPs can be connected to, you should create outbound rules for the following subnets:

| Subnet |
| --- |
| 216.181.230.0/23 |
| 209.239.233.0/24 |
| 209.220.191.0/24 |
| 65.132.197.0/24 |

If you would like to create more granular rules, the following IPs may be returned via DNS resultion depending on region and availability:

| Host | IPs |
| --- | --- |
| www.usaepay.com | 216.181.230.7, 209.220.191.7, 209.239.233.7, 65.132.197.7 |
| secure.usaepay.com | 216.181.230.8, 209.220.191.8, 209.239.233.8, 65.132.197.8 |
| usaepay.com | 216.181.230.100, 209.220.191.100, 209.239.233.100, 65.132.197.100 |
| www-01.usaepay.com | 216.181.230.9, 209.239.233.105 |
| www-02.usaepay.com | 216.181.231.9, 209.220.191.9, 216.181.230.76 |
| www-03.usaepay.com | 209.239.233.9, 216.181.230.9 |
| www-04.usaepay.com | 65.132.197.9, 216.181.231.9 |
| sandbox.usaepay.com | 216.181.230.129, 209.239.233.129 |

## Redundancy Strategies

### Passive, DNS Failover

This "strategy" is actually the default behavior of the primary www.usaepay.com and secure.usaepay.com urls. If your server/workstation is using DNS to resolve the default URLs you will receive the "default" datacenter (currently CA4 - 209.239.233.*). If there is maintenance or an outage, our DNS servers will automatically start resolving to one of the other two datacenters. Currently we are configured to automatically failover within 15 seconds of a failure. The time to live (TTL) flag on our DNS records is set to 3 minutes. Unfortunately your ISP's DNS servers (or even your server or browser) may cache the old DNS entry for longer, leading to longer failover times for some users. In these cases it might be necessary to manually override your dns server through the use of a host file entry. On a UNIX server this can be done by editing /etc/hosts and on windows by editing C:\Windows\System32\drivers\etc\hosts (or equivalent). To force www.usaepay.com to go to primary '02', you would add the line:

```
209.220.191.7  www.usaepay.com
```

To "implement" this strategy, merchants and customers simply need to retry the transaction every few minutes until the DNS has been updated. The only recommended coding change that developers might consider are the proper setting of the timeout variable and catching the time out error:

#### .NET DLL

```vb
Try

      usaepay.Timeout = 60
      usaepay.Sale()

    Catch ex As Exception

      If ex.Message = "Error writing to the gateway: Unable to connect to the remote server" Then

        errormessage = "Unable to process payment, please try again in a few minutes"

      Else

        errormessage = ex.Message

      End If

    End Try
```

#### PHP Library

```php
if($tran->Process())
    {
        echo "<b>Card approved</b><br>";
        echo "<b>Authcode:</b> " . $tran->authcode . "<br>";
        echo "<b>AVS Result:</b> " . $tran->avs_result . "<br>";
        echo "<b>Cvv2 Result:</b> " . $tran->cvv2_result . "<br>";
    } else if($tran->curlerror == 'connect() timed out!') {
        echo "<b>Unable to process payment, please try again in a few minutes<br>";
    } else {
        echo "<b>Card Declined</b> (" . $tran->result . ")<br>";
        echo "<b>Reason:</b> " . $tran->error . "<br>"; 
        if($tran->curlerror) echo "<b>Curl Error:</b> " . $tran->curlerror . "<br>";    
    }
```

### Active Failover, Backup URL Retry

Using this strategy, any failures to connect to the primary url are automatically retried on a secondary url. This method is useful in applications that do not have reliable network connections such as mobile internet solutions. In the event that the initial connection to the gateway fails, the developer will trap the error and automatically retry again on the second url. This process can be repeated for all urls if the developer chooses to.

If choosing this strategy it is important to consider the duplicate transaction problem. Many developers set the connection timeout too low and end up giving up before the gateway has finished processing. While it is rare, some processing backends can take as much as 120 seconds to respond. For example if a developer has their timeout set to 30 seconds and the gateway takes 45 seconds to complete an approval. The application would have returned a time out error even though the gateway approved the transaction and placed it in the batch. The application then retries the transaction on the backup url where it is again approved and placed in the batch. There are now two transactions on the gateway even though the application has only recorded one. While the obvious solution is to raise the application timeout, this can lead to customers giving up and retrying the transaction on their own.

There are two ways to deal with this problem. The first, and easier method is to use the duplicate folding functionality. Duplicate folding will check all incoming transactions for duplicates. If a duplicate is detected, the original transaction response details will be returned instead of processing the transaction again. In the scenario where the first transaction times out (but is authed on the gateway) and then retried on the backup url, the second call to the backup url will detect the duplicate and return the details that would have been returned on the first call if the connection hadn't been dropped prematurely.

When using this method its important to be careful that intentional duplicate charges are not accidentally folded. For example if a customer decides to buy the same product for the same amount on the same card.

### Connection Scoreboard for Load Balancing and Failover

Another strategy that works particular well for high traffic, multi-threaded applications is to maintain a connection scoreboard. The scoreboard keeps track of the number of open connections, hits (successful transactions) and errors for each url. This data is then used to select the best url to send the next transaction to. During normal operation, transactions will load balance between the primary urls. During an outage, after the first failure is recorded, all other transactions will automatically route to the other primary urls.

Example scoreboard:

| URL | Working | Hits | Errors | Last Error |
| --- | --- | --- | --- | --- |
| www-01.usaepay.com | 0 | 100 | 0 |  |
| www-02.usaepay.com | 1 | 100 | 0 |  |

In the above example, both links have successfully processed 100 transactions and we are currently in the middle of processing a transaction on www-02. The logic for selecting the next URL is to pull the url with the lowest errors, lowest working and lowest hits. In the above example we would select www-01 as the next url since it is currently idle (working=0) and has the same number of errors (0) as www-02.

Once an error occurs on one of the connections, the error counter will be increased:

| URL | Working | Hits | Errors | Last Error |
| --- | --- | --- | --- | --- |
| www-01.usaepay.com | 0 | 101 | 1 | 2010-01-01 11:12:59 |
| www-02.usaepay.com | 1 | 2310 | 0 |  |

Since we are sorting first by error count, the next url will now be www-02 because www-01 has a higher error count. Traffic will continue to go to www-02 until the error count is cleared. For this reason, error counts should be cleared periodically. This can be done by setting all error counts to 0 when the last error date is greater than a certain amount of time (ie, 60 minutes).

#### MySQL/PHP Example

The following is a "proof of concept" using php and a mysql database. The same thing should be possible in any language as long as you have the ability to share information between threads, sessions, application instances, users, etc.

SQL Scheme:

```sql
CREATE TABLE connections (
       url CHAR(6),
       working INT,
       hits INT,
       errors INT,
       lasterror DATETIME,
       UNIQUE KEY (url)
    );
    INSERT INTO connections SET url='www-01', working=0, hits=0, errors=0;
    INSERT INTO connections SET url='www-02', working=0, hits=0, errors=0;
    INSERT INTO connections SET url='www-03', working=0, hits=0, errors=0;
```

PHP Transaction Library

```php
// select url
    $res = mysql_query("SELECT url 
                        FROM connections 
                        ORDER BY errors,working,hits 
                        LIMIT 1");
    list($url) = mysql_fetch_row($res);

    // in case something is wrong with the mysql table
    if(!$url) $url='www-01';

    // update scoreboard to reflect that we are processing a transaction on this link
    mysql_query("UPDATE connections 
                 SET working=working+1 
                 WHERE url='" . mysql_real_escape_string($url) . "'");

    $tran->gatewayurl = 'https://' . $url . '.usaepay.com/gate';
    $res = $tran->Process();

    // log error, modify this statement to adjust what you consider a failure
    //  as is, this considers anything that causes an underlying http error to
    //  be a gateway failure.
    if(!$res && strlen($tran->curlerror)>0)
    {
       mysql_query("UPDATE connections 
                    SET working=working-1, errors=errors+1, lasterror=now() 
                    WHERE url='" . mysql_real_escape_string($url) . "'");
    }

    // else log success
    else {
       mysql_query("UPDATE connections 
                    SET working=working-1, hits=hits+1 
                    WHERE url='" . mysql_real_escape_string($url) . "'");

       // automatically clear stale error counts (optional)
       mysql_query("UPDATE connections 
                    SET errors=0, lasterror=null 
                    WHERE lasterror<'" . date('Y-m-d H:i:s', strtotime('-30 minutes')) . "'");

    }
```

### Pro-Active Failover, URL Monitoring

Using this strategy, the developer keeps a list of processing urls in a database or config file. Each url is then pinged every few minutes. If one fails, it is marked as down or otherwise removed from the list. The payment application is then coded to pull its active gateway url from the database or config file.

## Notifications

USAePay provides real time notification of network issues via our [twitter feed][22].

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://www.usaepay.com/login
[2]: https://secure.usaepay.com/login
[3]: https://www.usaepay.com/gate
[4]: https://secure.usaepay.com/gate
[5]: https://www.usaepay.com/soap/gate
[6]: https://secure.usaepay.com/soap/gate
[7]: https://www.usaepay.com/ping
[8]: https://secure.usaepay.com/ping
[9]: http://en.wikipedia.org/wiki/EV_SSL
[10]: https://www-01.usaepay.com/login
[11]: https://www-01.usaepay.com/gate
[12]: https://www-01.usaepay.com/soap/gate
[13]: https://www-01.usaepay.com/ping
[14]: https://www-02.usaepay.com/login
[15]: https://www-02.usaepay.com/gate
[16]: https://www-02.usaepay.com/soap/gate
[17]: https://www-02.usaepay.com/ping
[18]: https://www-03.usaepay.com/login
[19]: https://www-03.usaepay.com/gate
[20]: https://www-03.usaepay.com/soap/gate
[21]: https://www-03.usaepay.com/ping
[22]: http://twitter.com/usaepay


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Invoice Terms - USAePay Help

# Invoice Terms

Below are definitions for common invoicing terms.

| Term | Description |
| --- | --- |
| Net monthly account | Payment due on last day of the month following the one in which the invoice is dated |
| PIA | Payment in advance |
| Net 7 | Payment seven days after invoice date |
| Net 10 | Payment ten days after invoice date |
| Net 30 | Payment 30 days after invoice date |
| Net 60 | Payment 60 days after invoice date |
| Net 90 | Payment 90 days after invoice date |
| EOM | End of month |
| 21 MFI | 21st of the month following invoice date |
| 1% 10 Net 30 | 1% discount if payment received within ten days otherwise payment 30 days after invoice date |
| COD | Cash on delivery |
| Cash account | Account conducted on a cash basis, no credit |
| Letter of credit | A documentary credit confirmed by a bank, often used for export |
| Bill of exchange | A promise to pay at a later date, usually supported by a bank |
| CND | Cash next delivery |
| CBS | Cash before shipment |
| CIA | Cash in advance |
| CWO | Cash with order |
| 1MD | Monthly credit payment of a full month's supply |
| 2MD | As above plus an extra calendar month |
| Contra | Payment from the customer offset against the value of supplies purchased from the customer |
| Stage payment | Payment of agreed amounts at stage |

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Level 3 Processing - USAePay Help

# Level 3 Processing

The credit card processing system is setup on a three level system.

*   Level 1 transactions are standard retail transactions. The card holder is using a personal credit card issued from an American bank.
*   Level 2 transactions are normally corporate cards issued from an American bank.
*   Level 3 transactions are government credit cards or corporate cards.

These levels of requirements are made to determine if a certain transaction is a qualified transaction. A qualified transaction will ensure that the business gets the lowest qualified processing rate that they are signed up with. If the criteria for being qualified is not met, the transaction downgrades. A certified product that collects and submits the data properly to Visa/Mastercard is required to qualify for level 3 processing pricing.

## Requirements For Level 3 Processing

1.   Standard Credit Card Information
    *   credit card number,amount, expiration, billing address, zip code and invoice number.

2.   Merchant Name and Merchant State Code
3.   Sales Tax
    *   an amount must be submitted separately from the total transaction amount.
    *   Tax Identification, Item Tax Amount, Item Tax Rate, Item Tax Identifier, Alternate Tax Amount

4.   Freight/Shipping Amount (If N/A enter $0.00)
5.   Duty Amount (If N/A enter $0.00)
6.   Line Item Details:
    *   Product / Service ID (item id)
    *   Product/ Service Description
    *   Quantity
    *   Item Amount
    *   Unit of Measure

7.   Customer Code (Recommended, not required)

## Variables

### Transaction API

Merchants can pass information about the individual line items that make up an order. This data is visible on the transaction details page. Up to 100 lines may be stored per transaction.

| Field | Max Length | Description |
| --- | --- | --- |
| UMcommand |  | Processing Command. Possible values are: cc:sale, cc:authonly, cc:capture, cc:credit, cc:postauth, check:sale, check:credit, void, refund and creditvoid. Default is sale. |
| UMkey | ALL | The source key (assigned by the server when you created a source in your virtual terminal). |
| UMcard | CC:Sale, CC:AuthOnly, CC:Credit, CC:PostAuth | Credit Card Number with no spaces or dashes. |
| UMexpir | CC:Sale, CC:AuthOnly, CC:Credit, CC:PostAuth | Expiration Date in the form of MMYY with no spaces or punctuation. |
| UMtax |  | Portion of above charge amount (UMamount) that is sales tax. |
| UMduty |  | Duty charge (Required only for level 3) |
| UMinvoice | * | Unique Invoice or order number. 10 digits. While not required, it is strongly recommended that this field be populated for CC:Sale, CC:AuthOnly, CC:PostAuth, CC:Credit, Check:Sale and Check:Credit |
| UMponum |  | Purchase Order number. Only required for corporate purchasing cards. |
| UMorderid |  | Order identifier. This field can be used to reference the order to which this transaction corresponds. This field can contain up to 64 characters and should be used instead of UMinvoice when orderid is longer that 10 digits. |
| UMname | * | Name on card or checking account. While not required, it is strongly recommended that this field be populated for CC:Sale, CC:AuthOnly, CC:PostAuth, CC:Credit, Check:Sale and Check:Credit |
| UMstreet | * | Billing Street Address for credit cards. Used for Address Verification System. While not required, this field should be populated for Fraud Prevention and to obtain the best rate for Ecommerce credit card transactions. It should be populated for CC:Sale and CC:AuthOnly |
| UMzip | * | Billing Zip Code for credit cards. Used for Address Verification System. While not required, this field should be populated for Fraud Prevention and to obtain the best rate for Ecommerce credit card transactions. It should be populated for CC:Sale and CC:AuthOnly |
| UMcvv2 | * | CVV2 data for Visa. Set to -2 if the code is not legible, -9 if the code is not on the card. While not required, this field should be populated for Fraud Prevention and to obtain the best rate for Ecommerce credit card transactions. It should be populated for CC:Sale and CC:AuthOnly |
| UMshipping |  | Portion of above charge amount (UMamount) that is for shipping charges. |
| UMshipzip |  | Shipping zip code |

| Field | Required | Description |
| --- | --- | --- |
| UMline*productrefnum | 12 | (optional) Gateway assigned product RefNum, used for inventory control. |
| UMline*sku | 32 | Product id, code or SKU |
| UMline*name | 255 | Item name or short description |
| UMline*description | 64k | Long description |
| UMline*cost | 00000000.00 | Cost of item per unit of measure (before tax or discount) |
| UMline*qty | 00000000.0000 | Quantity |
| UMline*taxable | 1 | Y = Taxable, N = Non-taxable |
| UMline*taxrate | 00.000 | Tax rate for line (only required for level 3 processing) |
| UMline*taxamount | 00000000.00 | Amount of tax charge for line (if left blank will be calculated from taxrate) |
| UMline*um | 12 | Unit of measure. If left blank or an invalid code is sent, EA (Each) will be used. See list of valid [unit of measure codes][1] |
| UMline*commoditycode | 12 | Commodity code (only required for level 3 processing). See http://www.unspsc.org/ for valid list of codes. |
| UMline*discountrate | 000.000 | Discount percentage for line (only required for level 3 processing) |
| UMline*discountamount | 00000000.00 | Discount amount for line (if left blank will be calculated from discountrate) |

*   replace the '*' with the line number. For example UMline1sku.

[Transaction API][2]

### SOAP API

#### Transaction Detail

| Type | Name | Description |
| --- | --- | --- |
| string | Invoice | Transaction invoice number. Will be truncated to 10 characters. If this field is not provided, the system will submit the RefNum in its place. |
| string | PONum | Purchase Order Number for commercial card transactions - 25 characters. (Required for Level 2 & 3) |
| string | OrderID | Transaction order ID. This field should be used to assign a unique order id to the transaction. The order ID can support 64 characters. |
| string | SessionID | Optional Session ID used for customer analytics and fraud prevention. Must be get generated using the [..:methods:getSession][3] method. See the [developer:profiling][4] guide for more information |
| string | Clerk | Sales Clerk. Optional value indicating the clerk/person processing transaction, for reporting purposes. |
| string | Terminal | Terminal Name. Optional value indicating the terminal used to process transaction, for reporting purposes. |
| string | Table | Restaurant Table Number. Optional value indicating the restaurant table, for reporting purposes |
| string | Description | Transaction description. |
| string | Comments | Comments. Free form text. |
| boolean | AllowPartialAuth | Allow a partial authorization if full amount is not available (Defaults to false) |
| double | Amount | Total billing amount. (Subtotal+Tax+Tip+Shipping-Discount=Amount.) |
| string | Currency | Currency Code. 3 digit currency code of total amount. |
| double | Tax | Portion of total amount that is tax. (Required for Level 2 & 3) |
| double | Tip | Portion of total amount that is tip. |
| boolean | NonTax | Determines whether a transaction is non-taxable. |
| double | Shipping | Portion of total amount that is shipping charges. (Required for Level 3) |
| double | ShipFromZip | Zipcode that the order is shipping from. (Required for Level 3) |
| double | Discount | Amount of discount. |
| double | Duty | Amount of duty charged. (Required for Level 3) |
| double | Subtotal | The amount of the transaction before tax, tip, shipping and discount have been applied. |

#### Line Item Properties

| Type | Name | Description |
| --- | --- | --- |
| string | ProductRefNum | Unique ID of the product |
| string | SKU | A stock-keeping unit is a unique identifier for each distinct product and service that can be purchased |
| string | ProductName | Name of the product |
| string | Description | Description of product or purchase |
| string | UnitPrice | Individual price of the unit |
| string | Qty | Total number of items |
| boolean | Taxable | Taxable good flag |
| [SOAP API][5] |  |  |

### PHP

| General Properties |  |
| --- | --- |
| key | Source Key generated by the Merchant Console at www.usaepay.com. |
| pin | Pin for Source Key. This field is required only if the merchant has set a Pin in the merchant console. |
| command | Command to run; Possible values are: cc:sale, cc:authonly, cc:capture, cc:credit, cc:postauth, check:sale, check:credit, void, refund and creditvoid Default is cc:sale. |
| card | Credit Card Number with no spaces or dashes. |
| exp | Expiration Date in the form of MMYY with no spaces or punctuation. |
| amount | Charge amount without $. The amount field should include the total amount to be charged, including sales tax. |
| tax | The portion of amount that is sales tax. |
| shipping | Shipping charges. |
| invoice | Unique ticket, invoice or order number. 10 digits. |
| orderid | Unique order identifier. Used to reference the order to which the transaction corresponds. This field can contain up to 64 characters and should be used instead of invoice when orderids longer that 10 digits are needed. |
| ponum | Customer purchase order number. Only required for commercial cards. |
| cardholder | Name as it appears on the creditcard. |
| street | Street Address for use in AVS check |
| zip | Zipcode for AVS check |
| description | Charge description (optional). |
| cvv2 | CVC/CVV2 code (optional). |

| Line Items | Description |
| --- | --- |
| refnum | (optional) Gateway assigned product RefNum, used for inventory control. |
| sku | Product id, code or SKU |
| name | item name or short description |
| description | Long description |
| cost | Cost of item per unit of measure(before tax and discount) |
| qty | Quantity |
| taxable | Y = Taxable, N = Non-taxable |
| taxrate | Tax rate for line **Required for level 3 processing** |
| taxamount | Amount of tax charge for line (if left blank will be calculated from taxrate) **Required for level 3 processing** |
| um | Unit of measure. If left blank or an invalid code is sent, EA (Each) will be used **Required for level 3 processing** |
| commoditycode | Commodity code **Required for level 3 processing** |
| discountrate | Discount percentage for line **Required for level 3 processing** |
| discountamount | Discount amount for line(if left blank will be calculated from discountrate) **Required for level 3 processing** |

[PHP][6]

## Processing Platform

Currently First Data North and TSYS are the only platforms that processes level 3 commercial cards.

*   FDMS(North) - (800) 542-1894
*   TSYS - (800) 552-8227

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/reference/umcodes/
[2]: https://help.usaepay.info/developer/transaction-api/
[3]: https://help.usaepay.info/developer/soap-api/methods/getsession/
[4]: https://help.usaepay.info/developer/reference/threatmetrix/
[5]: https://help.usaepay.info/developer/soap-api/
[6]: https://help.usaepay.info/developer/sdks/php/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Multi-Currency - USAePay Help

# Multi-Currency Processing

Most merchant accounts can process credit cards for customers in any country, as long as all of the amounts are in the merchant’s native currency. For example, a US merchant can accept an order on their website from a customer in Europe. In this case, the purchase amount would be in USD and the batch would be sent to the bank in USD. The customer’s purchase would be converted to Euros by Visa or MasterCard and the customer’s European issuing bank. The customer would see the converted purchase amount in Euros only on their credit card statement. This amount will not match the purchase amount that the customer viewed in USD. In addition to the purchase amount in Euros, they will see a line item representing the conversion cost. While this process works, it is not very clean or clear for the customers. The fact that the USD purchase amount, Euro purchase amount and conversion cost are all different amounts that the customer may or may not recognize, this results in the escalation of chargebacks.

Merchants looking to implement a more localized experience for their customers will need to implement multi-currency processing. With this type of processing, the end result that the customer sees on their credit card statement is the exact cost of the product or service at the time of purchase. Customers in Europe for example, would be directed to a website where all prices are listed in Euros, rather than in USD. During the entire purchasing process all of the amounts viewable to the customer will also be in Euros. Perhaps most importantly, the purchase total on the checkout page will match the amount that will appear on their credit card statement.

## In the Console vs. Through Integration

Multi-currency processing also provides an enhanced interface in the merchant console. The batch manager will list both the customer’s currency and the amount converted to the merchant’s currency. Currently, multi-currency processing requires the use of two merchant accounts; one for the processing of domestic transactions and a second for the processing of international transactions. Merchants should contact their merchant service provider for pricing and account signup. In the merchant console one user name can be connected to both accounts, but when integrating, two separate source keys are required. The application (shopping cart) must store both source keys and use the correct one based on whether the transaction is a domestic transaction or in an international currency. When sending transactions on the domestic source key, no extra fields are required. The merchant’s currency will always be utilized.

When processing an international transaction, the second non-domestic source key would be utilized. An extra field entitled 'currency' is required. This field indicates the currency of that particular transaction. The currency field must be set to the specific numerical code that is assigned to that currency. The list of codes can be found here: [Currency Codes][1]. The field entitled ‘amount’ would then contain the purchase amount in the specified currency, not in USD. When the gateway responds it will contain the following three extra fields: the customer’s currency code (typically 840 for US Dollars); the transaction amount converted to the customer’s currency; and, the conversion rate used.

## Fluctuating Currencies

For multi-currency processing to work, the prices listed on the merchant’s website must already be converted to the targeted country’s currency. Although conversion rates are constantly fluctuating up or down .01 to .03 of a percent, multi-currency processing provides an automated method for updating these currency conversions daily, no matter how many different currencies are loaded on a particular website. This functionality would be implemented with either the DotNet api or the SOAP interface.

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/currencycodes/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Public Keys - USAePay Help

# Public PGP Key

This is USAePay's current public PGP key for secure data transfers:

| Field |  |
| --- | --- |
| Key ID | C7CD51B5 |
| Finger Print | 536D EB5B 7B03 A6A5 74DB D583 A305 A8F7 C7CD 51B5 |
| Length | 4096 |
| Type | RSA |
| Expiration | 02/23/2027 |
| UserID | Kena Patel [kena.patel@nmi.com][1] |

```
-----BEGIN PGP PUBLIC KEY BLOCK-----

mQINBGmcrvgBEACyBGaTIXauJerm4P7E/QU0MjU3Nw6Lyn7ach4V6hglPNd0sRBT
S8wpNq1JdaLN5Kqu0FwbJxk5oxZxKzn72yIxETRw64jEIbRTiRDwmpVLhU9f1pJO
7llbcUbUxgq1Q8QteBjhO3zXNWN3XTr0lUsSWNOgxzhpKAm/ciUKqik5NiBrGZy2
2AU0BNQaXYHq4NXsODhLWFmTfMLplxVxzTr+cRJKZPdo+WYVlA8Z7whI0eq127/v
QBKdk2KFxVzOigHo+yKnAw+YXTlWKHkfQyqjw8EJz9d38zisCLsotZ1VV0SY6tuQ
WCz+pJe3r63ODgCD8I+JezNZP+76UQqeCz81wPJ7hgf5objb7W0I7OK53AkAwbCo
Bit8bevjCeOAdApEHw85v3HTHYaAGcSke/Vv807szic8NRLeU5STdK0smq+WOgqf
L7K4SIbWUFt31LVm+oJWf1Wb0k99qOuWs/plDqB6Piu5XJ+zCHZApaPVpAwWDjf0
+gv3oyj/r8dYMespCLMEdWlqxDgzoXS4174a468nZS4U9ysOpbCBTiMHu9t6zxKz
MoJM843dAdAxNgo4W1tawBeME1/NvgJFUSXYrA9amQdca1fnFyLCRe6hSf2bdCiv
CiDIxk+DR3ulmC8ZrF/K5dPqx2E52r8sTlSCxoy+wAwIah1DJEd7YB6+gQARAQAB
tB9LZW5hIFBhdGVsIDxrZW5hLnBhdGVsQG5taS5jb20+iQJUBBMBCAA+FiEEU23r
W3sDpqV029WDowWo98fNUbUFAmmcrvgCGwMFCQHhMtYFCwkIBwIGFQoJCAsCBBYC
AwECHgECF4AACgkQowWo98fNUbUCow//RBE2ZpLVeXOxDGsjgEHeAYSyTiUk3ObM
StqTF/obPOFm9V0+tYNBj9nFzOm9gkrhjJF3tly1MHSrXzfOYBejUkCAugAgaWEa
WvA1rvzKXgqsPyWsPfMMYZmIsgbZSUbDJvOMah4zxnJiJdze3ywFxQKdWUjzlKQl
QKjHsh1vyOadoItGkKrgXg/HODVdRJ4xpj8Qngao4jM5hxetDyCesNzvG8EWNOc4
BbrkG7yAtlHU0VGZh+HYR9hNA2pyvvBTJbxZPaZPpLeIS2jMHRVNBklBkxcQ3vir
GhSHOjoQPYx8Uj27ryEkzxGjHXjMbJdRprOXX7rgSwbbgcSWh7feMFjICVZ6Ug12
/44EH5hmGUStytDnBT+4abpjNbDmggEJ2CbISXUA88gjOLEmX0t2m6v7+6q40OyE
YSSFQ7qt6K80hWBciXFkQzV2XYKP/uGCvW8CsOChYkgUVVbyD/+zUoWSSY/CQdma
MbAyp8Yp1kmInVKe9x+fLSAfdrvlsVJ/2/ir9GFitlLHWdpexYMvHFE9T7VCVTE2
rAnNkzW5pCJWaJcdwd1x2LRCIPKwkJhE7LsUEDvYHUX9jbAtyj9OUZqgbtBpKosR
1tI+gEEbNB6KRnbObZfNNIuvV2xgwRSjfA2e79I31RfykEawN44awfvr3DMIuLqF
yXfijgLMXKG5Ag0EaZyu+AEQAK8ZkKRZz4sAmnJb3IAiHDlyfk2gTmwOafNCYLHx
5pEK71i7iCymvla1wRCJEaElb6cBNv9fYPlzGCU1e+ilu8bgxkLKxjposbR7s5Gy
DehsSuuUk9DqHQOrcjaaQgjLL1pvd9YX0tNMNmWLRiyY05OlF3JL5uxoA64i3VXT
aqVN/U9/GAgWtY4tdsudB7/TXRhvVJuZ7/PC/9mFN0Th6v+WIPnq9mpKOSiQlVjt
yYNYYkcePAMEBS/jvVzm0ZXJ5lH4ePz3g/CwpSJlntGLYgtnoNU/A/L6hQnLIZRm
erNmpXaojZzDyyifN2NMX3u2Fs6U+AZII7m169dIrX3xD+zaOWzjHq+UyERvkz32
jJwVfd9M7N4riQFhUMoTUqSe8x5v3/eZZ7zB1Sq2bDNvzsY9pHu+K3lDbwEtk4hl
tqX4BHK/85XLYHNWR/rDFX6XgI30/7eVnV8+i9MsxDvKXsVByBpSjO3zD7E8ie/2
G99PdU3jj4bdTFXHSjwrhFm23CA+bFiXqssDV1RojtiBKJnOX+3Qs7yLJKOlH9vO
BNgIuwvbcGOEx7V4YWG1PY2o8oc37pAZod3IzejS8eKyA98+eenYv/ju78/4swNO
yQQpNqzF9eBQqKUPN4wobCuaVP5ENIuVH8nVazxMT7Y0Wt8smF8SUsktlZ7QICzD
PLEVABEBAAGJAjwEGAEIACYWIQRTbetbewOmpXTb1YOjBaj3x81RtQUCaZyu+AIb
DAUJAeEy1gAKCRCjBaj3x81RtZhKD/0Tb5VyLWQSpdEsggDYPCEcM0EE1F1xSxtX
k7JxgCo7FZUZxKvH45Qxz8fmKjKwlGY2EcVr1MhQNe/9oqNbw1yVFc0wM8crbr8T
ByhIXIqTQM95Vqj+PJe1ylaMTaqPXnhuL7+/MbeOPfhygJ6/AEwiJXPHiQOMYByQ
BrgTEY9z2Z7K7Kn/7YEo9Se3Mnw39oKaarlLSeEBLjoVBv4O0rk8cD4RfWU3zwIV
Z5P8778N7tNZywVR6a2+Vrr4U9e3lKJJSi6rnY6Mh5clUcxZqYA7K+8aa1GOKAsr
puDQzc1QlgnoeeOj/2oNxT6eTbs/dUQ1bOxm9bcV2Gb18yLCE7bKhnXr33Hyn/AE
mUi+6IfJtNu0VDG4BhudO4qGfVb+2N/G4IIFNO+AbqU4ahD2dEDKUd1QFjropPtY
o/KDAPuxZ1MUSFvpdkJhpLP9jKkVOyffNL+uZwIbn7epG5WJ7HVQrR4NVOKw2k6k
SUaMMsr/cbFOejXd49SmxoqxkdQWhrORvQmHEjraqvQfl/1lXYhIUNtLxOojdVqz
Kx1nW+JdB4Nl3MFZOYmROYhC44F9kuMidwZZyP/wKkUaEOFkIOJY1Jo7xiU5B/QB
UuzD31xKOavJvOOK4bdJI2QUMioIMldTbuUdMaxuOl4UmoRHJeoRP/YQE9/Usjdu
fxZaslgeLA==
=+j0u

-----END PGP PUBLIC KEY BLOCK-----
```

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: mailto:kena.patel@nmi.com


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Sandbox System - USAePay Help

# Sandbox System

The sandbox provides a full simulation of the production gateway and is the ideal way to test your integration.

If you are a developer and your client, the merchant, has not setup a live gateway account, a sandbox account will allow you to develop your application without delay. The sandbox environment mimics a live Merchant Console account (with a few exceptions), so you can expect the same results when switching to the live account once it's setup. The exceptions mentioned above are:

*   The sandbox is self-contained and will not send any data to outside processing platforms (Visa, MC, or any banks). Charging a "live" credit card will not result in a charge showing up on the cardholder's bill.
*   While "live" credit cards and check information will work on the sandbox server, it is recommended that you use the test information. A list of test information can be found in the developer's center here:
    *   [Test Credit Cards][1]
    *   [Test ACH Data][2]

*   The sandbox is designed for testing functionality, and is not a load testing environment. Transaction throughput is intentionally throttled.
*   The transaction database is cleared every few months.
*   The sandbox does not support encrypted swipe or EMV testing. It does support un encrypted and manual entries on the devices.

so you can expect the same results when switching to the live account once it's setup. Your application will only need to swap the source key from the developer one to the live one.

# Requesting an Account

## Step 1: Access Developer Portal

Go to the following URL: [https://developer.usaepay.com/_developer/app/login][3]

### Existing Developer Portal Account

If you already have a developer portal login, enter your email and password, select **Login to your account**, and skip to the next step.

### No Developer Portal Account

If you do not have a developer portal login, select **[Register for an account][4]**

Fill out the form completely and select **Register new account** and the developer portal will open automatically.

## Step 2: Request Sandbox Account in Developer Portal

To set up a sandbox account, click the **Request Test Account** tab in the sidebar and fill out the form _COMPLETELY_ to obtain your login credentials. The fields are:

*   **Requested Username**- The username you would like to use to login to the sandbox account.
*   **Contact Name**
*   **Company Name**
*   **Email**- The email you would like the test account credentials sent to.

Leaving any fields blank will delay your account setup. When complete click **Submit Request**. If successful, the message below will be shown.

Processing time takes a few hours during regular office hours based on Pacific Standard Time. Requests on weekends may be processed with a 24 hour or more turnaround time.

# Accessing Your Sandbox Account

Once your sandbox account is created, you will receive an email with your login credentials. Login with these credentials at [https://sandbox.usaepay.com/login][5].

# Switching from Development to Production

When testing and development is completed switch the source key to the key generated from the live account and change the processing URL from `sandbox.usaepay.com/gate` to `www.usaepay.com/gate`

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/reference/testcards/
[2]: https://help.usaepay.info/developer/reference/testcheckdata/
[3]: https://developer.usaepay.com/_developer/app/login
[4]: https://developer.usaepay.com/_developer/app/register
[5]: https://sandbox.usaepay.com/login


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Status Codes - USAePay Help

# Transaction Status Codes

The transaction status codes are used to indicate where in the transaction life cycle a transaction is. These codes can be used when searching on the status field.

| Code | Label | Meaning |
| --- | --- | --- |
| N | Queued | New Transaction (hasn't been processed yet) |
| P | Pending | For credit cards, batch hasn't closed yet. For checks, hasn't been sent to Bank yet. |
| B | Submitted | For checks, sent to bank and void no longer available. |
| F | Funded | Funded (for checks, the date that the money left the account. |
| S | Settled | For credit cards batch has been closed and transaction has settled. For checks, the transaction has cleared. |
| E | Error | Transaction encountered a post processing error. Not common. |
| V | Voided | Check transaction that has been voided |
| R | Returned | Check transaction that has been returned by the bank (ie for insufficient funds) |
| T | Timed out | Check transaction, no update has been received from the processor in 5 days. |
| M | Manager Approval Req. | Transaction has been put on hold pending manager approval. (checks) |

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Test Cards - USAePay Help

# Test Cards

The following list of test credit card numbers maybe used in a [Sandbox Account][1]. The use of live credit card information in a test environment is strongly discourage. It is recommended that the card numbers on this page be used instead.

For more information on the response codes and their meanings see:

*   [AVS Response Codes][2]
*   [CVV2 Response Codes][3]
*   [Card Level Codes][4]

Please note that while the sandbox test platform does its best to simulate what you will see in production, there may be subtle differences depending on the platform being used. At this time we are only simulating the FDMS Nashville responses on the sandbox server.

## AVS Responses

| Card Number | Expiration | CVV2 Code | AVS Response | CVV2 Response | CAVV Response | Card Level |
| --- | --- | --- | --- | --- | --- | --- |
| 4000100011112224 | 0924 | 123 | YYY | M |  | A |
| 4000100111112223 | 0924 | 321 | YYX | M |  | A |
| 4000100211112222 | 0924 | 999 | NYZ | M |  | A |
| 4000100311112221 | 0924 | 999 | NYW | M |  | A |
| 4000100411112220 | 0924 | 999 | YNA | M |  | A |
| 4000100511112229 | 0924 | 999 | NNN | M |  | A |
| 4000100611112228 | 0924 | 999 | XXW | M |  | A |
| 4000100711112227 | 0924 | 999 | XXU | M |  | A |
| 4000100811112226 | 0924 | 999 | XXR | M |  | A |
| 4000100911112225 | 0924 | 999 | XXS | M |  | A |
| 4000101011112222 | 0924 | 999 | XXE | M |  | A |
| 4000101111112221 | 0924 | 999 | XXG | M |  | A |
| 4000101211112220 | 0924 | 999 | YYG | M |  | A |
| 4000101311112229 | 0924 | 999 | GGG | M |  | A |
| 4000101411112228 | 0924 | 999 | YGG | M |  | A |
| 4000101511112227 | 0924 | 999 | NN | M |  | A |
| 4000101611112226 | 0924 | 999 | N/A | M |  | A |

## CVV2 Responses

| Card Number | Expiration | CVV2 Code | AVS Response | CVV2 Response | CAVV Response | Card Level |
| --- | --- | --- | --- | --- | --- | --- |
| 4000200011112222 | 0924 | any | YYY | M |  | A |
| 4000200111112221 | 0924 | any | YYY | N |  | A |
| 4000200211112220 | 0924 | any | YYY | P |  | A |
| 4000200311112229 | 0924 | any | YYY | S |  | A |
| 4000200411112228 | 0924 | any | YYY | U |  | A |
| 4000200511112227 | 0924 | any | YYY | X |  | A |
| 5555444433332226 | 0924 | any | YYY | M |  |  |
| 5555444433332234 | 0924 | any | YYY | N |  |  |
| 5555444433332242 | 0924 | any | YYY | P |  |  |
| 5555444433332259 | 0924 | any | YYY | S |  |  |
| 5555444433332267 | 0924 | any | YYY | U |  |  |
| 5555444433332275 | 0924 | any | YYY | X |  |  |
| 371122223332225 | 0924 | any | YYY | M |  |  |
| 371122223332233 | 0924 | any | YYY | n/a |  |  |
| 371122223332241 | 0924 | any | CVV2 No Match (Decline) |  |  |  |
| 6011222233332224 | 0924 | any | YYY | M |  |  |
| 6011222233332232 | 0924 | any | YYY | N |  |  |
| 6011222233332240 | 0924 | any | YYY | P |  |  |
| 6011222233332257 | 0924 | any | YYY | S |  |  |
| 6011222233332265 | 0924 | any | YYY | U |  |  |
| 6011222233332273 | 0924 | any | YYY | X |  |  |

## CAVV Responses

| Card Number | Expiration | CVV2 Code | AVS Response | CVV2 Response | CAVV Response | Card Level |
| --- | --- | --- | --- | --- | --- | --- |
| 4000600011112223 | 0924 | 999 | YYY | M | 1 | A |
| 4000600111112222 | 0924 | 999 | YYY | M | 2 | A |
| 4000600211112221 | 0924 | 999 | YYY | M | 3 | A |
| 4000600311112220 | 0924 | 999 | YYY | M | 4 | A |
| 4000600411112229 | 0924 | 999 | YYY | M | 6 | A |
| 4000600511112228 | 0924 | 999 | YYY | M | 7 | A |
| 4000600611112227 | 0924 | 999 | YYY | M | 8 | A |
| 4000600711112226 | 0924 | 999 | YYY | M | 9 | A |
| 4000600811112225 | 0924 | 999 | YYY | M | A | A |
| 4000600911112224 | 0924 | 999 | YYY | M | B | A |
| 4000601011112221 | 0924 | 999 | YYY | M | C | A |
| 4000601111112220 | 0924 | 999 | YYY | M | D | A |

## Card Level Responses

| Card Number | Expiration | CVV2 Code | AVS Response | CVV2 Response | CAVV Response | Card Level |
| --- | --- | --- | --- | --- | --- | --- |
| 4000700011112221 | 0924 | 999 | YYY | M |  | A |
| 4000700111112220 | 0924 | 999 | YYY | M |  | B |
| 4000700211112229 | 0924 | 999 | YYY | M |  | C |
| 4000700311112228 | 0924 | 999 | YYY | M |  | D |
| 4000700411112227 | 0924 | 999 | YYY | M |  | G |
| 4000700511112226 | 0924 | 999 | YYY | M |  | H |
| 4000700611112225 | 0924 | 999 | YYY | M |  | I |
| 4000700711112224 | 0924 | 999 | YYY | M |  | K |
| 4000700811112223 | 0924 | 999 | YYY | M |  | S |
| 4000700911112222 | 0924 | 999 | YYY | M |  | U |
| 4000701011112229 | 0924 | 999 | YYY | M |  | G1 |
| 4000701111112228 | 0924 | 999 | YYY | M |  | G2 |
| 4000701211112227 | 0924 | 999 | YYY | M |  | J1 |
| 4000701311112226 | 0924 | 999 | YYY | M |  | J2 |
| 4000701411112225 | 0924 | 999 | YYY | M |  | J3 |
| 4000701511112224 | 0924 | 999 | YYY | M |  | J4 |
| 4000701611112223 | 0924 | 999 | YYY | M |  | K1 |
| 4000701711112222 | 0924 | 999 | YYY | M |  | S1 |
| 4000701811112221 | 0924 | 999 | YYY | M |  | S2 |
| 4000701911112220 | 0924 | 999 | YYY | M |  | S3 |

## Decline Responses

| Card Number | Expiration | CVV2 Code | Decline Code | Message |
| --- | --- | --- | --- | --- |
| 4000300011112220 | 0924 | 999 | - | Declined |
| 4000300001112222 | 0924 | 999 | 04 | Pickup Card |
| 4000300211112228 | 0924 | 999 | 05 | Do not Honor |
| 4000300311112227 | 0924 | 999 | 12 | Invalid Transaction |
| 4000300411112226 | 0924 | 999 | 15 | Invalid Issuer |
| 4000300511112225 | 0924 | 999 | 25 | Unable to locate Record |
| 4000300611112224 | 0924 | 999 | 51 | Insufficient funds |
| 4000300711112223 | 0924 | 999 | 55 | Invalid Pin |
| 4000300811112222 | 0924 | 999 | 57 | Transaction Not Permitted |
| 4000300911112221 | 0924 | 999 | 62 | Restricted Card |
| 4000301011112228 | 0924 | 999 | 65 | Excess withdrawal count |
| 4000301111112227 | 0924 | 999 | 75 | Allowable number of pin tries exceeded |
| 4000301211112226 | 0924 | 999 | 78 | No checking account |
| 4000301311112225 | 0924 | 999 | 97 | Declined for CVV failure |

## Fraud Profiler Response

| Card Number | Expiration | Profiler Response |
| --- | --- | --- |
| 4000301411112224 | 0924 | review |
| 4000301511112223 | 0924 | reject |

## Referral Response

| Card Number | Expiration | CVV2 Code | AVS Response | CVV2 Response | CAVV Response | Card Level |
| --- | --- | --- | --- | --- | --- | --- |
| 4000300111112229 | 0924 | 999 | - | - |  |  |
|  |  |  |  |  |  |  |

## Partial Authorization Cards

| Card Number | Expiration | Authorized Amount |
| --- | --- | --- |
| 4000000011112275 | 0924 | 50% |
| 4000000011112283 | 0924 | 75% |

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/reference/sandbox/
[2]: https://help.usaepay.info/developer/reference/avs/
[3]: https://help.usaepay.info/developer/reference/cvv2/
[4]: https://help.usaepay.info/developer/reference/cardlevelcodes/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Test ACH Data - USAePay Help

# Test ACH Data

The following list of test check information is made available for testing check processing functionality on our [Sandbox System][1]. The account data should not be used in production. All approved transactions will be moved to “submitted” at 5pm PST and on the following day at 8am PST will move to “settled”.

| Routing | Account | Amount | Response | Reason |
| :---: | :---: | :---: | --- | --- |
| 987654321 | Any | Any | Error | Invalid routing Number |
| Any | Any | 5.99 | Decline | Returned check for this account |
| Any | Any | 9999.99 | ManagerApproval | Warning: You have exceeded your allocated monthly transaction volume |

*   Any other combination of 9 digit routing number and account number will return an approval.

# Return Message Test ACH Data

If you use the following check information transactions will be moved to “submitted” at 5pm PST and the following account numbers will trigger returns at 8am on the 3rd day.

| Routing | Account | Amount | Response | Reason |
| :---: | :---: | :---: | :---: | --- |
| Any | 10178101 | Any | Returned | R01: Insufficient Funds |
| Any | 10178102 | Any | Returned | R02: Account Closed |
| Any | 10178103 | Any | Returned | R03: No Account/Unable to Locate Account |
| Any | 10178104 | Any | Returned | R04: Invalid Account Number |
| Any | 10178106 | Any | Returned | R06: Returned per Originating Depository Financial Institution's Request |
| Any | 10178107 | Any | Returned | R07: Authorization Revoked by Customer |
| Any | 10178108 | Any | Returned | R08: Payment Stopped |
| Any | 10178110 | Any | Returned | R10: Customer Advises not Authorized |
| Any | 10178116 | Any | Returned | R16: Account Frozen |

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/reference/sandbox/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Timeouts - USAePay Help

# API Timeout Handling

*   [Transaction API][1] parameter: "UMtimeout=15" 
*   [Transaction API][2] and [Soap API][3] HTTP Header: "ApiTimeout: 15"
*   Built-in support: [PHP Library][4] "$tran->timeout=15;"

## Background

There are variety of reasons why a transaction or api call may take longer than expected, from processor lag to systems issues or network outages. While we make every effort to minimize the total time an api call takes, there will be situations where a time out will be exceeded. In the typical configuration there are several timeouts in play:

1.   time the gateway will wait for a response from the processing networks
2.   time the developers HTTP client will wait for a response from the gateway
3.   max execution time of the developers script
4.   time the customer web browser will wait for a response from the developer's server

Item 1 varies depending on the backend processor, available failover options and the type of request (ie authorization vs batch close). Max timeouts range from 45s to 10min.

The developer typically has control over items 2 and 3. Developers often try to set #2 as low as possible to cover the majority of transactions and maintain a consistent user experience. Setting #2 too high can lead to end users giving up and either abandoning the sale or making a duplicate attempt. Setting #2 too low and transactions that would have been approved are lost.

In scenarios where an end customer is connecting to the developer's server, you must also take into account the web browser's timeout. ThS last item is dependent on the software being used by the customer and typically ranges from 1 to 5 minutes.

## What Can Go Wrong

When running transactions the biggest problem merchants encountered is multiple authorizations hitting the customer's account. Take the scenario where the developer has their timeout set to 15 seconds but the processor is slow and returns an approval after 18 seconds. These funds are now held on the customers account and the approved transaction will show up in the merchant's batch. The problem is that the developer's software gave up at 15 seconds and reported the transaction as an error. Assuming this error was also reported to the clerk or customer, chances are that the transaction is going to be retried. If the retry was also approved, there is now a duplicate transaction in the batch and the customer has double the funds held in their account.

## Solutions

### Timeout Auto-Reversal

The easiest solution is to pass in to the gateway the requested maximum time you are willing to wait for a reply. This value should be equal to or less than items #2 (HTTP timeout), #3 (application timeout) and #4 (customer browser timeout) above. The gateway starts a timer as soon as the request is received. There are three possible outcomes:

1.   Transaction takes less than the timeout requested and everything proceeds normally
2.   The timer is checked just before the request is sent out to the platform for authorization. If the timer exceeds the requested timeout, the transaction will be immediately returned as an error. The authorization does not get sent to the platform and no funds are held. 
3.   The timer is checked again right before the response is sent back to the developer. If the timeout was exceeded, the system automatically performs a void::release. Any funds that were held should be automatically returned to the cardholder immediately (depending on issuer participation) and the transaction will be marked as a void. This will prevent the transaction from being settled.

The timeout value can be configured three ways, listed in order of precedence:

1.   **API Parameter:** In the transaction api, the **UMtimeout** field can be set to the number of seconds desired. In soap UMtimeout can be passed into the runTransactionAPI method. For runTransaction, UMtimeout can be specified in the CustomFields array.
2.   **HTTP Header:** The transaction and soap api support the HTTP Header "**ApiTimeout**". 
3.   **Source key setting:** The merchant can log into their console and edit the settings of the source key. The API Timeout field contains the default timeout in seconds for all calls to this source key

In the event that multiple of the above are set, the API Parameter will override the HTTP header and the source key setting. The HTTP header will override the source key setting.

Please note that the timeout parameter does not affect how long the gateway will take to respond to your request. It only controls the maximum time an approval will be returned. To ensure that the HTTP call to the gateway never exceeds a given amount of time, you will need to implement a client side timeout. This is typically available in the development language or library being used.

## Testing

The following special test cards can be used in the sandbox environment to simulate slow processing. Each card has a predefined amount of time that it will take to process. This simulates a slow platform response.

### Slow Processing Cards

| Card Number | Expiration | Processing Time |
| --- | --- | --- |
| 4000000011112226 | 0919 | 5s |
| 4000000011112234 | 0919 | 15s |
| 4000000011112242 | 0919 | 30s |
| 4000000011112259 | 0919 | 45s |
| 4000000011112267 | 0919 | 60s |

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/transaction-api/
[2]: https://help.usaepay.info/developer/transaction-api/
[3]: https://help.usaepay.info/developer/soap-api/
[4]: https://help.usaepay.info/developer/sdks/php/


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Tokenization - USAePay Help

# Tokenization

Tokenization is the process of breaking a stream of text/numbers up into words, phrases, symbols, or other meaningful elements called tokens. This token can then be used in place of a credit card number when processing a transaction. This is useful when a developer does not want to under take the security requirements of storing card data. The tokens can be stored by the developer without needing to meet PCI requirements for PAN storage.

Tokenization can either be performed using a payment form or directly via the transaction api. When using the payment form, the credit card entry interface is presented by USAePay. The transaction api option allows the developer to present the interface (the data is still submitted directly to USAePay). Tokens can also be generated using the SOAP API. SOAP API token generation is usually used to tokenize cards that were stored previously by the merchant. Transaction API is the more secure method because it's done on our servers.

USAePay implements tokenization using the cc:save command. This command validates the card data and then returns a card reference token (UMcardRef). This card reference number is alpha numeric and can be up to 19 characters in length. The card reference number can be used in the card number (UMcard) field in most scenarios. What you get after saving a card is the token, card type and last 4 digits of the credit card. Tokens only store the card number and the expiration date. Tokens can also be used across multiple merchants. They are not related to a specific merchant.

## Step 1a: Payment Form

Log into the merchant console and create a new source key. Customize the paymentform and use the following templates.

Payment Form:

```
<html>

    <head>
    <title>Credit Card Information</title>
    </head>

    <body link="#000080" vlink="#000080" alink="#000080" text="#000000" bgcolor="#D4D7E4">
      [CardSwipeScript]
    <form name="epayform" action="[UMformURL]" method="POST" 
     onSubmit="document.epayform.submitbutton.value='Please Wait... Processing';" 
     autocomplete="off">
    <input type="hidden" name="UMsubmit" value="1">
    <input type="hidden" name="UMkey" value="[UMkey]">
    <input type="hidden" name="UMamount" value="[UMamount]">
    <input type="hidden" name="UMinvoice" value="[UMinvoice]">
    <input type="hidden" name="UMredir" value="[UMredir]">
    <input type="hidden" name="UMhash" value="[UMhash]">
    <input type="hidden" name="UMcommand" value="cc:save">
    <input type="hidden" name="UMechofields" value="[UMechofields]">
    <input type="hidden" name="UMformString" value="[UMformString]">
    <div align="center">
    <table border="0" width="500" cellpadding="4" cellspacing="0">
    <tr>
        <td bgcolor="#C4C7D4" width="500" colspan="2">
            <b><font face="Verdana, Arial">Credit Card Information:</font> </b>
            <img border="0" src="/images/visa.gif" width="44" height="28" hspace="3">
            <img border="0" src="/images/mastercard.gif" width="44" height="28" hspace="3">
            <img border="0" src="/images/amex.gif" width="44" height="28" hspace="3">
            <img border="0" src="/images/discover.gif" width="44" height="28" hspace="3">
        </td>
    </tr>
    <tr>
        <td bgcolor="#F0F0F0" width="234" align="right">
            <font face="Verdana" size="2">Card Number:</font>
        </td>
        <td bgcolor="#F0F0F0" width="266">
            <input type="text" name="UMcard" size="19">
        </td>
    </tr>
    <tr>
        <td bgcolor="#F0F0F0" width="234" align="right">
            <font face="Verdana" size="2">Card Expiration Date: </font>
        </td>
        <td bgcolor="#F0F0F0" width="266">
            <input type="text" name="UMexpir" size="4"> MMYY
        </td>
    </tr>
    <tr>
        <td bgcolor="#F0F0F0" width="500" colspan="2" align="center">
            <input type="submit" name="submitbutton" value="Store Card">
        </td>
    </tr>
    </table>
    </div>
    </form>
    </body>
    </html>
```

Result Page:

```
<html>
    <head><title> Result </title></head> 
    <body link="#000080" vlink="#000080" alink="#000080" text="#000000" bgcolor="#D4D7E4">
    <div align="center">
        <table border="0" width="500" cellpadding="4" cellspacing="0">

            <tr>
                <td bgcolor="#C4C7D4" width="500" colspan="2">
                <font face="Verdana, Arial">
                <p align="center">
                [ifApproved]
                    <center><font size=4><b>Card Data Stored</b></font></center><br>
                    Thank you, your card data has been stored.<br><br>
                    <table>
                    <tr><td><b>Card Ref: </b></td><td>[UMcardRef]</td></tr>
                    <tr><td><b>Card #: </b></td><td>[UMmaskedCardNum]  ([UMcardType])</td></tr>
                    </table>
                    <br><br>
                [/ifApproved]
                [ifDeclined]
                    <center><font size=4><b>Card Declined</b></font></center>
                    The bank has declined your card.  Please check your information and try again.
                    <br><br>
                [/ifDeclined]
                [ifError]
                    <center><font size=4><b>Unable to Store Card Data</b></font></center><br>
                    <font size=3>The system was unable to process your request: <b> "[UMerror]"</b>. 
                    Please use your browser back button and try again or contact the merchant for assistance.
                [/ifError]
                    <br><br><br>
                </p>
                </td>
            </tr>
        </table>
    </div>
    </body>
    </html>
```

The payment form link is shown on the source key edit screen. When linking to the payment form, pass the variable "UMredir" with the URL in your application that will accept the card token (UMcardRef). See step 2 below.

## Step 1b: Transaction API (Direct Submit)

Display a form to the customer that posts the credit card information to the transaction api with the "cc:save" command. The following form fields are needed:

| Field | Description |
| --- | --- |
| UMcommand | Hidden field set to "cc:save" |
| UMredir | Hidden field set to the url of your application that the customer will return to |
| UMkey | Source key generated in merchant console |
| UMhash | Hash created from pin |
| UMcard | The credit card number |
| UMexpir | The card expiration date in MMYY format |

The following is an example form:

```
<form action="https://usaepay.com/gate" method="POST" autocomplete="off">
    <input type="hidden" name="UMcommand" value="cc:save">
    <input type="hidden" name="UMredir" value="https://mysite.com/app?sessionid=1233324">
    <input type="hidden" name="UMkey" value="AAAABBBBCCCCDDDDEEEEFFFF">
    <input type="hidden" name="UMhash" value="sdsdfsdfsdfsdf">
    <input type="hidden" name="UMamount" value="0.00">
    <input type="hidden" name="UMinvoice" value="1">
    Card Number: <input type="text" name="UMcard" size="19"><br/>
    Card Expiration Date: <input type="text" name="UMexpir" size="4"> MMYY <br/>
    <input type="submit" name="submitbutton" value="Store Card">
    </form>
```

When the customer completes the form and submits the data they will be redirected back to your UMredir variable.

## Step 2: Response Variables

| Field | Description |
| --- | --- |
| UMcardRef | Card reference token. 16-19 digit alphanumeric string. It is returned with dashes but it is not required that these be stored. |
| UMcardType | The type of card that was submitted, ie “Visa” |
| UMmaskedCardNum | The masked out card number including the last 4 digits |

The card reference number (UMcardRef) is the token and can be safely stored and used by your application.

## Step 3: Processing a Sale

Processing a sale is handled the same as it would with a normal card number. The token (UMcardRef) should be passed in the same field that the full card number would have. In the transaction api for example, pass the token in the UMcard field. Use 0000 for the UMexpir field. The CVV/CVV2/CID value is not stored at the time of tokenization however it can be provided by the customer at the time of the transaction with a token.

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Transaction Codes - USAePay Help

# Transaction Codes

All transactions are tagged with three fields: Type, Status and Response. These three fields are essential in determining what a transaction is and where it is in the transaction flow.

# Transaction Type Codes

## Credit Cards

| Code | Description |
| --- | --- |
| A | Auth Only (will show in Queued Transactions) |
| C | Credit Card Refund (Credit) |
| S | Credit Card Sale (Simple Charge or New Order) |
| L | Capture (Capturing an Auth Only from the Queue) |
| V | Voided Credit Card Sale |
| Z | Voided Credit Card Refund (Credit) |
| _ | Verify Credit Card |

## Debit Cards

| Code | Description |
| --- | --- |
| D | Debit Card Sale |
| N | Debit Card Refund |
| W | Voided Debit Card Sale |

## Gift Cards

| Code | Description |
| --- | --- |
| F | Gift Card Sale |
| 3 | Gift Card Refund |
| T | Activation |
| 2 | Add Value |
| Q | Balance Inquiry |
| 4 | Change Status |
| 5 | Transfer Value |
| 6 | Generate |

## ACH (Electronic Checks)

| Code | Description |
| --- | --- |
| K | Check Sale |
| R | Check Refund |
| H | Reverse ACH |

## Offline (Reporting Only)

| Code | Description |
| --- | --- |
| G | Cash Sale |
| E | Cash Refund |
| X | Credit Card Sale |
| Y | Credit Card Refund |
| J | Check (Physical Check) |
| M | Gift Certificate |
| I | Invoice |

## Other

| Code | Description |
| --- | --- |
| P | Secure Vault Payment |

## Internal Gateway

| Code | Description |
| --- | --- |
| B | Batch Close Queued (this type is obsolete, we now mark pending transactions with batch id -2) |
| U | Upload Command |
| O | Open Batch |

# Transaction Status Codes

| Code | Description |
| --- | --- |
| N | New Transaction (has not been processed yet) |
| P | Pending (credit cards: batch has not closed / checks: has not been sent to fed yet) |
| B | Submitted (checks: sent to fed, void no longer available) |
| F | Funded (checks: date the money left the account) |
| S | Settled (credit cards: batch has been closed / checks: transaction has cleared) |
| E | Error |
| V | Voided (checks) |
| R | Returned (checks), Authorization Released (credit cards) |
| T | Timed Out (checks: no update from processor for 5 days) |
| M | On Hold, Manager Approval Required (checks) |
| H | On Hold, Pending Processor Review (checks) |

# Transaction Response Codes

| Code | Description |
| --- | --- |
| A | Approved |
| D | Declined |
| E | Error |
| V | Verification (Cardinal VbV) |

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

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


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Unit of Measure Codes - USAePay Help

# Unit of Measure Codes

The following is a list of valid unit of measure codes that should be used when sending in line item detail. Unit of measure may also be sent as an ANSI x-12 code but not all units of measure are valid for level 3 commercial card processing. For example, if AC is sent, ACR will be used for level 3 but if AG is sent, EA will be used for level 3 because there is not an equivalent Angstrom um.

| Code | Description |
| --- | --- |
| ACR | Acre |
| AMP | Ampere |
| APZ | Troy Ounce |
| BAR | Bar |
| BFT | Board Feet |
| BHP | Brake horse power |
| BHX | Hundred Boxes |
| BLL | Barrel |
| BTU | British Thermal Unit (BTU) |
| BX | Box |
| CEN | Hundred |
| CGM | Centigram |
| CMK | Square Centimeter |
| CMT | Centimeter |
| CS | Case |
| CUR | Curie |
| DAY | Days |
| DLT | Deciliter |
| DMK | Square Decimeter |
| DMQ | Cubic Decimeter |
| DMT | Decimeter |
| DWT | Pennyweight |
| DZN | Dozen |
| DZR | Dozen Pair |
| EA or EAC | Each |
| FAR | Farad |
| FOT | Foot |
| FTK | Square Foot |
| FTQ | Cubic Feet |
| GBQ | Gigabecquerel |
| GGR | Great Gross (Dozen Gross) |
| GLI | Gallon |
| GRM | Gram |
| GRN | Grain GB, US (64,798910 mg) |
| GRO | Gross |
| GRT | Gross Ton |
| HAR | Hectare |
| HGM | Hectogram |
| HLT | Hectoliter |
| HMT | Hectometer |
| HTZ | Hertz |
| HUR | Hours |
| INH | Inch |
| INK | Square Inch |
| INQ | Cubic Inches |
| JOU | Joules |
| KEL | Kelvin |
| KGM | Kilogram |
| KMK | Square Kilometer |
| KMQ | Kilograms per Cubic Meter |
| KPA | Kilopascal |
| KWH | Kilowatt Hour |
| KWT | Kilowatt |
| LEF | Leaf |
| LTR | Liter |
| MBR | Millibar |
| MCU | Millicurie |
| MGM | Milligram |
| MHZ | Megahertz |
| MI.T | Milliliter |
| MIK | Square Mile |
| MIL | Thousand |
| MIN | Minutes |
| MMK | Square Millimeters |
| MMT | Millimeter |
| MON | Months |
| MQH | Cubic Meter Per Hour |
| MTK | Square Meter |
| MTQ | Cubic Meter |
| MTR | Meter |
| NEW | Newton |
| NMI | Nautical Mile |
| NTT | Net Ton (2,000 LB). |
| OHM | Ohm |
| PAL | Pascal |
| PCE PCB PSC | Piece |
| PK | Package |
| PTI | Pint |
| QTI | Quart |
| RPM | Revolutions Per Minute |
| SEC | Seconds |
| SET | Set |
| SIE | Siemens |
| TNE | Metric Ton Kilograms |
| VLT | Volt |
| WCD | Cord |
| WEE | Week |
| WTT | Watt |
| YDK | Square Yard |
| YDQ | Cubic Yard |

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Verify By Visa/MasterCard Secure Code - USAePay Help

# VPAS (Verified by Visa) and UCAF (Mastercard Secure Code)

The gateway supports VPAS (Verified by Visa) and UCAF (Mastercard Securecode) with both an integrated authentication system and support for third party verification. Using the integrated system provides a quick, easy method for developers to support Verified by Visa and Mastercard Secure Code without requiring complicated XML messaging formats.

To use the integrated solution, the merchant must first have an account with Cardinal Commerce. (If the merchant does not have an account, but requests authentication, the transaction will be handled as if the cardholder is not enrolled in VPAS and/or UCAF.) The following process is required to use the integrated solution:

1.   Merchant's site collects cardholder information
2.   Merchant's site sends an authorization to gate.php with the UMcardauth flag set to true
3.   Gateway checks to see if the cardholder is enrolled in the VPAS and/or UCAF program. If the cardholder has not set a password, the transaction is processed normally (gate.php will return UMstatus=Approved or UMstatus=Declined). If the cardholder does have a password then gate.php will return UMstatus=Verification indicating that the merchant's site needs to prompt the user for a password. In the response, gate.php will also send back UMacsurl and UMpayload.
4.   Merchant's site must send the customer's browser to the URL contained in UMacsurl with three get values: PAReq set to the value in UMpayload, TermUrl set to the URL on the merchant's site that will continue the transaction, MD set to some identifying information (such as the order number) that will allow the order to proceed.
5.   Customer enters their username and password. If authentication is successful, they will be sent back to TermUrl with the variable PaRes set.
6.   Merchant's set sends a second authentication request to the gateway, identical to the one sent in step 2, except that UMpares is set to the value of PaRes.
7.   Then gate.php returns Approved or Declined as usual.

If you are using a third party verification system, or implementing the Cardinal Commerce API on the merchant's side, simply pass UMcavv and UMeci with the authentication request. (Please note: UMxid is obsolete and will be ignored.)

×Close
#### Search

From here you can search these documents. Enter your search terms below.


---

Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Webhooks - USAePay Help

# Webhooks

Webhooks typically are used to connect two different applications. When an event happens on the first application, it serializes data about that event and sends it to a webhook URL provided by the second application.

A merchant might want to set up a web hook to trigger an event every time inventory decreases in their merchant console to keep a third party shopping cart in sync.

In this case, the merchant would set up a web hook in their merchant console (application #1) for any inventory adjustments (event = `product.inventory.adjusted`) and when that event is triggered, then their merchant console will send an inventory JSON payload to the shopping cart (application #2) URL provided.

From your Dashboard, you can access these reports by clicking ‘Settings’ on the side menu bar, then 'Webhooks' in the dropdown.

## Two Factor

All Users will be prompted to enter one of their two factor authentication methods in order to access the section. If the User does not have two factor setup, they will need to enable it for their Username.

To learn how to setup Two Factor Authentication for your User, go [here][1].

# Add Webhook

In order to add a new webhook, click the **Add Webhook** button at the top of the page. You can then start filling out all of the fields in the modal. Once you are done, click the **Add Hook** button at the bottom right of the modal. If you want to cancel, click the **Close** button at the bottom left of the modal.

_**All of the fields are required**_

## Fields

*   Webhook Label
    *   The name of the webhook you are creating.

*   Hook URL
    *   Where the hook will send the data once it is triggered.
    *   The trigger and the data will be different depending on the hook or hooks that have been chosen.
    *   The merchant will need to acquire the webhook URL from a second application.

*   Events
    *   You can select the hook events from the dropdown list. You must choose 1 or more.
    *   A list of [Events][2] can be seen below.

*   Authentication
    *   Security
    *   More will be discussed in the [Authentication][3] section below

*   Signature
    *   For verification of the Webhook's origin
    *   More will be discussed in the [Signature][4] section below

## Events

| Event | Description |
| :--- | :--- |
| `cau.submitted` | Card has been submitted to processor for update |
| `cau.updated_expiration` | An updated expiration data has been received |
| `cau.updated_card` | An updated card number has been received |
| `cau.contact_customer` | Received message from issuer to contact customer for a new card number |
| `cau.account_closed` | Received notification of account closure |
| `ach.submitted` | ACH status updated to submitted. |
| `ach.settled` | ACH status updated to settled. |
| `ach.returned` | ACH status updated to returned. |
| `ach.failed` | ACH status updated to failed. |
| `ach.note_added` | Note added to ACH transaction. |
| `product.inventory.ordered` | The inventory has been adjusted after a sale. |
| `product.inventory.adjusted` | The inventory has been adjusted in the Products Database. |
| `transaction.sale.success` | A sale transaction has been approved |
| `transaction.sale.failure` | A sale transaction failed - includes declines and errors |
| `transaction.sale.voided` | A sale transaction was voided |
| `transaction.sale.captured` | A sale transaction was captured |
| `transaction.sale.adjusted` | A sale transaction was adjusted |
| `transaction.sale.queued` | A sale transaction was queued |
| `transaction.sale.unvoided` | A voided sale transaction was unvoided |
| `transaction.refund.success` | A refund transaction approved |
| `transaction.refund.voided` | A refund transaction voided |
| `settlement.batch.success` | A batch settles successfully |
| `settlement.batch.failure` | A batch receives an error when attempting to settle |

## Authentication

This setting is for the Authorization header, this will have the webhook authenticate itself with the server at the URL provided.

*   **None:** No Authentication header sent
*   **Basic:** Sends a basic authentication header with the included username and password

### Basic

## Signature

This sends a signature to the webhook using the X-Signature header. The merchant will be able to validate the incoming webhook with the shared secret (API Key or Signature Key) using the signature header. This is to verify where the webhook came from.

*   **None:** no signature is sent
*   **Source Key:** This uses the chosen source key as the signature key
*   **Signature Key:** This sends a signature key that is unique to the merchant

### API Key

### Signature Key

# Edit Webhook

*   To edit a webhook, click on an existing webhook in the list, this will open a new modal. Any of the fields can be edited and events can be added or removed.
*   If any changes need to be made to the Authentication section, the Authentication settings will need to be reset by clicking **Reset Webhook Authentication**. If you want to cancel this change, simply click the button again, which now says **Keep Current Configuration**.

*   To save changes, click **Save Changes**.

### Cancel Changes

*   To cancel any changes, they need to click **Close**, then confirm by clicking **Abandon Changes**.

### Delete Webhook

To delete the webhook, click the **Delete Hook** button at the bottom of the modal. Another modal will appear to confirm the delete.

# Example

*   Authentication: None
*   Signature: None

```
{
  "type": "event",
  "event_triggered": "2021-04-07 15:14:26",
  "event_type": "transaction.sale.success",
  "event_body": {
    "merchant": {
      "merch_key": "7qwnw4j2kmqvrb3"
    },
    "object": {
      "type": "transaction",
      "key": "1nftvzx7xtcggm3",
      "amount_detail": {
        "amount": "5.00"
      },
      "authcode": "538228"
    }
  },
  "event_id": "c088bc2884a2c3b4043991b04d96ceda7c65043778233dedb26bfd5853705dfd"
}
```

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/merchant/guide/twofactor/
[2]: https://help.usaepay.info/merchant/guide/settings/webhooks/#events
[3]: https://help.usaepay.info/merchant/guide/settings/webhooks/#authentication
[4]: https://help.usaepay.info/merchant/guide/settings/webhooks/#signature
