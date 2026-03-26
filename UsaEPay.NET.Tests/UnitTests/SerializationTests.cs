using System.Text.Json;
using TUnit.Core;
using UsaEPay.NET.Models;
using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class SerializationTests
{
    #region Request Serialization

    [Test]
    public async Task Request_Serializes_CommandField()
    {
        var request = new UsaEPayRequest
        {
            Command = "cc:sale",
            Amount = 10.00m,
            CreditCard = new CreditCard
            {
                Number = "4111111111111111",
                Cvc = "123",
                Expiration = "1225"
            },
            BillingAddress = new Address
            {
                FirstName = "John",
                LastName = "Doe"
            }
        };

        var json = JsonSerializer.Serialize(request, USAePaySerializerContext.Default.Options);

        await Assert.That(json).Contains("\"command\":\"cc:sale\"");
        await Assert.That(json).Contains("\"number\":\"4111111111111111\"");
        await Assert.That(json).Contains("\"first_name\":\"John\"");
    }

    [Test]
    public async Task Request_Serializes_CreditCardObject()
    {
        var request = new UsaEPayRequest
        {
            Command = "cc:sale",
            CreditCard = new CreditCard
            {
                Number = "4242424242424242",
                Cvc = "456",
                Expiration = "0326",
                CardHolder = "Jane Smith"
            }
        };

        var json = JsonSerializer.Serialize(request, USAePaySerializerContext.Default.Options);

        await Assert.That(json).Contains("\"creditcard\"");
        await Assert.That(json).Contains("\"number\":\"4242424242424242\"");
        await Assert.That(json).Contains("\"cvc\":\"456\"");
        await Assert.That(json).Contains("\"expiration\":\"0326\"");
        await Assert.That(json).Contains("\"cardholder\":\"Jane Smith\"");
    }

    [Test]
    public async Task Request_NullFields_OmittedFromJson()
    {
        var request = new UsaEPayRequest
        {
            Command = "cc:sale",
            Amount = 5.00m
        };

        var json = JsonSerializer.Serialize(request, USAePaySerializerContext.Default.Options);

        await Assert.That(json).DoesNotContain("\"creditcard\"");
        await Assert.That(json).DoesNotContain("\"billing_address\"");
        await Assert.That(json).DoesNotContain("\"shipping_address\"");
        await Assert.That(json).DoesNotContain("\"custom_fields\"");
        await Assert.That(json).DoesNotContain("\"trankey\"");
        await Assert.That(json).DoesNotContain("\"refnum\"");
        await Assert.That(json).DoesNotContain("\"invoice\"");
    }

    [Test]
    public async Task Request_DefaultBoolFields_OmittedFromJson()
    {
        var request = new UsaEPayRequest
        {
            Command = "cc:sale"
        };

        var json = JsonSerializer.Serialize(request, USAePaySerializerContext.Default.Options);

        await Assert.That(json).DoesNotContain("\"save_card\"");
        await Assert.That(json).DoesNotContain("\"save_customer\"");
        await Assert.That(json).DoesNotContain("\"send_receipt\"");
        await Assert.That(json).DoesNotContain("\"ignore_duplicate\"");
    }

    #endregion

    #region Response Deserialization

    [Test]
    public async Task Response_Deserializes_FromSampleJson()
    {
        var json = """
        {
            "type": "transaction",
            "key": "tnx_abc123",
            "refnum": "100099",
            "result_code": "A",
            "result": "Approved",
            "authcode": "345678",
            "status_code": "P",
            "status": "Pending",
            "creditcard": {
                "number": "4111xxxxxxxx1111",
                "cvc": "123",
                "expiration": "1225"
            },
            "invoice": "INV-001",
            "amount": "50.00",
            "billing_address": {
                "first_name": "John",
                "last_name": "Doe",
                "street": "123 Main St"
            }
        }
        """;

        var response = JsonSerializer.Deserialize<UsaEPayResponse>(json, USAePaySerializerContext.Default.Options);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Type).IsEqualTo("transaction");
        await Assert.That(response.Key).IsEqualTo("tnx_abc123");
        await Assert.That(response.ReferenceNumber).IsEqualTo("100099");
        await Assert.That(response.ResultCode).IsEqualTo("A");
        await Assert.That(response.Result).IsEqualTo("Approved");
        await Assert.That(response.AuthCode).IsEqualTo("345678");
        await Assert.That(response.Amount).IsEqualTo(50.00m);
        await Assert.That(response.Invoice).IsEqualTo("INV-001");
        await Assert.That(response.CreditCard).IsNotNull();
        await Assert.That(response.CreditCard!.Number).IsEqualTo("4111xxxxxxxx1111");
        await Assert.That(response.BillingAddress).IsNotNull();
        await Assert.That(response.BillingAddress!.FirstName).IsEqualTo("John");
    }

    [Test]
    public async Task Response_Deserializes_WithMissingOptionalFields()
    {
        var json = """
        {
            "type": "transaction",
            "key": "tnx_minimal",
            "result_code": "A",
            "result": "Approved",
            "amount": "10.00"
        }
        """;

        var response = JsonSerializer.Deserialize<UsaEPayResponse>(json, USAePaySerializerContext.Default.Options);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Key).IsEqualTo("tnx_minimal");
        await Assert.That(response.CreditCard).IsNull();
        await Assert.That(response.BillingAddress).IsNull();
        await Assert.That(response.Check).IsNull();
        await Assert.That(response.Batch).IsNull();
    }

    #endregion

    #region BatchListResponse Deserialization

    [Test]
    public async Task BatchListResponse_Deserializes_Correctly()
    {
        var json = """
        {
            "type": "list",
            "limit": "20",
            "offset": "0",
            "total": "2",
            "data": [
                {
                    "type": "batch",
                    "key": "batch_001",
                    "batchrefnum": "1001",
                    "status": "closed",
                    "opened": "2024-01-01 08:00:00",
                    "closed": "2024-01-01 20:00:00",
                    "total_amount": 5000.00,
                    "total_count": "50",
                    "sales_amount": 4500.00,
                    "sales_count": "45",
                    "voids_amount": 200.00,
                    "voids_count": "2",
                    "refunds_amount": 300.00,
                    "refunds_count": "3"
                },
                {
                    "type": "batch",
                    "key": "batch_002",
                    "batchrefnum": "1002",
                    "status": "open"
                }
            ]
        }
        """;

        var response = JsonSerializer.Deserialize<UsaEPayBatchListResponse>(json, USAePaySerializerContext.Default.Options);

        await Assert.That(response).IsNotNull();
        await Assert.That(response!.Type).IsEqualTo("list");
        await Assert.That(response.Limit).IsEqualTo(20L);
        await Assert.That(response.Offset).IsEqualTo(0L);
        await Assert.That(response.Total).IsEqualTo(2L);
        await Assert.That(response.Data).HasCount().EqualTo(2);
        await Assert.That(response.Data[0].Key).IsEqualTo("batch_001");
        await Assert.That(response.Data[0].BatchReferenceNumber).IsEqualTo(1001L);
        await Assert.That(response.Data[0].Status).IsEqualTo("closed");
        await Assert.That(response.Data[0].TotalAmount).IsEqualTo(5000.00m);
        await Assert.That(response.Data[0].TotalCount).IsEqualTo(50L);
        await Assert.That(response.Data[0].SalesCount).IsEqualTo(45L);
        await Assert.That(response.Data[1].Key).IsEqualTo("batch_002");
        await Assert.That(response.Data[1].Status).IsEqualTo("open");
    }

    #endregion

    #region Amount as String Deserialization

    [Test]
    public async Task Response_AmountAsString_DeserializesCorrectly()
    {
        var json = """
        {
            "type": "transaction",
            "key": "tnx_amount_string",
            "result_code": "A",
            "result": "Approved",
            "amount": "123.45"
        }
        """;

        var response = JsonSerializer.Deserialize<UsaEPayResponse>(json, USAePaySerializerContext.Default.Options);

        await Assert.That(response!.Amount).IsEqualTo(123.45m);
    }

    [Test]
    public async Task Response_AmountAsNumber_DeserializesCorrectly()
    {
        var json = """
        {
            "type": "transaction",
            "key": "tnx_amount_num",
            "result_code": "A",
            "result": "Approved",
            "amount": 67.89
        }
        """;

        var response = JsonSerializer.Deserialize<UsaEPayResponse>(json, USAePaySerializerContext.Default.Options);

        await Assert.That(response!.Amount).IsEqualTo(67.89m);
    }

    #endregion
}
