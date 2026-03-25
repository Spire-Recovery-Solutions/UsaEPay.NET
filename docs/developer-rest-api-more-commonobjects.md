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
