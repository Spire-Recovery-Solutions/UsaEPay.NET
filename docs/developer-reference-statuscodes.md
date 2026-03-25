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
