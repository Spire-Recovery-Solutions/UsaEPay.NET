Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# CVV Result Codes - USAePay Help

# Card Security Codes

## Introduction

Card Verification Value (CVV) provides an additional level of online fraud protection. The card security codes are 3 or 4 digit codes printed or embossed on Visa, Mastercard, American Express and Discover cards. These codes are also referred to as CVV2, CVC, CSC or CCID. Their purpose is to provide additional protection against fraudulent card use.

## Storage and Security Considerations

The card security codes are considered highly sensitive data and should never be stored, even in an encrypted format. Storage of this value will place the merchant in jeopardy with PCI and CISP compliance.

## Reference

### Where To Find the Code

#### American Express

A four digit non-embossed number on the face of the card.

#### Discover

A three digit non-embossed number on the back of the card printed within the signature panel after the account number.

#### MasterCard

A three digit non-embossed number on the back of the card printed within the signature panel after the account number.

#### Visa

A three digit non-embossed number on the back of the card printed within the signature panel after the account number.

### Response Codes

The following is a list of result codes for the CVV2/CVC2/CID verification system and what each one indicates.

The card security codes are 3 or 4 digit codes printed or embossed on Visa, Mastercard, American Express and Discover cards. These codes are also referred to as CVV2, CVC, CSC or CCID. Their purpose is to provide additional protection against fraudulent card use. Below is a list of possible result codes.

| Code | Meaning |
| --- | --- |
| M | Match |
| N | No Match |
| P | Not Processed |
| S | Should be on card but not so indicated |
| U | Issuer Not Certified |
| X | No response from association |
| (blank) | No CVV2/CVC data available for transaction. |

### Additional Information

For addition information on card security codes please visit the following links:

*   [Wikipedia Card Security code page][1]
*   [Visa Information on CVV2][2]

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: http://en.wikipedia.org/wiki/Card_Security_Code
[2]: http://usa.visa.com/personal/security/visa_security_program/3_digit_security_code.html
