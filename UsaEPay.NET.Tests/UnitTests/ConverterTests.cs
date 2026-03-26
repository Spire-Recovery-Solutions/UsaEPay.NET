using System.Text.Json;
using System.Text.Json.Serialization;
using UsaEPay.NET.Converter;
using UsaEPay.NET.Models.Enumerations.Event;

namespace UsaEPay.NET.Tests.UnitTests;

public sealed class ConverterTests
{
    #region ParseStringToDecimalConverter

    [Test]
    public async Task DecimalConverter_StringValue_ParsesCorrectly()
    {
        var json = """{"value":"10.50"}""";
        var result = JsonSerializer.Deserialize<DecimalHolder>(json);
        await Assert.That(result!.Value).IsEqualTo(10.50m);
    }

    [Test]
    public async Task DecimalConverter_NullValue_ReturnsZero()
    {
        var json = """{"value":null}""";
        var result = JsonSerializer.Deserialize<DecimalHolder>(json);
        await Assert.That(result!.Value).IsEqualTo(0m);
    }

    [Test]
    public async Task DecimalConverter_NumberToken_ReturnsDecimal()
    {
        var json = """{"value":25.75}""";
        var result = JsonSerializer.Deserialize<DecimalHolder>(json);
        await Assert.That(result!.Value).IsEqualTo(25.75m);
    }

    [Test]
    public async Task DecimalConverter_InvalidString_ThrowsJsonException()
    {
        var json = """{"value":"notanumber"}""";
        await Assert.That(() => JsonSerializer.Deserialize<DecimalHolder>(json)).ThrowsExactly<JsonException>();
    }

    private sealed class DecimalHolder
    {
        [JsonPropertyName("value")]
        [JsonConverter(typeof(ParseStringToDecimalConverter))]
        public decimal Value { get; set; }
    }

    #endregion

    #region ParseStringToLongConverter

    [Test]
    public async Task LongConverter_StringValue_ParsesCorrectly()
    {
        var json = """{"value":"100"}""";
        var result = JsonSerializer.Deserialize<LongHolder>(json);
        await Assert.That(result!.Value).IsEqualTo(100L);
    }

    [Test]
    public async Task LongConverter_NumberToken_ReturnsLong()
    {
        var json = """{"value":100}""";
        var result = JsonSerializer.Deserialize<LongHolder>(json);
        await Assert.That(result!.Value).IsEqualTo(100L);
    }

    [Test]
    public async Task LongConverter_NullValue_ReturnsZero()
    {
        var json = """{"value":null}""";
        var result = JsonSerializer.Deserialize<LongHolder>(json);
        await Assert.That(result!.Value).IsEqualTo(0L);
    }

    [Test]
    public async Task LongConverter_InvalidString_ThrowsJsonException()
    {
        var json = """{"value":"abc"}""";
        await Assert.That(() => JsonSerializer.Deserialize<LongHolder>(json)).ThrowsExactly<JsonException>();
    }

    private sealed class LongHolder
    {
        [JsonPropertyName("value")]
        [JsonConverter(typeof(ParseStringToLongConverter))]
        public long Value { get; set; }
    }

    #endregion

    #region UsaEPayStringToDateTimeOffsetConverter

    [Test]
    public async Task DateTimeOffsetConverter_ValidFormat_ParsesCorrectly()
    {
        var json = """{"value":"2024-01-15 10:30:00"}""";
        var result = JsonSerializer.Deserialize<DateTimeOffsetHolder>(json);
        var parsedLocal = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Local);
        var expected = new DateTimeOffset(parsedLocal);
        await Assert.That(result!.Value).IsEqualTo(expected);
    }

    [Test]
    public async Task DateTimeOffsetConverter_NullValue_ReturnsNull()
    {
        var json = """{"value":null}""";
        var result = JsonSerializer.Deserialize<DateTimeOffsetHolder>(json);
        await Assert.That(result!.Value).IsNull();
    }

    [Test]
    public async Task DateTimeOffsetConverter_EmptyString_ReturnsNull()
    {
        var json = """{"value":""}""";
        var result = JsonSerializer.Deserialize<DateTimeOffsetHolder>(json);
        await Assert.That(result!.Value).IsNull();
    }

    [Test]
    public async Task DateTimeOffsetConverter_InvalidFormat_ThrowsException()
    {
        var json = """{"value":"not-a-date"}""";
        await Assert.That(() => JsonSerializer.Deserialize<DateTimeOffsetHolder>(json)).ThrowsException();
    }

    [Test]
    public async Task DateTimeOffsetConverter_Write_FormatsCorrectly()
    {
        var holder = new DateTimeOffsetHolder { Value = new DateTimeOffset(2024, 6, 15, 14, 30, 0, TimeSpan.Zero) };
        var json = JsonSerializer.Serialize(holder);
        await Assert.That(json).Contains("2024-06-15 14:30:00");
    }

    [Test]
    public async Task DateTimeOffsetConverter_WriteNull_WritesNull()
    {
        var holder = new DateTimeOffsetHolder { Value = null };
        var json = JsonSerializer.Serialize(holder);
        await Assert.That(json).Contains("null");
    }

    private sealed class DateTimeOffsetHolder
    {
        [JsonPropertyName("value")]
        [JsonConverter(typeof(UsaEPayStringToDateTimeOffsetConverter))]
        public DateTimeOffset? Value { get; set; }
    }

    #endregion

    #region EventTypeConverter

    [Test]
    [Arguments("ach.submitted", EventType.AchSubmitted)]
    [Arguments("ach.settled", EventType.AchSettled)]
    [Arguments("ach.returned", EventType.AchReturned)]
    [Arguments("ach.voided", EventType.AchVoided)]
    [Arguments("ach.failed", EventType.AchFailed)]
    [Arguments("ach.note_added", EventType.AchNoteAdded)]
    [Arguments("product.inventory.ordered", EventType.ProductInventoryOrdered)]
    [Arguments("product.inventory.adjusted", EventType.ProductInventoryAdjusted)]
    [Arguments("transaction.sale.success", EventType.TransactionSaleSuccess)]
    [Arguments("transaction.sale.failure", EventType.TransactionSaleFailure)]
    [Arguments("transaction.sale.voided", EventType.TransactionSaleVoided)]
    [Arguments("transaction.sale.captured", EventType.TransactionSaleCaptured)]
    [Arguments("transaction.sale.adjusted", EventType.TransactionSaleAdjusted)]
    [Arguments("transaction.sale.queued", EventType.TransactionSaleQueued)]
    [Arguments("transaction.sale.unvoided", EventType.TransactionSaleUnvoided)]
    [Arguments("transaction.refund.success", EventType.TransactionRefundSuccess)]
    [Arguments("transaction.refund.voided", EventType.TransactionRefundVoided)]
    [Arguments("settlement.batch.success", EventType.SettlementBatchSuccess)]
    [Arguments("settlement.batch.failure", EventType.SettlementBatchFailure)]
    public async Task EventTypeConverter_Read_ParsesCorrectly(string jsonValue, EventType expected)
    {
        var json = "\"" + jsonValue + "\"";
        var result = JsonSerializer.Deserialize<EventType>(json);
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("cardupdate.created", EventType.CauCreated)]
    [Arguments("cardupdate.submitted", EventType.CauSubmitted)]
    [Arguments("cardupdate.updated_expiration", EventType.CauUpdatedExpiration)]
    [Arguments("cardupdate.updated_card", EventType.CauUpdatedCard)]
    [Arguments("cardupdate.contact_customer", EventType.CauContactCustomer)]
    [Arguments("cardupdate.account_closed", EventType.CauAccountClosed)]
    public async Task EventTypeConverter_Read_CardupdatePrefix_ParsesCorrectly(string jsonValue, EventType expected)
    {
        var json = "\"" + jsonValue + "\"";
        var result = JsonSerializer.Deserialize<EventType>(json);
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("cau.created", EventType.CauCreated)]
    [Arguments("cau.submitted", EventType.CauSubmitted)]
    [Arguments("cau.updated_expiration", EventType.CauUpdatedExpiration)]
    [Arguments("cau.updated_card", EventType.CauUpdatedCard)]
    [Arguments("cau.contact_customer", EventType.CauContactCustomer)]
    [Arguments("cau.account_closed", EventType.CauAccountClosed)]
    public async Task EventTypeConverter_Read_CauPrefix_ParsesCorrectly(string jsonValue, EventType expected)
    {
        var json = "\"" + jsonValue + "\"";
        var result = JsonSerializer.Deserialize<EventType>(json);
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task EventTypeConverter_Read_UnknownValue_ThrowsJsonException()
    {
        var json = "\"unknown.event\"";
        await Assert.That(() => JsonSerializer.Deserialize<EventType>(json)).ThrowsExactly<JsonException>();
    }

    [Test]
    [Arguments(EventType.AchSubmitted, "ach.submitted")]
    [Arguments(EventType.AchSettled, "ach.settled")]
    [Arguments(EventType.AchReturned, "ach.returned")]
    [Arguments(EventType.AchVoided, "ach.voided")]
    [Arguments(EventType.AchFailed, "ach.failed")]
    [Arguments(EventType.AchNoteAdded, "ach.note_added")]
    [Arguments(EventType.CauCreated, "cardupdate.created")]
    [Arguments(EventType.CauSubmitted, "cardupdate.submitted")]
    [Arguments(EventType.CauUpdatedExpiration, "cardupdate.updated_expiration")]
    [Arguments(EventType.CauUpdatedCard, "cardupdate.updated_card")]
    [Arguments(EventType.CauContactCustomer, "cardupdate.contact_customer")]
    [Arguments(EventType.CauAccountClosed, "cardupdate.account_closed")]
    [Arguments(EventType.ProductInventoryOrdered, "product.inventory.ordered")]
    [Arguments(EventType.ProductInventoryAdjusted, "product.inventory.adjusted")]
    [Arguments(EventType.TransactionSaleSuccess, "transaction.sale.success")]
    [Arguments(EventType.TransactionSaleFailure, "transaction.sale.failure")]
    [Arguments(EventType.TransactionSaleVoided, "transaction.sale.voided")]
    [Arguments(EventType.TransactionSaleCaptured, "transaction.sale.captured")]
    [Arguments(EventType.TransactionSaleAdjusted, "transaction.sale.adjusted")]
    [Arguments(EventType.TransactionSaleQueued, "transaction.sale.queued")]
    [Arguments(EventType.TransactionSaleUnvoided, "transaction.sale.unvoided")]
    [Arguments(EventType.TransactionRefundSuccess, "transaction.refund.success")]
    [Arguments(EventType.TransactionRefundVoided, "transaction.refund.voided")]
    [Arguments(EventType.SettlementBatchSuccess, "settlement.batch.success")]
    [Arguments(EventType.SettlementBatchFailure, "settlement.batch.failure")]
    public async Task EventTypeConverter_Write_SerializesCorrectly(EventType eventType, string expectedString)
    {
        var json = JsonSerializer.Serialize(eventType);
        await Assert.That(json).IsEqualTo($"\"{expectedString}\"");
    }

    [Test]
    [Arguments(EventType.AchSubmitted)]
    [Arguments(EventType.CauCreated)]
    [Arguments(EventType.TransactionSaleSuccess)]
    [Arguments(EventType.SettlementBatchSuccess)]
    public async Task EventTypeConverter_RoundTrip_PreservesValue(EventType original)
    {
        var json = JsonSerializer.Serialize(original);
        var deserialized = JsonSerializer.Deserialize<EventType>(json);
        await Assert.That(deserialized).IsEqualTo(original);
    }

    #endregion
}
