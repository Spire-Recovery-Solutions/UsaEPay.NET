using UsaEPay.NET.Models.Classes;
using System.Collections.Generic;

namespace UsaEPay.NET.Factories
{
    public static class UsaEPayRequestFactory
    {
        /// <summary>
        /// Processing a credit/debit card sale uses the sale command. 
        /// An example of this transaction type is shown here with custom fields.
        /// </summary>
        public static UsaEPayRequest CreditCardSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string country, string phone, string creditCardNumber, string expiration, int cvc, string invoice, string clientIP, Dictionary<string, string> customFields)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = creditCardNumber,
                    Cvc = cvc,
                    Expiration = expiration,
                    CardHolder = $"{firstName} {lastName}"
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip,
                    Country = country,
                    Phone = phone,
                },
                Invoice = invoice,
                ClientIP = clientIP,
                CustomFields = customFields
            };
        }

        /// <summary>
        /// Processing a credit/debit card sale uses the sale command. 
        /// An example of this transaction type is shown here without custom fields.
        /// </summary>
        public static UsaEPayRequest CreditCardSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string creditCardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = creditCardNumber,
                    Cvc = cvc,
                    Expiration = expiration,
                    CardHolder = $"{firstName} {lastName}"
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };
        }

        /// <summary>
        /// Creates a request for processing a check sale transaction with custom fields.
        /// </summary>
        public static UsaEPayRequest CheckSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string country, string phone, string routing, string account, string accountType, string checkNumber, string invoice, string clientIP, Dictionary<string, string> customFields)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "check:sale",
                Amount = amount,
                Check = new Check
                {
                    Number = checkNumber,
                    AccountType = accountType,
                    Account = account,
                    Routing = routing,
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip,
                    Country = country,
                    Phone = phone,
                },
                Invoice = invoice,
                ClientIP = clientIP,
                CustomFields = customFields
            };
        }

        /// <summary>
        /// Creates a simplified request for processing a sale through a checking or savings account without custom fields.
        /// </summary>
        public static UsaEPayRequest CheckSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string routing, string account, string accountType, string checkNumber)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "check:sale",
                Amount = amount,
                Check = new Check
                {
                    Number = checkNumber,
                    AccountType = accountType,
                    Account = account,
                    Routing = routing,
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };
        }

        /// <summary>
        /// Creates a request which process a sale using a token with custom fields in the place of a credit card number 
        /// </summary>
        public static UsaEPayRequest TokenSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string country, string phone, string token, int cvc, string invoice, string clientIP, Dictionary<string, string> customFields)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = token,
                    Cvc = cvc
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip,
                    Country = country,
                    Phone = phone,
                },
                Invoice = invoice,
                ClientIP = clientIP,
                CustomFields = customFields
            };
        }

        /// <summary>
        /// Creates a simplified request for processing a tokenized credit card sale transaction without custom fields.
        /// </summary>
        public static UsaEPayRequest TokenSaleRequest(decimal amount, string firstName, string lastName, string address, string address2, string city, string state, string zip, string token, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = token,
                    Cvc = cvc
                },
                BillingAddress = new Address
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Street = address,
                    Street2 = address2,
                    City = city,
                    State = state,
                    PostalCode = zip
                }
            };
        }

        /// <summary>
        /// Creates a request for processing a sale with a payment_key in place of a card number. 
        /// The payment_key is a one time use token. To create a reusable token, set the save_card flag to true.
        /// </summary>
        public static UsaEPayRequest PaymentKeySaleRequest(decimal amount, string paymentKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:sale",
                Amount = amount,
                PaymentKey = paymentKey
            };
        }

        /// <summary>
        /// Creates a request for logging a cash sale transaction.
        /// </summary>
        public static UsaEPayRequest CashSaleRequest(decimal amount)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cash:sale",
                Amount = amount
            };
        }

        /// <summary>
        /// Creates a request for processing a quick sale transaction.
        /// This works for Credit Card, Token, and Check transactions.
        /// </summary>
        public static UsaEPayRequest QuickSaleRequest(decimal amount, string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                TransactionKey = transactionKey,
                Command = "quicksale",
                Amount = amount
            };
        }

        /// <summary>
        /// Creates a request for processing a credit/debit card authorization without capturing funds.
        /// </summary>
        public static UsaEPayRequest AuthOnlySaleRequest(decimal amount, string cardHolder, string cardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "authonly",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    CardHolder = cardHolder,
                    Number = cardNumber,
                    Expiration = expiration,
                    Cvc = cvc
                }
            };
        }

        /// <summary>
        /// Creates a request for processing a credit card refund.
        /// </summary>
        public static UsaEPayRequest CreditCardRefundRequest(decimal amount, string cardHolder, string cardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:credit",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    CardHolder = cardHolder,
                    Number = cardNumber,
                    Expiration = expiration,
                    Cvc = cvc
                }
            };
        }

        /// <summary>
        /// Creates a request for processing a check refund.
        /// </summary>
        public static UsaEPayRequest CheckRefundRequest(decimal amount, string accountHolder, string accountNumber, string routingNumber, string accountType, string checkNumber)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "check:credit",
                Amount = amount,
                Check = new Check
                {
                    AccountHolder = accountHolder,
                    Number = checkNumber,
                    Account = accountNumber,
                    AccountType = accountType,
                    Routing = routingNumber
                }
            };
        }

        /// <summary>
        /// Creates a request for processing a cash refund transaction.
        /// </summary>
        public static UsaEPayRequest CashRefundRequest(decimal amount)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cash:refund",
                Amount = amount
            };
        }

        /// <summary>
        /// Creates a request for processing a quick refund transaction.
        /// </summary>
        public static UsaEPayRequest QuickRefundRequest(decimal amount, string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "quickrefund",
                Amount = amount,
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for capturing a pre-authorized credit card payment.
        /// </summary>
        public static UsaEPayRequest CapturePaymentRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:capture",
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for capturing, reauthorizing, and overriding a payment.
        /// </summary>
        public static UsaEPayRequest CapturePaymentReauthRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:capture:reauth",
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for capturing a payment with an override.
        /// </summary>
        public static UsaEPayRequest CapturePaymentOverrideRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:capture:override",
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for capturing a payment with an error.
        /// </summary>
        public static UsaEPayRequest CapturePaymentErrorRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:capture:error",
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for voiding a credit card payment.
        /// </summary>
        public static UsaEPayRequest CreditVoidRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "creditvoid",
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for posting an authorized credit card payment.
        /// </summary>
        public static UsaEPayRequest PostPaymentRequest(decimal amount, string authCode, string cardHolder, string cardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:postauth",
                Amount = amount,
                CreditCard = new CreditCard
                {
                    Number = cardNumber,
                    Expiration = expiration,
                    Cvc = cvc,
                    CardHolder = cardHolder
                }
            };
        }

        /// <summary>
        /// Creates a request for voiding a payment transaction.
        /// </summary>
        public static UsaEPayRequest VoidPaymentRequest(string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "void",
                TransactionKey = transactionKey
            };
        }

        /// <summary>
        /// Creates a request for releasing funds from a voided credit card transaction.
        /// </summary>
        public static UsaEPayRequest ReleaseFundsRequest(string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:void:release",
                TransactionKey = transactionKey
            };
        }

        /// <summary>
        /// Creates a request for unvoiding a transaction.
        /// </summary>
        public static UsaEPayRequest UnvoidRequest(string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "unvoid",
                TransactionKey = transactionKey
            };
        }

        /// <summary>
        /// Creates a request for adjusting a payment transaction.
        /// </summary>
        public static UsaEPayRequest AdjustPaymentRequest(string tranKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:adjust",
                TransactionKey = tranKey
            };
        }

        /// <summary>
        /// Creates a request for adjusting a refunded credit card payment.
        /// </summary>
        public static UsaEPayRequest AdjustPaymentRefundRequest(string transKey, decimal amount)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:refund:adjust",
                TransactionKey = transKey,
                Amount = amount
            };
        }

        /// <summary>
        /// Creates a request for tokenizing a credit card for later use.
        /// </summary>
        public static UsaEPayRequest TokenizeCardRequest(string cardHolder, string creditCardNumber, string expiration, int cvc)
        {
            return new UsaEPayRequest
            {
                Endpoint = "transactions",
                RequestType = RestSharp.Method.Post,
                Command = "cc:save",

                CreditCard = new CreditCard
                {
                    Number = creditCardNumber,
                    Cvc = cvc,
                    Expiration = expiration,
                    CardHolder = cardHolder
                },
            };
        }

        /// <summary>
        /// Creates a request for retrieving details of a specific transaction.
        /// </summary>
        public static UsaEPayGetRequest RetrieveTransactionDetailsRequest(string transactionId)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"transactions/{transactionId}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving details of a specific token.
        /// </summary>
        public static UsaEPayGetRequest RetrieveTokenDetailsRequest(string tokenId)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"tokens/{tokenId}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of batches.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBatchListRequest()
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"batches"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a filtered list of batches by date.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBatchListByDateRequest(long openBefore, long openAfter)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"batches?openedge={openBefore}&openedlt={openAfter}"
            };
        }
    }
}
