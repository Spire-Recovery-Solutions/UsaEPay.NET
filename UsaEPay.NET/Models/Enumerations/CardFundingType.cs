namespace UsaEPay.NET.Models.Enumerations
{
    /// <summary>
    /// Funding type of a card (how the card is funded), as reported in the <c>bin.type</c> field
    /// of a transaction looked up with the <c>return_bin</c> flag.
    /// </summary>
    public enum CardFundingType
    {
        /// <summary>
        /// The funding type is missing, blank or not one of the recognized values.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Credit card.
        /// </summary>
        Credit,

        /// <summary>
        /// Debit card.
        /// </summary>
        Debit,

        /// <summary>
        /// Prepaid card.
        /// </summary>
        Prepaid
    }
}
