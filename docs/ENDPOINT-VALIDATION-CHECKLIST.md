# USAePay.NET SDK — Endpoint Validation Checklist

## Legend
- **Curl**: Raw curl probe executed against sandbox
- **SDK**: Factory method + request/response models exist
- **Test**: Unit test (U) and/or Integration test (I) exists
- **Fields**: All response fields validated against wire format

---

## 1. TRANSACTIONS (POST /api/v2/transactions)

### Sale Operations
| # | Command | Curl | SDK | Test | Fields | Notes |
|---|---------|:----:|:---:|:----:|:------:|-------|
| 1 | `cc:sale` (card) | ✅ | ✅ | U+I | ✅ | Full field probe with all optional fields |
| 2 | `cc:sale` (token) | ✅ | ✅ | I | ✅ | Token passed via creditcard.number |
| 3 | `cc:sale` (payment_key) | ❌ | ✅ | ❌ | ❌ | No sandbox payment_key to test |
| 4 | `check:sale` | ✅ | ✅ | I | ✅ | proc_refnum confirmed |
| 5 | `cash:sale` | ✅ | ✅ | ❌ | ✅ | Sandbox returns error 80 "not allowed from this source" |
| 6 | `quicksale` | ✅ | ✅ | I | ✅ | Uses trankey from prior sale |
| 7 | `customer:sale` | ✅ | ✅ | U | ❌ | Probed via customer endpoint agent |
| 8 | `authonly` | ✅ | ✅ | I | ✅ | |

### Capture Operations
| # | Command | Curl | SDK | Test | Fields | Notes |
|---|---------|:----:|:---:|:----:|:------:|-------|
| 9 | `cc:capture` | ✅ | ✅ | I | ✅ | |
| 10 | `cc:capture:reauth` | ❌ | ✅ | I | ❌ | Needs expired auth to test properly |
| 11 | `cc:capture:override` | ❌ | ✅ | I | ❌ | Same |
| 12 | `cc:capture:error` | ❌ | ✅ | I | ❌ | Same |

### Refund Operations
| # | Command | Curl | SDK | Test | Fields | Notes |
|---|---------|:----:|:---:|:----:|:------:|-------|
| 13 | `cc:credit` (open) | ✅ | ✅ | I | ✅ | |
| 14 | `check:credit` | ✅ | ✅ | I | ✅ | proc_refnum confirmed |
| 15 | `cash:refund` | ✅ | ✅ | ❌ | ✅ | Sandbox returns error 80 |
| 16 | `refund` (connected) | ✅ | ✅ | I | ✅ | Partial refund tested |
| 17 | `quickrefund` | ✅ | ✅ | I | ✅ | |
| 18 | `customer:refund` | ❌ | ✅ | ❌ | ❌ | Not probed |

### Void/Adjust Operations
| # | Command | Curl | SDK | Test | Fields | Notes |
|---|---------|:----:|:---:|:----:|:------:|-------|
| 19 | `void` | ✅ | ✅ | I | ✅ | |
| 20 | `unvoid` | ✅ | ✅ | I | ✅ | |
| 21 | `cc:void:release` | ✅ | ✅ | I | ✅ | |
| 22 | `creditvoid` | ✅ | ✅ | I | ✅ | |
| 23 | `cc:adjust` | ✅ | ✅ | I | ✅ | |
| 24 | `cc:refund:adjust` | ✅ | ✅ | I | ✅ | |
| 25 | `refund:adjust` | ❌ | ✅ | ❌ | ❌ | Alias — not separately probed |

### Post Auth
| # | Command | Curl | SDK | Test | Fields | Notes |
|---|---------|:----:|:---:|:----:|:------:|-------|
| 26 | `cc:postauth` | ✅ | ✅ | ❌ | ✅ | |

### Tokenization
| # | Command | Curl | SDK | Test | Fields | Notes |
|---|---------|:----:|:---:|:----:|:------:|-------|
| 27 | `cc:save` | ✅ | ✅ | I | ✅ | savedcard.expiration confirmed |

---

## 2. TRANSACTION RETRIEVAL

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 28 | `/transactions/{key}` | GET | ✅ | ✅ | I | ✅ | Full detail with platform, available_actions |
| 29 | `/transactions?limit=N` | GET | ✅ | ✅ | I | ✅ | |
| 30 | `/transactions/{key}/send` | POST | ❌ | ✅ | ❌ | ❌ | Receipt send — not probed |
| 31 | `/transactions/{key}/receipts/{id}` | GET | ❌ | ✅ | ❌ | ❌ | Receipt retrieve — not probed |

---

## 3. TOKENS

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 32 | `/tokens` | POST | ✅ | ✅ | ❌ | ✅ | Create from trankey — wrapped in {"token":{}} |
| 33 | `/tokens/{token}` | GET | ✅ | ✅ | ❌ | ❌ | Sandbox returned "not found" |
| 34 | `/tokens` (bulk array) | POST | ❌ | ❌ | ❌ | ❌ | No factory method |

---

## 4. BATCHES

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 35 | `/batches?limit=N` | GET | ✅ | ✅ | I | ✅ | List uses different field names (sales/credits) — now fixed |
| 36 | `/batches/{key}` | GET | ✅ | ✅ | I | ✅ | |
| 37 | `/batches/current` | GET | ✅ | ✅ | I | ✅ | |
| 38 | `/batches/{key}/transactions` | GET | ❌ | ✅ | I | ❌ | Not separately curl-probed |
| 39 | `/batches/current/transactions` | GET | ✅ | ✅ | ❌ | ✅ | |
| 40 | `/batches/current/close` | POST | ✅ | ✅ | I | ✅ | |

---

## 5. CUSTOMERS

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 41 | `/customers` | POST | ✅ | ✅ | U | ✅ | custom_fields now captured |
| 42 | `/customers/{key}` | GET | ✅ | ✅ | U | ✅ | payment_methods + billing_schedules confirmed |
| 43 | `/customers?limit=N` | GET | ✅ | ✅ | U | ✅ | |
| 44 | `/customers/{key}` | PUT | ✅ | ✅ | U | ✅ | |
| 45 | `/customers/{key}` | DELETE | ✅ | ✅ | U | ✅ | |
| 46 | `/customers/bulk` | DELETE | ❌ | ✅ | U | ❌ | Not curl-probed |
| 47 | `/customers` (from txn) | POST | ✅ | ✅ | ❌ | ✅ | Create from transaction_key |
| 48 | `/customers/{key}/transactions` | GET | ✅ | ✅ | ❌ | ✅ | Customer transaction history |

---

## 6. CUSTOMER PAYMENT METHODS

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 49 | `/customers/{key}/payment_methods` | POST | ✅ | ✅ | U | ✅ | pay_type now captured. API expects array. |
| 50 | `/customers/{key}/payment_methods/{key}` | GET | ✅ | ✅ | U | ✅ | |
| 51 | `/customers/{key}/payment_methods` | GET | ✅ | ✅ | U | ✅ | |
| 52 | `/customers/{key}/payment_methods/{key}` | PUT | ✅ | ✅ | U | ✅ | |
| 53 | `/customers/{key}/payment_methods/{key}` | DELETE | ✅ | ✅ | U | ✅ | |
| 54 | `/customers/{key}/payment_methods/bulk` | DELETE | ❌ | ✅ | U | ❌ | Not curl-probed |

---

## 7. BILLING SCHEDULES

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 55 | `/customers/{key}/billing_schedules` | POST | ✅ | ✅ | U | ✅ | send_receipt type fixed (bool) |
| 56 | `/customers/{key}/billing_schedules/{key}` | GET | ✅ | ✅ | U | ✅ | rules array confirmed |
| 57 | `/customers/{key}/billing_schedules` | GET | ✅ | ✅ | U | ✅ | |

---

## 8. PRODUCTS (sandbox not configured — error 41001)

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 58 | `/products` | POST | ⚠️ | ✅ | U | ❌ | Sandbox: "not configured for product database" |
| 59 | `/products/{key}` | GET | ⚠️ | ✅ | U | ❌ | Same |
| 60 | `/products?limit=N` | GET | ⚠️ | ✅ | U | ❌ | Same |
| 61 | `/products/{key}` | PUT | ⚠️ | ✅ | U | ❌ | Same |
| 62 | `/products/{key}` | DELETE | ⚠️ | ✅ | U | ❌ | Same |

---

## 9. PRODUCT CATEGORIES (sandbox not configured)

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 63-67 | CRUD | All | ⚠️ | ✅ | U | ❌ | Sandbox: error 41001 |

---

## 10. INVENTORY LOCATIONS (sandbox not configured)

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 68-72 | CRUD | All | ⚠️ | ✅ | U | ❌ | Sandbox: error 41001 |

---

## 11. INVENTORY (sandbox not configured)

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 73-77 | CRUD | All | ⚠️ | ✅ | U | ❌ | Sandbox: error 41001 |

---

## 12. BULK TRANSACTIONS

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 78 | `/bulk_transactions` | POST | ❌ | ❌ | ❌ | ❌ | Upload — requires multipart, TODO in code |
| 79 | `/bulk_transactions/{key}` | GET | ❌ | ✅ | U | ❌ | Not curl-probed |
| 80 | `/bulk_transactions/current` | GET | ❌ | ✅ | U | ❌ | Not curl-probed |
| 81 | `/bulk_transactions/{key}/transactions` | GET | ❌ | ✅ | U | ❌ | Not curl-probed |
| 82 | `/bulk_transactions/current/transactions` | GET | ❌ | ✅ | U | ❌ | Not curl-probed |
| 83 | `/bulk_transactions/{key}/pause` | POST | ❌ | ✅ | U | ❌ | Not curl-probed |
| 84 | `/bulk_transactions/current/pause` | POST | ❌ | ✅ | U | ❌ | Not curl-probed |
| 85 | `/bulk_transactions/{key}/resume` | POST | ❌ | ✅ | U | ❌ | Not curl-probed |
| 86 | `/bulk_transactions/current/resume` | POST | ❌ | ✅ | U | ❌ | Not curl-probed |

---

## 13. PAYMENT ENGINE — DEVICES (sandbox not configured)

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 87-93 | Register, CRUD, Settings, TermConfig, Delete | All | ⚠️ | ✅ | U | ❌ | Requires physical terminal |

---

## 14. PAYMENT ENGINE — PAY REQUESTS (sandbox not configured)

| # | Endpoint | Method | Curl | SDK | Test | Fields | Notes |
|---|----------|--------|:----:|:---:|:----:|:------:|-------|
| 94-96 | Create, Retrieve, Cancel | All | ⚠️ | ✅ | U | ❌ | Requires physical terminal |

---

## 15. DECLINE / ERROR HANDLING

| # | Scenario | Curl | SDK | Test | Fields | Notes |
|---|----------|:----:|:---:|:----:|:------:|-------|
| 97 | Generic decline (4000300011112220) | ✅ | ✅ | I | ✅ | error_code: 10127 |
| 98 | Do not Honor (4000300211112228) | ✅ | ✅ | I | ✅ | error_code: 10205 |
| 99 | Insufficient funds (4000300611112224) | ✅ | ✅ | I | ✅ | error_code: 10251 |
| 100 | Transaction not permitted (4000300811112222) | ✅ | ✅ | I | ✅ | error_code: 10257 |
| 101 | Restricted card (4000300911112221) | ✅ | ✅ | I | ✅ | error_code: 10262 |
| 102 | CVV failure (4000301311112225) | ✅ | ✅ | I | ✅ | error_code: 10297 |
| 103 | Gateway error (bad command) | ✅ | ✅ | ❌ | ✅ | error_code: 20019 (int not string) |

---

## 16. CARD BRAND / AVS / PARTIAL AUTH

| # | Scenario | Curl | SDK | Test | Fields | Notes |
|---|----------|:----:|:---:|:----:|:------:|-------|
| 104 | Visa sale | ✅ | ✅ | I | ✅ | |
| 105 | MasterCard sale | ❌ | ✅ | I | ❌ | Not curl-probed |
| 106 | Amex sale | ❌ | ✅ | I | ❌ | Not curl-probed |
| 107 | Discover sale | ❌ | ✅ | I | ❌ | Not curl-probed |
| 108 | AVS YYY | ✅ | ✅ | I | ✅ | |
| 109 | AVS NYZ | ❌ | ✅ | I | ❌ | |
| 110 | AVS YNA | ❌ | ✅ | I | ❌ | |
| 111 | AVS NNN | ❌ | ✅ | I | ❌ | |
| 112 | AVS XXR | ❌ | ✅ | I | ❌ | |
| 113 | AVS XXS | ❌ | ✅ | I | ❌ | |
| 114 | Partial Auth 50% | ❌ | ✅ | I | ❌ | |
| 115 | Partial Auth 75% | ❌ | ✅ | I | ❌ | |

---

## SUMMARY

| Category | Total | Curl Probed | SDK Coverage | Tested | Fields Validated |
|----------|-------|-------------|--------------|--------|------------------|
| Transaction commands | 27 | 22 | 27 | 21 | 22 |
| Transaction retrieval | 4 | 2 | 4 | 2 | 2 |
| Tokens | 3 | 2 | 2 | 0 | 1 |
| Batches | 6 | 5 | 6 | 4 | 5 |
| Customers | 8 | 7 | 8 | 5 | 7 |
| Payment Methods | 6 | 5 | 6 | 5 | 5 |
| Billing Schedules | 3 | 3 | 3 | 3 | 3 |
| Products | 5 | 0 (blocked) | 5 | 5 | 0 |
| Categories | 5 | 0 (blocked) | 5 | 5 | 0 |
| Inventory Locations | 5 | 0 (blocked) | 5 | 5 | 0 |
| Inventory | 5 | 0 (blocked) | 5 | 5 | 0 |
| Bulk Transactions | 9 | 0 | 8 | 8 | 0 |
| Payment Engine | 10 | 0 (blocked) | 10 | 10 | 0 |
| Decline/Error | 7 | 7 | 7 | 6 | 7 |
| Card/AVS/Partial | 12 | 1 | 12 | 12 | 1 |
| **TOTAL** | **115** | **54** | **113** | **96** | **53** |

### Blocked by sandbox configuration:
- Products, Categories, Inventory, Locations (error 41001)
- Payment Engine Devices/PayRequests (requires physical terminal)
- Cash sale/refund (error 80 "not allowed from this source")
- Bulk transaction upload (requires multipart file — no factory method)
