Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Tokenization - USAePay Help

# Tokenization

Tokenization is the process of breaking a stream of text/numbers up into words, phrases, symbols, or other meaningful elements called tokens. This token can then be used in place of a credit card number when processing a transaction. This is useful when a developer does not want to under take the security requirements of storing card data. The tokens can be stored by the developer without needing to meet PCI requirements for PAN storage.

Tokenization can either be performed using a payment form or directly via the transaction api. When using the payment form, the credit card entry interface is presented by USAePay. The transaction api option allows the developer to present the interface (the data is still submitted directly to USAePay). Tokens can also be generated using the SOAP API. SOAP API token generation is usually used to tokenize cards that were stored previously by the merchant. Transaction API is the more secure method because it's done on our servers.

USAePay implements tokenization using the cc:save command. This command validates the card data and then returns a card reference token (UMcardRef). This card reference number is alpha numeric and can be up to 19 characters in length. The card reference number can be used in the card number (UMcard) field in most scenarios. What you get after saving a card is the token, card type and last 4 digits of the credit card. Tokens only store the card number and the expiration date. Tokens can also be used across multiple merchants. They are not related to a specific merchant.

## Step 1a: Payment Form

Log into the merchant console and create a new source key. Customize the paymentform and use the following templates.

Payment Form:

```
<html>

    <head>
    <title>Credit Card Information</title>
    </head>

    <body link="#000080" vlink="#000080" alink="#000080" text="#000000" bgcolor="#D4D7E4">
      [CardSwipeScript]
    <form name="epayform" action="[UMformURL]" method="POST" 
     onSubmit="document.epayform.submitbutton.value='Please Wait... Processing';" 
     autocomplete="off">
    <input type="hidden" name="UMsubmit" value="1">
    <input type="hidden" name="UMkey" value="[UMkey]">
    <input type="hidden" name="UMamount" value="[UMamount]">
    <input type="hidden" name="UMinvoice" value="[UMinvoice]">
    <input type="hidden" name="UMredir" value="[UMredir]">
    <input type="hidden" name="UMhash" value="[UMhash]">
    <input type="hidden" name="UMcommand" value="cc:save">
    <input type="hidden" name="UMechofields" value="[UMechofields]">
    <input type="hidden" name="UMformString" value="[UMformString]">
    <div align="center">
    <table border="0" width="500" cellpadding="4" cellspacing="0">
    <tr>
        <td bgcolor="#C4C7D4" width="500" colspan="2">
            <b><font face="Verdana, Arial">Credit Card Information:</font> </b>
            <img border="0" src="/images/visa.gif" width="44" height="28" hspace="3">
            <img border="0" src="/images/mastercard.gif" width="44" height="28" hspace="3">
            <img border="0" src="/images/amex.gif" width="44" height="28" hspace="3">
            <img border="0" src="/images/discover.gif" width="44" height="28" hspace="3">
        </td>
    </tr>
    <tr>
        <td bgcolor="#F0F0F0" width="234" align="right">
            <font face="Verdana" size="2">Card Number:</font>
        </td>
        <td bgcolor="#F0F0F0" width="266">
            <input type="text" name="UMcard" size="19">
        </td>
    </tr>
    <tr>
        <td bgcolor="#F0F0F0" width="234" align="right">
            <font face="Verdana" size="2">Card Expiration Date: </font>
        </td>
        <td bgcolor="#F0F0F0" width="266">
            <input type="text" name="UMexpir" size="4"> MMYY
        </td>
    </tr>
    <tr>
        <td bgcolor="#F0F0F0" width="500" colspan="2" align="center">
            <input type="submit" name="submitbutton" value="Store Card">
        </td>
    </tr>
    </table>
    </div>
    </form>
    </body>
    </html>
```

Result Page:

```
<html>
    <head><title> Result </title></head> 
    <body link="#000080" vlink="#000080" alink="#000080" text="#000000" bgcolor="#D4D7E4">
    <div align="center">
        <table border="0" width="500" cellpadding="4" cellspacing="0">

            <tr>
                <td bgcolor="#C4C7D4" width="500" colspan="2">
                <font face="Verdana, Arial">
                <p align="center">
                [ifApproved]
                    <center><font size=4><b>Card Data Stored</b></font></center><br>
                    Thank you, your card data has been stored.<br><br>
                    <table>
                    <tr><td><b>Card Ref: </b></td><td>[UMcardRef]</td></tr>
                    <tr><td><b>Card #: </b></td><td>[UMmaskedCardNum]  ([UMcardType])</td></tr>
                    </table>
                    <br><br>
                [/ifApproved]
                [ifDeclined]
                    <center><font size=4><b>Card Declined</b></font></center>
                    The bank has declined your card.  Please check your information and try again.
                    <br><br>
                [/ifDeclined]
                [ifError]
                    <center><font size=4><b>Unable to Store Card Data</b></font></center><br>
                    <font size=3>The system was unable to process your request: <b> "[UMerror]"</b>. 
                    Please use your browser back button and try again or contact the merchant for assistance.
                [/ifError]
                    <br><br><br>
                </p>
                </td>
            </tr>
        </table>
    </div>
    </body>
    </html>
```

The payment form link is shown on the source key edit screen. When linking to the payment form, pass the variable "UMredir" with the URL in your application that will accept the card token (UMcardRef). See step 2 below.

## Step 1b: Transaction API (Direct Submit)

Display a form to the customer that posts the credit card information to the transaction api with the "cc:save" command. The following form fields are needed:

| Field | Description |
| --- | --- |
| UMcommand | Hidden field set to "cc:save" |
| UMredir | Hidden field set to the url of your application that the customer will return to |
| UMkey | Source key generated in merchant console |
| UMhash | Hash created from pin |
| UMcard | The credit card number |
| UMexpir | The card expiration date in MMYY format |

The following is an example form:

```
<form action="https://usaepay.com/gate" method="POST" autocomplete="off">
    <input type="hidden" name="UMcommand" value="cc:save">
    <input type="hidden" name="UMredir" value="https://mysite.com/app?sessionid=1233324">
    <input type="hidden" name="UMkey" value="AAAABBBBCCCCDDDDEEEEFFFF">
    <input type="hidden" name="UMhash" value="sdsdfsdfsdfsdf">
    <input type="hidden" name="UMamount" value="0.00">
    <input type="hidden" name="UMinvoice" value="1">
    Card Number: <input type="text" name="UMcard" size="19"><br/>
    Card Expiration Date: <input type="text" name="UMexpir" size="4"> MMYY <br/>
    <input type="submit" name="submitbutton" value="Store Card">
    </form>
```

When the customer completes the form and submits the data they will be redirected back to your UMredir variable.

## Step 2: Response Variables

| Field | Description |
| --- | --- |
| UMcardRef | Card reference token. 16-19 digit alphanumeric string. It is returned with dashes but it is not required that these be stored. |
| UMcardType | The type of card that was submitted, ie “Visa” |
| UMmaskedCardNum | The masked out card number including the last 4 digits |

The card reference number (UMcardRef) is the token and can be safely stored and used by your application.

## Step 3: Processing a Sale

Processing a sale is handled the same as it would with a normal card number. The token (UMcardRef) should be passed in the same field that the full card number would have. In the transaction api for example, pass the token in the UMcard field. Use 0000 for the UMexpir field. The CVV/CVV2/CID value is not stored at the time of tokenization however it can be provided by the customer at the time of the transaction with a token.

×Close
#### Search

From here you can search these documents. Enter your search terms below.
