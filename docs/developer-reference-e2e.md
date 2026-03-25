Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# End to End Encryption - USAePay Help

# End To End Encryption

End to end encryption provides an enhanced layer of security for swiped and keyed transactions by encrypting the card data at point of entry. For swiped transactions this typically happens in the mag reader head, for keyed transactions, this happens in a standalone, tamper resistant keypad. The data remains encrypted while it passes through the device, software and communication channels to the gateway. Once in the secure environment of the gateway it is decrypted and used for processing.

Encrypted card data may be passed in the same fields as the clear text card and swipe data is passed. The data must be proceeded by "enc://" and if binary, base 64 encoded. For example, if the following data is read from the device:

```
%B4444*********7779^EXAMPLE TEST CARD^2512*********?;4444*******7779=2512*********?|0600|411785952BA27844F49434FFC261A5CE6E6F3F46BE836D8612B56A53DB480167FD63DA9892B0F471626CDC0B75376AF6759403CA58A4C263|350518BC1F8D63CBD2C47D19FC3C1824D3AFB5CC54AC878595902B927DE850D3||61400200|19CFF0CF6F24A9FAFAFF80EF8258F1C1A81A9D90DB474413E127206B3C32DF4885223C20777CB9FAB21D38864B92BDA43D6699610EDC7D62|C516F135E93DFEB|25776C75DC32EEFD|FFFF87BABCDEF000001|1316||0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

You would submit the following in the UMmagstripe variable (transaction api), CreditCard.MagStripe (soap api) or creditcard.magstripe (rest api):

```
UMmagstripe = "enc://JUI0NDQ0KioqKioqKioqNzc3OV5FWEFNUExFIFRFU1QgQ0FSRF4yNTEyKioqKioqKioqPzs0NDQ0KioqKioqKjc3Nzk9MjUxMioqKioqKioqKj98MDYwMHw0MTE3ODU5NTJCQTI3ODQ0RjQ5NDM0RkZDMjYxQTVDRTZFNkYzRjQ2QkU4MzZEODYxMkI1NkE1M0RCNDgwMTY3RkQ2M0RBOTg5MkIwRjQ3MTYyNkNEQzBCNzUzNzZBRjY3NTk0MDNDQTU4QTRDMjYzfDM1MDUxOEJDMUY4RDYzQ0JEMkM0N0QxOUZDM0MxODI0RDNBRkI1Q0M1NEFDODc4NTk1OTAyQjkyN0RFODUwRDN8fDYxNDAwMjAwfDE5Q0ZGMENGNkYyNEE5RkFGQUZGODBFRjgyNThGMUMxQTgxQTlEOTBEQjQ3NDQxM0UxMjcyMDZCM0MzMkRGNDg4NTIyM0MyMDc3N0NCOUZBQjIxRDM4ODY0QjkyQkRBNDNENjY5OTYxMEVEQzdENjJ8QzUxNkYxMzVFOTNERkVCfDI1Nzc2Qzc1REMzMkVFRkR8RkZGRjg3QkFCQ0RFRjAwMDAwMXwxMzE2fHwwMDAweHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4eHh4"
```

# Data Format

The encrypted card data can be passed in several different formats. If using a supported manufacturer format, the developer can pass the entire raw block of data as it was read from the reader and the gateway will handle parsing it. It is important that the gateway receive all data components to be able to decrypt successfully. The most common mistake made by developers is to send only the encrypted data and omit the KSN block which contains the key info necessary for decryption.

 For developers implementing their own encryption or using an unsupported raw format, the data can be sent in json format with the fields below. When using this format it is not necessary to base64 encode the entire block, just the encrypted data element.

## Generic format

| Field | Required | Description |
| --- | --- | --- |
| k | yes | Key ID. For DUKPT based encryption this should be the full KSN block in binhex format including the KSID, device serial and encryption counter |
| t | yes* | All tracks encrypted in one data block. This should be either base64 or binhex encoded. |
| t1 | yes* | Encrypted track 1 data. This should be either base64 or binhex encoded. |
| t2 | yes* | Encrypted track 2 data. This should be either base64 or binhex encoded. |
| t3 | yes* | Encrypted track 3 data. This should be either base64 or binhex encoded. |
| m | no | Masked track data, all tracks in single string |
| c | no | Encrypted, manually keyed card number and expiration. |
| s | no | Some encryption backends will require a device serial number. This is different than the device serial that is present in the KSN. |
| p | no | Specify the encryption format parser. Supported formats: "i" == Ingenico On Guard. Currently only required if using the OnGuard format. |
| b | no | Specify the encryption backend provider. If omitted, requests will be routed automatically based on ksn |

*   Device will either concatenate all tracks together before encryption or encrypt each track individually. Use t if encrypted all together in single block and use t1, t2, t3 if encrypted separately.

```json
enc://{"k":"FFFF9019F8E999000009","t1":"411785952BA27844F49434FFC261A5CE6E6F3F46BE836D8612B56A53DB480167FD63DA9892B0F471626CDC0B75376AF6759403CA58A4C263","t2":"350518BC1F8D63CBD2C47D19FC3C1824D3AFB5CC54AC878595902B927DE850D3","m":"%B4444*********7779^EXAMPLE TEST CARD^2512*********?;4444*******7779=2512*********?"}
```

## IDTech Format

Devices from IDTech or that use IDTech swiper heads share a common data format. The data block should start with \x02 followed by two bytes indicating length, followed by \x80 or \x85. Some devices may output raw binary while others may return a binhex encoding. You can pass the entire raw block of data as it was read from the reader and the gateway will handle parsing it. If in binary format, base64 encode. If in binhex the data can be left as is.

## Magtek Format

Supported Magtek devices output a pipe "|" delimited format. The entire block of data is needed for decryption and should be passed into the gateway as is.

```
%B4444*********7779^EXAMPLE TEST CARD^2512*********?;4444*******7779=2512*********?|0600|411785952BA27844F49434FFC261A5CE6E6F3F46BE836D8612B56A53DB480167FD63DA9892B0F471626CDC0B75376AF6759403CA58A4C263|350518BC1F8D63CBD2C47D19FC3C1824D3AFB5CC54AC878595902B927DE850D3||61400200|19CFF0CF6F24A9FAFAFF80EF8258F1C1A81A9D90DB474413E127206B3C32DF4885223C20777CB9FAB21D38864B92BDA43D6699610EDC7D62|C516F135E93DFEB|25776C75DC32EEFD|FFFF87BABCDEF000001|1316||0000xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
```

# Devices

The following is only a partial list of supported devices. For assistance with a device not listed please see the dcontact integration support.

## IDTech SecureKey

Supported. Data can be passed through without modification, but the data must be proceeded by "enc://" and if binary, base 64 encoded.

## Magtek iDynamo

If you are using the idynamo "mtSCRALib" library, you can use the following example:

```
NSString *responseString = [mtSCRALib getResponseData];
NSData *responseData = [responseString dataUsingEncoding:NSASCIIStringEncoding];
NSString *encodedString = [NSString stringWithFormat:@"enc://%@", [^] [responseData base64Encoding]];
```

and then send encodedString to the gateway as MagStripe.

×Close
#### Search

From here you can search these documents. Enter your search terms below.
