using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using UsaEPay.NET.Models.Enumerations.Event;

namespace UsaEPay.NET.Converter
{
    public class EventTypeConverter : JsonConverter<EventType>
    {
        public override EventType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string value = reader.GetString();

            return value switch
            {
                "ach.submitted" => EventType.AchSubmitted,
                "ach.settled" => EventType.AchSettled,
                "ach.returned" => EventType.AchReturned,
                "ach.voided" => EventType.AchVoided,
                "ach.failed" => EventType.AchFailed,
                "ach.note_added" => EventType.AchNoteAdded,

                "cardupdate.created" => EventType.CauCreated,
                "cardupdate.submitted" => EventType.CauSubmitted,
                "cardupdate.updated_expiration" => EventType.CauUpdatedExpiration,
                "cardupdate.updated_card" => EventType.CauUpdatedCard,
                "cardupdate.contact_customer" => EventType.CauContactCustomer,
                "cardupdate.account_closed" => EventType.CauAccountClosed,

                "product.inventory.ordered" => EventType.ProductInventoryOrdered,
                "product.inventory.adjusted" => EventType.ProductInventoryAdjusted,

                "transaction.sale.success" => EventType.TransactionSaleSuccess,
                "transaction.sale.failure" => EventType.TransactionSaleFailure,
                "transaction.sale.voided" => EventType.TransactionSaleVoided,
                "transaction.sale.captured" => EventType.TransactionSaleCaptured,
                "transaction.sale.adjusted" => EventType.TransactionSaleAdjusted,
                "transaction.sale.queued" => EventType.TransactionSaleQueued,
                "transaction.sale.unvoided" => EventType.TransactionSaleUnvoided,

                "transaction.refund.success" => EventType.TransactionRefundSuccess,
                "transaction.refund.voided" => EventType.TransactionRefundVoided,

                "settlement.batch.success" => EventType.SettlementBatchSuccess,
                "settlement.batch.failure" => EventType.SettlementBatchFailure,

                _ => throw new JsonException($"Value '{value}' cannot be converted to EventType.")
            };
        }

        public override void Write(Utf8JsonWriter writer, EventType value, JsonSerializerOptions options)
        {
            string stringValue = value switch
            {
                EventType.AchSubmitted => "ach.submitted",
                EventType.AchSettled => "ach.settled",
                EventType.AchReturned => "ach.returned",
                EventType.AchVoided => "ach.voided",
                EventType.AchFailed => "ach.failed",
                EventType.AchNoteAdded => "ach.note_added",

                EventType.CauCreated => "cardupdate.created",
                EventType.CauSubmitted => "cardupdate.submitted",
                EventType.CauUpdatedExpiration => "cardupdate.updated_expiration",
                EventType.CauUpdatedCard => "cardupdate.updated_card",
                EventType.CauContactCustomer => "cardupdate.contact_customer",
                EventType.CauAccountClosed => "cardupdate.account_closed",

                EventType.ProductInventoryOrdered => "product.inventory.ordered",
                EventType.ProductInventoryAdjusted => "product.inventory.adjusted",

                EventType.TransactionSaleSuccess => "transaction.sale.success",
                EventType.TransactionSaleFailure => "transaction.sale.failure",
                EventType.TransactionSaleVoided => "transaction.sale.voided",
                EventType.TransactionSaleCaptured => "transaction.sale.captured",
                EventType.TransactionSaleAdjusted => "transaction.sale.adjusted",
                EventType.TransactionSaleQueued => "transaction.sale.queued",
                EventType.TransactionSaleUnvoided => "transaction.sale.unvoided",

                EventType.TransactionRefundSuccess => "transaction.refund.success",
                EventType.TransactionRefundVoided => "transaction.refund.voided",

                EventType.SettlementBatchSuccess => "settlement.batch.success",
                EventType.SettlementBatchFailure => "settlement.batch.failure",

                _ => throw new InvalidOperationException($"Invalid value '{value}' for EventType.")
            };

            writer.WriteStringValue(stringValue);
        }
    }
}