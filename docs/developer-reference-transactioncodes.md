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
