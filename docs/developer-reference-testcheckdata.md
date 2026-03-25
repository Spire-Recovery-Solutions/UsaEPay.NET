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
