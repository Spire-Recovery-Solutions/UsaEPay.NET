using UsaEPay.NET.Models.Classes;

namespace UsaEPay.NET.Factories
{
    public static class UsaEPayRequestFactory
    {
        /// <summary>
        /// Processing a credit/debit card sale uses the sale command. 
        /// An example of this transaction type is shown here with or without custom fields.
        /// </summary>
        public static UsaEPayRequest CreditCardSaleRequest(UsaEPayTransactionParams tranParams,
            Dictionary<string, string>? customFields = null)
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
                ClientIp = tranParams.ClientIp,
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
            Dictionary<string, string>? customFields = null)
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
                ClientIp = tranParams.ClientIp,
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
            Dictionary<string, string>? customFields = null)
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
                ClientIp = tranParams.ClientIp,
                CustomFields = customFields
            };

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
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.QuickSale,
                Amount = tranParams.Amount
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;
            return request;
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
        public static UsaEPayRequest ConnectedRefundRequest(UsaEPayTransactionParams tranParams, Dictionary<string, string>? customFields = null)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.Refund,
                Amount = tranParams.Amount,
                Email = tranParams.Email,
                ClientIp = tranParams.ClientIp
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;

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
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.QuickRefund,
                Amount = tranParams.Amount,
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;
            return request;
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
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CreditVoid,
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;
            return request;
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
                AuthCode = tranParams.AuthCode,
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
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.VoidPayment,
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;
            return request;
        }

        /// <summary>
        /// Creates a request for releasing funds from a voided credit card transaction.
        /// </summary>
        public static UsaEPayRequest ReleaseFundsRequest(UsaEPayTransactionParams tranParams)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.ReleaseFunds,
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;
            return request;
        }

        /// <summary>
        /// Creates a request for unvoiding a transaction.
        /// </summary>
        public static UsaEPayRequest UnvoidRequest(UsaEPayTransactionParams tranParams)
        {
            var request = new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.Unvoid,
            };
            if (!string.IsNullOrEmpty(tranParams.TransactionKey))
                request.TransactionKey = tranParams.TransactionKey;
            else if (!string.IsNullOrEmpty(tranParams.Refnum))
                request.ReferenceNumber = tranParams.Refnum;
            return request;
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
                    CardHolder = tranParams.CardHolder,
                    AvsStreet = tranParams.Address,
                    AvsPostalCode = tranParams.Zip
                },
                Invoice = tranParams.Invoice,
                OrderId = tranParams.OrderId,
            };
        }

        /// <summary>
        /// Creates a request for generating a token from an existing transaction.
        /// </summary>
        public static UsaEPayRequest CreateTokenFromTransactionRequest(string transactionKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Tokens,
                RequestType = RestSharp.Method.Post,
                TransactionKey = transactionKey
            };
        }

        /// <summary>
        /// Creates a request for processing a sale using a stored customer payment method.
        /// </summary>
        public static UsaEPayRequest CustomerSaleRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CustomerSale,
                Amount = tranParams.Amount,
                CustomerKey = tranParams.CustomerKey,
                PaymentMethodKey = tranParams.PaymentMethodKey,
            };
        }

        /// <summary>
        /// Creates a request for processing a refund using a stored customer payment method.
        /// </summary>
        public static UsaEPayRequest CustomerRefundRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.CustomerRefund,
                Amount = tranParams.Amount,
                CustomerKey = tranParams.CustomerKey,
                PaymentMethodKey = tranParams.PaymentMethodKey,
            };
        }

        /// <summary>
        /// Creates a request for processing a sale using a payment key (Client JS Library token).
        /// </summary>
        public static UsaEPayRequest PaymentKeySaleRequest(UsaEPayTransactionParams tranParams)
        {
            return new UsaEPayRequest
            {
                Endpoint = UsaEPayEndpoints.Transactions,
                RequestType = RestSharp.Method.Post,
                Command = UsaEPayCommandTypes.TransactionSale,
                Amount = tranParams.Amount,
                PaymentKey = tranParams.PaymentKey,
            };
        }

        /// <summary>
        /// Creates a request for sending a transaction receipt to an email address.
        /// </summary>
        public static UsaEPayRequest SendReceiptRequest(string transactionKey, string email)
        {
            return new UsaEPayRequest
            {
                Endpoint = $"{UsaEPayEndpoints.TransactionSend}/{transactionKey}/send",
                RequestType = RestSharp.Method.Post,
                Email = email,
            };
        }

        /// <summary>
        /// Creates a request for retrieving a specific transaction receipt.
        /// </summary>
        public static UsaEPayGetRequest RetrieveReceiptRequest(string transactionKey, string receiptId)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.TransactionReceipts}/{transactionKey}/receipts/{receiptId}"
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
        public static UsaEPayGetRequest RetrieveBatchListRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Batches}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a filtered list of batches by date.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBatchListByDateRequest(string? openedBefore = null, string? openedAfter = null, string? closedBefore = null, string? closedAfter = null, string? openedGt = null, string? openedLe = null, string? closedGt = null, string? closedLe = null, int limit = 20, int offset = 0)
        {
            var queryParams = new List<string>();
            queryParams.Add($"limit={limit}");
            queryParams.Add($"offset={offset}");
            if (openedBefore != null) queryParams.Add($"openedlt={openedBefore}");
            if (openedAfter != null) queryParams.Add($"openedge={openedAfter}");
            if (closedBefore != null) queryParams.Add($"closedlt={closedBefore}");
            if (closedAfter != null) queryParams.Add($"closedge={closedAfter}");
            if (openedGt != null) queryParams.Add($"openedgt={openedGt}");
            if (openedLe != null) queryParams.Add($"openedle={openedLe}");
            if (closedGt != null) queryParams.Add($"closedgt={closedGt}");
            if (closedLe != null) queryParams.Add($"closedle={closedLe}");

            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Batches}?{string.Join("&", queryParams)}"
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
        public static UsaEPayGetRequest RetrieveCurrentBatchTransactionsRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.CurrentBatchTransactions}?limit={limit}&offset={offset}"
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

        /// <summary>
        /// Creates a request for creating a new customer.
        /// </summary>
        public static UsaEPayCustomerRequest CreateCustomerRequest(UsaEPayCustomerRequest customerRequest)
        {
            customerRequest.Endpoint = UsaEPayEndpoints.Customers;
            customerRequest.RequestType = RestSharp.Method.Post;
            return customerRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific customer by customer key.
        /// </summary>
        public static UsaEPayGetRequest RetrieveCustomerRequest(string custKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of customers.
        /// </summary>
        public static UsaEPayGetRequest RetrieveCustomerListRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing customer.
        /// </summary>
        public static UsaEPayCustomerRequest UpdateCustomerRequest(string custKey, UsaEPayCustomerRequest customerRequest)
        {
            customerRequest.Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}";
            customerRequest.RequestType = RestSharp.Method.Put;
            return customerRequest;
        }

        /// <summary>
        /// Creates a request for deleting a specific customer.
        /// </summary>
        public static UsaEPayCustomerRequest DeleteCustomerRequest(string custKey)
        {
            return new UsaEPayCustomerRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        /// <summary>
        /// Creates a request for bulk deleting customers by keys.
        /// </summary>
        public static UsaEPayBulkDeleteRequest BulkDeleteCustomersRequest(string[] custKeys)
        {
            return new UsaEPayBulkDeleteRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/bulk",
                RequestType = RestSharp.Method.Delete,
                Keys = custKeys
            };
        }

        // TODO: Upload Bulk Transaction File requires multipart file upload which is beyond
        // the current SDK's request pattern. This placeholder needs special handling for
        // POST /bulk_transactions with multipart form data (file upload).
        // public static ??? UploadBulkTransactionFileRequest(string filePath) { ... }

        /// <summary>
        /// Creates a request for retrieving the status of a specific bulk transaction file.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBulkTransactionStatusRequest(string bulkKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactions}/{bulkKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving the status of the current bulk transaction file.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBulkTransactionCurrentRequest()
        {
            return new UsaEPayGetRequest
            {
                Endpoint = UsaEPayEndpoints.BulkTransactionsCurrent
            };
        }

        /// <summary>
        /// Creates a request for retrieving transactions from a specific bulk transaction file.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBulkTransactionFileTransactionsRequest(string bulkKey, int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactions}/{bulkKey}/{UsaEPayEndpoints.Transactions}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving transactions from the current bulk transaction file.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBulkTransactionCurrentTransactionsRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactionsCurrent}/{UsaEPayEndpoints.Transactions}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for pausing a specific bulk transaction file.
        /// </summary>
        public static UsaEPayRequest PauseBulkTransactionRequest(string bulkKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactions}/{bulkKey}/pause",
                RequestType = RestSharp.Method.Post
            };
        }

        /// <summary>
        /// Creates a request for pausing the current bulk transaction file.
        /// </summary>
        public static UsaEPayRequest PauseBulkTransactionCurrentRequest()
        {
            return new UsaEPayRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactionsCurrent}/pause",
                RequestType = RestSharp.Method.Post
            };
        }

        /// <summary>
        /// Creates a request for resuming a paused bulk transaction file.
        /// </summary>
        public static UsaEPayRequest ResumeBulkTransactionRequest(string bulkKey)
        {
            return new UsaEPayRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactions}/{bulkKey}/resume",
                RequestType = RestSharp.Method.Post
            };
        }

        /// <summary>
        /// Creates a request for resuming the current bulk transaction file.
        /// </summary>
        public static UsaEPayRequest ResumeBulkTransactionCurrentRequest()
        {
            return new UsaEPayRequest
            {
                Endpoint = $"{UsaEPayEndpoints.BulkTransactionsCurrent}/resume",
                RequestType = RestSharp.Method.Post
            };
        }

        /// <summary>
        /// Creates a request for retrieving a customer's transaction history.
        /// </summary>
        public static UsaEPayGetRequest RetrieveCustomerTransactionsRequest(string custKey, int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.Transactions}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for creating a billing schedule for an existing customer.
        /// </summary>
        public static UsaEPayBillingScheduleRequest CreateBillingScheduleRequest(string custKey, UsaEPayBillingScheduleRequest scheduleRequest)
        {
            scheduleRequest.Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.BillingSchedules}";
            scheduleRequest.RequestType = RestSharp.Method.Post;
            return scheduleRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific customer billing schedule.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBillingScheduleRequest(string custKey, string scheduleKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.BillingSchedules}/{scheduleKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of customer billing schedules.
        /// </summary>
        public static UsaEPayGetRequest RetrieveBillingScheduleListRequest(string custKey, int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.BillingSchedules}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for adding a payment method to an existing customer.
        /// </summary>
        public static UsaEPayPaymentMethodRequest AddPaymentMethodRequest(string custKey, UsaEPayPaymentMethodRequest paymentMethodRequest)
        {
            paymentMethodRequest.Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.PaymentMethods}";
            paymentMethodRequest.RequestType = RestSharp.Method.Post;
            return paymentMethodRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific customer payment method.
        /// </summary>
        public static UsaEPayGetRequest RetrievePaymentMethodRequest(string custKey, string methodKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.PaymentMethods}/{methodKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of customer payment methods.
        /// </summary>
        public static UsaEPayGetRequest RetrievePaymentMethodListRequest(string custKey, int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.PaymentMethods}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing customer payment method.
        /// </summary>
        public static UsaEPayPaymentMethodRequest UpdatePaymentMethodRequest(string custKey, string methodKey, UsaEPayPaymentMethodRequest paymentMethodRequest)
        {
            paymentMethodRequest.Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.PaymentMethods}/{methodKey}";
            paymentMethodRequest.RequestType = RestSharp.Method.Put;
            return paymentMethodRequest;
        }

        /// <summary>
        /// Creates a request for deleting a specific customer payment method.
        /// </summary>
        public static UsaEPayPaymentMethodRequest DeletePaymentMethodRequest(string custKey, string methodKey)
        {
            return new UsaEPayPaymentMethodRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.PaymentMethods}/{methodKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        /// <summary>
        /// Creates a request for bulk deleting customer payment methods by keys.
        /// </summary>
        public static UsaEPayBulkDeleteRequest BulkDeletePaymentMethodsRequest(string custKey, string[] methodKeys)
        {
            return new UsaEPayBulkDeleteRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Customers}/{custKey}/{UsaEPayEndpoints.PaymentMethods}/bulk",
                RequestType = RestSharp.Method.Delete,
                Keys = methodKeys
            };
        }

        #region Payment Engine - Devices

        /// <summary>
        /// Creates a request for registering a new payment engine device.
        /// </summary>
        public static UsaEPayDeviceRequest RegisterDeviceRequest(UsaEPayDeviceRequest deviceRequest)
        {
            deviceRequest.Endpoint = UsaEPayEndpoints.PaymentEngineDevices;
            deviceRequest.RequestType = RestSharp.Method.Post;
            return deviceRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific payment engine device.
        /// </summary>
        public static UsaEPayGetRequest RetrieveDeviceRequest(string deviceKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.PaymentEngineDevices}/{deviceKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of payment engine devices.
        /// </summary>
        public static UsaEPayGetRequest RetrieveDeviceListRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.PaymentEngineDevices}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing payment engine device.
        /// </summary>
        public static UsaEPayDeviceRequest UpdateDeviceRequest(string deviceKey, UsaEPayDeviceRequest deviceRequest)
        {
            deviceRequest.Endpoint = $"{UsaEPayEndpoints.PaymentEngineDevices}/{deviceKey}";
            deviceRequest.RequestType = RestSharp.Method.Put;
            return deviceRequest;
        }

        /// <summary>
        /// Creates a request for updating the settings of a payment engine device.
        /// </summary>
        public static UsaEPayDeviceRequest UpdateDeviceSettingsRequest(string deviceKey, UsaEPayDeviceRequest deviceRequest)
        {
            deviceRequest.Endpoint = $"{UsaEPayEndpoints.PaymentEngineDevices}/{deviceKey}/settings";
            deviceRequest.RequestType = RestSharp.Method.Put;
            return deviceRequest;
        }

        /// <summary>
        /// Creates a request for updating the terminal configuration of a payment engine device.
        /// </summary>
        public static UsaEPayDeviceRequest UpdateDeviceTerminalConfigRequest(string deviceKey, UsaEPayDeviceRequest deviceRequest)
        {
            deviceRequest.Endpoint = $"{UsaEPayEndpoints.PaymentEngineDevices}/{deviceKey}/terminal-config";
            deviceRequest.RequestType = RestSharp.Method.Put;
            return deviceRequest;
        }

        /// <summary>
        /// Creates a request for deleting a payment engine device.
        /// </summary>
        public static UsaEPayDeviceRequest DeleteDeviceRequest(string deviceKey)
        {
            return new UsaEPayDeviceRequest
            {
                Endpoint = $"{UsaEPayEndpoints.PaymentEngineDevices}/{deviceKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        #endregion

        #region Payment Engine - Pay Requests

        /// <summary>
        /// Creates a request for starting a new payment request on a device.
        /// </summary>
        public static UsaEPayPayRequestRequest CreatePayRequestRequest(UsaEPayPayRequestRequest payRequest)
        {
            payRequest.Endpoint = UsaEPayEndpoints.PaymentEnginePayRequests;
            payRequest.RequestType = RestSharp.Method.Post;
            return payRequest;
        }

        /// <summary>
        /// Creates a request for retrieving the status of a specific payment request.
        /// </summary>
        public static UsaEPayGetRequest RetrievePayRequestRequest(string requestKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.PaymentEnginePayRequests}/{requestKey}"
            };
        }

        /// <summary>
        /// Creates a request for canceling a payment request.
        /// </summary>
        public static UsaEPayPayRequestRequest CancelPayRequestRequest(string requestKey)
        {
            return new UsaEPayPayRequestRequest
            {
                Endpoint = $"{UsaEPayEndpoints.PaymentEnginePayRequests}/{requestKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        #endregion

        #region Inventory Locations

        /// <summary>
        /// Creates a request for creating a new inventory location.
        /// </summary>
        public static UsaEPayInventoryLocationRequest CreateInventoryLocationRequest(UsaEPayInventoryLocationRequest locationRequest)
        {
            locationRequest.Endpoint = UsaEPayEndpoints.InventoryLocations;
            locationRequest.RequestType = RestSharp.Method.Post;
            return locationRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific inventory location by location key.
        /// </summary>
        public static UsaEPayGetRequest RetrieveInventoryLocationRequest(string locationKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.InventoryLocations}/{locationKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of inventory locations.
        /// </summary>
        public static UsaEPayGetRequest RetrieveInventoryLocationListRequest(int limit = 100, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.InventoryLocations}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing inventory location.
        /// </summary>
        public static UsaEPayInventoryLocationRequest UpdateInventoryLocationRequest(string locationKey, UsaEPayInventoryLocationRequest locationRequest)
        {
            locationRequest.Endpoint = $"{UsaEPayEndpoints.InventoryLocations}/{locationKey}";
            locationRequest.RequestType = RestSharp.Method.Put;
            return locationRequest;
        }

        /// <summary>
        /// Creates a request for deleting a specific inventory location.
        /// </summary>
        public static UsaEPayInventoryLocationRequest DeleteInventoryLocationRequest(string locationKey)
        {
            return new UsaEPayInventoryLocationRequest
            {
                Endpoint = $"{UsaEPayEndpoints.InventoryLocations}/{locationKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        #endregion

        #region Inventory

        /// <summary>
        /// Creates a request for creating a new inventory.
        /// </summary>
        public static UsaEPayInventoryRequest CreateInventoryRequest(UsaEPayInventoryRequest inventoryRequest)
        {
            inventoryRequest.Endpoint = UsaEPayEndpoints.Inventory;
            inventoryRequest.RequestType = RestSharp.Method.Post;
            return inventoryRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific inventory by inventory key.
        /// </summary>
        public static UsaEPayGetRequest RetrieveInventoryRequest(string inventoryKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Inventory}/{inventoryKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of inventories.
        /// </summary>
        public static UsaEPayGetRequest RetrieveInventoryListRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Inventory}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing inventory.
        /// </summary>
        public static UsaEPayInventoryRequest UpdateInventoryRequest(string inventoryKey, UsaEPayInventoryRequest inventoryRequest)
        {
            inventoryRequest.Endpoint = $"{UsaEPayEndpoints.Inventory}/{inventoryKey}";
            inventoryRequest.RequestType = RestSharp.Method.Put;
            return inventoryRequest;
        }

        /// <summary>
        /// Creates a request for deleting a specific inventory.
        /// </summary>
        public static UsaEPayInventoryRequest DeleteInventoryRequest(string inventoryKey)
        {
            return new UsaEPayInventoryRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Inventory}/{inventoryKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        #endregion

        #region Products

        /// <summary>
        /// Creates a request for creating a new product.
        /// </summary>
        public static UsaEPayProductRequest CreateProductRequest(UsaEPayProductRequest productRequest)
        {
            productRequest.Endpoint = UsaEPayEndpoints.Products;
            productRequest.RequestType = RestSharp.Method.Post;
            return productRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific product by product key.
        /// </summary>
        public static UsaEPayGetRequest RetrieveProductRequest(string productKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Products}/{productKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of products.
        /// </summary>
        public static UsaEPayGetRequest RetrieveProductListRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Products}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing product.
        /// </summary>
        public static UsaEPayProductRequest UpdateProductRequest(string productKey, UsaEPayProductRequest productRequest)
        {
            productRequest.Endpoint = $"{UsaEPayEndpoints.Products}/{productKey}";
            productRequest.RequestType = RestSharp.Method.Put;
            return productRequest;
        }

        /// <summary>
        /// Creates a request for deleting a specific product.
        /// </summary>
        public static UsaEPayProductRequest DeleteProductRequest(string productKey)
        {
            return new UsaEPayProductRequest
            {
                Endpoint = $"{UsaEPayEndpoints.Products}/{productKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        #endregion

        #region Product Categories

        /// <summary>
        /// Creates a request for creating a new product category.
        /// </summary>
        public static UsaEPayProductCategoryRequest CreateProductCategoryRequest(UsaEPayProductCategoryRequest categoryRequest)
        {
            categoryRequest.Endpoint = UsaEPayEndpoints.ProductCategories;
            categoryRequest.RequestType = RestSharp.Method.Post;
            return categoryRequest;
        }

        /// <summary>
        /// Creates a request for retrieving a specific product category by category key.
        /// </summary>
        public static UsaEPayGetRequest RetrieveProductCategoryRequest(string categoryKey)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.ProductCategories}/{categoryKey}"
            };
        }

        /// <summary>
        /// Creates a request for retrieving a list of product categories.
        /// </summary>
        public static UsaEPayGetRequest RetrieveProductCategoryListRequest(int limit = 20, int offset = 0)
        {
            return new UsaEPayGetRequest
            {
                Endpoint = $"{UsaEPayEndpoints.ProductCategories}?limit={limit}&offset={offset}"
            };
        }

        /// <summary>
        /// Creates a request for updating an existing product category.
        /// </summary>
        public static UsaEPayProductCategoryRequest UpdateProductCategoryRequest(string categoryKey, UsaEPayProductCategoryRequest categoryRequest)
        {
            categoryRequest.Endpoint = $"{UsaEPayEndpoints.ProductCategories}/{categoryKey}";
            categoryRequest.RequestType = RestSharp.Method.Put;
            return categoryRequest;
        }

        /// <summary>
        /// Creates a request for deleting a specific product category.
        /// </summary>
        public static UsaEPayProductCategoryRequest DeleteProductCategoryRequest(string categoryKey)
        {
            return new UsaEPayProductCategoryRequest
            {
                Endpoint = $"{UsaEPayEndpoints.ProductCategories}/{categoryKey}",
                RequestType = RestSharp.Method.Delete
            };
        }

        #endregion
    }
}
