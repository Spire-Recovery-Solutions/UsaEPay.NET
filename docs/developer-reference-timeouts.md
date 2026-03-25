Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Timeouts - USAePay Help

# API Timeout Handling

*   [Transaction API][1] parameter: "UMtimeout=15" 
*   [Transaction API][2] and [Soap API][3] HTTP Header: "ApiTimeout: 15"
*   Built-in support: [PHP Library][4] "$tran->timeout=15;"

## Background

There are variety of reasons why a transaction or api call may take longer than expected, from processor lag to systems issues or network outages. While we make every effort to minimize the total time an api call takes, there will be situations where a time out will be exceeded. In the typical configuration there are several timeouts in play:

1.   time the gateway will wait for a response from the processing networks
2.   time the developers HTTP client will wait for a response from the gateway
3.   max execution time of the developers script
4.   time the customer web browser will wait for a response from the developer's server

Item 1 varies depending on the backend processor, available failover options and the type of request (ie authorization vs batch close). Max timeouts range from 45s to 10min.

The developer typically has control over items 2 and 3. Developers often try to set #2 as low as possible to cover the majority of transactions and maintain a consistent user experience. Setting #2 too high can lead to end users giving up and either abandoning the sale or making a duplicate attempt. Setting #2 too low and transactions that would have been approved are lost.

In scenarios where an end customer is connecting to the developer's server, you must also take into account the web browser's timeout. ThS last item is dependent on the software being used by the customer and typically ranges from 1 to 5 minutes.

## What Can Go Wrong

When running transactions the biggest problem merchants encountered is multiple authorizations hitting the customer's account. Take the scenario where the developer has their timeout set to 15 seconds but the processor is slow and returns an approval after 18 seconds. These funds are now held on the customers account and the approved transaction will show up in the merchant's batch. The problem is that the developer's software gave up at 15 seconds and reported the transaction as an error. Assuming this error was also reported to the clerk or customer, chances are that the transaction is going to be retried. If the retry was also approved, there is now a duplicate transaction in the batch and the customer has double the funds held in their account.

## Solutions

### Timeout Auto-Reversal

The easiest solution is to pass in to the gateway the requested maximum time you are willing to wait for a reply. This value should be equal to or less than items #2 (HTTP timeout), #3 (application timeout) and #4 (customer browser timeout) above. The gateway starts a timer as soon as the request is received. There are three possible outcomes:

1.   Transaction takes less than the timeout requested and everything proceeds normally
2.   The timer is checked just before the request is sent out to the platform for authorization. If the timer exceeds the requested timeout, the transaction will be immediately returned as an error. The authorization does not get sent to the platform and no funds are held. 
3.   The timer is checked again right before the response is sent back to the developer. If the timeout was exceeded, the system automatically performs a void::release. Any funds that were held should be automatically returned to the cardholder immediately (depending on issuer participation) and the transaction will be marked as a void. This will prevent the transaction from being settled.

The timeout value can be configured three ways, listed in order of precedence:

1.   **API Parameter:** In the transaction api, the **UMtimeout** field can be set to the number of seconds desired. In soap UMtimeout can be passed into the runTransactionAPI method. For runTransaction, UMtimeout can be specified in the CustomFields array.
2.   **HTTP Header:** The transaction and soap api support the HTTP Header "**ApiTimeout**". 
3.   **Source key setting:** The merchant can log into their console and edit the settings of the source key. The API Timeout field contains the default timeout in seconds for all calls to this source key

In the event that multiple of the above are set, the API Parameter will override the HTTP header and the source key setting. The HTTP header will override the source key setting.

Please note that the timeout parameter does not affect how long the gateway will take to respond to your request. It only controls the maximum time an approval will be returned. To ensure that the HTTP call to the gateway never exceeds a given amount of time, you will need to implement a client side timeout. This is typically available in the development language or library being used.

## Testing

The following special test cards can be used in the sandbox environment to simulate slow processing. Each card has a predefined amount of time that it will take to process. This simulates a slow platform response.

### Slow Processing Cards

| Card Number | Expiration | Processing Time |
| --- | --- | --- |
| 4000000011112226 | 0919 | 5s |
| 4000000011112234 | 0919 | 15s |
| 4000000011112242 | 0919 | 30s |
| 4000000011112259 | 0919 | 45s |
| 4000000011112267 | 0919 | 60s |

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/transaction-api/
[2]: https://help.usaepay.info/developer/transaction-api/
[3]: https://help.usaepay.info/developer/soap-api/
[4]: https://help.usaepay.info/developer/sdks/php/
