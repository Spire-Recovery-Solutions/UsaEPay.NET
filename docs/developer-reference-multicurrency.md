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
