Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Verify By Visa/MasterCard Secure Code - USAePay Help

# VPAS (Verified by Visa) and UCAF (Mastercard Secure Code)

The gateway supports VPAS (Verified by Visa) and UCAF (Mastercard Securecode) with both an integrated authentication system and support for third party verification. Using the integrated system provides a quick, easy method for developers to support Verified by Visa and Mastercard Secure Code without requiring complicated XML messaging formats.

To use the integrated solution, the merchant must first have an account with Cardinal Commerce. (If the merchant does not have an account, but requests authentication, the transaction will be handled as if the cardholder is not enrolled in VPAS and/or UCAF.) The following process is required to use the integrated solution:

1.   Merchant's site collects cardholder information
2.   Merchant's site sends an authorization to gate.php with the UMcardauth flag set to true
3.   Gateway checks to see if the cardholder is enrolled in the VPAS and/or UCAF program. If the cardholder has not set a password, the transaction is processed normally (gate.php will return UMstatus=Approved or UMstatus=Declined). If the cardholder does have a password then gate.php will return UMstatus=Verification indicating that the merchant's site needs to prompt the user for a password. In the response, gate.php will also send back UMacsurl and UMpayload.
4.   Merchant's site must send the customer's browser to the URL contained in UMacsurl with three get values: PAReq set to the value in UMpayload, TermUrl set to the URL on the merchant's site that will continue the transaction, MD set to some identifying information (such as the order number) that will allow the order to proceed.
5.   Customer enters their username and password. If authentication is successful, they will be sent back to TermUrl with the variable PaRes set.
6.   Merchant's set sends a second authentication request to the gateway, identical to the one sent in step 2, except that UMpares is set to the value of PaRes.
7.   Then gate.php returns Approved or Declined as usual.

If you are using a third party verification system, or implementing the Cardinal Commerce API on the merchant's side, simply pass UMcavv and UMeci with the authentication request. (Please note: UMxid is obsolete and will be ignored.)

×Close
#### Search

From here you can search these documents. Enter your search terms below.
