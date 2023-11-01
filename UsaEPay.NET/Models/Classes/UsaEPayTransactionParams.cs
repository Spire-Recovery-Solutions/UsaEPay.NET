using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsaEPay.NET.Models.Classes
{
    /// <summary>
    /// Represents the parameters for a USA ePay transaction.
    /// </summary>
    public class UsaEPayTransactionParams
    {
        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Gets or sets the first name of the customer.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the customer.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the street address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the additional address information.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the city.
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        public string Zip { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string Phone { get; set; }

        
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the cardholder's name.
        /// </summary>
        public string CardHolder { get; set; }

        /// <summary>
        /// Gets or sets the account holder's name for check transactions.
        /// </summary>
        public string AccountHolder { get; set; }

        /// <summary>
        /// Gets or sets the account number for check transactions.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Gets or sets the credit card number.
        /// </summary>
        public string CardNumber { get; set; }

        /// <summary>
        /// Gets or sets the card expiration date.
        /// </summary>
        public string Expiration { get; set; }

        /// <summary>
        /// Gets or sets the card verification code.
        /// </summary>
        public int Cvc { get; set; }

        /// <summary>
        /// Gets or sets the invoice number for the transaction.
        /// </summary>
        public string Invoice { get; set; }

        /// <summary>
        /// Gets or sets the client's IP address.
        /// </summary>
        public string ClientIP { get; set; }

        /// <summary>
        /// Gets or sets the routing number for check transactions.
        /// </summary>
        public string Routing { get; set; }

        /// <summary>
        /// Gets or sets the account number for check transactions.
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the account type for check transactions (e.g., checking or savings).
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Gets or sets the check number for check transactions.
        /// </summary>
        public string CheckNumber { get; set; }
        
        /// <summary>
        /// Public description of the transaction 
        /// </summary>
        public string Description { get; set; }
    }
}
