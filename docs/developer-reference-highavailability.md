Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# High Availability - USAePay Help

# High Availability Developers Guide

## Introduction

USAePay provides a variety of tools and strategies to provide merchants with as close to 100% continuous processing of payments as possible. This guide provides developers with the background information and examples necessary to leverage these tools and maximize the availability of payment processing.

## System Architecture

USAePay operates multiple data centers across the country. Each data center is equipped with redundant power backup and generation, cooling, fire suppression, and network connections. In the event of a data center losing grid power, UPS and on site generators ensure continuous power. The data centers are geographically dispersed to prevent downtime due to natural disaster. The data centers are connected to multiple Internet backbone providers to mitigate peering issues between providers. They house all resources necessary to process payments, independent of any other location. During normal operation, all data is replicated between locations and all locations can utilize resources such as platform connections out of other locations. In the event of a major outage such as complete power loss in one location, the other locations can operate completely independently.

Within each data center, servers are operated in high-availability (HA) clusters. During normal operation, traffic is load balanced across all servers in the cluster to ensure highest possible performance. The cluster continuously monitors each server for availability. In the event a server goes off line for any reason (planned maintenance, hardware failure, software misconfiguration) the cluster automatically stops routing traffic to the server. Whenever possible, servers are engineered with redundant components such as multiple power supplies and hard drives in raid configurations.

Server and data center maintenance is always done in a non-invasive manner that does not affect transaction processing.

## URLS

### Default: www.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www.usaepay.com/login][1], [https://secure.usaepay.com/login][2] |
| Transaction API | [https://www.usaepay.com/gate][3], [https://secure.usaepay.com/gate][4] |
| SOAP API | [https://www.usaepay.com/soap/gate][5], [https://secure.usaepay.com/soap/gate][6] |
| Ping | [https://www.usaepay.com/ping][7], [https://secure.usaepay.com/ping][8] |

Currently the default url is setup to direct traffic the primary processing location '03'. In the event of an outage at '03', DNS is updated to route these urls to primary location '01' or '02'. The difference between www.usaepay.com and secure.usaepay.com is the type of SSL certificate used. www.usaepay.com uses an "unchained" 2-year Verisign certificate. This certificate should work with the widest range of ssl libraries including those that do not support chained certificates. Secure.usaepay.com uses an [extended validation (EV) ssl certificate][9]. This provides the green bar in modern web browsers but causes some issues with certificate validation in older SSL libaries.

### Primary Processing Location: www-01.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www-01.usaepay.com/login][10] |
| Transaction API | [https://www-01.usaepay.com/gate][11] |
| SOAP API | [https://www-01.usaepay.com/soap/gate][12] |
| Ping | [https://www-01.usaepay.com/ping][13] |

This url sends traffic directly to the primary processing location '01'. It is recommended that this is the first backup url that developers try. This location has all resources necessary to operate independently of the other primary location.

### Primary Processing Location: www-02.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www-02.usaepay.com/login][14] |
| Transaction API | [https://www-02.usaepay.com/gate][15] |
| SOAP API | [https://www-02.usaepay.com/soap/gate][16] |
| Ping | [https://www-02.usaepay.com/ping][17] |

This url sends traffic directly to the primary processing location '02'. It is recommended that this is the second backup url that developers try. This location has all resources necessary to operate independently of the other primary location.

### Primary Processing Location: www-03.usaepay.com

| Use | URL |
| --- | --- |
| Login | [https://www-03.usaepay.com/login][18] |
| Transaction API | [https://www-03.usaepay.com/gate][19] |
| SOAP API | [https://www-03.usaepay.com/soap/gate][20] |
| Ping | [https://www-03.usaepay.com/ping][21] |

This url sends traffic directly to the primary processing location '03'. During normal operation, this url is identical to the default www.usaepay.com url.

## Testing Connectivity

USAePay does not allowing 'pinging' its servers. ICMP Echo requests (Ping) are dropped at the edge firewalls to prevent network probing and simplistic DDOS attacks. Ping requests sent to any of our servers will result in time out messages. Ping timeouts do not mean that the server is not available. The following example is the normal expected output:

```
PING www.usaepay.com (209.239.233.7): 56 data bytes
```

1.   -- www.usaepay.com ping statistics ---

10 packets transmitted, 0 packets received, 100% packet loss

To test your ability to connect to a given url, access the ping urls listed above. They will respond with a state and cluster id. As long as the response you receive starts with the string "UP" then the url is available for use:

```
# curl https://www.usaepay.com/ping
    UP:ca403
```

The string "DOWN" will appear if the datacenter is not recommended for use. The majority of the time the url will still be able to accept transactions. The DOWN flag is used to indicate planned maintenance where there is the potential for a disruption of service.

The second part of the string indicates which cluster in which datacenter you are connecting to. For example "ca403" means that you were routed to cluster 3 in datacenter ca4.

## Firewall Rules

If your network is utilizing outbound firewall rules to restrict which IPs can be connected to, you should create outbound rules for the following subnets:

| Subnet |
| --- |
| 216.181.230.0/23 |
| 209.239.233.0/24 |
| 209.220.191.0/24 |
| 65.132.197.0/24 |

If you would like to create more granular rules, the following IPs may be returned via DNS resultion depending on region and availability:

| Host | IPs |
| --- | --- |
| www.usaepay.com | 216.181.230.7, 209.220.191.7, 209.239.233.7, 65.132.197.7 |
| secure.usaepay.com | 216.181.230.8, 209.220.191.8, 209.239.233.8, 65.132.197.8 |
| usaepay.com | 216.181.230.100, 209.220.191.100, 209.239.233.100, 65.132.197.100 |
| www-01.usaepay.com | 216.181.230.9, 209.239.233.105 |
| www-02.usaepay.com | 216.181.231.9, 209.220.191.9, 216.181.230.76 |
| www-03.usaepay.com | 209.239.233.9, 216.181.230.9 |
| www-04.usaepay.com | 65.132.197.9, 216.181.231.9 |
| sandbox.usaepay.com | 216.181.230.129, 209.239.233.129 |

## Redundancy Strategies

### Passive, DNS Failover

This "strategy" is actually the default behavior of the primary www.usaepay.com and secure.usaepay.com urls. If your server/workstation is using DNS to resolve the default URLs you will receive the "default" datacenter (currently CA4 - 209.239.233.*). If there is maintenance or an outage, our DNS servers will automatically start resolving to one of the other two datacenters. Currently we are configured to automatically failover within 15 seconds of a failure. The time to live (TTL) flag on our DNS records is set to 3 minutes. Unfortunately your ISP's DNS servers (or even your server or browser) may cache the old DNS entry for longer, leading to longer failover times for some users. In these cases it might be necessary to manually override your dns server through the use of a host file entry. On a UNIX server this can be done by editing /etc/hosts and on windows by editing C:\Windows\System32\drivers\etc\hosts (or equivalent). To force www.usaepay.com to go to primary '02', you would add the line:

```
209.220.191.7  www.usaepay.com
```

To "implement" this strategy, merchants and customers simply need to retry the transaction every few minutes until the DNS has been updated. The only recommended coding change that developers might consider are the proper setting of the timeout variable and catching the time out error:

#### .NET DLL

```vb
Try

      usaepay.Timeout = 60
      usaepay.Sale()

    Catch ex As Exception

      If ex.Message = "Error writing to the gateway: Unable to connect to the remote server" Then

        errormessage = "Unable to process payment, please try again in a few minutes"

      Else

        errormessage = ex.Message

      End If

    End Try
```

#### PHP Library

```php
if($tran->Process())
    {
        echo "<b>Card approved</b><br>";
        echo "<b>Authcode:</b> " . $tran->authcode . "<br>";
        echo "<b>AVS Result:</b> " . $tran->avs_result . "<br>";
        echo "<b>Cvv2 Result:</b> " . $tran->cvv2_result . "<br>";
    } else if($tran->curlerror == 'connect() timed out!') {
        echo "<b>Unable to process payment, please try again in a few minutes<br>";
    } else {
        echo "<b>Card Declined</b> (" . $tran->result . ")<br>";
        echo "<b>Reason:</b> " . $tran->error . "<br>"; 
        if($tran->curlerror) echo "<b>Curl Error:</b> " . $tran->curlerror . "<br>";    
    }
```

### Active Failover, Backup URL Retry

Using this strategy, any failures to connect to the primary url are automatically retried on a secondary url. This method is useful in applications that do not have reliable network connections such as mobile internet solutions. In the event that the initial connection to the gateway fails, the developer will trap the error and automatically retry again on the second url. This process can be repeated for all urls if the developer chooses to.

If choosing this strategy it is important to consider the duplicate transaction problem. Many developers set the connection timeout too low and end up giving up before the gateway has finished processing. While it is rare, some processing backends can take as much as 120 seconds to respond. For example if a developer has their timeout set to 30 seconds and the gateway takes 45 seconds to complete an approval. The application would have returned a time out error even though the gateway approved the transaction and placed it in the batch. The application then retries the transaction on the backup url where it is again approved and placed in the batch. There are now two transactions on the gateway even though the application has only recorded one. While the obvious solution is to raise the application timeout, this can lead to customers giving up and retrying the transaction on their own.

There are two ways to deal with this problem. The first, and easier method is to use the duplicate folding functionality. Duplicate folding will check all incoming transactions for duplicates. If a duplicate is detected, the original transaction response details will be returned instead of processing the transaction again. In the scenario where the first transaction times out (but is authed on the gateway) and then retried on the backup url, the second call to the backup url will detect the duplicate and return the details that would have been returned on the first call if the connection hadn't been dropped prematurely.

When using this method its important to be careful that intentional duplicate charges are not accidentally folded. For example if a customer decides to buy the same product for the same amount on the same card.

### Connection Scoreboard for Load Balancing and Failover

Another strategy that works particular well for high traffic, multi-threaded applications is to maintain a connection scoreboard. The scoreboard keeps track of the number of open connections, hits (successful transactions) and errors for each url. This data is then used to select the best url to send the next transaction to. During normal operation, transactions will load balance between the primary urls. During an outage, after the first failure is recorded, all other transactions will automatically route to the other primary urls.

Example scoreboard:

| URL | Working | Hits | Errors | Last Error |
| --- | --- | --- | --- | --- |
| www-01.usaepay.com | 0 | 100 | 0 |  |
| www-02.usaepay.com | 1 | 100 | 0 |  |

In the above example, both links have successfully processed 100 transactions and we are currently in the middle of processing a transaction on www-02. The logic for selecting the next URL is to pull the url with the lowest errors, lowest working and lowest hits. In the above example we would select www-01 as the next url since it is currently idle (working=0) and has the same number of errors (0) as www-02.

Once an error occurs on one of the connections, the error counter will be increased:

| URL | Working | Hits | Errors | Last Error |
| --- | --- | --- | --- | --- |
| www-01.usaepay.com | 0 | 101 | 1 | 2010-01-01 11:12:59 |
| www-02.usaepay.com | 1 | 2310 | 0 |  |

Since we are sorting first by error count, the next url will now be www-02 because www-01 has a higher error count. Traffic will continue to go to www-02 until the error count is cleared. For this reason, error counts should be cleared periodically. This can be done by setting all error counts to 0 when the last error date is greater than a certain amount of time (ie, 60 minutes).

#### MySQL/PHP Example

The following is a "proof of concept" using php and a mysql database. The same thing should be possible in any language as long as you have the ability to share information between threads, sessions, application instances, users, etc.

SQL Scheme:

```sql
CREATE TABLE connections (
       url CHAR(6),
       working INT,
       hits INT,
       errors INT,
       lasterror DATETIME,
       UNIQUE KEY (url)
    );
    INSERT INTO connections SET url='www-01', working=0, hits=0, errors=0;
    INSERT INTO connections SET url='www-02', working=0, hits=0, errors=0;
    INSERT INTO connections SET url='www-03', working=0, hits=0, errors=0;
```

PHP Transaction Library

```php
// select url
    $res = mysql_query("SELECT url 
                        FROM connections 
                        ORDER BY errors,working,hits 
                        LIMIT 1");
    list($url) = mysql_fetch_row($res);

    // in case something is wrong with the mysql table
    if(!$url) $url='www-01';

    // update scoreboard to reflect that we are processing a transaction on this link
    mysql_query("UPDATE connections 
                 SET working=working+1 
                 WHERE url='" . mysql_real_escape_string($url) . "'");

    $tran->gatewayurl = 'https://' . $url . '.usaepay.com/gate';
    $res = $tran->Process();

    // log error, modify this statement to adjust what you consider a failure
    //  as is, this considers anything that causes an underlying http error to
    //  be a gateway failure.
    if(!$res && strlen($tran->curlerror)>0)
    {
       mysql_query("UPDATE connections 
                    SET working=working-1, errors=errors+1, lasterror=now() 
                    WHERE url='" . mysql_real_escape_string($url) . "'");
    }

    // else log success
    else {
       mysql_query("UPDATE connections 
                    SET working=working-1, hits=hits+1 
                    WHERE url='" . mysql_real_escape_string($url) . "'");

       // automatically clear stale error counts (optional)
       mysql_query("UPDATE connections 
                    SET errors=0, lasterror=null 
                    WHERE lasterror<'" . date('Y-m-d H:i:s', strtotime('-30 minutes')) . "'");

    }
```

### Pro-Active Failover, URL Monitoring

Using this strategy, the developer keeps a list of processing urls in a database or config file. Each url is then pinged every few minutes. If one fails, it is marked as down or otherwise removed from the list. The payment application is then coded to pull its active gateway url from the database or config file.

## Notifications

USAePay provides real time notification of network issues via our [twitter feed][22].

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://www.usaepay.com/login
[2]: https://secure.usaepay.com/login
[3]: https://www.usaepay.com/gate
[4]: https://secure.usaepay.com/gate
[5]: https://www.usaepay.com/soap/gate
[6]: https://secure.usaepay.com/soap/gate
[7]: https://www.usaepay.com/ping
[8]: https://secure.usaepay.com/ping
[9]: http://en.wikipedia.org/wiki/EV_SSL
[10]: https://www-01.usaepay.com/login
[11]: https://www-01.usaepay.com/gate
[12]: https://www-01.usaepay.com/soap/gate
[13]: https://www-01.usaepay.com/ping
[14]: https://www-02.usaepay.com/login
[15]: https://www-02.usaepay.com/gate
[16]: https://www-02.usaepay.com/soap/gate
[17]: https://www-02.usaepay.com/ping
[18]: https://www-03.usaepay.com/login
[19]: https://www-03.usaepay.com/gate
[20]: https://www-03.usaepay.com/soap/gate
[21]: https://www-03.usaepay.com/ping
[22]: http://twitter.com/usaepay
