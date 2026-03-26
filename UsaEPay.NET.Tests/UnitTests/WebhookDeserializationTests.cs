using System.Text.Json;
using TUnit.Core;
using UsaEPay.NET.Models;
using UsaEPay.NET.Models.Enumerations.Event;
using UsaEPay.NET.Models.Events;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class WebhookDeserializationTests
{
    private static readonly JsonSerializerOptions Options = USAePaySerializerContext.Default.Options;

    #region ACH Events

    [Test]
    public async Task ACHSubmitted_Deserializes_AllFields()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-03-15 14:30:00",
            "event_type": "ach.submitted",
            "event_id": "evt_ach_sub_001",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_abc123"
                },
                "object": {
                    "type": "transaction",
                    "key": "tnx_ach_001",
                    "refnum": "200001",
                    "orderid": "ORD-5001",
                    "check": {
                        "trackingcode": "TRK123456"
                    },
                    "uri": "transactions/tnx_ach_001"
                },
                "changes": {
                    "old": {
                        "status": "pending",
                        "processed": "2025-03-14 10:00:00"
                    },
                    "new": {
                        "status": "submitted",
                        "processed": "2025-03-15 14:30:00"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<ACHStatusEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.Type).IsEqualTo("event");
        await Assert.That(result.EventType).IsEqualTo(EventType.AchSubmitted);
        await Assert.That(result.EventId).IsEqualTo("evt_ach_sub_001");
        await Assert.That(result.EventTriggered).IsNotNull();

        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Merchant).IsNotNull();
        await Assert.That(result.EventBody.Merchant!.MerchKey).IsEqualTo("merch_abc123");

        await Assert.That(result.EventBody.Object).IsNotNull();
        await Assert.That(result.EventBody.Object!.Type).IsEqualTo("transaction");
        await Assert.That(result.EventBody.Object.Key).IsEqualTo("tnx_ach_001");
        await Assert.That(result.EventBody.Object.Refnum).IsEqualTo(200001L);
        await Assert.That(result.EventBody.Object.Orderid).IsEqualTo("ORD-5001");
        await Assert.That(result.EventBody.Object.Check).IsNotNull();
        await Assert.That(result.EventBody.Object.Check!.Trackingcode).IsEqualTo("TRK123456");
        await Assert.That(result.EventBody.Object.Uri).IsEqualTo("transactions/tnx_ach_001");

        await Assert.That(result.EventBody.Changes).IsNotNull();
        await Assert.That(result.EventBody.Changes!.Old).IsNotNull();
        await Assert.That(result.EventBody.Changes.Old!.Status).IsEqualTo("pending");
        await Assert.That(result.EventBody.Changes.New).IsNotNull();
        await Assert.That(result.EventBody.Changes.New!.Status).IsEqualTo("submitted");
    }

    [Test]
    public async Task ACHSettled_Deserializes_SettledField()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-03-16 09:00:00",
            "event_type": "ach.settled",
            "event_id": "evt_ach_stl_002",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_abc123"
                },
                "object": {
                    "type": "transaction",
                    "key": "tnx_ach_002",
                    "refnum": "200002",
                    "orderid": "ORD-5002",
                    "check": {
                        "trackingcode": "TRK789012"
                    },
                    "uri": "transactions/tnx_ach_002"
                },
                "changes": {
                    "old": {
                        "status": "submitted",
                        "processed": "2025-03-15 14:30:00"
                    },
                    "new": {
                        "status": "settled",
                        "processed": "2025-03-16 09:00:00",
                        "settled": "2025-03-16"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<ACHStatusEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.AchSettled);
        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Changes).IsNotNull();
        await Assert.That(result.EventBody.Changes!.Old!.Status).IsEqualTo("submitted");
        await Assert.That(result.EventBody.Changes.New).IsNotNull();
        await Assert.That(result.EventBody.Changes.New!.Status).IsEqualTo("settled");
        await Assert.That(result.EventBody.Changes.New.Settled).IsEqualTo("2025-03-16");
    }

    [Test]
    public async Task ACHReturned_Deserializes_ReturnedAndReasonFields()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-03-18 11:45:00",
            "event_type": "ach.returned",
            "event_id": "evt_ach_ret_003",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_abc123"
                },
                "object": {
                    "type": "transaction",
                    "key": "tnx_ach_003",
                    "refnum": "200003",
                    "orderid": "ORD-5003",
                    "check": {
                        "trackingcode": "TRK345678"
                    },
                    "uri": "transactions/tnx_ach_003"
                },
                "changes": {
                    "old": {
                        "status": "submitted",
                        "processed": "2025-03-15 14:30:00"
                    },
                    "new": {
                        "status": "returned",
                        "processed": "2025-03-18 11:45:00",
                        "returned": "2025-03-18",
                        "reason": "R01 - Insufficient Funds"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<ACHStatusEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.AchReturned);
        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Changes).IsNotNull();
        await Assert.That(result.EventBody.Changes!.New).IsNotNull();
        await Assert.That(result.EventBody.Changes.New!.Status).IsEqualTo("returned");
        await Assert.That(result.EventBody.Changes.New.Returned).IsEqualTo("2025-03-18");
        await Assert.That(result.EventBody.Changes.New.Reason).IsEqualTo("R01 - Insufficient Funds");
    }

    [Test]
    public async Task ACHNoteAdded_Deserializes_BanknoteField()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-03-19 08:20:00",
            "event_type": "ach.note_added",
            "event_id": "evt_ach_note_004",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_abc123"
                },
                "object": {
                    "type": "transaction",
                    "key": "tnx_ach_004",
                    "refnum": "200004",
                    "orderid": "ORD-5004",
                    "check": {
                        "trackingcode": "TRK901234"
                    },
                    "uri": "transactions/tnx_ach_004"
                },
                "changes": {
                    "old": {
                        "status": "submitted",
                        "processed": "2025-03-15 14:30:00"
                    },
                    "new": {
                        "status": "submitted",
                        "processed": "2025-03-19 08:20:00",
                        "banknote": "Customer contacted regarding payment"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<ACHStatusEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.AchNoteAdded);
        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Changes).IsNotNull();
        await Assert.That(result.EventBody.Changes!.New).IsNotNull();
        await Assert.That(result.EventBody.Changes.New!.Banknote).IsEqualTo("Customer contacted regarding payment");
    }

    #endregion

    #region Card Update Events

    [Test]
    public async Task CardUpdateCreated_Deserializes_CardObjectFields()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-04-01 10:00:00",
            "event_type": "cardupdate.created",
            "event_id": "evt_cau_crt_001",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_xyz789"
                },
                "object": {
                    "key": "cau_key_001",
                    "type": "cardupdate",
                    "original_card": {
                        "number": "4111xxxxxxxx1111",
                        "expiration": "1225",
                        "type": "Visa"
                    },
                    "added": "2025-04-01 10:00:00",
                    "status": "queued",
                    "status_description": "Queued for processing",
                    "source": {
                        "object": "customer",
                        "type": "payment_method",
                        "key": "pm_src_001"
                    }
                },
                "changes": {
                    "old": {
                        "status": "new"
                    },
                    "new": {
                        "status": "queued"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<CardUpdateEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.CauCreated);
        await Assert.That(result.EventId).IsEqualTo("evt_cau_crt_001");

        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Merchant).IsNotNull();
        await Assert.That(result.EventBody.Merchant!.MerchKey).IsEqualTo("merch_xyz789");

        var obj = result.EventBody.Object;
        await Assert.That(obj).IsNotNull();
        await Assert.That(obj!.Key).IsEqualTo("cau_key_001");
        await Assert.That(obj.Type).IsEqualTo("cardupdate");
        await Assert.That(obj.Status).IsEqualTo("queued");
        await Assert.That(obj.StatusDescription).IsEqualTo("Queued for processing");
        await Assert.That(obj.Added).IsNotNull();

        await Assert.That(obj.OriginalCard).IsNotNull();
        await Assert.That(obj.OriginalCard!.Number).IsEqualTo("4111xxxxxxxx1111");
        await Assert.That(obj.OriginalCard.Expiration).IsEqualTo("1225");
        await Assert.That(obj.OriginalCard.Type).IsEqualTo("Visa");

        await Assert.That(obj.Source).IsNotNull();
        await Assert.That(obj.Source!.Object).IsEqualTo("customer");
        await Assert.That(obj.Source.Type).IsEqualTo("payment_method");
        await Assert.That(obj.Source.Key).IsEqualTo("pm_src_001");

        await Assert.That(result.EventBody.Changes).IsNotNull();
        await Assert.That(result.EventBody.Changes!.Old!.Status).IsEqualTo("new");
        await Assert.That(result.EventBody.Changes.New!.Status).IsEqualTo("queued");
    }

    [Test]
    public async Task CardUpdateUpdatedExpiration_Deserializes_OriginalAndUpdatedCard()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-04-05 15:30:00",
            "event_type": "cardupdate.updated_expiration",
            "event_id": "evt_cau_exp_002",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_xyz789"
                },
                "object": {
                    "key": "cau_key_002",
                    "type": "cardupdate",
                    "original_card": {
                        "number": "4242xxxxxxxx4242",
                        "expiration": "0325",
                        "type": "Visa"
                    },
                    "added": "2025-04-01 10:00:00",
                    "status": "updated_expiration",
                    "status_description": "Expiration date updated",
                    "source": {
                        "object": "customer",
                        "type": "payment_method",
                        "key": "pm_src_002"
                    }
                },
                "changes": {
                    "old": {
                        "status": "submitted",
                        "original_card": {
                            "number": "4242xxxxxxxx4242",
                            "expiration": "0325",
                            "type": "Visa"
                        }
                    },
                    "new": {
                        "status": "updated_expiration",
                        "updated_card": {
                            "number": "4242xxxxxxxx4242",
                            "expiration": "0328",
                            "type": "Visa"
                        }
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<CardUpdateEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.CauUpdatedExpiration);

        var changes = result.EventBody!.Changes;
        await Assert.That(changes).IsNotNull();

        await Assert.That(changes!.Old).IsNotNull();
        await Assert.That(changes.Old!.Status).IsEqualTo("submitted");
        await Assert.That(changes.Old.OriginalCard).IsNotNull();
        await Assert.That(changes.Old.OriginalCard!.Number).IsEqualTo("4242xxxxxxxx4242");
        await Assert.That(changes.Old.OriginalCard.Expiration).IsEqualTo("0325");
        await Assert.That(changes.Old.OriginalCard.Type).IsEqualTo("Visa");

        await Assert.That(changes.New).IsNotNull();
        await Assert.That(changes.New!.Status).IsEqualTo("updated_expiration");
        await Assert.That(changes.New.UpdatedCard).IsNotNull();
        await Assert.That(changes.New.UpdatedCard!.Number).IsEqualTo("4242xxxxxxxx4242");
        await Assert.That(changes.New.UpdatedCard.Expiration).IsEqualTo("0328");
        await Assert.That(changes.New.UpdatedCard.Type).IsEqualTo("Visa");
    }

    [Test]
    public async Task CardUpdateAccountClosed_Deserializes_CardaccountClosedField()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-04-10 12:00:00",
            "event_type": "cardupdate.account_closed",
            "event_id": "evt_cau_cls_003",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_xyz789"
                },
                "object": {
                    "key": "cau_key_003",
                    "type": "cardupdate",
                    "original_card": {
                        "number": "5500xxxxxxxx0004",
                        "expiration": "0626",
                        "type": "Mastercard"
                    },
                    "added": "2025-04-01 10:00:00",
                    "status": "account_closed",
                    "status_description": "Account has been closed",
                    "source": {
                        "object": "customer",
                        "type": "payment_method",
                        "key": "pm_src_003"
                    }
                },
                "changes": {
                    "old": {
                        "status": "submitted",
                        "cardaccount_closed": "N"
                    },
                    "new": {
                        "status": "account_closed",
                        "cardaccount_closed": "Y"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<CardUpdateEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.CauAccountClosed);

        var changes = result.EventBody!.Changes;
        await Assert.That(changes).IsNotNull();
        await Assert.That(changes!.Old!.CardaccountClosed).IsEqualTo("N");
        await Assert.That(changes.New!.CardaccountClosed).IsEqualTo("Y");
        await Assert.That(changes.New.Status).IsEqualTo("account_closed");
    }

    #endregion

    #region Transaction Events

    [Test]
    public async Task TransactionSaleSuccess_Deserializes_TransactionObject()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-05-01 16:45:00",
            "event_type": "transaction.sale.success",
            "event_id": "evt_txn_sale_001",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_def456"
                },
                "object": {
                    "type": "transaction",
                    "key": "tnx_sale_001",
                    "refnum": "300001",
                    "result_code": "A",
                    "result": "Approved",
                    "authcode": "AUTH99",
                    "amount": "125.50",
                    "invoice": "INV-8001",
                    "creditcard": {
                        "number": "4111xxxxxxxx1111",
                        "expiration": "1226",
                        "type": "Visa"
                    },
                    "billing_address": {
                        "first_name": "Jane",
                        "last_name": "Smith",
                        "street": "456 Oak Ave"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<TransactionEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.TransactionSaleSuccess);
        await Assert.That(result.EventId).IsEqualTo("evt_txn_sale_001");
        await Assert.That(result.EventTriggered).IsNotNull();

        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Merchant).IsNotNull();
        await Assert.That(result.EventBody.Merchant!.MerchKey).IsEqualTo("merch_def456");

        var txn = result.EventBody.Transaction;
        await Assert.That(txn).IsNotNull();
        await Assert.That(txn!.Type).IsEqualTo("transaction");
        await Assert.That(txn.Key).IsEqualTo("tnx_sale_001");
        await Assert.That(txn.ReferenceNumber).IsEqualTo("300001");
        await Assert.That(txn.ResultCode).IsEqualTo("A");
        await Assert.That(txn.Result).IsEqualTo("Approved");
        await Assert.That(txn.AuthCode).IsEqualTo("AUTH99");
        await Assert.That(txn.Amount).IsEqualTo(125.50m);
        await Assert.That(txn.Invoice).IsEqualTo("INV-8001");

        await Assert.That(txn.CreditCard).IsNotNull();
        await Assert.That(txn.CreditCard!.Number).IsEqualTo("4111xxxxxxxx1111");

        await Assert.That(txn.BillingAddress).IsNotNull();
        await Assert.That(txn.BillingAddress!.FirstName).IsEqualTo("Jane");
        await Assert.That(txn.BillingAddress.LastName).IsEqualTo("Smith");
    }

    [Test]
    public async Task TransactionRefundSuccess_Deserializes_TransactionObject()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-05-02 09:15:00",
            "event_type": "transaction.refund.success",
            "event_id": "evt_txn_ref_002",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_def456"
                },
                "object": {
                    "type": "transaction",
                    "key": "tnx_refund_001",
                    "refnum": "300002",
                    "result_code": "A",
                    "result": "Approved",
                    "authcode": "REFAUTH1",
                    "amount": "45.00",
                    "invoice": "INV-8002",
                    "creditcard": {
                        "number": "4111xxxxxxxx1111",
                        "expiration": "1226",
                        "type": "Visa"
                    }
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<TransactionEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.TransactionRefundSuccess);
        await Assert.That(result.EventId).IsEqualTo("evt_txn_ref_002");

        var txn = result.EventBody!.Transaction;
        await Assert.That(txn).IsNotNull();
        await Assert.That(txn!.Type).IsEqualTo("transaction");
        await Assert.That(txn.Key).IsEqualTo("tnx_refund_001");
        await Assert.That(txn.ReferenceNumber).IsEqualTo("300002");
        await Assert.That(txn.ResultCode).IsEqualTo("A");
        await Assert.That(txn.Result).IsEqualTo("Approved");
        await Assert.That(txn.AuthCode).IsEqualTo("REFAUTH1");
        await Assert.That(txn.Amount).IsEqualTo(45.00m);
        await Assert.That(txn.Invoice).IsEqualTo("INV-8002");
    }

    #endregion

    #region Settlement Events

    [Test]
    public async Task SettlementBatchSuccess_Deserializes_BatchFields()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-05-03 22:00:00",
            "event_type": "settlement.batch.success",
            "event_id": "evt_stl_suc_001",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_ghi012"
                },
                "object": {
                    "type": "batch",
                    "key": "batch_key_001",
                    "batchnum": "247",
                    "batchrefnum": "90001",
                    "response": "Approved",
                    "totalsales": "5432.10",
                    "numsales": 38,
                    "totalcredits": "120.00",
                    "uri": "batches/batch_key_001"
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<SettlementEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.SettlementBatchSuccess);
        await Assert.That(result.EventId).IsEqualTo("evt_stl_suc_001");

        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Merchant).IsNotNull();
        await Assert.That(result.EventBody.Merchant!.MerchKey).IsEqualTo("merch_ghi012");

        var obj = result.EventBody.Object;
        await Assert.That(obj).IsNotNull();
        await Assert.That(obj!.Type).IsEqualTo("batch");
        await Assert.That(obj.Key).IsEqualTo("batch_key_001");
        await Assert.That(obj.Batchnum).IsEqualTo(247L);
        await Assert.That(obj.Batchrefnum).IsEqualTo(90001L);
        await Assert.That(obj.Response).IsEqualTo("Approved");
        await Assert.That(obj.Totalsales).IsEqualTo(5432.10m);
        await Assert.That(obj.Numsales).IsEqualTo(38L);
        await Assert.That(obj.Totalcredits).IsEqualTo("120.00");
        await Assert.That(obj.Uri).IsEqualTo("batches/batch_key_001");
    }

    [Test]
    public async Task SettlementBatchFailure_Deserializes_ReasonField()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-05-04 22:05:00",
            "event_type": "settlement.batch.failure",
            "event_id": "evt_stl_fail_002",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_ghi012"
                },
                "object": {
                    "type": "batch",
                    "key": "batch_key_002",
                    "batchnum": "248",
                    "batchrefnum": "90002",
                    "response": "Error",
                    "totalsales": "0",
                    "numsales": 0,
                    "totalcredits": "0.00",
                    "reason": "Processor communication timeout",
                    "uri": "batches/batch_key_002"
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<SettlementEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.SettlementBatchFailure);
        await Assert.That(result.EventId).IsEqualTo("evt_stl_fail_002");

        var obj = result.EventBody!.Object;
        await Assert.That(obj).IsNotNull();
        await Assert.That(obj!.Response).IsEqualTo("Error");
        await Assert.That(obj.Reason).IsEqualTo("Processor communication timeout");
        await Assert.That(obj.Totalsales).IsEqualTo(0m);
        await Assert.That(obj.Numsales).IsEqualTo(0L);
    }

    #endregion

    #region Product Inventory Events

    [Test]
    public async Task ProductInventoryAdjusted_Deserializes_InventoryFields()
    {
        var json = """
        {
            "type": "event",
            "event_triggered": "2025-06-01 13:00:00",
            "event_type": "product.inventory.adjusted",
            "event_id": "evt_inv_adj_001",
            "event_body": {
                "merchant": {
                    "merch_key": "merch_jkl345"
                },
                "object": {
                    "type": "inventory",
                    "key": "inv_key_001",
                    "locationid": "LOC-100",
                    "qtyonhand": 42,
                    "qtyonhand_change": "-3",
                    "uri": "products/inv_key_001/inventory"
                }
            }
        }
        """;

        var result = JsonSerializer.Deserialize<ProductInventoryEventResponse>(json, Options);

        await Assert.That(result).IsNotNull();
        await Assert.That(result!.EventType).IsEqualTo(EventType.ProductInventoryAdjusted);
        await Assert.That(result.EventId).IsEqualTo("evt_inv_adj_001");
        await Assert.That(result.EventTriggered).IsNotNull();

        await Assert.That(result.EventBody).IsNotNull();
        await Assert.That(result.EventBody!.Merchant).IsNotNull();
        await Assert.That(result.EventBody.Merchant!.MerchKey).IsEqualTo("merch_jkl345");

        var obj = result.EventBody.Object;
        await Assert.That(obj).IsNotNull();
        await Assert.That(obj!.Type).IsEqualTo("inventory");
        await Assert.That(obj.Key).IsEqualTo("inv_key_001");
        await Assert.That(obj.LocationId).IsEqualTo("LOC-100");
        await Assert.That(obj.QtyOnHand).IsEqualTo(42L);
        await Assert.That(obj.QtyOnHandChange).IsEqualTo("-3");
        await Assert.That(obj.Uri).IsEqualTo("products/inv_key_001/inventory");
    }

    #endregion
}
