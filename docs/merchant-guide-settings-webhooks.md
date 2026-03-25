Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Webhooks - USAePay Help

# Webhooks

Webhooks typically are used to connect two different applications. When an event happens on the first application, it serializes data about that event and sends it to a webhook URL provided by the second application.

A merchant might want to set up a web hook to trigger an event every time inventory decreases in their merchant console to keep a third party shopping cart in sync.

In this case, the merchant would set up a web hook in their merchant console (application #1) for any inventory adjustments (event = `product.inventory.adjusted`) and when that event is triggered, then their merchant console will send an inventory JSON payload to the shopping cart (application #2) URL provided.

From your Dashboard, you can access these reports by clicking ‘Settings’ on the side menu bar, then 'Webhooks' in the dropdown.

## Two Factor

All Users will be prompted to enter one of their two factor authentication methods in order to access the section. If the User does not have two factor setup, they will need to enable it for their Username.

To learn how to setup Two Factor Authentication for your User, go [here][1].

# Add Webhook

In order to add a new webhook, click the **Add Webhook** button at the top of the page. You can then start filling out all of the fields in the modal. Once you are done, click the **Add Hook** button at the bottom right of the modal. If you want to cancel, click the **Close** button at the bottom left of the modal.

_**All of the fields are required**_

## Fields

*   Webhook Label
    *   The name of the webhook you are creating.

*   Hook URL
    *   Where the hook will send the data once it is triggered.
    *   The trigger and the data will be different depending on the hook or hooks that have been chosen.
    *   The merchant will need to acquire the webhook URL from a second application.

*   Events
    *   You can select the hook events from the dropdown list. You must choose 1 or more.
    *   A list of [Events][2] can be seen below.

*   Authentication
    *   Security
    *   More will be discussed in the [Authentication][3] section below

*   Signature
    *   For verification of the Webhook's origin
    *   More will be discussed in the [Signature][4] section below

## Events

| Event | Description |
| :--- | :--- |
| `cau.submitted` | Card has been submitted to processor for update |
| `cau.updated_expiration` | An updated expiration data has been received |
| `cau.updated_card` | An updated card number has been received |
| `cau.contact_customer` | Received message from issuer to contact customer for a new card number |
| `cau.account_closed` | Received notification of account closure |
| `ach.submitted` | ACH status updated to submitted. |
| `ach.settled` | ACH status updated to settled. |
| `ach.returned` | ACH status updated to returned. |
| `ach.failed` | ACH status updated to failed. |
| `ach.note_added` | Note added to ACH transaction. |
| `product.inventory.ordered` | The inventory has been adjusted after a sale. |
| `product.inventory.adjusted` | The inventory has been adjusted in the Products Database. |
| `transaction.sale.success` | A sale transaction has been approved |
| `transaction.sale.failure` | A sale transaction failed - includes declines and errors |
| `transaction.sale.voided` | A sale transaction was voided |
| `transaction.sale.captured` | A sale transaction was captured |
| `transaction.sale.adjusted` | A sale transaction was adjusted |
| `transaction.sale.queued` | A sale transaction was queued |
| `transaction.sale.unvoided` | A voided sale transaction was unvoided |
| `transaction.refund.success` | A refund transaction approved |
| `transaction.refund.voided` | A refund transaction voided |
| `settlement.batch.success` | A batch settles successfully |
| `settlement.batch.failure` | A batch receives an error when attempting to settle |

## Authentication

This setting is for the Authorization header, this will have the webhook authenticate itself with the server at the URL provided.

*   **None:** No Authentication header sent
*   **Basic:** Sends a basic authentication header with the included username and password

### Basic

## Signature

This sends a signature to the webhook using the X-Signature header. The merchant will be able to validate the incoming webhook with the shared secret (API Key or Signature Key) using the signature header. This is to verify where the webhook came from.

*   **None:** no signature is sent
*   **Source Key:** This uses the chosen source key as the signature key
*   **Signature Key:** This sends a signature key that is unique to the merchant

### API Key

### Signature Key

# Edit Webhook

*   To edit a webhook, click on an existing webhook in the list, this will open a new modal. Any of the fields can be edited and events can be added or removed.
*   If any changes need to be made to the Authentication section, the Authentication settings will need to be reset by clicking **Reset Webhook Authentication**. If you want to cancel this change, simply click the button again, which now says **Keep Current Configuration**.

*   To save changes, click **Save Changes**.

### Cancel Changes

*   To cancel any changes, they need to click **Close**, then confirm by clicking **Abandon Changes**.

### Delete Webhook

To delete the webhook, click the **Delete Hook** button at the bottom of the modal. Another modal will appear to confirm the delete.

# Example

*   Authentication: None
*   Signature: None

```
{
  "type": "event",
  "event_triggered": "2021-04-07 15:14:26",
  "event_type": "transaction.sale.success",
  "event_body": {
    "merchant": {
      "merch_key": "7qwnw4j2kmqvrb3"
    },
    "object": {
      "type": "transaction",
      "key": "1nftvzx7xtcggm3",
      "amount_detail": {
        "amount": "5.00"
      },
      "authcode": "538228"
    }
  },
  "event_id": "c088bc2884a2c3b4043991b04d96ceda7c65043778233dedb26bfd5853705dfd"
}
```

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/merchant/guide/twofactor/
[2]: https://help.usaepay.info/merchant/guide/settings/webhooks/#events
[3]: https://help.usaepay.info/merchant/guide/settings/webhooks/#authentication
[4]: https://help.usaepay.info/merchant/guide/settings/webhooks/#signature
