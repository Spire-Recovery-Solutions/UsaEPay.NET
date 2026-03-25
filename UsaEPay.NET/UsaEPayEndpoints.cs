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
    }
}
