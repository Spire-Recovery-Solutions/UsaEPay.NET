using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace UsaEPay.NET.Models.Enumerations.Event
{
    
    /// <summary>
    /// Enum representing various events.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EventType
    {
        /// <summary>
        /// ACH status updated to submitted.
        /// </summary>
        [JsonPropertyName("ach.submitted")] 
        AchSubmitted,

        /// <summary>
        /// ACH status updated to settled.
        /// </summary>
        [JsonPropertyName("ach.settled")] 
        AchSettled,

        /// <summary>
        /// ACH status updated to returned.
        /// </summary>
        [JsonPropertyName("ach.returned")] 
        AchReturned,

        /// <summary>
        /// ACH status updated to voided.
        /// </summary>
        [JsonPropertyName("ach.voided")] 
        AchVoided,

        /// <summary>
        /// ACH status updated to failed.
        /// </summary>
        [JsonPropertyName("ach.failed")] 
        AchFailed,

        /// <summary>
        /// Note added to ACH transaction.
        /// </summary>
        [JsonPropertyName("ach.note_added")]
        AchNoteAdded,

        // Card Update (CAU) Events

        /// <summary>
        /// Card has been queued for update.
        /// </summary>
        [JsonPropertyName("cau.created")] 
        CauCreated,

        /// <summary>
        /// Card has been submitted to the processor for update.
        /// </summary>
        [JsonPropertyName("cau.submitted")] 
        CauSubmitted,

        /// <summary>
        /// An updated expiration date has been received.
        /// </summary>
        [JsonPropertyName("cau.updated_expiration")]
        CauUpdatedExpiration,

        /// <summary>
        /// An updated card number has been received.
        /// </summary>
        [JsonPropertyName("cau.updated_card")] 
        CauUpdatedCard,

        /// <summary>
        /// Received a message from the issuer to contact the customer for a new card number.
        /// </summary>
        [JsonPropertyName("cau.contact_customer")]
        CauContactCustomer,

        /// <summary>
        /// Received notification of account closure.
        /// </summary>
        [JsonPropertyName("cau.account_closed")]
        CauAccountClosed,

        // Product Inventory Events

        /// <summary>
        /// The inventory has been adjusted after a sale.
        /// </summary>
        [JsonPropertyName("product.inventory.ordered")]
        ProductInventoryOrdered,

        /// <summary>
        /// The inventory has been adjusted in the Products Database.
        /// </summary>
        [JsonPropertyName("product.inventory.adjusted")]
        ProductInventoryAdjusted,

        // Transaction Events

        /// <summary>
        /// A transaction sale approved.
        /// </summary>
        [JsonPropertyName("transaction.sale.success")]
        TransactionSaleSuccess,

        /// <summary>
        /// A transaction sale failed - includes declines and errors.
        /// </summary>
        [JsonPropertyName("transaction.sale.failure")]
        TransactionSaleFailure,

        /// <summary>
        /// A transaction sale was voided.
        /// </summary>
        [JsonPropertyName("transaction.sale.voided")]
        TransactionSaleVoided,

        /// <summary>
        /// A transaction sale was captured.
        /// </summary>
        [JsonPropertyName("transaction.sale.captured")]
        TransactionSaleCaptured,

        /// <summary>
        /// A transaction sale was adjusted.
        /// </summary>
        [JsonPropertyName("transaction.sale.adjusted")]
        TransactionSaleAdjusted,

        /// <summary>
        /// A transaction sale was queued.
        /// </summary>
        [JsonPropertyName("transaction.sale.queued")]
        TransactionSaleQueued,

        /// <summary>
        /// A voided transaction sale was unvoided.
        /// </summary>
        [JsonPropertyName("transaction.sale.unvoided")]
        TransactionSaleUnvoided,

        /// <summary>
        /// A transaction refund approved.
        /// </summary>
        [JsonPropertyName("transaction.refund.success")]
        TransactionRefundSuccess,

        /// <summary>
        /// A transaction refund was voided.
        /// </summary>
        [JsonPropertyName("transaction.refund.voided")]
        TransactionRefundVoided,

        // Settlement Batch Events

        /// <summary>
        /// A batch settles successfully.
        /// </summary>
        [JsonPropertyName("settlement.batch.success")]
        SettlementBatchSuccess,

        /// <summary>
        /// A batch receives an error when attempting to settle.
        /// </summary>
        [JsonPropertyName("settlement.batch.failure")]
        SettlementBatchFailure,
    }
}
