using UsaEPay.NET.Models.Classes;
using System.Collections.Generic;

namespace UsaEPay.NET.Factories
{
    public static class UsaEPayRequestFactory
    {
        /// <summary>
        /// Processing a credit/debit card sale uses the sale command. 
        /// An example of this transaction type is shown here with or without custom fields.
        /// </summary>
        public static UsaEPayRequest CreditCardSaleRequest(UsaEPayTransactionParams tranParams,
            Dictionary<string, string> customFields = null)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.TransactionSale,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,

                CreditCard = new CreditCard
                {
                    Number = tranParams.CardNumber,
                    Cvc = tranParams.Cvc,
                    Expiration = tranParams.Expiration,
                    CardHolder = tranParams.AccountHolder,

                },
                BillingAddress = new Address
                {
                    FirstName = tranParams.FirstName,
                    LastName = tranParams.LastName,
                    Street = tranParams.Address,
                    Street2 = tranParams.Address2,
                    City = tranParams.City,
                    State = tranParams.State,
                    PostalCode = tranParams.Zip,
                    Country = tranParams.Country,
                    Phone = tranParams.Phone,
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
                ClientIP = tranParams.ClientIP,
                SaveCard = false
            };
            if (customFields != null)
            {
                request.CustomFields = customFields;
            }

            return request;
        }

        /// <summary>
        /// Creates a request for processing a check sale transaction with or without custom fields.
        /// </summary>
        public static UsaEPayRequest CheckSaleRequest(UsaEPayTransactionParams tranParams,
            Dictionary<string, string> customFields = null)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CheckSale,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,
                Check = new Check
                {
                    Number = tranParams.CheckNumber,
                    AccountType = tranParams.AccountType,
                    Account = tranParams.Account,
                    Routing = tranParams.Routing,
                },
                BillingAddress = new Address
                {
                    FirstName = tranParams.FirstName,
                    LastName = tranParams.LastName,
                    Street = tranParams.Address,
                    Street2 = tranParams.Address2,
                    City = tranParams.City,
                    State = tranParams.State,
                    PostalCode = tranParams.Zip,
                    Country = tranParams.Country,
                    Phone = tranParams.Phone,
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
                ClientIP = tranParams.ClientIP,
                SaveCard = false
            };
            if (customFields != null)
            {
                request.CustomFields = customFields;
            }

            return request;
        }

        /// <summary>
        /// Creates a request which process a sale using a token with or without custom fields in the place of a credit card number 
        /// </summary>
        public static UsaEPayRequest TokenSaleRequest(UsaEPayTransactionParams tranParams,
            Dictionary<string, string> customFields = null)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.TransactionSale,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,
                CreditCard = new CreditCard
                {
                    Number = tranParams.Token,
                    Cvc = tranParams.Cvc
                },
                BillingAddress = new Address
                {
                    FirstName = tranParams.FirstName,
                    LastName = tranParams.LastName,
                    Street = tranParams.Address,
                    Street2 = tranParams.Address2,
                    City = tranParams.City,
                    State = tranParams.State,
                    PostalCode = tranParams.Zip,
                    Country = tranParams.Country,
                    Phone = tranParams.Phone,
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
                ClientIP = tranParams.ClientIP,
                CustomFields = customFields
            };
            if (customFields != null)
            {
                request.CustomFields = customFields;
            }

            return request;
        }

        /// <summary>
        /// Creates a request for logging a cash sale transaction.
        /// </summary>
        public static UsaEPayRequest CashSaleRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CashSale,
                Amount = tranParams.Amount
            };
        }

        /// <summary>
        /// Creates a request for processing a quick sale transaction.
        /// This works for Credit Card, Token, and Check transactions.
        /// </summary>
        public static UsaEPayRequest QuickSaleRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                TransactionKey = tranParams.TransactionKey,
                Command = UsaEPayCommandTypes.QuickSale,
                Amount = tranParams.Amount
            };
        }

        /// <summary>
        /// Creates a request for processing a credit/debit card authorization without capturing funds.
        /// </summary>
        public static UsaEPayRequest AuthOnlySaleRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.AuthOnlySale,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,
                CreditCard = new CreditCard
                {
                    CardHolder = tranParams.AccountHolder,
                    Number = tranParams.CardNumber,
                    Expiration = tranParams.Expiration,
                    Cvc = tranParams.Cvc
                },
                BillingAddress = new Address
                {
                    FirstName = tranParams.FirstName,
                    LastName = tranParams.LastName,
                    Street = tranParams.Address,
                    Street2 = tranParams.Address2,
                    City = tranParams.City,
                    State = tranParams.State,
                    PostalCode = tranParams.Zip,
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
            };
        }

        /// <summary>
        /// Creates a request for processing a credit card refund.
        /// </summary>
        public static UsaEPayRequest CreditCardRefundRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CreditCardRefund,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,
                CreditCard = new CreditCard
                {
                    CardHolder = tranParams.CardHolder,
                    Number = tranParams.CardNumber,
                    Expiration = tranParams.Expiration,
                    Cvc = tranParams.Cvc
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
            };
        }

        /// <summary>
        /// Creates a request for processing a check refund.
        /// </summary>
        public static UsaEPayRequest CheckRefundRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CheckRefund,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,
                Check = new Check
                {
                    AccountHolder = tranParams.AccountHolder,
                    Number = tranParams.CheckNumber,
                    Account = tranParams.AccountNumber,
                    AccountType = tranParams.AccountType,
                    Routing = tranParams.Routing
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
            };
        }

        /// <summary>
        /// Creates a request for processing a cash refund transaction.
        /// </summary>
        public static UsaEPayRequest CashRefundRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CashRefund,
                Amount = tranParams.Amount
            };
        }

        /// <summary>
        /// Creates a request for processing a connected refund transaction.
        /// </summary>
        public static UsaEPayRequest ConnectedRefundRequest(UsaEPayTransactionParams tranParams, Dictionary<string, string> customFields = null)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.Refund,
                TransactionKey = tranParams.TransactionKey,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                ClientIP = tranParams.ClientIP
            };

            if (customFields != null)
            {
                request.CustomFields = customFields;
            }

            return request;
        }

        /// <summary>
        /// Creates a request for processing a quick refund transaction.
        /// </summary>
        public static UsaEPayRequest QuickRefundRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.QuickRefund,
                Amount = tranParams.Amount,
                TransactionKey = tranParams.TransactionKey,

            };
        }

        /// <summary>
        /// Creates a request for capturing a pre-authorized credit card payment.
        /// </summary>
        public static UsaEPayRequest CapturePaymentRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CapturePayment,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for capturing, reauthorizing, and overriding a payment.
        /// </summary>
        public static UsaEPayRequest CapturePaymentReauthRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CapturePaymentReauth,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for capturing a payment with an override.
        /// </summary>
        public static UsaEPayRequest CapturePaymentOverrideRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CapturePaymentOverride,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for capturing a payment with an error.
        /// </summary>
        public static UsaEPayRequest CapturePaymentErrorRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CapturePaymentError,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for voiding a credit card payment.
        /// </summary>
        public static UsaEPayRequest CreditVoidRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CreditVoid,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for posting an authorized credit card payment.
        /// </summary>
        public static UsaEPayRequest PostPaymentRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.PostPayment,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                Description = tranParams.Description,
                CreditCard = new CreditCard
                {
                    Number = tranParams.CardNumber,
                    Expiration = tranParams.Expiration,
                    Cvc = tranParams.Cvc,
                    CardHolder = tranParams.CardHolder
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
            };
        }

        /// <summary>
        /// Creates a request for voiding a payment transaction.
        /// </summary>
        public static UsaEPayRequest VoidPaymentRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.VoidPayment,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for releasing funds from a voided credit card transaction.
        /// </summary>
        public static UsaEPayRequest ReleaseFundsRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.ReleaseFunds,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for unvoiding a transaction.
        /// </summary>
        public static UsaEPayRequest UnvoidRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.Unvoid,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for adjusting a payment transaction.
        /// </summary>
        public static UsaEPayRequest AdjustPaymentRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.AdjustPayment,
                TransactionKey = tranParams.TransactionKey
            };
        }

        /// <summary>
        /// Creates a request for adjusting a refunded credit card payment.
        /// </summary>
        public static UsaEPayRequest AdjustPaymentRefundRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.AdjustPaymentRefund,
                TransactionKey = tranParams.TransactionKey,
                Amount = tranParams.Amount
            };
        }

        /// <summary>
        /// Creates a request for tokenizing a credit card for later use.
        /// </summary>
        public static UsaEPayRequest TokenizeCardRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.TokenizeCard,

                CreditCard = new CreditCard
                {
                    Number = tranParams.CardNumber,
                    Cvc = tranParams.Cvc,
                    Expiration = tranParams.Expiration,
                    CardHolder = tranParams.CardHolder
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
            };
        }

        /// <summary>
        /// Creates a request for closing a current batch
        /// </summary>
        public static UsaEPayRequest CloseCurrentBatchRequest()
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.CloseCurrentBatch,
                RequestType = RestSharp.Method.Post
            };
        }

        /// <summary>
        /// Creates a request for retrieving details of a specific transaction.
        /// </summary>
        public static UsaEPayGetRequest RetrieveTransactionDetailsRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Transactions}/{tranParams.TransactionKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving details of a specific token.
        /// </summary>
        public static UsaEPayGetRequest RetrieveTokenDetailsRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Tokens}/{tranParams.Token}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a specific batch by batchKey.
        /// </summary>
        public static UsaEPayGetRequest RetrieveSpecificBatchRequest(string batchKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Batches}/{batchKey}"
            };
        }

        public static UsaEPayGetRequest RetrieveCurrentBatchRequest()
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.CurrentBatch}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of batches.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBatchListRequest()
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Batches}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a filtered list of batches by date.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBatchListByDateRequest(long openBefore, long openAfter)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Batches}?openedge={openBefore}&openedlt={openAfter}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving batch transactions by batchId
        /// </summary>
        public static UsaEPayGetRequest RetrieveBatchTransactionsByIdRequest(string batchId, int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Batches}/{batchId}/{UsaEPayEndpoints.Transactions}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving current batch transactions
        /// </summary>
        public static UsaEPayGetRequest RetrieveCurrentBatchTransactionsRequest()
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.CurrentBatchTransactions}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of transaction.
        /// </summary>
        public static UsaEPayGetRequest RetrieveTransactionsRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Transactions}?limit={limit}&offset={offset}"
            };
        }
    }
}
