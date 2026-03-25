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
