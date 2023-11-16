using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using UsaEPay.NET.Converter;

namespace UsaEPay.NET.Models.Enumerations.Event
{
    
    /// <summary>
    /// Enum representing various events.
    /// </summary>
    [JsonConverter(typeof(EventTypeConverter))]
    public enum EventType
    {
        /// <summary>
        /// ACH status updated to submitted.
        /// </summary>
        AchSubmitted,

        /// <summary>
        /// ACH status updated to settled.
        /// </summary>
        AchSettled,

        /// <summary>
        /// ACH status updated to returned.
        /// </summary>
        AchReturned,

        /// <summary>
        /// ACH status updated to voided.
        /// </summary> 
        AchVoided,

        /// <summary>
        /// ACH status updated to failed.
        /// </summary>
        AchFailed,

        /// <summary>
        /// Note added to ACH transaction.
        /// </summary>
        AchNoteAdded,

        // Card Update (CAU) Events

        /// <summary>
        /// Card has been queued for update.
        /// </summary> 
        CauCreated,

        /// <summary>
        /// Card has been submitted to the processor for update.
        /// </summary>
        CauSubmitted,

        /// <summary>
        /// An updated expiration date has been received.
        /// </summary>
        CauUpdatedExpiration,

        /// <summary>
        /// An updated card number has been received.
        /// </summary>
        CauUpdatedCard,

        /// <summary>
        /// Received a message from the issuer to contact the customer for a new card number.
        /// </summary>
        CauContactCustomer,

        /// <summary>
        /// Received notification of account closure.
        /// </summary>
        CauAccountClosed,

        // Product Inventory Events

        /// <summary>
        /// The inventory has been adjusted after a sale.
        /// </summary>
        ProductInventoryOrdered,

        /// <summary>
        /// The inventory has been adjusted in the Products Database.
        /// </summary>
        ProductInventoryAdjusted,

        // Transaction Events

        /// <summary>
        /// A transaction sale approved.
        /// </summary>
        TransactionSaleSuccess,

        /// <summary>
        /// A transaction sale failed - includes declines and errors.
        /// </summary>
        TransactionSaleFailure,

        /// <summary>
        /// A transaction sale was voided.
        /// </summary>
        TransactionSaleVoided,

        /// <summary>
        /// A transaction sale was captured.
        /// </summary>
        TransactionSaleCaptured,

        /// <summary>
        /// A transaction sale was adjusted.
        /// </summary>
        TransactionSaleAdjusted,

        /// <summary>
        /// A transaction sale was queued.
        /// </summary>
        TransactionSaleQueued,

        /// <summary>
        /// A voided transaction sale was unvoided.
        /// </summary>
        TransactionSaleUnvoided,

        /// <summary>
        /// A transaction refund approved.
        /// </summary>
        TransactionRefundSuccess,

        /// <summary>
        /// A transaction refund was voided.
        /// </summary>
        TransactionRefundVoided,

        // Settlement Batch Events

        /// <summary>
        /// A batch settles successfully.
        /// </summary>
        SettlementBatchSuccess,

        /// <summary>
        /// A batch receives an error when attempting to settle.
        /// </summary>
        SettlementBatchFailure,
    }
}