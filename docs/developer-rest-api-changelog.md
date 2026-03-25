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
