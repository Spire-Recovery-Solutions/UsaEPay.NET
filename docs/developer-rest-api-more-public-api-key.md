Published Time: Mon, 16 Mar 2026 19:34:26 GMT

# Public API Key - USAePay Help

# Public API Key

## Retrieve Public API Key

This endpoint will generate a new public API key associated with the API key. If a Public API Key already exists for this API Key, the gateway will return the existing key instead. Public API Keys are used in the [Client JS Library][1] to create `payment_key`s

The API endpoint is as follows:

```
GET /api/v2/publickey
```

### Example Request

```
curl -X GET https://sandbox.usaepay.com/api/v2/publickey
    -H "Content-Type: application/json"
    -H "Authorization: Basic X1Y4N1F0YjUxM0NkM3ZhYk03UkMwVGJ0SldlU284cDc6czIvYWJjZGVmZ2hpamtsbW5vcC9iNzRjMmZhOTFmYjBhMDk3NTVlMzc3ZWU4ZTIwYWE4NmQyYjkyYzNkMmYyNzcyODBkYjU5NWY2MzZiYjE5MGU2"
```

### Example Response

```
"_P120LOb42a1SF4nN54z1Hp0256K7bl3ki0x8iuJP9"
```

×Close
#### Search

From here you can search these documents. Enter your search terms below.

[1]: https://help.usaepay.info/developer/client-js
