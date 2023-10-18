namespace UsaEPay.NET
{
    public class UsaEPayCommandTypes
    {
        /// <summary>
        /// Transaction command for credit card sale.
        /// </summary>
        public const string TransactionSale = "cc:sale";

        /// <summary>
        /// Transaction command for check sale.
        /// </summary>
        public const string CheckSale = "check:sale";

        /// <summary>
        /// Transaction command for cash sale.
        /// </summary>
        public const string CashSale = "cash:sale";

        /// <summary>
        /// Transaction command for quick sale.
        /// </summary>
        public const string QuickSale = "quicksale";

        /// <summary>
        /// Transaction command for authorization only sale.
        /// </summary>
        public const string AuthOnlySale = "authonly";

        /// <summary>
        /// Transaction command for credit card refund.
        /// </summary>
        public const string CreditCardRefund = "cc:credit";

        /// <summary>
        /// Transaction command for check refund.
        /// </summary>
        public const string CheckRefund = "check:credit";

        /// <summary>
        /// Transaction command for cash refund.
        /// </summary>
        public const string CashRefund = "cash:refund";

        /// <summary>
        /// Transaction command for quick refund.
        /// </summary>
        public const string QuickRefund = "quickrefund";

        /// <summary>
        /// Transaction command for capturing a payment.
        /// </summary>
        public const string CapturePayment = "cc:capture";

        /// <summary>
        /// Transaction command for capturing and reauthorizing a payment.
        /// </summary>
        public const string CapturePaymentReauth = "cc:capture:reauth";

        /// <summary>
        /// Transaction command for capturing a payment with an override.
        /// </summary>
        public const string CapturePaymentOverride = "cc:capture:override";

        /// <summary>
        /// Transaction command for capturing a payment with an error.
        /// </summary>
        public const string CapturePaymentError = "cc:capture:error";

        /// <summary>
        /// Transaction command for adjusting a payment.
        /// </summary>
        public const string AdjustPayment = "cc:adjust";

        /// <summary>
        /// Transaction command for adjusting a refunded credit card payment.
        /// </summary>
        public const string AdjustPaymentRefund = "cc:refund:adjust";

        /// <summary>
        /// Transaction command for voiding a credit card payment.
        /// </summary>
        public const string CreditVoid = "creditvoid";

        /// <summary>
        /// Transaction command for posting an authorized credit card payment.
        /// </summary>
        public const string PostPayment = "cc:postauth";

        /// <summary>
        /// Transaction command for voiding a payment.
        /// </summary>
        public const string VoidPayment = "void";

        /// <summary>
        /// Transaction command for releasing funds from a voided credit card transaction.
        /// </summary>
        public const string ReleaseFunds = "cc:void:release";

        /// <summary>
        /// Transaction command for unvoiding a transaction.
        /// </summary>
        public const string Unvoid = "unvoid";

        /// <summary>
        /// Transaction command for tokenizing a credit card for later use.
        /// </summary>
        public const string TokenizeCard = "cc:save";
    }
}
