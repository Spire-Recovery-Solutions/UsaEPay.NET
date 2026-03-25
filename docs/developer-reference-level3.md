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
