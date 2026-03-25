Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Sandbox System - USAePay Help

# Sandbox System

The sandbox provides a full simulation of the production gateway and is the ideal way to test your integration.

If you are a developer and your client, the merchant, has not setup a live gateway account, a sandbox account will allow you to develop your application without delay. The sandbox environment mimics a live Merchant Console account (with a few exceptions), so you can expect the same results when switching to the live account once it's setup. The exceptions mentioned above are:

*   The sandbox is self-contained and will not send any data to outside processing platforms (Visa, MC, or any banks). Charging a "live" credit card will not result in a charge showing up on the cardholder's bill.
*   While "live" credit cards and check information will work on the sandbox server, it is recommended that you use the test information. A list of test information can be found in the developer's center here:
    *   [Test Credit Cards][1]
    *   [Test ACH Data][2]

*   The sandbox is designed for testing functionality, and is not a load testing environment. Transaction throughput is intentionally throttled.
*   The transaction database is cleared every few months.
*   The sandbox does not support encrypted swipe or EMV testing. It does support un encrypted and manual entries on the devices.

so you can expect the same results when switching to the live account once it's setup. Your application will only need to swap the source key from the developer one to the live one.

# Requesting an Account

## Step 1: Access Developer Portal

Go to the following URL: [https://developer.usaepay.com/_developer/app/login][3]

### Existing Developer Portal Account

If you already have a developer portal login, enter your email and password, select **Login to your account**, and skip to the next step.

### No Developer Portal Account

If you do not have a developer portal login, select **[Register for an account][4]**

Fill out the form completely and select **Register new account** and the developer portal will open automatically.

## Step 2: Request Sandbox Account in Developer Portal

To set up a sandbox account, click the **Request Test Account** tab in the sidebar and fill out the form _COMPLETELY_ to obtain your login credentials. The fields are:

*   **Requested Username**- The username you would like to use to login to the sandbox account.
*   **Contact Name**
*   **Company Name**
*   **Email**- The email you would like the test account credentials sent to.

Leaving any fields blank will delay your account setup. When complete click **Submit Request**. If successful, the message below will be shown.

Processing time takes a few hours during regular office hours based on Pacific Standard Time. Requests on weekends may be processed with a 24 hour or more turnaround time.

# Accessing Your Sandbox Account

Once your sandbox account is created, you will receive an email with your login credentials. Login with these credentials at [https://sandbox.usaepay.com/login][5].

# Switching from Development to Production

When testing and development is completed switch the source key to the key generated from the live account and change the processing URL from `sandbox.usaepay.com/gate` to `www.usaepay.com/gate`

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/reference/testcards/
[2]: https://help.usaepay.info/developer/reference/testcheckdata/
[3]: https://developer.usaepay.com/_developer/app/login
[4]: https://developer.usaepay.com/_developer/app/register
[5]: https://sandbox.usaepay.com/login
