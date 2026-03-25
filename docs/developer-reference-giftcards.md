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
