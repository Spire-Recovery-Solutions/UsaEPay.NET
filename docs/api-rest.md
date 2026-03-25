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
