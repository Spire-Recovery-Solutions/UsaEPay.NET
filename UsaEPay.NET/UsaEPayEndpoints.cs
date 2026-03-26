namespace UsaEPay.NET
{
    public static class UsaEPayEndpoints
    {
        /// <summary>
        /// Endpoint for transactions.
        /// </summary>
        public const string Transactions = "transactions";

        /// <summary>
        /// Endpoint for closing the current batch.
        /// </summary>
        public const string CloseCurrentBatch = "batches/current/close";

        /// <summary>
        /// Endpoint for batches.
        /// </summary>
        public const string Batches = "batches";

        /// <summary>
        /// Endpoint for the current batch.
        /// </summary>
        public const string CurrentBatch = "batches/current";

        /// <summary>
        /// Endpoint for current batch transactions.
        /// </summary>
        public const string CurrentBatchTransactions = "batches/current/transactions";

        /// <summary>
        /// Endpoint for tokens.
        /// </summary>
        public const string Tokens = "tokens";

        /// <summary>
        /// Endpoint for sending a transaction receipt (used as transactions/{trankey}/send).
        /// </summary>
        public const string TransactionSend = "transactions";

        /// <summary>
        /// Endpoint for transaction receipts (used as transactions/{trankey}/receipts/{receiptid}).
        /// </summary>
        public const string TransactionReceipts = "transactions";

        /// <summary>
        /// Endpoint for customers.
        /// </summary>
        public const string Customers = "customers";

        /// <summary>
        /// Endpoint for bulk transactions.
        /// </summary>
        public const string BulkTransactions = "bulk_transactions";

        /// <summary>
        /// Endpoint for the current bulk transaction.
        /// </summary>
        public const string BulkTransactionsCurrent = "bulk_transactions/current";

        /// <summary>
        /// Endpoint for customer recurring billing schedules.
        /// </summary>
        public const string BillingSchedules = "billing_schedules";

        /// <summary>
        /// Endpoint for customer payment methods (used as customers/{custkey}/payment_methods).
        /// </summary>
        public const string PaymentMethods = "payment_methods";

        /// <summary>
        /// Endpoint for inventory.
        /// </summary>
        public const string Inventory = "inventory";

        /// <summary>
        /// Endpoint for inventory locations.
        /// </summary>
        public const string InventoryLocations = "inventory/location";

        /// <summary>
        /// Endpoint for payment engine devices.
        /// </summary>
        public const string PaymentEngineDevices = "paymentengine/devices";

        /// <summary>
        /// Endpoint for payment engine pay requests.
        /// </summary>
        public const string PaymentEnginePayRequests = "paymentengine/payrequests";

        /// <summary>
        /// Endpoint for products.
        /// </summary>
        public const string Products = "products";

        /// <summary>
        /// Endpoint for product categories.
        /// </summary>
        public const string ProductCategories = "products/categories";
    }
}
